using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Parsing;

namespace Analyzer.Pipeline.Tests
{
    [TestClass()]
    public class TestDepth
    {
        [TestMethod()]
        public void TestDepthOfInh()
        {
            // Specify the path to the DLL file
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\depthofinh.dll";

            ParsedDLLFile dllFile = new(path);

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            DepthOfInheritance analyzer = new(dllFiles);

            // Run the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            Dictionary<Type , int> depthMap = analyzer.CalculateDepthOfInheritance( dllFile );

            Assert.IsNotNull( depthMap, "DepthMap is NULL!");

            foreach (KeyValuePair<Type , int> pair in depthMap)
            {
                Console.WriteLine( $"Key: {pair.Key}, Value: {pair.Value}" );
            }

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                Console.WriteLine(dll.Key);

                AnalyzerResult res = dll.Value;

                Assert.AreEqual(res.Verdict, 1 );

                Console.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
            }

        }
    }
}
