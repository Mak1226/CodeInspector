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
    public class TestControl
    {
        [TestMethod()]
        public void Test1()
        {
            // Specify the path to the DLL file
            string path = "..\\..\\..\\..\\Analyzer\\TestDLLs\\controlflow.dll";

            // Create a list of DLL paths
            ParsedDLLFile dllFile = new(path);

            //DllFilePaths.Add(path);

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            ReviewUselessControlFlowRule analyzer = new(dllFiles);

            // Run the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                Console.WriteLine(dll.Key);

                AnalyzerResult res = dll.Value;

                Console.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
            }

        }
    }
}
