/******************************************************************************
* Filename    = TestAvoidGotoStatements.cs
* 
* Author      = Thanmayee
* 
* Project     = AnalyzerTests
*
* Description = Test class to verify the functionality of the AvoidGotoStatementsAnalyzer
*****************************************************************************/

using System.Reflection;
using Analyzer;
using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnalyzerTests.Pipeline
{
    [TestClass]
    public class TestAvoidGotoStatements
    {
        [TestMethod]
        public void TestWithGotoStatements()
        {
            string dllFile = Assembly.GetExecutingAssembly().Location;
            ParsedDLLFile parsedDLL = new( dllFile );

            List<ParsedDLLFile> parseddllFiles = new() { parsedDLL };

            AvoidGotoStatementsAnalyzer avoidGotoStatements = new( parseddllFiles );

            Dictionary<string , AnalyzerResult> resultObj = avoidGotoStatements.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["AnalyzerTests.dll"];
            Assert.AreEqual( 0 , result.Verdict );
        }

        [TestMethod]
        public void TestWithGotoStatements1()
        {
            string dllFile = Assembly.GetExecutingAssembly().Location;
            ParsedDLLFile parsedDLL = new( dllFile );

            List<ParsedDLLFile> parseddllFiles = new() { parsedDLL };

            AvoidGotoStatementsAnalyzer avoidGotoStatements = new( parseddllFiles );

            Dictionary<string , AnalyzerResult> resultObj = avoidGotoStatements.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["AnalyzerTests.dll"];
            Assert.AreEqual( 0 , result.Verdict );
        }


        [TestMethod]
        public void TestWithoutGotoStatements1()
        {
            string dllFile = Assembly.GetExecutingAssembly().Location;
            ParsedDLLFile parsedDLL = new( dllFile );

            List<ParsedDLLFile> parseddllFiles = new() { parsedDLL };

            AvoidGotoStatementsAnalyzer avoidGotoStatements = new( parseddllFiles );

            Dictionary<string , AnalyzerResult> resultObj = avoidGotoStatements.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["AnalyzerTests.dll"];
            Assert.AreNotEqual( 1 , result.Verdict );
        }
    }
}
