/******************************************************************************
 * Filename    = TestAsync.cs
 * 
 * Author      = Arun Sankar
 *
 * Product     = Analyzer
 * 
 * Project     = AnalyzerTests
 *
 * Description = Unit Tests for AsyncMethodAnalyzer class
 *****************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Parsing;
using System.Reflection;
using System.Diagnostics;

namespace Analyzer.Pipeline.Tests
{
    /// <summary>
    /// Test class for the AsyncMethodAnalyzer.
    /// </summary>
    [TestClass()]
    public class TestAsync
    {
        /// <summary>
        /// Test method for multiple asynchronous methods.
        /// </summary>
        [TestMethod()]
        public void TestMultipleAsyncMethods()
        {
            // Specify the path to the DLL file
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\ClassLibrary1.dll";
            //string path = Assembly.GetExecutingAssembly().Location;
            // Create a list of DLL paths
            ParsedDLLFile dllFile = new(path);

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            AsyncMethodAnalyzer analyzer = new(dllFiles);

            // Run the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                //Console.WriteLine(dll.Key);

                AnalyzerResult res = dll.Value;

                Trace.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);

                Assert.AreEqual( res.Verdict , 0 );
            }

        }

        /// <summary>
        /// Test method for no asynchronous methods.
        /// </summary>
        [TestMethod()]
        public void TestNoAsync()
        {
            // Specify the path to the DLL file
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\depthofinh.dll";
            //string path = Assembly.GetExecutingAssembly().Location;
            // Create a list of DLL paths
            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            AsyncMethodAnalyzer analyzer = new( dllFiles );

            // Run the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                //Console.WriteLine(dll.Key);

                AnalyzerResult res = dll.Value;

                Trace.WriteLine( res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage );

                Assert.AreEqual( res.Verdict , 1 );
            }

        }
    }
}
