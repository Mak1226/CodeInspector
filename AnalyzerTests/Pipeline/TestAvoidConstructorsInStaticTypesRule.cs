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
    /// Test class for testing the analyzer- AvoidConstructorsInStaticTypesRule.
    /// </summary>
    [TestClass()]
    public class TestAvoidConstructorsInStaticTypesRule
    {
        /// <summary>
        /// Test method for a case in which all classes follow the rule of not having visible non-static constructor 
        /// for classes having only static methods and fields.
        /// </summary>
        [TestMethod()]
        public void TestGoodExample()
        {
            List<ParsedDLLFile> DllFileObjs = new List<ParsedDLLFile>();

            var path = "..\\..\\..\\..\\Analyzer\\TestDLLs\\ACIST.dll";
            var parsedDllObj = new ParsedDLLFile(path);

            DllFileObjs.Add(parsedDllObj); 

            AvoidConstructorsInStaticTypes avoidConstructorInStaticTypes = new(DllFileObjs);

            var resultObj = avoidConstructorInStaticTypes.AnalyzeAllDLLs();

            var result = resultObj["ACIST.dll"];
            Assert.AreEqual(1, result.Verdict);
        }

        /// <summary>
        /// Test method for a case in which classes don't follow the above mentioned rule.
        /// </summary>
        [TestMethod()]
        public void TestBadExample()
        {
            List<ParsedDLLFile> DllFileObjs = new List<ParsedDLLFile>();

            var path = "..\\..\\..\\..\\Analyzer\\TestDLLs\\ACIST1.dll";
            var parsedDllObj = new ParsedDLLFile(path);

            DllFileObjs.Add(parsedDllObj);

            AvoidConstructorsInStaticTypes avoidConstructorInStaticTypes = new(DllFileObjs);

            var resultObj = avoidConstructorInStaticTypes.AnalyzeAllDLLs();

            var result = resultObj["ACIST1.dll"];
            Assert.AreEqual(0, result.Verdict);
        }

    }
}
