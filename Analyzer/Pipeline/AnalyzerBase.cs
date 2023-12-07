/******************************************************************************
* Filename    = AnalyzerBase.cs
*
* Author      = Mangesh Dalvi
* 
* Roll No     = 112001010
*
* Product     = Code Inspector
* 
* Project     = Analyzer
*
* Description = A base class providing a common structure for various analyzers.
******************************************************************************/

using Analyzer.Parsing;
using Logging;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// A base class providing a common structure for various analyzers.
    /// </summary>
    public abstract class AnalyzerBase
    {
        public List<ParsedDLLFile> parsedDLLFilesList { get; }
        private Dictionary<string, AnalyzerResult> _result;
        protected string analyzerID;

        /// <summary>
        /// Initializes a new instance of the Base Analyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files for analysis.</param>
        public AnalyzerBase(List<ParsedDLLFile> dllFiles)
        {
            // Set the parsedDLLFiles field with the provided DLL files
            parsedDLLFilesList = dllFiles;
            _result = new Dictionary<string , AnalyzerResult>();
            analyzerID = "";
        }

        /// <summary>
        /// Analyzes all parsed DLL files and returns the results in a dictionary.
        /// </summary>
        /// <returns>Dictionary containing analysis results for each DLL file.</returns>
        public Dictionary<string, AnalyzerResult> AnalyzeAllDLLs()
        {
            _result = new Dictionary<string , AnalyzerResult>();

            foreach (ParsedDLLFile parsedDLL in  parsedDLLFilesList)
            {
                try
                {
                    _result[parsedDLL.DLLFileName] = AnalyzeSingleDLL( parsedDLL );
                }
                catch (Exception ex)
                {
                    Trace.Write($"[Analyzer {analyzerID}] : Analyzing {parsedDLL.DLLFileName} caused an exception {ex.GetType().Name} : {ex}\n");

                    string errorMsg = "Internal error, analyzer failed to execute";
                    _result[parsedDLL.DLLFileName] = new AnalyzerResult(analyzerID, 0, errorMsg);
                }
            }


            return _result;
        }

        /// <summary>
        /// Performs analysis on a single parsed DLL file and returns the result.
        /// </summary>
        /// <param name="_parsedDLLFile">The parsed DLL file to analyze.</param>
        /// <returns>Analysis result for the specified DLL file.</returns>
        protected abstract AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile _parsedDLLFile);
    }
}
