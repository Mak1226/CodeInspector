/******************************************************************************
* Filename    = TestAvoidSwitchStatements.cs
* 
* Author      = Thanmayee
* 
* Project     = AnalyzerTests
*
* Description = Test class to verify the functionality of the AvoidSwitchStatementsAnalyzer
*****************************************************************************/

using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnalyzerTests.Pipeline
{
    [TestClass]
    public class TestAvoidSwitchStatements
    {
        /*[TestMethod()]
        public void TestNoSwitch()
        {
            string dllFile = Assembly.GetExecutingAssembly().Location;
            ParsedDLLFile parsedDLL = new( dllFile );

            List<ParsedDLLFile> parseddllFiles = new() { parsedDLL };

            AvoidSwitchStatementsAnalyzer avoidSwitchStatements = new( parseddllFiles );

            Dictionary<string , AnalyzerResult> resultObj = avoidSwitchStatements.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["AnalyzerTests.dll"];
            Assert.AreEqual(1, result.Verdict);
        }

        [TestMethod()]
        public void TestNoSwitch1()
        {
            string dllFile = Assembly.GetExecutingAssembly().Location;
            ParsedDLLFile parsedDLL = new( dllFile );

            List<ParsedDLLFile> parseddllFiles = new() { parsedDLL };

            AvoidSwitchStatementsAnalyzer avoidSwitchStatements = new( parseddllFiles );

            Dictionary<string , AnalyzerResult> resultObj = avoidSwitchStatements.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["AnalyzerTests.dll"];
            Assert.AreEqual(1, result.Verdict);
        }*/

        [TestMethod()]
        public void TestSwitch()
        {
            string dllFile = Assembly.GetExecutingAssembly().Location;
            ParsedDLLFile parsedDLL = new( dllFile );

            List<ParsedDLLFile> parseddllFiles = new() { parsedDLL };

            AvoidSwitchStatementsAnalyzer avoidSwitchStatements = new( parseddllFiles );

            Dictionary<string , AnalyzerResult> resultObj = avoidSwitchStatements.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["AnalyzerTests.dll"];
            Assert.AreEqual(0, result.Verdict);
        }

        [TestMethod()]
        public void TestSwitch1()
        {
            string dllFile = Assembly.GetExecutingAssembly().Location;
            ParsedDLLFile parsedDLL = new( dllFile );

            List<ParsedDLLFile> parseddllFiles = new() { parsedDLL };

            AvoidSwitchStatementsAnalyzer avoidSwitchStatements = new( parseddllFiles );

            Dictionary<string , AnalyzerResult> resultObj = avoidSwitchStatements.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["AnalyzerTests.dll"];
            Assert.AreEqual(0, result.Verdict);
        }
    }
}
