using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public interface IAnalyzerResult
    {
        public string AnalyserID { get; }
        public int Verdict { get; }
        public string ErrorMessage { get; }
    }
}