using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Sriracha.Publisher
{
    public class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            string invokedVerb;
            object invokedVerbInstance;
            bool result = CommandLine.Parser.Default.ParseArguments(args, options,
                (verb, subOptions) =>
                {
                    invokedVerb = verb;
                    invokedVerbInstance = subOptions;
                });
            if (!result)
            {
                // Values are available here
                Console.WriteLine("No good, " + CommandLine.Text.HelpText.AutoBuild(options));
            }
            else 
            {
                Console.WriteLine("good");
            }
            Console.ReadKey();
        }
    }
}
