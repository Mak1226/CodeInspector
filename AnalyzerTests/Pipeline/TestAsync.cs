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
using Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Parsing;
using System.Reflection;
using System.Diagnostics;

namespace AnalyzerTests.Pipeline
{
    //ClassLibrary1.dll is generated from the below class

    //public class AsyncMethodTestClass
    //{
    //    public async Task AsyncMethod1()
    //    {
    //        Console.WriteLine( "Async Method 1 - Start" );
    //        await Task.Delay( 1000 ); // Simulating asynchronous operation
    //        Console.WriteLine( "Async Method 1 - End" );
    //    }

    //    public async Task<int> AsyncMethod2()
    //    {
    //        Console.WriteLine( "Async Method 2 - Start" );
    //        await Task.Delay( 1500 ); // Simulating asynchronous operation
    //        Console.WriteLine( "Async Method 2 - End" );
    //        return 42;
    //    }

    //    public async Task<string> AsyncMethod3( string input )
    //    {
    //        Console.WriteLine( $"Async Method 3 - Start with input: {input}" );
    //        await Task.Delay( 2000 ); // Simulating asynchronous operation
    //        Console.WriteLine( $"Async Method 3 - End with input: {input}" );
    //        return $"Result: {input}";
    //    }
    //}

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

            // RenderImageBytes the analyzer
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
            // This consists of just few classes, no class logic, no methods either
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\depthofinh.dll";

            // Create a list of DLL paths
            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of AsyncMethodAnalyzer
            AsyncMethodAnalyzer analyzer = new( dllFiles );

            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {

                AnalyzerResult res = dll.Value;

                Trace.WriteLine( res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage );

                Assert.AreEqual( res.Verdict , 1 );
            }

        }
    }
}
