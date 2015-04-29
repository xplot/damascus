//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Net;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.WindowsAzure;
//using Microsoft.WindowsAzure.Diagnostics;
//using Microsoft.WindowsAzure.ServiceRuntime;
//using Microsoft.WindowsAzure.Storage;
//using NServiceBus.Hosting.Azure;

//namespace Damascus.MessageChannel
//{
//    public class WorkerRole : RoleEntryPoint
//    {
//        private readonly NServiceBusRoleEntrypoint nsb = new NServiceBusRoleEntrypoint();

//        public override bool OnStart()
//        {
//            Trace.TraceInformation("Starting NServiceBus");
            
//            nsb.Start();
//            return base.OnStart();
//        }

//        public override void OnStop()
//        {
//            Trace.TraceInformation("NServiceBus is Stopping");
//            nsb.Start();
//            base.OnStop();
//        }
//    }
//}
