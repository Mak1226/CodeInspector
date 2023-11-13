using Analyzer.Pipeline;
using System;
using System.Collections.Generic;

namespace Analyzer
{
    
    public class Analyzer : IAnalyzer
    {
        private MainPipeline _customAnalyzerPipeline;
        List<string> _pathOfDLLFilesOfStudent;
        IDictionary<int, bool> _teacherOptions;

        public Analyzer()
        {
            
        }

        public void Configure(IDictionary<int, bool> TeacherOptions, bool TeacherFlag)
        {
            _teacherOptions = TeacherOptions;
            //_customAnalyzerPipeline.AddTeacherOptions(TeacherOptions);
        }

        public void LoadDLLFile(List<string> PathOfDLLFilesOfStudent, string? PathOfDLLFileOfTeacher)
        {
            _pathOfDLLFilesOfStudent = PathOfDLLFilesOfStudent;
            //_customAnalyzerPipeline.AddDLLFiles(PathOfDLLFilesOfStudent);
        }

        public List<AnalyzerResult> Run()
        {
            _customAnalyzerPipeline = new MainPipeline();
            _customAnalyzerPipeline.AddTeacherOptions(_teacherOptions);
            _customAnalyzerPipeline.AddDLLFiles(_pathOfDLLFilesOfStudent);
            return _customAnalyzerPipeline.Start();
        }

        public void GetRelationshipGraph()
        {
            return;
        }
    }
}