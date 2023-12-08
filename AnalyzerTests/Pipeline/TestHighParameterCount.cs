/******************************************************************************
 * Filename    = TestHighParameterCount.cs
 * 
 * Author      = Arun Sankar
 *
 * Product     = Analyzer
 * 
 * Project     = AnalyzerTests
 *
 * Description = Unit Tests for TestHighParameterCount class
 *****************************************************************************/

using System.Reflection;
using Analyzer;
using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnalyzerTests.Pipeline
{

    /// <summary>
    /// Test class for the HighParameterCountRule.
    /// </summary>
    [TestClass()]
    public class TestHighParameterCount
    {

        //xyz.dll is given below

        //public class HighParams
        //{
        //    public static void HighParameterMethod( int param1 , int param2 , int param3 , int param4 , int param5 , int param6 )
        //    {
        //        Console.WriteLine( "Method with high parameter count?" );
        //    }
        //}

        public class LowParams
        {
            public static void LowParameterMethod( int param1 , int param2 , int param3 , int param4 , int param5 )
            {
                Console.WriteLine( "Method with high parameter count?" );
            }
        }

        /// <summary>
        /// Test method for low parameter count.
        /// </summary>
        [TestMethod()]
        public void TestLowParams()
        {
            // Specify the path to the DLL file
            //string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\xyz.dll";
            string path = Assembly.GetExecutingAssembly().Location;
            ParsedDLLFile dllFile = new(path);

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of HighParameterCountRule
            HighParameterCountRule analyzer = new(dllFiles);

            // RenderImageBytes the analyzer
            Dictionary<string, AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string, AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Assert.AreEqual(res.Verdict, 1);

                Console.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
            }

        }

        /// <summary>
        /// Test method for high parameter count.
        /// </summary>
        [TestMethod()]
        public void TestHighParams()
        {
            // Specify the path to the DLL file
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\xyz.dll";
            //string path = Assembly.GetExecutingAssembly().Location;
            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of HighParameterCountRule
            HighParameterCountRule analyzer = new( dllFiles );

            // RenderImageBytes the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Assert.AreEqual( res.Verdict , 0 );

                Console.WriteLine( res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage );
            }

        }
    }
}
