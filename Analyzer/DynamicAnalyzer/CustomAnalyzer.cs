using Analyzer.Parsing;
using Analyzer.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.DynamicAnalyzer
{
    /// <summary>
    /// Represents a custom analyzer for extending analysis logic, it inherits from AnalyzerBase.
    /// Teachers can implement specific analysis logic using this class.
    /// </summary>
    public class CustomAnalyzer : AnalyzerBase
    {
        private string _errorMessage;
        private int _verdict;
        private readonly string _analyzerID;

        /// <summary>
        /// Initializes a new instance of the CustomAnalyzer class.
        /// </summary>
        /// <param name="dllFiles">A list of ParsedDLLFile objects to analyze.</param>
        public CustomAnalyzer(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            _errorMessage = "This is an error message";
            _verdict = 0;
            _analyzerID = "This is an analyzer ID";

        }

        /// <summary>
        /// Analyzes each of the ParsedDLLFile separately.
        /// </summary>
        /// <param name="_parsedDLLFile">The ParsedDLLFile to analyze.</param>
        /// <returns>An AnalyzerResult object containing the analysis result.</returns>

        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile _parsedDLLFile)
        {
            // Write your analyzer logic here

            return new AnalyzerResult(_analyzerID, _verdict, _errorMessage);
        }

    }
}
