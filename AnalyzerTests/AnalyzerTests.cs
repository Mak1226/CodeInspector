/******************************************************************************
* Filename    = AnalyzerTests.cs
*
* Author      = Mangesh Dalvi
* 
* Roll No     = 112001010
*
* Product     = Code Inspector
* 
* Project     = AnalyzerTests
*
* Description =  Unit tests for the Analyzer class.
******************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analyzer.Tests
{
    /// <summary>
    /// Unit tests for the Analyzer class.
    /// </summary>
    [TestClass()]
    public class AnalyzerTests
    {
        /// <summary>
        /// Tests the Analyzer's ability to analyze all analyzers in the pipeline for a given DLL.
        /// </summary>
        [TestMethod()]
        public void AllAnalyzerInPipelineTest()
        {
            Analyzer analyzer = new();

            IDictionary<int , bool> teacherOptions = new Dictionary<int , bool>
            {
                [101] = true,
                [102] = true,
                [103] = true,
                [104] = true,
                [105] = true,
                [106] = true,
                [107] = true,
                [108] = true,
                [109] = true,
                [110] = true,
                [111] = true,
                [112] = true,
                [113] = true,
                [114] = true,
                [115] = true,
                [116] = true,
                [117] = true,
                [118] = true,
                [119] = true,
                [120] = true
            };

            analyzer.Configure(teacherOptions);

            List<string> paths = new()
            {
                "..\\..\\..\\TestDLLs\\Abstract.dll"
            };

            analyzer.LoadDLLFileOfStudent(paths);

            Dictionary<string, List<AnalyzerResult>> result = analyzer.Run();

            Dictionary<string , List<AnalyzerResult>> original = new()
            {
                ["Abstract.dll"] = new List<AnalyzerResult> {

                    new AnalyzerResult("101", 1, "No violation found."),
                    new AnalyzerResult("102", 0, "Classes ClassLibrary1.BadBase, ClassLibrary1.badabstractclass contains only static fields and methods, but has non-static, visible constructor. Try changing it to private or make it static."),
                    new AnalyzerResult("103", 1, "No violation found."),
                    new AnalyzerResult("104", 1, "No violation found."),
                    new AnalyzerResult("105", 1, "Depth of inheritance rule followed by all classes."),
                    new AnalyzerResult("106", 1, "No readonly array fields found."),
                    new AnalyzerResult("107", 1, "No switch statements found."),
                    new AnalyzerResult("108", 1, "No violations found."),
                    new AnalyzerResult("109", 1, "No unused local variables found."),
                    new AnalyzerResult("110", 1, "No occurrences of useless control flow found."),
                    new AnalyzerResult("111", 0, "Incorrect Abstract Class Naming : BadBase , badabstractclass "),
                    new AnalyzerResult("112", 0, "Incorrect Class Naming : <Module> Incorrect Class Naming : badabstractclass "),
                    new AnalyzerResult("113", 1, "No methods have cyclomatic complexity greater than 10"),
                    new AnalyzerResult("114", 1, "No violation found"),
                    new AnalyzerResult("115", 1, "No Violation Found"),
                    new AnalyzerResult("116", 1, "No violation found"),
                    new AnalyzerResult("117", 1, "No goto statements found."),
                    new AnalyzerResult("118", 1, "No Violation Found"),
                    new AnalyzerResult("119", 1, "methods with a high number of parameters found."),
                    new AnalyzerResult("120", 1, "No violation found")

                }
            };

            //Methods having cyclomatic complexity greater than { _maxAllowedComplexity}:\n[NOTE: Switch case complexity is not accurate]\n

            // Sort the lists based on AnalyserID
            foreach (string key in original.Keys)
            {
                original[key] = original[key].OrderBy(result => result.AnalyserID).ToList();
            }

            // Sort the lists based on AnalyserID
            foreach (string key in result.Keys)
            {
                result[key] = result[key].OrderBy(result => result.AnalyserID).ToList();
            }

            foreach(KeyValuePair<string , List<AnalyzerResult>> dll in result)
            {
                Console.WriteLine(dll.Key);

                foreach(AnalyzerResult res in dll.Value)
                {
                    Console.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
                }
            }

            foreach (KeyValuePair<string, List<AnalyzerResult>> dll in result)
            {
                Assert.IsTrue(original.ContainsKey(dll.Key), $"Expected DLL '{dll.Key}' not found in the original results.");

                List<AnalyzerResult> originalResults = original[dll.Key];
                List<AnalyzerResult> actualResults = dll.Value;

                Assert.AreEqual(originalResults.Count, actualResults.Count, $"Result count for DLL '{dll.Key}' is different.");

                for (int i = 0; i < originalResults.Count; i++)
                {
                    AnalyzerResult originalResult = originalResults[i];
                    AnalyzerResult actualResult = actualResults[i];

                    Assert.AreEqual(originalResult.AnalyserID, actualResult.AnalyserID, $"AnalyserID mismatch for DLL '{dll.Key}' at index {i}.");
                    Assert.AreEqual(originalResult.Verdict, actualResult.Verdict, $"Verdict mismatch for DLL '{dll.Key}' at index {i}.");
                }
            }

        }

        /// <summary>
        /// Tests the behavior when the teacher provides invalid configuration options for analyzers.
        /// </summary>
        [TestMethod()]
        public void InvalidTeacherConfiguration()
        {
            Analyzer analyzer = new();

            IDictionary<int, bool> teacherOptions = new Dictionary<int, bool>
            {
                [200] = true,
                [201] = true,
            };

            analyzer.Configure(teacherOptions);

            List<string> paths = new()
            {
                "..\\..\\..\\TestDLLs\\Abstract.dll"
            };

            analyzer.LoadDLLFileOfStudent(paths);

            Dictionary<string, List<AnalyzerResult>> result = analyzer.Run();

            Dictionary<string , List<AnalyzerResult>> original = new()
            {
                ["Abstract.dll"] = new List<AnalyzerResult> {

                new AnalyzerResult("200", 1, "Analyser does not exists"),
                new AnalyzerResult("201", 1, "Analyser does not exists"),
            }
            };

            // Sort the lists based on AnalyserID
            foreach (string key in original.Keys)
            {
                original[key] = original[key].OrderBy(result => result.AnalyserID).ToList();
            }

            // Sort the lists based on AnalyserID
            foreach (string key in result.Keys)
            {
                result[key] = result[key].OrderBy(result => result.AnalyserID).ToList();
            }

            foreach (KeyValuePair<string, List<AnalyzerResult>> dll in result)
            {
                Assert.IsTrue(original.ContainsKey(dll.Key), $"Expected DLL '{dll.Key}' not found in the original results.");

                List<AnalyzerResult> originalResults = original[dll.Key];
                List<AnalyzerResult> actualResults = dll.Value;

                Assert.AreEqual(originalResults.Count, actualResults.Count, $"Result count for DLL '{dll.Key}' is different.");

                for (int i = 0; i < originalResults.Count; i++)
                {
                    AnalyzerResult originalResult = originalResults[i];
                    AnalyzerResult actualResult = actualResults[i];

                    Assert.AreEqual(originalResult.AnalyserID, actualResult.AnalyserID, $"AnalyserID mismatch for DLL '{dll.Key}' at index {i}.");
                    Assert.AreEqual(originalResult.Verdict, actualResult.Verdict, $"Verdict mismatch for DLL '{dll.Key}' at index {i}.");
                }
            }

        }

        /// <summary>
        /// Tests the behavior when an invalid path of DLL is provided.
        /// </summary>
        [TestMethod()]
        public void InvalidPathOfDLL()
        {
            Analyzer analyzer = new();

            IDictionary<int, bool> teacherOptions = new Dictionary<int, bool>
            {
                [101] = true,
            };

            analyzer.Configure(teacherOptions);

            List<string> paths = new()
            {
                "..\\..\\..\\..\\..\\..\\TestDLLs\\Abstract.dll"
            };

            analyzer.LoadDLLFileOfStudent(paths);

            Dictionary<string, List<AnalyzerResult>> result = analyzer.Run();

            Assert.AreEqual("Failed to parse Abstract.dll", result["Abstract.dll"][0].ErrorMessage );

        }

        /// <summary>
        /// Tests the Analyzer's behavior when only a few teacher options are enabled.
        /// </summary>
        [TestMethod()]
        public void OnlyFewTeacherOptions()
        {
            Analyzer analyzer = new();

            IDictionary<int, bool> teacherOptions = new Dictionary<int, bool>
            {
                [101] = false,
                [102] = true,
                [103] = true,
                [105] = false,
                [108] = false,
                [110] = false,
                [115] = false
            };

            analyzer.Configure(teacherOptions);

            List<string> paths = new()
            {
                "..\\..\\..\\TestDLLs\\ACIST.dll"
            };

            analyzer.LoadDLLFileOfStudent(paths);

            Dictionary<string, List<AnalyzerResult>> result = analyzer.Run();

            Dictionary<string , List<AnalyzerResult>> original = new()
            {
                ["ACIST.dll"] = new List<AnalyzerResult> {

                new AnalyzerResult("102", 1, "No violation found"),
                new AnalyzerResult("103", 1, "No violation found.")
            }
            };

            foreach (string key in original.Keys)
            {
                original[key] = original[key].OrderBy(result => result.AnalyserID).ToList();
            }

            // Sort the lists based on AnalyserID
            foreach (string key in result.Keys)
            {
                result[key] = result[key].OrderBy(result => result.AnalyserID).ToList();
            }

            foreach (KeyValuePair<string, List<AnalyzerResult>> dll in result)
            {
                Assert.IsTrue(original.ContainsKey(dll.Key), $"Expected DLL '{dll.Key}' not found in the original results.");

                List<AnalyzerResult> originalResults = original[dll.Key];
                List<AnalyzerResult> actualResults = dll.Value;

                Assert.AreEqual(originalResults.Count, actualResults.Count, $"Result count for DLL '{dll.Key}' is different.");

                for (int i = 0; i < originalResults.Count; i++)
                {
                    AnalyzerResult originalResult = originalResults[i];
                    AnalyzerResult actualResult = actualResults[i];

                    Assert.AreEqual(originalResult.AnalyserID, actualResult.AnalyserID, $"AnalyserID mismatch for DLL '{dll.Key}' at index {i}.");
                    Assert.AreEqual(originalResult.Verdict, actualResult.Verdict, $"Verdict mismatch for DLL '{dll.Key}' at index {i}.");
                    Assert.AreEqual(originalResult.ErrorMessage, actualResult.ErrorMessage, $"ErrorMessage mismatch for DLL '{dll.Key}' at index {i}.");
                }
            }
        }

        /// <summary>
        /// Tests the Analyzer's behavior when multiple DLL files are provided.
        /// </summary>
        [TestMethod]
        public void MultipleDllFiles()
        {
            Analyzer analyzer = new();

            IDictionary<int, bool> teacherOptions = new Dictionary<int, bool>
            {
                [102] = true,
                [105] = true,
                [108] = true,
            };

            analyzer.Configure(teacherOptions);

            List<string> paths = new()
            {
                "..\\..\\..\\TestDLLs\\ACIST1.dll",
                "..\\..\\..\\TestDLLs\\Depthofinh.dll",
                "..\\..\\..\\TestDLLs\\Proxy.dll",
            };

            analyzer.LoadDLLFileOfStudent(paths);

            Dictionary<string, List<AnalyzerResult>> result = analyzer.Run();

            Dictionary<string , List<AnalyzerResult>> original = new()
            {
                ["ACIST1.dll"] = new List<AnalyzerResult> {

                new AnalyzerResult("102", 0, "Classes ACIST1.BadExampleBase, ACIST1.BadExample contains only static fields and methods, but has non-static, visible constructor. Try changing it to private or make it static."),
                new AnalyzerResult("105", 1, "Depth of inheritance rule followed by all classes."),
                new AnalyzerResult("108", 1, "No violations found."),

            } ,

                ["Depthofinh.dll"] = new List<AnalyzerResult> {

                new AnalyzerResult("102", 0, "Classes depthofinh.BaseClass, depthofinh.DerivedClass, depthofinh.DerivedClass2, depthofinh.ViolatingClass contains only static fields and methods, but has non-static, visible constructor. Try changing it to private or make it static."),
                new AnalyzerResult("105", 0, " Classes violating depth of inheritance rule:\r\ndepthofinh.ViolatingClass: Depth - 4"),
                new AnalyzerResult("108", 1, "No violations found.")

            } ,

                ["Proxy.dll"] = new List<AnalyzerResult> {

                new AnalyzerResult("102", 1, "No violation found"),
                new AnalyzerResult("105", 1, "Depth of inheritance rule followed by all classes."),
                new AnalyzerResult("108", 1, "No violations found."),

            }
            };

            foreach (KeyValuePair<string, List<AnalyzerResult>> dll in result)
            {
                Console.WriteLine(dll.Key);

                foreach (AnalyzerResult res in dll.Value)
                {
                    Console.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
                }
            }

            foreach (string key in original.Keys)
            {
                original[key] = original[key].OrderBy(result => result.AnalyserID).ToList();
            }

            // Sort the lists based on AnalyserID
            foreach (string key in result.Keys)
            {
                result[key] = result[key].OrderBy(result => result.AnalyserID).ToList();
            }

            foreach (KeyValuePair<string, List<AnalyzerResult>> dll in result)
            {
                Assert.IsTrue(original.ContainsKey(dll.Key), $"Expected DLL '{dll.Key}' not found in the original results.");

                List<AnalyzerResult> originalResults = original[dll.Key];
                List<AnalyzerResult> actualResults = dll.Value;

                Assert.AreEqual(originalResults.Count, actualResults.Count, $"Result count for DLL '{dll.Key}' is different.");

                for (int i = 0; i < originalResults.Count; i++)
                {
                    AnalyzerResult originalResult = originalResults[i];
                    AnalyzerResult actualResult = actualResults[i];

                    Assert.AreEqual(originalResult.AnalyserID, actualResult.AnalyserID, $"AnalyserID mismatch for DLL '{dll.Key}' at index {i}.");
                    Assert.AreEqual(originalResult.Verdict, actualResult.Verdict, $"Verdict mismatch for DLL '{dll.Key}' at index {i}.");

                }
            }
        }

        /// <summary>
        /// Tests the RelationShip Graph generation when different kinds of removable namespaces will be given
        /// </summary>
        [TestMethod]
        public void CheckRelationshipGraphGeneration()
        {
            Analyzer analyzer = new();

            List<string> paths = new()
            {
                "..\\..\\..\\TestDLLs\\BridgePattern.dll"
            };

            analyzer.LoadDLLFileOfStudent(paths);

            // Above DLL contains everything in single namespace named "BridgePattern"
            byte[] classDiagramBytes = analyzer.GetRelationshipGraph(new());
            Assert.AreNotEqual(0 , classDiagramBytes.Length);

            // Removing BridgePattern namespace from the graph
            byte[] diagWithoutBridgeBytes = analyzer.GetRelationshipGraph( new() { "BridgePattern" } );
            Assert.AreEqual(0, diagWithoutBridgeBytes.Length);
        }

    }
}
