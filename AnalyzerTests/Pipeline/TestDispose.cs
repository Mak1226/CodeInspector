/******************************************************************************
 * Filename    = TestDispose.cs
 * 
 * Author      = Arun Sankar
 *
 * Product     = Analyzer
 * 
 * Project     = AnalyzerTests
 *
 * Description = Unit Tests for DisposableFieldsShouldBeDisposedRule class
 *****************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Analyzer.Parsing;

namespace Analyzer.Pipeline.Tests
{
    /// <summary>
    /// Test class for the DisposableFieldsShouldBeDisposedRule.
    /// </summary>
    [TestClass()]
    public class TestDispose
    {
        /// <summary>
        /// Test method for DisposableFieldsShouldBeDisposedRule when violation is present.
        /// </summary>
        /// [TestMethod()]
        public void TestViolation()
        {
            // Specify the path to the DLL file
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\disposetestviolated.dll";

            // Create a list of DLL paths
            ParsedDLLFile dllFile = new(path);

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            DisposableFieldsShouldBeDisposedRule analyzer = new(dllFiles);

            // Run the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                Trace.WriteLine(dll.Key);

                AnalyzerResult res = dll.Value;

                Assert.AreEqual( res.Verdict , 0 );

                Trace.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
            }

        }

        /// <summary>
        /// Test method for DisposableFieldsShouldBeDisposedRule when no violation is present.
        /// </summary>
        [TestMethod()]
        public void TestNoViolation()
        {
            // Specify the path to the DLL file
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\disposetest.dll";

            // Create a list of DLL paths
            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            DisposableFieldsShouldBeDisposedRule analyzer = new( dllFiles );

            // Run the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Assert.AreEqual( res.Verdict , 1 );

                Trace.WriteLine( res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage );
            }

        }

        /// <summary>
        /// Test method for DisposableFieldsShouldBeDisposedRule when there are multiple disposable fields.
        /// </summary>
        [TestMethod()]
        public void TestMultipleDisposableFields()
        {
            // Specify the path to the DLL file
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\MultipleDisposableFieldsTestClass.dll";

            // Create a list of DLL paths
            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            DisposableFieldsShouldBeDisposedRule analyzer = new( dllFiles );

            // Run the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Assert.AreEqual( res.Verdict , 0 );

                Trace.WriteLine( res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage );
            }

        }

        /// <summary>
        /// Test method for DisposableFieldsShouldBeDisposedRule on a DLL with no disposable fields.
        /// </summary>
        [TestMethod()]
        public void TestRandom()
        {
            // Specify the path to the DLL file
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\depthofinh.dll";

            // Create a list of DLL paths
            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            DisposableFieldsShouldBeDisposedRule analyzer = new( dllFiles );

            // Run the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Assert.AreEqual( res.Verdict , 1 );

                Trace.WriteLine( res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage );
            }

        }

        /// <summary>
        /// Test method for DisposableFieldsShouldBeDisposedRule when there is a derived class.
        /// </summary>
        [TestMethod()]
        public void TestBaseAndDerived()
        {
            // Specify the path to the DLL file
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\branchcoveragedispose.dll";

            // Create a list of DLL paths
            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            DisposableFieldsShouldBeDisposedRule analyzer = new( dllFiles );

            // Run the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Assert.AreEqual( res.Verdict , 1 );

                Trace.WriteLine( res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage );
            }

        }
    }
}
