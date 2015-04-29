using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Damascus.Core
{
    public class ReflectionWorkflowBuilder:IWorkflowBuilder
    {
        private Assembly[] assemblies;

        public ReflectionWorkflowBuilder(Assembly[] assemblies)
        {
            this.assemblies = assemblies;
        }

        public IWorkflow BuilWorkflow(string key)
        {
            var type = GetTypeFromAssemblies(key);
            return (IWorkflow)Activator.CreateInstance(type, true);
        }

        private Type GetTypeFromAssemblies(string type)
        {
            return (from assembly in assemblies
                from assemblyType in assembly.GetTypes()
                where assemblyType.Name == type
                select assemblyType).SingleOrDefault();


        }
    }
}
