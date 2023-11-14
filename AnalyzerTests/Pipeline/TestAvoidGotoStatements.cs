using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer.Pipeline;
using System.Collections.Generic;
using Analyzer.Parsing;

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
            // Arrange
            List<string> dllFilePaths = new List<string>
            {
                "..\\..\\..\\..\\Analyzer\\TestDLLs\\Goto.dll"
                // Add another DLL or modify the existing one to contain a method with goto statements
            };

            ParsedDLLFiles dllFiles = new ParsedDLLFiles(dllFilePaths);
            AvoidGotoStatementsAnalyzer analyzer = new AvoidGotoStatementsAnalyzer(dllFiles);

            // Act
            var result = analyzer.Run();

            // Assert
            Assert.AreNotEqual(1, result.Verdict); // Verdict should not be 1 for failing
        }
    }
}
