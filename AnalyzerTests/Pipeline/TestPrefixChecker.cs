using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AnalyzerTests.Pipeline
{
    /// <summary>
    /// Test class for testing the analyzer - PrefixChecker.
    /// </summary>
    [TestClass()]
    public class TestPrefixChecker
    {
        /// <summary>
        /// Test method for a case in which all classes follow the rule 
        /// </summary>
        [TestMethod()]
        public void TestGoodExample()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\Prefix1.dll";
            var parsedDllObj = new ParsedDLLFile( path );

            DllFileObjs.Add( parsedDllObj );

            PrefixCheckerAnalyzer prefixChecker = new( DllFileObjs );

            Dictionary<string , Analyzer.AnalyzerResult> resultObj = prefixChecker.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["Prefix1.dll"];
            Assert.AreEqual( 1 , result.Verdict );
        }

        /// <summary>
        /// Test method for a case in which classes don't follow the above mentioned rule.
        /// </summary>
        [TestMethod()]
        public void TestBadExample()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\Prefix.dll";
            var parsedDllObj = new ParsedDLLFile( path );

            DllFileObjs.Add( parsedDllObj );

            PrefixCheckerAnalyzer prefixChecker = new( DllFileObjs );

            Dictionary<string , Analyzer.AnalyzerResult> resultObj = prefixChecker.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["Prefix.dll"];
            Assert.AreEqual( 0 , result.Verdict );
        }

    }
}
