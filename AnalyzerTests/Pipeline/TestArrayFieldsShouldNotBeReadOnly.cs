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

            List<string> DllFilePaths = new List<string>();

            DllFilePaths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\Rules.dll");

            ParsedDLLFiles dllFiles = new(DllFilePaths);

            ArrayFieldsShouldNotBeReadOnlyRule arrayFields = new(dllFiles);

            var result = arrayFields.Run();

            Assert.AreEqual(1, result.Verdict);
        }
    }
}
