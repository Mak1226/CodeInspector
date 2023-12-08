using Analyzer;
using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnalyzerTests.Pipeline
{
    [TestClass]
    public class TestSwitchStatementDefaultCaseChecker
    {
        [TestMethod]
        public void Test1()
        {
            string path = "..\\..\\..\\TestDLLs\\BasicSwitchCase.dll";

            ParsedDLLFile dllFile = new(path);
            List<ParsedDLLFile> dllFiles = new() { dllFile };
            SwitchStatementDefaultCaseChecker switchStatementDefaultCaseChecker = new(dllFiles);
            Dictionary<string, AnalyzerResult> result = switchStatementDefaultCaseChecker.AnalyzeAllDLLs();

            Dictionary<string, AnalyzerResult> original = new()
            {
                ["116"] = new AnalyzerResult("116", 1, "No violation found" )
            };
            foreach (KeyValuePair<string, AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Console.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
            }

            Assert.AreEqual(original.ToString(), result.ToString());

        }
      
        [TestMethod]
        public void Test2()
        {
            string path = "..\\..\\..\\TestDLLs\\SwitchStatementwithoutDC.dll";

            ParsedDLLFile dllFile = new(path);
            List<ParsedDLLFile> dllFiles = new() { dllFile };
            SwitchStatementDefaultCaseChecker switchStatementDefaultCaseChecker = new(dllFiles);
            Dictionary<string, AnalyzerResult> result = switchStatementDefaultCaseChecker.AnalyzeAllDLLs();

            Dictionary<string, AnalyzerResult> original = new()
            {
                ["116"] = new AnalyzerResult("116", 0, "BasicSwitchCase.temp")
            };

            Assert.AreEqual(original.ToString(), result.ToString());

        }

    }
}
