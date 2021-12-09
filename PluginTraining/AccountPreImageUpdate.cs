using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTraining
{
    public class AccountPreImageUpdate:IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            #region Get Target and References
            // Console Logging CRM
            ITracingService tracing = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            //Getting the context and can be used to get the user that triggered, the message name, the pipeline, the stage...
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            // Get the factory in order to create the service.
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            #endregion
            //contactid that has been created.
            Entity target = (Entity)context.InputParameters["Target"];
            // get PreImage from Context

            if (context.PreEntityImages.Contains("AccountPreImage") && context.PreEntityImages["AccountPreImage"] is Entity)
            {

                Entity preMessageImage = (Entity)context.PreEntityImages["AccountPreImage"];
                // get topic field value before database update perform
                if (target.Attributes.Contains("address1_city"))
                {
                    tracing.Trace("REAL ENTITY: " + target.Attributes["address1_city"]);
                }
                if (preMessageImage.Attributes.Contains("address1_city"))
                {
                    tracing.Trace("PRE IMAGE: " + preMessageImage.Attributes["address1_city"]);
                }
                if(preMessageImage.Attributes["address1_city"] == target.Attributes["address1_city"])
                {
                    //Dont update this attribute.
                }
            }
        }
    }
}
