/******************************************************************************
 * Filename     = MockAnalyzer.cs
 * 
 * Author       = Lekshmi
 *
 * Product      = Analyzer
 * 
 * Project      = ContentUnitTesting
 *
 * Description  = MockAnalyzer for Unit Testing
*****************************************************************************/
using Analyzer;

namespace ContentUnitTesting.ContentClientServerTest
{
    /// <summary>
    /// A mock implementation of the IAnalyzer interface for unit testing purposes.
    /// </summary>
    internal class MockAnalyzer : IAnalyzer
    {
        private IDictionary<int, bool> teacherOptions;
        private List<string> dllFilePath;
        /// <summary>
        /// Initializes a new instance of the MockAnalyzer class.
        /// </summary>
        public MockAnalyzer()
        {
            teacherOptions = new Dictionary<int, bool>();
            dllFilePath = new List<string>();
        }
        /// <summary>
        /// Configures the mock analyzer with the specified teacher options.
        /// </summary>
        /// <param name="TeacherOptions">The dictionary containing teacher 
        /// configuration options.</param>
        public void Configure(IDictionary<int, bool> TeacherOptions)
        {
            teacherOptions = TeacherOptions;
        }
        /// <summary>
        /// Retrieves the teacher configuration options set for the mock analyzer.
        /// </summary>
        /// <returns>The dictionary containing teacher configuration options.</returns>
        public IDictionary<int, bool> GetTeacherOptions()
        {
            return teacherOptions;
        }
        /// <summary>
        /// Placeholder implementation that throws a NotImplementedException.
        /// </summary>
        public byte[] GetRelationshipGraph(List<string> removableNamespaces)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads the paths of DLL files provided by a student for analysis.
        /// </summary>
        /// <param name="PathOfDLLFilesOfStudent">The list of paths to DLL files submitted by the student.</param>
        public void LoadDLLFileOfStudent(List<string> PathOfDLLFilesOfStudent)
        {
            dllFilePath = PathOfDLLFilesOfStudent;
        }

        /// <summary>
        /// Retrieves the list of DLL file paths loaded by the mock analyzer.
        /// </summary>
        /// <returns>The list of paths to DLL files loaded by the analyzer.</returns>
        public List<string> GetDllFilePath()
        {
            return dllFilePath;
        }

        /// <summary>
        /// Placeholder implementation that throws a NotImplementedException.
        /// </summary>
        public void LoadDLLOfCustomAnalyzers(List<string> PathOfDLLFilesOfCustomAnalyzers)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Runs custom analyzers and returns the results as a dictionary.
        /// </summary>
        /// <returns>The dictionary containing the results of custom analyzers.</returns>
        public Dictionary<string, List<AnalyzerResult>> RnuCustomAnalyzers()
        {
            return new Dictionary<string, List<AnalyzerResult>>
            {
                { "File1", new List<AnalyzerResult> { new AnalyzerResult("AnalyzerCustom", 1, "Some errors") } },
                // Add more initial values as needed
            };
        }

        /// <summary>
        /// Runs the default analyzer and returns the results as a dictionary.
        /// </summary>
        /// <returns>The dictionary containing the results of the default analyzer.</returns>
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
