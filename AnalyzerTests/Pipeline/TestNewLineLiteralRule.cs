using Analyzer;
using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnalyzerTests.Pipeline
{
    [TestClass]
    /*
    namespace ForDLLs
{
    public class NewLineLiteral
    {
        static void NoNewLineLiteralsMethod(string[] args)
        {
            string s = "hello!";
            Console.WriteLine(s);
            Console.WriteLine("something is there\nyou should look somewhere");
        }
    }
}*/

    public class TestNewLineLiteralRule
    {
        [TestMethod]
        public void Test1()
        {
            string path = "..\\..\\..\\TestDLLs\\NoNewLineLiteral.dll";

            ParsedDLLFile dllFile = new(path);
            List<ParsedDLLFile> dllFiles = new() { dllFile };
            NewLineLiteralRule newLineLiteralRule = new(dllFiles);
            Dictionary<string, AnalyzerResult> result = newLineLiteralRule.AnalyzeAllDLLs();

            Dictionary<string, AnalyzerResult> original = new()
            {
                ["114"] = new AnalyzerResult("114", 0, "NewLineLiteral.NoNewLineLiteralsMethod")
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
            string path = "..\\..\\..\\TestDLLs\\NewLineLiteral.dll";

            ParsedDLLFile dllFile = new(path);
            List<ParsedDLLFile> dllFiles = new() { dllFile };
            NewLineLiteralRule newLineLiteralRule = new(dllFiles);
            Dictionary<string, AnalyzerResult> result = newLineLiteralRule.AnalyzeAllDLLs();

            Dictionary<string, AnalyzerResult> original = new()
            {
                ["114"] = new AnalyzerResult("114", 0, "NewLineLiteral.NoNewLineLiteralsMethod")
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
