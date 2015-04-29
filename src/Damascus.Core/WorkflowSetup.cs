using System;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Damascus.Core.Serialization;

namespace Damascus.Core
{
    public class WorkflowSetup
    {
        private WorkflowEngine workflowEngine;
        private static Assembly[] assemblies;

        public static WorkflowSetup Configure(params Assembly[] assembliesToScan)
        {
            assemblies = assembliesToScan;
            var engine = new WorkflowEngine(new MemorySerializer());
            return new WorkflowSetup{workflowEngine = engine};
        }

        public WorkflowEngine WorkflowEngine
        {
            get
            {
                if(workflowEngine.Serializer == null)
                    throw new InvalidOperationException(@"The serializer for the workflow engine has not been properly set.
                                                        Try calling WithSerializer with a valid Serializer.");
                return workflowEngine;
            }
        }

        public WorkflowSetup WithSerializer(IDataSerializer serializer)
        {
            if (workflowEngine == null)
                throw new Exception("Call Configure First");
            else
                workflowEngine.Serializer = serializer;
            return this;
        }

        public WorkflowSetup WithCastleWindsor(IWindsorContainer builder)
        {
            if (workflowEngine == null)
                throw new Exception("Call Configure First");

            foreach (var assembly in assemblies)
            {
                builder.Register(
                    AllTypes.FromAssembly(assembly)
                    .BasedOn<IWorkflow>()
                    .LifestyleTransient()
                    .Configure(workflow => workflow.Named(GetWorkflowTypeName(workflow.Implementation)))
                    );
            }
            workflowEngine.WorkflowBuilder = new CastleWindsorWorkflowBuilder(builder);
            return this;
        }

        public WorkflowSetup WithDefaultBuilder()
        {
            if (workflowEngine == null)
                throw new Exception("Call Configure First");
            else
                workflowEngine.WorkflowBuilder = new ReflectionWorkflowBuilder(assemblies);
            return this;
        }

        public static string GetWorkflowTypeName(Type workflowType)
        {
            var att = (WorkflowNameAttribute)workflowType.GetCustomAttribute(typeof(WorkflowNameAttribute));

            if (att != null)
                return att.Name;
            return workflowType.Name;
        }
    }
}
