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
            List<string> dllFilePaths = new List<string>
            {
                "..\\..\\..\\..\\Analyzer\\TestDLLs\\Rules.dll"
            };

            List<ParsedDLLFile> dllFiles = dllFilePaths.Select(path => new ParsedDLLFile(path)).ToList();

            ArrayFieldsShouldNotBeReadOnlyRule arrayFields = new(dllFiles);

            var result = arrayFields.AnalyzeAllDLLs();

            Assert.AreEqual(1, result[dllFiles[0].DLLFileName].Verdict);
        }
    }
}