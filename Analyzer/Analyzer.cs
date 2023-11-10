using Analyzer.Pipeline;
using System;
using System.Collections.Generic;

namespace Analyzer
{
    public class Analyzer : IAnalyzer
    {
        private readonly MainPipeline _customAnalyzerPipeline;

        public Analyzer()
        {
            _customAnalyzerPipeline = new MainPipeline(); 
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