using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;

namespace Damascus.Core
{
    public class CastleWindsorWorkflowBuilder:IWorkflowBuilder
    {
        private IWindsorContainer container;

        public CastleWindsorWorkflowBuilder(IWindsorContainer container)
        {
            this.container = container;
        }

        public IWorkflow BuilWorkflow(string type)
        {
            return container.Resolve<IWorkflow>(type);
        }
    }
}
