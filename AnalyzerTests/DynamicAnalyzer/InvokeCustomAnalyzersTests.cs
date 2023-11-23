/******************************************************************************
* Filename    = AnalyzerFactoryTests.cs
*
* Author      = Mangesh Dalvi, Yukta Salunkhe
* 
* Roll No     = 112001010, 112001052
*
* Product     = Code Inspector
* 
* Project     = AnalyzerTests
*
* Description = Test class for testing the functionality of invoking a custom analyzer on student dlls
******************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analyzer.DynamicAnalyzer.Tests
{
    /// <summary>
    /// Test class for testing the functionality of invoking a custom analyzer on student dlls
    /// </summary>
    [TestClass()]
    public class InvokeCustomAnalyzersTests
    {

        /* 
        
        TeacherAnalyserCheckingAbstractClassNaming.dll

        using Analyzer.Parsing;
        using Analyzer.Pipeline;
        using System;
        using System.Reflection;

        namespace Analyzer.DynamicAnalyzer
        {
            public class CustomAnalyzer : AnalyzerBase
            {
                private readonly string _analyzerID;
                private string _errorMessage;
                private int _verdict;

                public CustomAnalyzer(List<ParsedDLLFile> dllFiles) : base(dllFiles)
                {
                    _errorMessage = "This is an error message";
                    _verdict = 0;
                    _analyzerID = "Teacher"; // update analyzer ID here
                }

                protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
                {
                    _verdict = 1;
                    _errorMessage = "";

                    foreach (ParsedClass classObj in parsedDLLFile.classObjList)
                    {
                        if (classObj.TypeObj.GetTypeInfo().IsAbstract && !IsValidAbstractClassName(classObj.TypeObj.Name))
                        {
                            _errorMessage += $"{(_verdict == 1 ? "Incorrect Abstract Class Naming : " : ", ")}{classObj.TypeObj.Name}";
                            _verdict = 0;
                        }
                    }

                    if(_verdict == 1)
                    {
                        _errorMessage = "All abstract classes are named correctly";
                    }

                    return new AnalyzerResult(_analyzerID, _verdict, _errorMessage);
                }

                private bool IsValidAbstractClassName(string className)
                {
                    return className.EndsWith("Base") && char.IsLower(className[0]);
                }
            }
        }
        */

        /// <summary>
        /// Invoke custom analyzers test with correct abstract class naming
        /// </summary>
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

            expected["CasingChecker.dll"].Add(new AnalyzerResult("Teacher", 1, "All abstract classes are named correctly"));

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

        /// <summary>
        /// Invoke custom analyzers test with wrong abstract class naming
        /// </summary>
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

            expected["NotValidAbstractClassNaming.dll"].Add(new AnalyzerResult("Teacher", 0, "Incorrect Abstract Class Naming : someClass"));

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
