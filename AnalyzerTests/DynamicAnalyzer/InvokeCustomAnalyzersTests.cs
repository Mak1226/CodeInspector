using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer.DynamicAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace Analyzer.DynamicAnalyzer.Tests
{
    [TestClass()]
    public class InvokeCustomAnalyzersTests
    {
        [TestMethod()]
        public void InvokeCustomAnalyzersTest()
        {

            var analyzer = new Analyzer();

            List<string> paths = new();

            paths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\ClassLibrary1.dll");

            paths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\BridgePattern.dll");

            analyzer.LoadDLLFileOfStudent(paths);

            List<string> tdlls = new();

            string path = "C:\\Users\\HP\\Desktop\\software\\Demo\\Test1\\ClassLibrary2\\bin\\Debug\\net6.0\\ClassLibrary2.dll";

            tdlls.Add(path);

            analyzer.LoadDLLOfCustomAnalyzers(tdlls);

            Dictionary<string, List<AnalyzerResult>> result = analyzer.RnuCustomAnalyzers();

            foreach (var dll in result)
            {
                Console.WriteLine(dll.Key);

                foreach (var res in dll.Value)
                {
                    Console.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
                }
            }
            
            Dictionary<string, List<AnalyzerResult>> expected = new();
            expected["ClassLibrary1.dll"] = new List<AnalyzerResult>();
            expected["BridgePattern.dll"] = new List<AnalyzerResult>();

            expected["ClassLibrary1.dll"].Add(new AnalyzerResult("This is an analyzer ID", 0, "This is an error message m"));
            expected["BridgePattern.dll"].Add(new AnalyzerResult("This is an analyzer ID", 0, "This is an error message m"));

            Assert.AreEqual(expected["ClassLibrary1.dll"].ToString(), result["ClassLibrary1.dll"].ToString());
            Assert.AreEqual(expected["BridgePattern.dll"].ToString(), result["BridgePattern.dll"].ToString());

        }
    }
}