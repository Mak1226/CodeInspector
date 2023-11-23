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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerTests.Pipeline
{
    [TestClass()]
    public class TestArrayFieldsShouldNotBeReadOnly
    {

        [TestMethod()]
        public void GoodTest()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\array1.dll";
            var parsedDllObj = new ParsedDLLFile(path);

            DllFileObjs.Add(parsedDllObj);

            ArrayFieldsShouldNotBeReadOnlyRule arrayFiledsShouldNotBeReadOnly = new(DllFileObjs);

            Dictionary<string, Analyzer.AnalyzerResult> resultObj = arrayFiledsShouldNotBeReadOnly.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["array1.dll"];
            Assert.AreEqual(1, result.Verdict);
        }

        [TestMethod()]
        public void GoodTest1()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\notReadOnly.dll";
            var parsedDllObj = new ParsedDLLFile(path);

            DllFileObjs.Add(parsedDllObj);

            ArrayFieldsShouldNotBeReadOnlyRule arrayFiledsShouldNotBeReadOnly = new(DllFileObjs);

            Dictionary<string, Analyzer.AnalyzerResult> resultObj = arrayFiledsShouldNotBeReadOnly.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["notReadOnly.dll"];
            Assert.AreNotEqual(0, result.Verdict);
        }


        [TestMethod()]
        public void BadTest()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\Array.dll";
            var parsedDllObj = new ParsedDLLFile(path);

            DllFileObjs.Add(parsedDllObj);

            ArrayFieldsShouldNotBeReadOnlyRule arrayFiledsShouldNotBeReadOnly = new(DllFileObjs);

            Dictionary<string, Analyzer.AnalyzerResult> resultObj = arrayFiledsShouldNotBeReadOnly.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["Array.dll"];
            Assert.AreEqual(0, result.Verdict);
        }

        [TestMethod()]
        public void BadTest1()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\readOnly.dll";
            var parsedDllObj = new ParsedDLLFile(path);

            DllFileObjs.Add(parsedDllObj);

            ArrayFieldsShouldNotBeReadOnlyRule arrayFiledsShouldNotBeReadOnly = new(DllFileObjs);

            Dictionary<string, Analyzer.AnalyzerResult> resultObj = arrayFiledsShouldNotBeReadOnly.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["readOnly.dll"];
            Assert.AreEqual(0, result.Verdict);
        }
    }
}
