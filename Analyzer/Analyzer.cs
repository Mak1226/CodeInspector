using Analyzer.Pipeline;
using System;
using System.Collections.Generic;

namespace Analyzer
{
    
    public class Analyzer : IAnalyzer
    {
        private MainPipeline _customAnalyzerPipeline;

        public Analyzer()
        {
            List<string> _pathOfDLLFilesOfStudent;
            IDictionary<int, bool> _teacherOptions;

            _customAnalyzerPipeline = new MainPipeline();
            _pathOfDLLFilesOfStudent = new();
            _teacherOptions = new Dictionary<int, bool> ();

            _customAnalyzerPipeline.AddDLLFiles(_pathOfDLLFilesOfStudent);
            _customAnalyzerPipeline.AddTeacherOptions(_teacherOptions);
        }

        public void Configure(IDictionary<int, bool> TeacherOptions, bool TeacherFlag)
        {
            _customAnalyzerPipeline.AddTeacherOptions(TeacherOptions);
        }

        public void LoadDLLFile(List<string> PathOfDLLFilesOfStudent, string? PathOfDLLFileOfTeacher)
        {
            _customAnalyzerPipeline.AddDLLFiles(PathOfDLLFilesOfStudent);
        }

        public List<AnalyzerResult> Run()
        {
            return _customAnalyzerPipeline.Start();
        }

        public void GetRelationshipGraph()
        {
            return;
        }
    }
}