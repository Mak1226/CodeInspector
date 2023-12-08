/******************************************************************************
 * Filename    = TestDepthOfInheritance.cs
 * 
 * Author      = Arun Sankar
 *
 * Product     = Analyzer
 * 
 * Project     = AnalyzerTests
 *
 * Description = Unit Tests for TestDepthOfInheritance class
 *****************************************************************************/

using System.Diagnostics;
using System.Reflection;
using Analyzer;
using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnalyzerTests.Pipeline
{
    public class BaseClass
    {
        // Base class logic
    }

    public class DerivedClass : BaseClass
    {
        // Derived class logic
    }

    public class DerivedClass2 : DerivedClass
    {
        // DerivedClass2 class logic
    }

    public class ViolatingClass : DerivedClass2
    {
        // ViolatingClass class logic
    }

    /// <summary>
    /// Unit tests for the <see cref="DepthOfInheritance"/> class.
    /// </summary>
    [TestClass()]
    public class TestDepth
    {

        /// <summary>
        /// Tests the depth of inheritance analyzer.
        /// </summary>
        [TestMethod()]
        public void TestDepthOfInh()
        {
            // Specify the path to the DLL file, which is the code above the Test Class
            string path = Assembly.GetExecutingAssembly().Location;
            ParsedDLLFile dllFile = new(path);

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            DepthOfInheritance analyzer = new(dllFiles);

            // Run the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            Dictionary<Type , int> depthMap = analyzer.CalculateDepthOfInheritance( dllFile );

            Assert.IsNotNull( depthMap, "DepthMap is NULL!");

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Assert.AreEqual(res.Verdict, 0 );

                Trace.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
            }

        }
    }
}
