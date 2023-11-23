/******************************************************************************
* Filename    = IAnalyzerResult.cs
*
* Author      = Mangesh Dalvi
* 
* Roll No     = 112001010
*
* Product     = Code Inspector
* 
* Project     = Analyzer
*
* Description = Represents the result of an analysis performed by an analyzer.
******************************************************************************/

namespace Analyzer
{
    /// <summary>
    /// Represents the result of an analysis performed by an analyzer.
    /// </summary>
    public interface IAnalyzerResult
    {
        /// <summary>
        /// Gets the unique identifier of the analyzer.
        /// </summary>
        string AnalyserID { get; }

        /// <summary>
        /// Gets the verdict of the analysis. It indicates the result of the analysis.
        /// </summary>
        int Verdict { get; }

        /// <summary>
        /// Gets the error message associated with the analysis result, if any.
        /// </summary>
        string ErrorMessage { get; }
    }
}
