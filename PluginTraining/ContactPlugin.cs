using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTraining
{
    public class ContactPlugin : IPlugin
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
            CreateACase(service, tracing, target);
        }

        private void CreateACase(IOrganizationService service, ITracingService tracing, Entity contact)
        {
            Entity crmCase = new Entity("incident");
            // new_facility
            tracing.Trace("We have created the entity as empty");

            crmCase.Attributes["title"] = "My case created from a plugin";
            crmCase.Attributes["customerid"] = new EntityReference(contact.LogicalName, contact.Id);
            // Lookup = EntityReference
            tracing.Trace("We have added attributes to my entity");
            try
            {
                service.Create(crmCase);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("Error in creating case - " + ex.Message);
            }
            tracing.Trace("We have added created the entity");

        }
    }
}
