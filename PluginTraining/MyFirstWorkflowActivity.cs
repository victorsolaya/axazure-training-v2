using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTraining
{
    public class MyFirstWorkflowActivity : CodeActivity
    {
        [Input("account")]
        [ReferenceTarget("account")]
        [RequiredArgument]
        public InArgument<EntityReference> Account { get; set; }
        protected override void Execute(CodeActivityContext executionContext)
        {
            //Create the tracing service
            ITracingService tracing = executionContext.GetExtension<ITracingService>();

            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            EntityReference account = Account.Get<EntityReference>(executionContext);
            var newContact = new Entity("contact");
            newContact.Attributes["parentcustomerid"] = account;
            newContact.Attributes["firstname"] = "Workflow";
            newContact.Attributes["lastname"] = "Activity";
            try
            {
                service.Create(newContact);

            }
            catch (Exception ex)
            {
                string message = $"There was a problem creating the contact";
                throw new InvalidPluginExecutionException(message);
            }
        }
    }
}
