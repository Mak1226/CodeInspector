using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public static class AnalyzerFactory
    {
        public static List<Tuple<int, string>> GetAllConfigurationOptions()
        {
            List<Tuple<int, string>> analyzers = new()
            {
                Tuple.Create(101, "Abstract type no public constructor"),
                Tuple.Create(102, "Avoid constructor in static types"),
                Tuple.Create(103, "Avoid unused private fields"),
                Tuple.Create(104, "Avoid empty interface")
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