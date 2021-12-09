using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PluginTraining
{
    public class DadJokeAPI: IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            if (context.MessageName.Equals("vso_dadjoke") && context.Stage.Equals(30))
            {
                string searchterm = (string)context.InputParameters["vso_searchterm"];
                var joke = GetJoke(searchterm);
                context.OutputParameters["vso_joke"] = joke;
            }
        }

        public string GetJoke(string searchTerm)
        {
            string URL = "https://icanhazdadjoke.com/search";
            string parameterSearch = $"?term={searchTerm}";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);


            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(parameterSearch).Result;
            if(response.IsSuccessStatusCode)
            {
                var jokesObject = response.Content.ReadAsStringAsync().Result;
                DadJokeResponse jokes = DeserializeJSON.DeserializeJSONResponse(jokesObject);
                var totalJokes = jokes.total_jokes;
                var randomNumber = new Random();
                var getNumberRandomly = randomNumber.Next(0, totalJokes);
                var jokeSelected = jokes.results[getNumberRandomly].joke;
                return jokeSelected;
                // Deserialize JSON
            }
            return string.Empty;
        }
    }
}
