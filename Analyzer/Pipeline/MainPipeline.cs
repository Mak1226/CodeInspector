using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Pipeline
{
    public class MainPipeline
    {

        private IDictionary<int, bool> _teacherOptions;
        private List<string> _studentDLLFiles;
        private Dictionary<string, AnalyzerBase> _allAnalyzers;

        public MainPipeline()
        {
            _allAnalyzers = new();
            _teacherOptions = new Dictionary<int, bool> ();
            _studentDLLFiles = new List<string>();
        }


        public void AddTeacherOptions(IDictionary<int, bool> TeacherOptions)
        {
            _teacherOptions = TeacherOptions;
        }

        public void AddDLLFiles(List<string> PathOfDLLFilesOfStudent)
        {
            _studentDLLFiles = PathOfDLLFilesOfStudent;
            GenerateAnalysers();
        }

        private void GenerateAnalysers()
        {
            ParsedDLLFiles parsedDLLFiles = new(_studentDLLFiles);

            _allAnalyzers["103"] = new AvoidUnusedPrivateFieldsRule(parsedDLLFiles);
        }

        public List<AnalyzerResult> Start()
        {
            List<AnalyzerResult> results = new();

            foreach(var flag in _teacherOptions)
            {
                if(flag.Value)
                {
                    results.Add(_allAnalyzers[flag.Key.ToString()].Run());
                }
            }

            return results;
        }

    }
}