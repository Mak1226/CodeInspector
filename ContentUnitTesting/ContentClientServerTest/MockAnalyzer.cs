using Analyzer;

namespace ContentUnitTesting.ContentClientServerTest
{
    internal class MockAnalyzer : IAnalyzer
    {
        private IDictionary<int, bool> _teacherOptions;
        private List<string> _dllFilePath;
        private List<string> _dllFilePathCustom;
        public MockAnalyzer()
        {
            _teacherOptions = new Dictionary<int, bool>();
            _dllFilePath = new List<string>();
        }
        public void Configure(IDictionary<int, bool> TeacherOptions)
        {
            _teacherOptions = TeacherOptions;
        }

        public IDictionary<int, bool> GetTeacherOptions()
        {
            return _teacherOptions;
        }

        public byte[] GetRelationshipGraph(List<string> removableNamespaces)
        {
            throw new NotImplementedException();
        }

        public void LoadDLLFileOfStudent(List<string> PathOfDLLFilesOfStudent)
        {
            _dllFilePath = PathOfDLLFilesOfStudent;
        }

        public List<string> GetDllFilePath()
        {
            return _dllFilePath;
        }

        public void LoadDLLOfCustomAnalyzers(List<string> PathOfDLLFilesOfCustomAnalyzers)
        {
            _dllFilePathCustom = PathOfDLLFilesOfCustomAnalyzers;
        }

        public List<string> GetDLLOfCustomAnalyzers()
        {
            return _dllFilePathCustom;
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
