/******************************************************************************
* Filename    = TestArrayFieldsShouldNotBeReadOnly.cs
* 
* Author      = Thanmayee
* 
* Project     = AnalyzerTests
*
* Description = Test class to verify the functionality of the ArrayFieldsShouldNotBeReadOnly
*****************************************************************************/

using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerTests.Pipeline
{
    [TestClass()]
    public class TestArrayFieldsShouldNotBeReadOnlyRule
    {
        /// <summary>
        /// Test method for a case where no classes have readonly array fields.
        /// </summary>
        [TestMethod()]
        public void TestNoReadOnlyArrayFields()
        {
            string dllFile = Assembly.GetExecutingAssembly().Location;
            ParsedDLLFile parsedDLL = new( dllFile );

            List<ParsedDLLFile> parseddllFiles = new() { parsedDLL };

            ArrayFieldsShouldNotBeReadOnlyRule arrayFiledsShouldNotBeReadOnly = new( parseddllFiles );

            Dictionary<string , AnalyzerResult> resultObj = arrayFiledsShouldNotBeReadOnly.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["AnalyzerTests.dll"];
            Assert.AreEqual( 1 , result.Verdict );
        }

        [TestMethod()]
        public void TestReadOnlyArrayFields()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\Array.dll";
            var parsedDllObj = new ParsedDLLFile( path );

            DllFileObjs.Add( parsedDllObj );

            ArrayFieldsShouldNotBeReadOnlyRule arrayFiledsShouldNotBeReadOnly = new( DllFileObjs );

            Dictionary<string , Analyzer.AnalyzerResult> resultObj = arrayFiledsShouldNotBeReadOnly.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["Array.dll"];
            Assert.AreEqual( 0 , result.Verdict );
        }
    }
}
