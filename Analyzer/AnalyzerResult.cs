using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public class AnalyzerResult : IAnalyzerResult
    {
        public string AnalyserID { get; }
        public int Verdict { get; }
        public string ErrorMessage { get; }
 
        public AnalyzerResult(string analyserID, int verdict, string errorMessage)
        {
            AnalyserID = analyserID;
            Verdict = verdict;
            ErrorMessage = errorMessage;
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            AnalyzerResult other = (AnalyzerResult)obj;
            return AnalyserID == other.AnalyserID && Verdict == other.Verdict && ErrorMessage == other.ErrorMessage;
        }
    }
}