using Analyzer;

namespace ContentUnitTesting.ContentClientServerTest
{
    internal class MockAnalyzer : IAnalyzer
    {
        public void Configure(IDictionary<int, bool> TeacherOptions)
        {
            throw new NotImplementedException();
        }

        public byte[] GetRelationshipGraph(List<string> removableNamespaces)
        {
            throw new NotImplementedException();
        }

        public void LoadDLLFileOfStudent(List<string> PathOfDLLFilesOfStudent)
        {
            throw new NotImplementedException();
        }

        public void LoadDLLOfCustomAnalyzers(List<string> PathOfDLLFilesOfCustomAnalyzers)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<AnalyzerResult>> RnuCustomAnalyzers()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<AnalyzerResult>> Run()
        {
            throw new NotImplementedException();
        }
    }
}
