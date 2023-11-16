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
            string path = "..\\..\\..\\..\\Analyzer\\TestDLLs\\TestUnused.dll";

            // Create a list of DLL paths
            ParsedDLLFile dllFile = new(path);

            //DllFilePaths.Add(path);

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            RemoveUnusedLocalVariablesRule analyzer = new(dllFiles);

            // Run the analyzer
            var result = analyzer.AnalyzeSingleDLL();

            // Assert that no unused local variables were found
            Assert.AreEqual(2, result.Verdict, "Unexpected number of unused local variables.");
        }
    }
}