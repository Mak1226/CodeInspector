using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.DynamicAnalyzer
{
    public class InvokeCustomAnalyzers
    {
        public List<string> _pathOfDLLFilesOfCustomAnalyzers;
        public List<string> _pathOfDLLFilesOfStudent;

        public InvokeCustomAnalyzers(List<string> PathOfDLLFilesOfCustomAnalyzers, List<string> PathOfDLLFilesOfStudent)
        {
            _pathOfDLLFilesOfCustomAnalyzers = PathOfDLLFilesOfCustomAnalyzers;
            _pathOfDLLFilesOfStudent = PathOfDLLFilesOfStudent;
        }

        public Dictionary<string, List<AnalyzerResult>> Start()
        {
            List<ParsedDLLFile> studentParsedDlls = new List<ParsedDLLFile>();
            Dictionary<string, List<AnalyzerResult> > analyzerResults = new();


            foreach (string studentDll in _pathOfDLLFilesOfStudent)
            {
                ParsedDLLFile studentParsedDll = new ParsedDLLFile(studentDll);
                studentParsedDlls.Add(studentParsedDll);
            }

            foreach (ParsedDLLFile file in studentParsedDlls)
            {
                analyzerResults[file.DLLFileName] = new List<AnalyzerResult>();
            }

            foreach (string customAnalyzer in _pathOfDLLFilesOfCustomAnalyzers)
            {

                Assembly customAnalyzerAssembly = Assembly.Load(File.ReadAllBytes(customAnalyzer));

                Type type = customAnalyzerAssembly.GetType("Analyzer.DynamicAnalyzer.CustomAnalyzer");

                var teacher = Activator.CreateInstance(type, new Object[] {studentParsedDlls});

                MethodInfo method = type.GetMethod("AnalyzeAllDLLs");

                var currentAnalyzerResult = method.Invoke(teacher, null);

                //res -> Dictionary<string, AnalyzerResult>
                foreach (var item in currentAnalyzerResult as Dictionary<string, AnalyzerResult>)
                {
                    Console.WriteLine(item.Key);
                    Console.WriteLine(item.Value.AnalyserID);
                    Console.WriteLine(item.Value.ErrorMessage);
                    Console.WriteLine(item.Value.Verdict);
                }

                foreach (KeyValuePair<string, AnalyzerResult> dllResult in currentAnalyzerResult as Dictionary<string, AnalyzerResult>)
                {
                    analyzerResults[dllResult.Key].Add(dllResult.Value);
                }
            }

            return analyzerResults;
        }   

    }
}