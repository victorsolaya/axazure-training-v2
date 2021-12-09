using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTraining
{
    public class AccountPlugin : IPlugin
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

            /*
                Plugin for the account.
                Every time I change the city of the account, all the contacts related with my account are going to be updated with that city
            */
            UpdateAllTheContactsFromAccount(service, tracing, target);
        }

        private void UpdateAllTheContactsFromAccount(IOrganizationService service, ITracingService tracing, Entity account)
        {
            //RetrieveMultiple all the contacts with this account
            string query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='contact'>
                                <attribute name='fullname' />
                                <attribute name='address1_city' />
                                <attribute name='contactid' />
                                <order attribute='fullname' descending='false' />
                                <filter type='and'>
                                  <condition attribute='parentcustomerid' operator='eq' value='{account.Id}' />
                                </filter>
                              </entity>
                            </fetch>";
            EntityCollection allTheContacts = new EntityCollection();
            try
            {
                allTheContacts = service.RetrieveMultiple(new FetchExpression(query));
            }
            catch (Exception ex)
            {
                string message = $"There was a problem retriving the contacts because {ex.Message}";
                tracing.Trace(message);
                throw new InvalidPluginExecutionException(message);
            }

            foreach(var contactToUpdate in allTheContacts.Entities)
            {
                // Update all the contacts with the new city
                contactToUpdate.Attributes["address1_city"] = (string)account.Attributes["address1_city"];//Account address city
                try
                {
                    service.Update(contactToUpdate);
                }
                catch (Exception ex)
                {
                    string message = $"There was a problem updating the contact {contactToUpdate.Id} because {ex.Message}";
                    tracing.Trace(message);
                    throw new InvalidPluginExecutionException(message);
                }
            }

        }
    }
}
