using Analyzer.Parsing;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// A base class providing a common structure for various analyzers.
    /// </summary>
    public abstract class AnalyzerBase
    {
        public List<ParsedDLLFile> parsedDLLFilesList { get; }
        private Dictionary<string, AnalyzerResult> _result;

        /// <summary>
        /// Initializes a new instance of the Base Analyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files for analysis.</param>
        public AnalyzerBase(List<ParsedDLLFile> dllFiles)
        {
            // Set the parsedDLLFiles field with the provided DLL files
            parsedDLLFilesList = dllFiles;
            _result = new Dictionary<string , AnalyzerResult>();
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
                _result[parsedDLL.DLLFileName] = AnalyzeSingleDLL(parsedDLL);
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