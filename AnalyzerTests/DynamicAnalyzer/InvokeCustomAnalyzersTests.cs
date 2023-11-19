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
    /// <summary>
    /// Test class for testing the functionality of invoking a custom analyzer on student dlls
    /// </summary>
    [TestClass()]
    public class InvokeCustomAnalyzersTests
    {
        [TestMethod()]
        public void InvokeCustomAnalyzersTest()
        {
            var analyzer = new Analyzer();

            // Load student's dll files
            List<string> paths = new();
            paths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\ClassLibrary1.dll");
            paths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\BridgePattern.dll");
            analyzer.LoadDLLFileOfStudent(paths);

            // Load the custom analyzer dll
            List<string> tdlls = new();
            string path = "C:\\Users\\HP\\Desktop\\software\\Demo\\Test1\\ClassLibrary2\\bin\\Debug\\net6.0\\ClassLibrary2.dll";
            tdlls.Add(path);
            analyzer.LoadDLLOfCustomAnalyzers(tdlls);

            // Run the custom Analyzer on student dlls and get the result
            Dictionary<string, List<AnalyzerResult>> result = analyzer.RnuCustomAnalyzers();

            // Defining the expected result
            Dictionary<string, List<AnalyzerResult>> expected = new();
            expected["ClassLibrary1.dll"] = new List<AnalyzerResult>();
            expected["BridgePattern.dll"] = new List<AnalyzerResult>();

            expected["ClassLibrary1.dll"].Add(new AnalyzerResult("This is an analyzer ID", 0, "This is an error message m"));
            expected["BridgePattern.dll"].Add(new AnalyzerResult("This is an analyzer ID", 0, "This is an error message m"));

            // Assert that the actual Analyzer result matches with the expected one.
            Assert.AreEqual(expected["ClassLibrary1.dll"].ToString(), result["ClassLibrary1.dll"].ToString());
            Assert.AreEqual(expected["BridgePattern.dll"].ToString(), result["BridgePattern.dll"].ToString());

        }
    }
}