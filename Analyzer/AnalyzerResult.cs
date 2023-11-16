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

        public static bool operator ==(AnalyzerResult obj1, AnalyzerResult obj2)
        {
           if(obj1.Verdict == obj2.Verdict && obj1.AnalyserID == obj2.AnalyserID && obj1.ErrorMessage==obj2.ErrorMessage) 
           { 
                return true;
           }

           return false;
        }

        public static bool operator !=(AnalyzerResult obj1, AnalyzerResult obj2)
        {
            return !(obj1 == obj2);
        }
    }
}