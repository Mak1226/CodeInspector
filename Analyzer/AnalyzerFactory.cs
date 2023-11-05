using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public class AnalyzerFactory
    {
        public static List<Tuple<string, string>> GetAllConfigurationOptions()
        {
            List<Tuple<string, string>> analyzers = new()
            {
                Tuple.Create("A1", "Description of Analyzer 1"),
                Tuple.Create("A2", "Description of Analyzer 2"),
                Tuple.Create("A3", "Description of Analyzer 3"),
                Tuple.Create("A4", "Description of Analyzer 4"),
                Tuple.Create("A5", "Description of Analyzer 5"),
                Tuple.Create("A6", "Description of Analyzer 6"),
                Tuple.Create("A7", "Description of Analyzer 7"),
            };

            return analyzers;
        }

        public static IAnalyzer GetAnalyser()
        {

            IAnalyzer Analyzer = new Analyzer();

            return Analyzer;
        }

    }
}