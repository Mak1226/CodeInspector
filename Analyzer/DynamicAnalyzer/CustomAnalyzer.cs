using Analyzer.Parsing;
using Analyzer.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.DynamicAnalyzer
{
    public class CustomAnalyzer : AnalyzerBase
    {
        private string _errorMessage;
        private int _verdict;
        private readonly string _analyzerID;

        public CustomAnalyzer(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            _errorMessage = "This is an error message";
            _verdict = 0;
            _analyzerID = "This is an analyzer ID";

        }

        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile _parsedDLLFile)
        {

            // Write your analyzer here

            return new AnalyzerResult(_analyzerID, _verdict, _errorMessage);
        }

    }
}
