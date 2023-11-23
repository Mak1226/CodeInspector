/******************************************************************************
* Filename    = TestAvoidSwitchStatements.cs
* 
* Author      = Thanmayee
* 
* Project     = Analyzer
*
* Description = Test class to verify the functionality of the AvoidSwitchStatementsAnalyzer
*****************************************************************************/
 
using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnalyzerTests.Pipeline
{
    [TestClass]
    public class TestAvoidSwitchStatements
    {
        [TestMethod()]
        public void TestNoSwitch()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\Rules.dll";
            var parsedDllObj = new ParsedDLLFile(path);

            DllFileObjs.Add(parsedDllObj);

            AvoidSwitchStatementsAnalyzer avoidSwitchStatements = new(DllFileObjs);

            Dictionary<string, Analyzer.AnalyzerResult> resultObj = avoidSwitchStatements.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["Rules.dll"];
            Assert.AreEqual(1, result.Verdict);
        }

        [TestMethod()]
        public void TestNoSwitch1()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\NoSwitch.dll";
            var parsedDllObj = new ParsedDLLFile(path);

            DllFileObjs.Add(parsedDllObj);

            AvoidSwitchStatementsAnalyzer avoidSwitchStatements = new(DllFileObjs);

            Dictionary<string, Analyzer.AnalyzerResult> resultObj = avoidSwitchStatements.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["NoSwitch.dll"];
            Assert.AreEqual(1, result.Verdict);
        }

        [TestMethod()]
        public void TestSwitch()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\Rules1.dll";
            var parsedDllObj = new ParsedDLLFile(path);

            DllFileObjs.Add(parsedDllObj);

            AvoidSwitchStatementsAnalyzer avoidSwitchStatements = new(DllFileObjs);

            Dictionary<string, Analyzer.AnalyzerResult> resultObj = avoidSwitchStatements.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["Rules1.dll"];
            Assert.AreEqual(0, result.Verdict);
        }

        [TestMethod()]
        public void TestSwitch1()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\Switch.dll";
            var parsedDllObj = new ParsedDLLFile(path);

            DllFileObjs.Add(parsedDllObj);

            AvoidSwitchStatementsAnalyzer avoidSwitchStatements = new(DllFileObjs);

            Dictionary<string, Analyzer.AnalyzerResult> resultObj = avoidSwitchStatements.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["Switch.dll"];
            Assert.AreEqual(0, result.Verdict);
        }
    }
}
