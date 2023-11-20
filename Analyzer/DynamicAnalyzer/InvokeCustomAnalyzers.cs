using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.DynamicAnalyzer
{
    /// <summary>
    /// Utility class for invoking custom analyzers on student DLL files.
    /// </summary>
    public class InvokeCustomAnalyzers
    {
        public List<string> _pathOfDLLFilesOfCustomAnalyzers;
        public List<string> _pathOfDLLFilesOfStudent;

        /// <summary>
        /// Initializes a new instance of the InvokeCustomAnalyzers class.
        /// </summary>
        /// <param name="PathOfDLLFilesOfCustomAnalyzers">List of paths to custom analyzer DLL files (Teacher added analyzers).</param>
        /// <param name="PathOfDLLFilesOfStudent">List of paths to student DLL files.</param>
        public InvokeCustomAnalyzers(List<string> PathOfDLLFilesOfCustomAnalyzers, List<string> PathOfDLLFilesOfStudent)
        {
            _pathOfDLLFilesOfCustomAnalyzers = PathOfDLLFilesOfCustomAnalyzers;
            _pathOfDLLFilesOfStudent = PathOfDLLFilesOfStudent;
        }

        /// <summary>
        /// Invokes each of the custom analyzers on the student dlls.
        /// </summary>
        /// <returns>A dictionary containing the analysis results for each student DLL file.</returns>

        public Dictionary<string, List<AnalyzerResult>> Start()
        {
            //Parses and stores all the student dll files in a list
            List<ParsedDLLFile> studentParsedDlls = new();

            foreach (string studentDll in _pathOfDLLFilesOfStudent)
            {
                ParsedDLLFile studentParsedDll = new(studentDll);
                studentParsedDlls.Add(studentParsedDll);
            }


            // analyzerResult stores the analysis result of the custom analyzers per students dll.
            Dictionary<string, List<AnalyzerResult>> analyzerResults = new();

            foreach (ParsedDLLFile file in studentParsedDlls)
            {
                analyzerResults[file.DLLFileName] = new List<AnalyzerResult>();
            }

            // Iterate over each custom analyzer DLL
            foreach (string customAnalyzer in _pathOfDLLFilesOfCustomAnalyzers)
            {
                // Load the custom analyzer assembly
                Assembly customAnalyzerAssembly = Assembly.Load(File.ReadAllBytes(customAnalyzer));
                Type type = customAnalyzerAssembly.GetType("Analyzer.DynamicAnalyzer.CustomAnalyzer");

                // Create an instance of the custom analyzer, passing the studentParsedDlls as parameter
                object? teacher = Activator.CreateInstance(type, new Object[] {studentParsedDlls});

                // Invoke the "AnalyzeAllDLLs" method of custom analyzer to run the analyzer logic for each of the students dll and get the result
                MethodInfo method = type.GetMethod("AnalyzeAllDLLs");
                object? currentAnalyzerResult = method.Invoke(teacher, null);

                //res -> Dictionary<string, AnalyzerResult>
                //foreach (var item in currentAnalyzerResult as Dictionary<string, AnalyzerResult>)
                //{
                //    Console.WriteLine(item.Key);
                //    Console.WriteLine(item.Value.AnalyserID);
                //    Console.WriteLine(item.Value.ErrorMessage);
                //    Console.WriteLine(item.Value.Verdict);
                //}

                foreach (KeyValuePair<string, AnalyzerResult> dllResult in currentAnalyzerResult as Dictionary<string, AnalyzerResult>)
                {
                    analyzerResults[dllResult.Key].Add(dllResult.Value);
                }
            }
            return analyzerResults;
        }   

    }
}
