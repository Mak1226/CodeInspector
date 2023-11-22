using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer.Parsing;

namespace Analyzer.Pipeline.Tests
{
    [TestClass()]
    public class TestNativeFieldsShouldNotBeVisible
    {
        [TestMethod()]
        public void TestHasPublicNativeField()
        {
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\NoEmptyInterfaces1.dll";

            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };
            NoEmptyInterface noEmptyInterfaces = new( dllFiles );

            Dictionary<string , AnalyzerResult> result = noEmptyInterfaces.AnalyzeAllDLLs();
            Console.WriteLine( result[dllFile.DLLFileName].ErrorMessage );
            Assert.AreEqual(  , result[dllFile.DLLFileName].Verdict );
        }
    }
}
