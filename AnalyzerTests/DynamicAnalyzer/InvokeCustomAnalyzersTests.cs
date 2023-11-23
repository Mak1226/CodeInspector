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
        public void InvokeCustomAnalyzersTestWithCorrectAbstractClassNaming()
        {
            var analyzer = new Analyzer();

            // Load student's dll files
            List<string> paths = new()
            {
                "..\\..\\..\\TestDLLs\\CasingChecker.dll",
            };
            analyzer.LoadDLLFileOfStudent(paths);

            // Load the custom analyzer dll
            List<string> tdlls = new();
            string path = "..\\..\\..\\TestDLLs\\TeacherAnalyserCheckingAbstractClassNaming.dll";

            tdlls.Add(path);
            analyzer.LoadDLLOfCustomAnalyzers(tdlls);

            // Run the custom Analyzer on student dlls and get the result
            Dictionary<string, List<AnalyzerResult>> result = analyzer.RnuCustomAnalyzers();

            // Defining the expected result
            Dictionary<string , List<AnalyzerResult>> expected = new()
            {
                ["CasingChecker.dll"] = new List<AnalyzerResult>() ,
            };

            expected["CasingChecker.dll"].Add(new AnalyzerResult("This is an analyzer ID", 1, "No incorrect abstract class naming found."));

            foreach (KeyValuePair<string, List<AnalyzerResult>> dll in result)
            {
                Console.WriteLine(dll.Key);

                foreach (AnalyzerResult res in dll.Value)
                {
                    Console.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
                }
            }

            // Assert
            foreach (KeyValuePair<string, List<AnalyzerResult>> dll in result)
            {
                Assert.IsTrue(expected.ContainsKey(dll.Key), $"Unexpected DLL: {dll.Key}");

                List<AnalyzerResult> expectedResults = expected[dll.Key];
                List<AnalyzerResult> actualResults = dll.Value;

                Assert.AreEqual(expectedResults.Count, actualResults.Count, $"Mismatched number of results for {dll.Key}");

                for (int i = 0; i < expectedResults.Count; i++)
                {
                    Assert.AreEqual(expectedResults[i].AnalyserID, actualResults[i].AnalyserID);
                    Assert.AreEqual(expectedResults[i].Verdict, actualResults[i].Verdict);
                    Assert.AreEqual(expectedResults[i].ErrorMessage, actualResults[i].ErrorMessage);
                }
            }
        }

        [TestMethod()]
        public void InvokeCustomAnalyzersTestWithWrongAbstractClassNaming()
        {
            var analyzer = new Analyzer();

            // Load student's dll files
            List<string> paths = new()
            {
                "..\\..\\..\\TestDLLs\\NotValidAbstractClassNaming.dll",
            };
            analyzer.LoadDLLFileOfStudent(paths);

            // Load the custom analyzer dll
            List<string> tdlls = new();
            string path = "..\\..\\..\\TestDLLs\\TeacherAnalyserCheckingAbstractClassNaming.dll";

            tdlls.Add(path);
            analyzer.LoadDLLOfCustomAnalyzers(tdlls);

            // Run the custom Analyzer on student dlls and get the result
            Dictionary<string, List<AnalyzerResult>> result = analyzer.RnuCustomAnalyzers();

            // Defining the expected result
            Dictionary<string, List<AnalyzerResult>> expected = new()
            {
                ["NotValidAbstractClassNaming.dll"] = new List<AnalyzerResult>(),
            };

            expected["NotValidAbstractClassNaming.dll"].Add(new AnalyzerResult("This is an analyzer ID", 0, "Incorrect Abstract Class Naming : someClass"));

            foreach (KeyValuePair<string, List<AnalyzerResult>> dll in result)
            {
                Console.WriteLine(dll.Key);

                foreach (AnalyzerResult res in dll.Value)
                {
                    Console.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
                }
            }

            // Assert
            foreach (KeyValuePair<string, List<AnalyzerResult>> dll in result)
            {
                Assert.IsTrue(expected.ContainsKey(dll.Key), $"Unexpected DLL: {dll.Key}");

                List<AnalyzerResult> expectedResults = expected[dll.Key];
                List<AnalyzerResult> actualResults = dll.Value;

                Assert.AreEqual(expectedResults.Count, actualResults.Count, $"Mismatched number of results for {dll.Key}");

                for (int i = 0; i < expectedResults.Count; i++)
                {
                    Assert.AreEqual(expectedResults[i].AnalyserID, actualResults[i].AnalyserID);
                    Assert.AreEqual(expectedResults[i].Verdict, actualResults[i].Verdict);
                    Assert.AreEqual(expectedResults[i].ErrorMessage, actualResults[i].ErrorMessage);
                }
            }
        }

    }
}
