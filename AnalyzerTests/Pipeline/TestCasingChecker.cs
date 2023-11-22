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
    public class TestCasingChecker
    {
        /// <summary>
        /// Test method for a case in which classes don't follow the above mentioned rule.
        /// </summary>
        [TestMethod()]
        public void TestBadExample()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\CasingChecker.dll";
            var parsedDllObj = new ParsedDLLFile(path);

            DllFileObjs.Add(parsedDllObj);

            CasingChecker casingChecker = new(DllFileObjs);

            Dictionary<string, Analyzer.AnalyzerResult> resultObj = casingChecker.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["CasingChecker.dll"];
            Assert.AreEqual(0, result.Verdict);
        }

    }
}

