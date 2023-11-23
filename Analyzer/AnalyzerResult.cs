/******************************************************************************
* Filename    = AnalyzerResult.cs
*
* Author      = Mangesh Dalvi
* 
* Roll No     = 112001010
*
* Product     = Code Inspector
* 
* Project     = Analyzer
*
* Description = Represents the result of an analysis.
******************************************************************************/

namespace Analyzer
{
    /// <summary>
    /// Represents the result of an analysis.
    /// </summary>
    public class AnalyzerResult : IAnalyzerResult
    {
        public string AnalyserID { get; }
        public int Verdict { get; }
        public string ErrorMessage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyzerResult"/> class.
        /// </summary>
        /// <param name="analyserID">The unique identifier of the analyzer.</param>
        /// <param name="verdict">The verdict of the analysis.</param>
        /// <param name="errorMessage">The error message associated with the analysis result.</param>
        public AnalyzerResult(string analyserID, int verdict, string errorMessage)
        {
            AnalyserID = analyserID;
            Verdict = verdict;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Determines whether two <see cref="AnalyzerResult"/> instances are equal.
        /// </summary>
        /// <param name="obj1">The first object to compare.</param>
        /// <param name="obj2">The second object to compare.</param>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        public static bool operator ==(AnalyzerResult obj1, AnalyzerResult obj2)
        {
           if(obj1.Verdict == obj2.Verdict && obj1.AnalyserID == obj2.AnalyserID && obj1.ErrorMessage==obj2.ErrorMessage) 
           { 
                return true;
           }

           return false;
        }

        /// <summary>
        /// Determines whether two <see cref="AnalyzerResult"/> instances are not equal.
        /// </summary>
        /// <param name="obj1">The first object to compare.</param>
        /// <param name="obj2">The second object to compare.</param>
        /// <returns>true if the specified objects are not equal; otherwise, false.</returns>
        public static bool operator !=(AnalyzerResult obj1, AnalyzerResult obj2)
        {
            return !(obj1 == obj2);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="AnalyzerResult"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            AnalyzerResult other = (AnalyzerResult)obj;
            return AnalyserID == other.AnalyserID && Verdict == other.Verdict && ErrorMessage == other.ErrorMessage;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(AnalyserID, Verdict, ErrorMessage);
        }

        /// <summary>
        /// Returns a string representation of the current <see cref="AnalyzerResult"/> instance.
        /// </summary>
        /// <returns>A string representation of the object.</returns>
        public override string ToString()
        {
            return $"AnalyzerID: {AnalyserID}, Verdict: {Verdict}, ErrorMessage: {ErrorMessage}";
        }
    }
}
