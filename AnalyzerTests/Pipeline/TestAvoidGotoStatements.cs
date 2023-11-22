using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer.Pipeline;
using System.Collections.Generic;
using Analyzer.Parsing;
using System.Linq;

namespace Analyzer.Pipeline.Tests
{
    [TestClass]
    public class AvoidGotoStatementsAnalyzerTests
    {
        /*[TestMethod]
        public void NoGotoStatements_ShouldPass()
        {
            // Arrange
            List<string> dllFilePaths = new List<string>
            {
                "..\\..\\..\\..\\Analyzer\\TestDLLs\\Goto.dll"
            };

            ParsedDLLFiles dllFiles = new ParsedDLLFiles(dllFilePaths);
            AvoidGotoStatementsAnalyzer analyzer = new AvoidGotoStatementsAnalyzer(dllFiles);

            // Act
            var result = analyzer.Run();

            // Assert
            Assert.AreEqual(1, result.Verdict); // Verdict should be 1 for passing
        }*/

        [TestMethod]
        public void GotoStatementsPresent_ShouldFail()
        {
            List<string> dllFilePaths = new() { "..\\..\\..\\..\\Analyzer\\TestDLLs\\Goto.dll" };

            List<ParsedDLLFile> dllFiles = dllFilePaths.Select(path => new ParsedDLLFile(path)).ToList();
            AvoidGotoStatementsAnalyzer analyzer = new(dllFiles);

            var result = analyzer.AnalyzeAllDLLs();

            Assert.AreEqual(0, result[dllFiles[0].DLLFileName].Verdict); // Verdict should be 0 for failing
        }
    }
}
