using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTraining
{
    public class PhonenumberChange : IPlugin
    {
//        protected override void Execute(CodeActivityContext executionContext)
        
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
            UpdateAccountCases(service, tracing, target);
            UpdateContactsCases(service, tracing, target);

        }

        private void UpdateContactsCases(IOrganizationService service, ITracingService tracing, Entity target)
        {
            string fetchxml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
  <entity name='incident'>
    <attribute name='title' />
    <attribute name='ticketnumber' />
    <attribute name='createdon' />
    <attribute name='incidentid' />
    <attribute name='vso_phonenumber' />
    <order attribute='title' descending='false' />
    <link-entity name='contact' from='contactid' to='primarycontactid' link-type='inner' alias='ab'>
      <filter type='and'>
        <condition attribute='parentcustomerid' operator='eq' value='{target.Id}'/>
      </filter>
    </link-entity>
  </entity>
</fetch>";

            EntityCollection incidents = new EntityCollection();
            try
            {
                incidents = service.RetrieveMultiple(new FetchExpression(fetchxml));
            }
            catch (Exception ex)
            {
                string message = $"There was a message retrieving incidents";
                throw new InvalidPluginExecutionException(message);
            }

            foreach (var incident in incidents.Entities)
            {
                incident.Attributes["vso_phonenumber"] = target.Attributes["mobilephone"];
                service.Update(incident);
            }
        }


        private void UpdateAccountCases(IOrganizationService service, ITracingService tracing, Entity target)
        {
            string fetchxml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
  <entity name='incident'>
    <attribute name='title' />
    <attribute name='ticketnumber' />
    <attribute name='createdon' />
    <attribute name='incidentid' />
    <attribute name='caseorigincode' />
    <order attribute='title' descending='false' />
    <filter type='and'>
      <condition attribute='customerid' operator='eq' value='{target.Id}' />
    </filter>
  </entity>
</fetch>";

            EntityCollection incidents = new EntityCollection();
            try
            {
                incidents = service.RetrieveMultiple(new FetchExpression(fetchxml));
            }
            catch (Exception ex)
            {
                string message = $"There was a message retrieving incidents";
                throw new InvalidPluginExecutionException(message);
            }

            foreach (var incident in incidents.Entities)
            {
                incident.Attributes["vso_phonenumber"] = target.Attributes["mobilephone"];
                service.Update(incident);
            }
        }
    }
}
