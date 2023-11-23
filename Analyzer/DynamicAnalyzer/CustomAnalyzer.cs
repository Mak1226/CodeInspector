/******************************************************************************
* Filename    = CustomAnalyzer.cs
* 
* Author      = Yukta Salunkhe, Mangesh Dalvi
* 
* Project     = Analyzer
*
* Description = Represents a custom analyzer for extending analysis logic, it inherits from AnalyzerBase. Teachers can implement specific analysis logic using this class.
*****************************************************************************/

/*using Analyzer.Parsing;
using Analyzer.Pipeline;

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
            _analyzerID = "This is an analyzer ID"; // update analyzer ID here

        }

        /// <summary>
        /// Analyzes each of the ParsedDLLFile separately.
        /// </summary>
        /// <param name="_parsedDLLFile">The ParsedDLLFile to analyze.</param>
        /// <returns>An AnalyzerResult object containing the analysis result.</returns>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile _parsedDLLFile)
        {
            // Write your analyzer logic here

            _errorMessage = "Add error msg inside this";
            _verdict = 0; // update final verdict here

            return new AnalyzerResult(_analyzerID, _verdict, _errorMessage);
        }

    }
}
*/
