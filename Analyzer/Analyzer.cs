using Analyzer.DynamicAnalyzer;
using Analyzer.Pipeline;
using System;
using System.Collections.Generic;

namespace Analyzer
{

    public class Analyzer : IAnalyzer
    {
        private List<string> _pathOfDLLFilesOfStudent;
        private IDictionary<int, bool> _teacherOptions;
        private List<string>? _pathOfDLLFilesOfCustomAnalyzers;

        public Analyzer()
        {
            _pathOfDLLFilesOfStudent = new List<string>();
            _teacherOptions = new Dictionary<int, bool>();
            _pathOfDLLFilesOfCustomAnalyzers = new List<string>();
            
        }

        public void Configure(IDictionary<int, bool> TeacherOptions)
        {
            _teacherOptions = TeacherOptions;
        }

        public void LoadDLLFileOfStudent(List<string> PathOfDLLFilesOfStudent)
        {
            _pathOfDLLFilesOfStudent = PathOfDLLFilesOfStudent;
        }

        public void LoadDLLOfCustomAnalyzers(List<string> PathOfDLLFilesOfCustomAnalyzers)
        {
            _pathOfDLLFilesOfCustomAnalyzers = PathOfDLLFilesOfCustomAnalyzers;
        }

        public Dictionary<string, List<AnalyzerResult>> Run()
        {
            MainPipeline _customAnalyzerPipeline = new();
            _customAnalyzerPipeline.AddDLLFiles(_pathOfDLLFilesOfStudent);
            _customAnalyzerPipeline.AddTeacherOptions(_teacherOptions);

            return _customAnalyzerPipeline.Start();
        }

        public Byte[] GetRelationshipGraph(List<string> removableNamespaces)
        {

            MainPipeline _customAnalyzerPipeline = new();
            _customAnalyzerPipeline.AddDLLFiles(_pathOfDLLFilesOfStudent);
            _customAnalyzerPipeline.AddTeacherOptions(_teacherOptions);

            return _customAnalyzerPipeline.GenerateClassDiagram(removableNamespaces);
        }

        public Dictionary<string, List<AnalyzerResult>> RnuCustomAnalyzers()
        {
            return  new InvokeCustomAnalyzers(_pathOfDLLFilesOfCustomAnalyzers, _pathOfDLLFilesOfStudent).Start();
        }
    }
}