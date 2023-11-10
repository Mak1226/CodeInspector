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
    public class TestAvoidSwitchStatemets
    {
        [TestMethod()]
        public void MainPipelineTest()
        {

            List<string> DllFilePaths = new List<string>();

            DllFilePaths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\DemoDLL.dll");

            ParsedDLLFiles dllFiles = new(DllFilePaths);

            AvoidSwitchStatementsAnalyzer avoidSwitchStatements = new(dllFiles);

            var result = avoidSwitchStatements.Run();

            Assert.AreEqual(1, result.Verdict);
        }
    }
}
