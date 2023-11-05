using System;
using System.Collections.Generic;

namespace Analyzer
{
    /// <summary>
    /// Dummy implementation
    /// </summary>
    public class Analyzer : IAnalyzer
    {
        private IDictionary<int, bool> _teacherOptions;
        private bool _teacherFlag;
        private List<string> _studentDLLFiles;
        private string _teacherDLLFile;

        public Analyzer()
        {
            _teacherFlag = false;
            _studentDLLFiles = new List<string>();
            _teacherOptions = new Dictionary<int, bool>();
            _teacherDLLFile = "";
        }

        public void Configure(IDictionary<int, bool> TeacherOptions, bool TeacherFlag)
        {
            _teacherOptions = TeacherOptions;
            _teacherFlag = TeacherFlag;
        }

        public void LoadDLLFile(List<string> PathOfDLLFilesOfStudent, string? PathOfDLLFileOfTeacher)
        {
            _studentDLLFiles = PathOfDLLFilesOfStudent;
            _teacherDLLFile = PathOfDLLFileOfTeacher ?? string.Empty;
        }

        public Tuple<Dictionary<string, string>, int> GetAnalysis()
        {
            var analysisResults = new Dictionary<string, string>
            {
                { "A1", "Passed" },
                { "A2", "Failed" }
            };

            int score = 85;

            return Tuple.Create(analysisResults, score);
        }

        public void GetRelationshipGraph()
        {
            return;
        }
    }
}