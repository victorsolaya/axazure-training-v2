using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTraining
{
    public class CasePreOpWordCounter : IPlugin
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
            string description = string.Empty;
            if (!target.Attributes.Contains("description"))
            {
                return;
            }
            
            description = (string)target.Attributes["description"];
            var counter = GetWordCount(description);
            target.Attributes["vso_descriptionwordcounter"] = counter;
        }

        private int GetWordCount(string description)
        {
            int wordCount = 0;
            wordCount = description.Split(' ').Length;
            return wordCount;
        }
    }
}
