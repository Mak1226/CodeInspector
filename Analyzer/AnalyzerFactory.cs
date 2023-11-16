using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public static class AnalyzerFactory
    {
        public static List<Tuple<string, string>> GetAllConfigurationOptions()
        {
            List<Tuple<string, string>> analyzers = new()
            {
                Tuple.Create("101", "Abstract type no public constructor"),
                Tuple.Create("102", "Avoid constructor in static types"),
                Tuple.Create("103", "Avoid unused private fields"),
                Tuple.Create("104", "Depth of inheritance should be less than 3"),
                Tuple.Create("105", "Prefix checker I should be capital"),
                Tuple.Create("106", "Avoid unused local variables"),
                Tuple.Create("107", "Check if not implemented method is there"),
                
            };

            return analyzers;
        }

        public static IAnalyzer GetAnalyzer()
        {

            IAnalyzer Analyzer = new Analyzer();

            return Analyzer;
        }

    }
}