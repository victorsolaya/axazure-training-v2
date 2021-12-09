using PluginTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTrainingConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var api = new DadJokeAPI();
            api.GetJoke("dog");
        }
    }
}
