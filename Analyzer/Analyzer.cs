/*
using Analyzer.Parsing;
using Analyzer.Pipeline;

namespace Analyzer
{
    public class Analyzer : IAnalyzer
    {

        IDictionary <int, bool> _teacherOptions;
        bool _teacherFlag;
        ParsedDLLFiles _parsedDLLFiles;

        Analyzer()
        {

        }

        public void Configure(IDictionary<int, bool> TeacherOptions, bool TeacherFlag)
        {
            _teacherOptions = TeacherOptions;
            _teacherFlag = TeacherFlag;
        }

        public void LoadDLLFile(List<string> PathOfDLLFilesOfStudent, string? PathOfDLLFileOfTeacher)
        {
            _parsedDLLFiles = new ParsedDLLFiles(PathOfDLLFilesOfStudent);

            if(_teacherFlag)
            {

            }

        }

        public Tuple<IDictionary<string, string>, int> GetAnalysis()
        {
            return null;
        }

        public void GetRelationshipGraph()
        {

            //MainPipeline mp = new MainPipeline();

        }
    }
}
*/