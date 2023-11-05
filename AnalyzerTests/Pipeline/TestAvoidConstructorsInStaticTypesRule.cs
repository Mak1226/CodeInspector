/*using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AnalyzerTests.Pipeline
{
    [TestClass()]
    public class TestAvoidConstructorsInStaticTypesRule
    {
        [TestMethod()]
        public void TestGoodExample()
        {
            List<string> DllFilePaths = new List<string>();

            DllFilePaths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\ACIST.dll");

            ParsedDLLFiles dllFiles = new(DllFilePaths);

            AvoidConstructorsInStaticTypes avoidConstructorInStaticTypes = new(dllFiles);

            var result = avoidConstructorInStaticTypes.GetScore();

            Assert.AreEqual(1, result);
        }
        
        [TestMethod()]
        public void TestBadExample() 
        {
            List<string> DllFilePaths = new List<string>();

            DllFilePaths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\ACIST1.dll");

            ParsedDLLFiles dllFiles = new(DllFilePaths);

            AvoidConstructorsInStaticTypes avoidConstructorInStaticTypes = new(dllFiles);

            var result = avoidConstructorInStaticTypes.GetScore();

            Assert.AreEqual(0, result);
        }
        
    }
}
*/