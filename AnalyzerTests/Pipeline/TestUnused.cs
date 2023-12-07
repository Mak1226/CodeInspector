/******************************************************************************
 * Filename    = RemoveUnusedLocalVariablesRule.cs
 * 
 * Author      = Arun Sankar
 *
 * Product     = Analyzer
 * 
 * Project     = AnalyzerTests
 *
 * Description = Unit Tests for RemoveUnusedLocalVariablesRule class
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
using Analyzer;


namespace AnalyzerTests.Pipeline
{
    //TestUnused.dll
    //public class TestClass
    //{
    //    public void SomeMethod()
    //    {
    //        int x = 42;
    //        int y = 10;
    //        int n = x + y;
    //    }
    //}

    public class LetUsTestVariables
    {
        public static void MethodWithUnusedVariables()
        {
            int x = 42;
            int y = 10;

            //Console.WriteLine(x+y);
            int u;
            int z;
            int thirdunused;
            int n = x + y;
        }

        public static void MethodWithUsedVariables()
        {
            int usedVariable1 = 3;

            Console.WriteLine( usedVariable1 );
        }

        public static void MethodWithNoLocals() => Console.WriteLine( "No local variables in this method." );
    }

    /// <summary>
    /// Test class for the RemoveUnusedLocalVariablesRule.
    /// </summary>
    [TestClass()]
    public class TestUnused
    {
        /// <summary>
        /// Test method for detecting unused local variables.
        /// </summary>
        [TestMethod()]
        public void TestUnusedLocalVariables()
        {
            // Specify the path to the DLL file
            //string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\TestUnused.dll";

            string path = Assembly.GetExecutingAssembly().Location;

            // Create a list of DLL paths
            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            RemoveUnusedLocalVariablesRule analyzer = new( dllFiles );

            // Run the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Trace.WriteLine( res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage );

                Assert.AreEqual( 0 , res.Verdict , "There are no unused local variables!" );
            }
        }

        /// <summary>
        /// Test method for detecting no unused local variables.
        /// </summary>
        [TestMethod()]
        public void TestNoUnusedLocalVariables()
        {
            // Specify the path to the DLL file
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\TestUnused.dll";

            //string path = Assembly.GetExecutingAssembly().Location;

            // Create a list of DLL paths
            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            RemoveUnusedLocalVariablesRule analyzer = new( dllFiles );

            // Run the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Trace.WriteLine( res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage );

                Assert.AreEqual( 1 , res.Verdict , "There are unused local variables!" );
            }
        }
    }
}
