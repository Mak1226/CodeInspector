using Analyzer.Pipeline;
using System;
using System.Collections.Generic;

namespace Analyzer
{

    public class Analyzer : IAnalyzer
    {
        private List<string> _pathOfDLLFilesOfStudent;
        private IDictionary<int, bool> _teacherOptions;

        public Analyzer()
        {
            _pathOfDLLFilesOfStudent = new List<string>();
            _teacherOptions = new Dictionary<int, bool>();
        }

        public void Configure(IDictionary<int, bool> TeacherOptions, bool TeacherFlag)
        {
            _teacherOptions = TeacherOptions;
        }

        public void LoadDLLFile(List<string> PathOfDLLFilesOfStudent, string? PathOfDLLFileOfTeacher)
        {
            _pathOfDLLFilesOfStudent = PathOfDLLFilesOfStudent;
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
    }
}