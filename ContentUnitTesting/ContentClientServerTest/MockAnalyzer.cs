using Analyzer;

namespace ContentUnitTesting.ContentClientServerTest
{
    internal class MockAnalyzer : IAnalyzer
    {
        private IDictionary<int, bool> teacherOptions;
        private List<string> dllFilePath;
        public MockAnalyzer()
        {
            teacherOptions = new Dictionary<int, bool>();
            dllFilePath = new List<string>();
        }
        public void Configure(IDictionary<int, bool> TeacherOptions)
        {
            teacherOptions = TeacherOptions;
        }

        public IDictionary<int, bool> GetTeacherOptions()
        {
            return teacherOptions;
        }

        public byte[] GetRelationshipGraph(List<string> removableNamespaces)
        {
            throw new NotImplementedException();
        }

        public void LoadDLLFileOfStudent(List<string> PathOfDLLFilesOfStudent)
        {
            dllFilePath = PathOfDLLFilesOfStudent;
        }

        public List<string> GetDllFilePath()
        {
            return dllFilePath;
        }

        public void LoadDLLOfCustomAnalyzers(List<string> PathOfDLLFilesOfCustomAnalyzers)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<AnalyzerResult>> RnuCustomAnalyzers()
        {
            return new Dictionary<string, List<AnalyzerResult>>
            {
                { "File1", new List<AnalyzerResult> { new AnalyzerResult("AnalyzerCustom", 1, "Some errors") } },
                // Add more initial values as needed
            };
        }

        public Dictionary<string, List<AnalyzerResult>> Run()
        {
            return new Dictionary<string, List<AnalyzerResult>>
            {
                { "File1", new List<AnalyzerResult> { new AnalyzerResult("Analyzer1", 1, "No errors") } },
                // Add more initial values as needed
            }; 
        }
    }
}
