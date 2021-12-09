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
    public class WordCounter : CodeActivity
    {
        [Input("Description")]
        [RequiredArgument]
        public InArgument<string> Description { get; set; }

        [Output("wordcount")]
        public OutArgument<int> WordCount { get; set; }
        protected override void Execute(CodeActivityContext executionContext)
        {
            //Create the tracing service
            ITracingService tracing = executionContext.GetExtension<ITracingService>();

            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            string descriptionRef = Description.Get<string>(executionContext);
            int wordcount = GetWordCount(descriptionRef);
            WordCount.Set(executionContext, wordcount);

        }

        private int GetWordCount(string description)
        {
            int wordCount = 0;
            wordCount = description.Split(' ').Length;
            return wordCount;
        }
    }
}
