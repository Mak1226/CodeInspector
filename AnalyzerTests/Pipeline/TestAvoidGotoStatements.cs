/******************************************************************************
* Filename    = TestAvoidGotoStatements.cs
* 
* Author      = Thanmayee
* 
* Project     = AnalyzerTests
*
* Description = Test class to verify the functionality of the AvoidGotoStatementsAnalyzer
*****************************************************************************/

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
        public void TestWithGotoStatements1()
        {
            // Create a list to hold parsed DLL files
            List<ParsedDLLFile> DllFileObjs = new();
            string path = "..\\..\\..\\TestDLLs\\Goto1.dll";

            // Create a ParsedDLLFile object for the specified DLL
            var parsedDllObj = new ParsedDLLFile(path);
            DllFileObjs.Add(parsedDllObj);

            AvoidGotoStatementsAnalyzer avoidGotoStatements = new(DllFileObjs);

            Dictionary<string, Analyzer.AnalyzerResult> resultObj = avoidGotoStatements.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["Goto1.dll"];
            Assert.AreEqual(0, result.Verdict);
        }


        [TestMethod]
        public void TestWithoutGotoStatements1()
        {
            // Create a list to hold parsed DLL files
            List<ParsedDLLFile> DllFileObjs = new();
            string path = "..\\..\\..\\TestDLLs\\Goto1.dll";

            // Create a ParsedDLLFile object for the specified DLL
            var parsedDllObj = new ParsedDLLFile(path);
            DllFileObjs.Add(parsedDllObj);

            AvoidGotoStatementsAnalyzer avoidGotoStatements = new(DllFileObjs);

            Dictionary<string, Analyzer.AnalyzerResult> resultObj = avoidGotoStatements.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["Goto1.dll"];
            Assert.AreNotEqual(1, result.Verdict);
        }
    }
}
