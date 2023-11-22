using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AnalyzerTests.Pipeline
{
    [TestClass]
    public class TestAvoidGotoStatements
    {
        [TestMethod]
        public void TestWithGotoStatements()
        {
            // Create a list to hold parsed DLL files
            List<ParsedDLLFile> DllFileObjs = new();
            string path = "..\\..\\..\\TestDLLs\\Goto.dll";

            // Create a ParsedDLLFile object for the specified DLL
            var parsedDllObj = new ParsedDLLFile(path);
            DllFileObjs.Add(parsedDllObj);

            AvoidGotoStatementsAnalyzer avoidGotoStatements = new(DllFileObjs);

            Dictionary<string, Analyzer.AnalyzerResult> resultObj = avoidGotoStatements.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["Goto.dll"];
            Assert.AreEqual(0, result.Verdict);
        }

        [TestMethod]
        public void TestWithoutGotoStatements()
        {
            // Create a list to hold parsed DLL files
            List<ParsedDLLFile> DllFileObjs = new();
            string path = "..\\..\\..\\TestDLLs\\NoGoto.dll";

            // Create a ParsedDLLFile object for the specified DLL
            var parsedDllObj = new ParsedDLLFile(path);

            // Add the parsed DLL object to the list
            DllFileObjs.Add(parsedDllObj);
            AvoidGotoStatementsAnalyzer avoidGotoStatements = new(DllFileObjs);

            // Run the analysis
            Dictionary<string, Analyzer.AnalyzerResult> resultObj = avoidGotoStatements.AnalyzeAllDLLs();

            // Assert that the analyzer verdict is 1 (indicating no goto statements were found)
            Analyzer.AnalyzerResult result = resultObj["NoGoto.dll"];
            Assert.AreEqual(1, result.Verdict);
        }
    }
}
