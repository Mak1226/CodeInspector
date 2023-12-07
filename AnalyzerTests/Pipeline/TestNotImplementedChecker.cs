using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil.Cil;
using System.Reflection;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer;


namespace AnalyzerTests.Pipeline
{
    [TestClass]
    public class TestNotImplementedChecker
    {
        [TestMethod]
        public void Test1()
        {
            string path = "..\\..\\..\\TestDLLs\\Implemented.dll";

            ParsedDLLFile dllFile = new(path);
            List<ParsedDLLFile> dllFiles = new() { dllFile };
            NotImplementedChecker notImplementedChecker = new(dllFiles);
            Dictionary<string, AnalyzerResult> result = notImplementedChecker.AnalyzeAllDLLs();

            Dictionary<string, AnalyzerResult> original = new()
            {
                ["118"] = new AnalyzerResult("118", 1, "No violation found" )
            };
            foreach (KeyValuePair<string, AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Console.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
            }

            Assert.AreEqual(original.ToString(), result.ToString());

        }

        [TestMethod]
        public void Test2()
        {
            string path = "..\\..\\..\\TestDLLs\\NotImplement.dll";

            ParsedDLLFile dllFile = new(path);
            List<ParsedDLLFile> dllFiles = new() { dllFile };
            NotImplementedChecker notImplementedChecker = new(dllFiles);
            Dictionary<string, AnalyzerResult> result = notImplementedChecker.AnalyzeAllDLLs();

            Dictionary<string, AnalyzerResult> original = new()
            {
                ["118"] = new AnalyzerResult("118", 0, "MathLibrary.Sub")
            };
            foreach (KeyValuePair<string, AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Console.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
            }

            Assert.AreEqual(original.ToString(), result.ToString());

        }

    }
}
