/******************************************************************************
* Filename    = AnalyzerResult.cs
*
* Author      = Nideesh N
*
* Product     = Analyzer
* 
* Project     = Cloud
*
* Description = Contains the structure of result given by the analyzer module
*****************************************************************************/

namespace ServerlessFunc
{
    /// <summary>
    /// Represents the result of an analysis performed by an analyzer.
    /// </summary>
    public class AnalyzerResult
    {
        /// <summary>
        /// Gets the unique identifier of the analyzer.
        /// </summary>
        public string AnalyserID { get; }

        /// <summary>
        /// Gets the verdict of the analysis. It represents the result or status of the analysis.
        /// </summary>
        public int Verdict { get; }

        /// <summary>
        /// Gets the error message associated with the analysis result. This is used to provide additional
        /// information in case of an error during the analysis.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Initializes a new instance of the AnalyzerResult class with the specified values.
        /// </summary>
        /// <param name="analyserID">The unique identifier of the analyzer.</param>
        /// <param name="verdict">The verdict of the analysis.</param>
        /// <param name="errorMessage">The error message associated with the analysis result.</param>
        public AnalyzerResult( string analyserID , int verdict , string errorMessage )
        {
            AnalyserID = analyserID;
            Verdict = verdict;
            ErrorMessage = errorMessage;
        }
    }
}
