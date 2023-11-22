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
        public void MainPipelineTest()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\Rules.dll";
            var parsedDllObj = new ParsedDLLFile(path);

            DllFileObjs.Add(parsedDllObj);

            ArrayFieldsShouldNotBeReadOnlyRule arrayFiledsShouldNotBeReadOnly = new(DllFileObjs);

            Dictionary<string, Analyzer.AnalyzerResult> resultObj = arrayFiledsShouldNotBeReadOnly.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["Rules.dll"];
            Assert.AreEqual(1, result.Verdict);
        }
    }
}
