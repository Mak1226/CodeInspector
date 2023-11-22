using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Pipeline.Tests
{
    public interface IInterfaceEmpty
    {

    }
    public class SampleTestsNoEmptyInterface
    {

    }

    [TestClass()]
    public class TestNoEmptyInterface
    {
        [TestMethod()]
        public void TestEmptyInterfacePresent()
        {
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\NoEmptyInterfaces1.dll";

            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };
            NoEmptyInterface noEmptyInterfaces = new( dllFiles );

            Dictionary<string , AnalyzerResult> result = noEmptyInterfaces.AnalyzeAllDLLs();
            Console.WriteLine( result[dllFile.DLLFileName].ErrorMessage );
            Assert.AreEqual( 0 , result[dllFile.DLLFileName].Verdict );
        }
    }
}
