using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessFunc
{
    public class AnalyzerResult 
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
    }
}
