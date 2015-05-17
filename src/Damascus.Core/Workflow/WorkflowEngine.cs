using System;
using System.Collections.Generic;
using Castle.Windsor;
using Damascus.Core.Exceptions;
using Damascus.Message;

namespace Damascus.Core
{
    public class WorkflowEngine
    {
        public IDataSerializer Serializer { get; set; }
        public IWorkflowBuilder WorkflowBuilder { get; set; }
        public Dictionary<string,Workflow> Workflows { get; private set; }

        //todo: Create a default constructor with a default serializer
        public WorkflowEngine(IDataSerializer serializer)
        {
            Serializer = serializer;
            Workflows = new Dictionary<string, Workflow>();
        }
        
        public string Process(WorkflowContext workflowContext)
        {
            var workflow = WorkflowBuilder.BuilWorkflow(workflowContext.WorkflowType);
            var workflowData = (!string.IsNullOrEmpty(workflowContext.DataKey)) ?
                Serializer.DeserializeData(GetDataKey(workflowContext.WorkflowType, workflowContext.DataKey)): 
                new DataStorage();

            if (workflowData != null)
            {
                workflow.Data = (DataStorage) workflowData;
                workflow.Id = workflowData["id"];
            }
            else
            {
                workflow.Id = workflowContext.WorkflowId;
            }

            var result = workflow.Execute(workflowContext.WorkflowStep, workflowContext.Parameters);

            if(!string.IsNullOrEmpty(workflowContext.DataKey))
                Serializer.SerializeData(GetDataKey(workflowContext.WorkflowType, workflowContext.DataKey), workflow.Data);
            return result;
        }

        public static string GetDataKey(string workflowType, string key)
        {
            return workflowType + "_" + key;
        }
    }

    public class WorkflowContext
    {
        public string WorkflowId { get; set; }
        public string DataKey { get; set; }
        public string WorkflowType { get; set; }
        public string WorkflowStep { get; set; }
        public IStepInput Parameters { get; set; }
    }
}
