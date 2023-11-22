using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnalyzerTests.Pipeline
{
    [TestClass]
    public class TestAvoidSwitchStatements
    {
        [TestMethod]
        public void MainPipelineTestWithoutSwitchStatements()
        {
            List<string> dllFilePaths = new()
            {
                "..\\..\\..\\..\\Analyzer\\TestDLLs\\Rules.dll"
            };

            List<ParsedDLLFile> dllFiles = dllFilePaths.Select(path => new ParsedDLLFile(path)).ToList();
            AvoidSwitchStatementsAnalyzer avoidSwitchStatements = new(dllFiles);

            Dictionary<string , Analyzer.AnalyzerResult> result = avoidSwitchStatements.AnalyzeAllDLLs();

            Assert.AreEqual(1, result[dllFiles[0].DLLFileName].Verdict); // Expecting success as no switch statements are present
        }
    }
}
