using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Parsing;

namespace Analyzer.Pipeline.Tests
{
    [TestClass()]
    public class TestUnused
    {
        [TestMethod()]
        public void TestRemoveUnusedLocalVariables()
        {
            // Specify the path to the DLL file
            string dllFilePath = "..\\..\\..\\..\\Analyzer\\TestDLLs\\TestUnused.dll";

            // Create a list of DLL paths
            var dllFilePaths = new System.Collections.Generic.List<string> { dllFilePath };

            // Parse the DLL files
            var parsedDLLFiles = new ParsedDLLFiles(dllFilePaths);

            // Create an instance of RemoveUnusedLocalVariablesRule
            var analyzer = new RemoveUnusedLocalVariablesRule(parsedDLLFiles);

            // Run the analyzer
            var result = analyzer.Run();

            // Assert that no unused local variables were found
            Assert.AreEqual(0, result.Verdict, "Unexpected number of unused local variables.");
        }
    }
}