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
            List<string> DllFilePaths = new List<string>();

            DllFilePaths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\ACIST.dll");

            ParsedDLLFiles dllFiles = new(DllFilePaths);

            AvoidConstructorsInStaticTypes avoidConstructorInStaticTypes = new(dllFiles);

            var resultObj = avoidConstructorInStaticTypes.Run();

            var result = resultObj.Verdict;
            Assert.AreEqual(1, result);
        }

        /// <summary>
        /// Test method for a case in which classes don't follow the above mentioned rule.
        /// </summary>
        [TestMethod()]
        public void TestBadExample() 
        {
            List<string> DllFilePaths = new List<string>();

            DllFilePaths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\ACIST1.dll");

            ParsedDLLFiles dllFiles = new(DllFilePaths);

            AvoidConstructorsInStaticTypes avoidConstructorInStaticTypes = new(dllFiles);

            var resultObj = avoidConstructorInStaticTypes.Run();

            var result = resultObj.Verdict;
            Assert.AreEqual(0, result);
        }
        
    }
}
