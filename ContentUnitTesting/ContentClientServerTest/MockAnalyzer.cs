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
using System.Drawing;
using System.Drawing.Imaging;
using Analyzer;

namespace ContentUnitTesting.ContentClientServerTest
{
    /// <summary>
    /// A mock implementation of the IAnalyzer interface for unit testing purposes.
    /// </summary>
    internal class MockAnalyzer : IAnalyzer
    {
        /// <summary>
        /// Initializes a new instance of the MockAnalyzer class.
        /// </summary>
        private IDictionary<int, bool> _teacherOptions;
        private List<string> _dllFilePath;
        private List<string> _dllFilePathCustom;
        bool _isGraph;
        public MockAnalyzer()
        {
            _teacherOptions = new Dictionary<int, bool>();
            _dllFilePath = new List<string>();
            _isGraph = false;
        }
        /// <summary>
        /// Configures the mock analyzer with the specified teacher options.
        /// </summary>
        /// <param name="TeacherOptions">The dictionary containing teacher 
        /// configuration options.</param>
        public void Configure(IDictionary<int, bool> TeacherOptions)
        {
            _teacherOptions = TeacherOptions;
        }
        /// <summary>
        /// Retrieves the teacher configuration options set for the mock analyzer.
        /// </summary>
        /// <returns>The dictionary containing teacher configuration options.</returns>
        public IDictionary<int, bool> GetTeacherOptions()
        {
            return _teacherOptions;
        }
        public void SetRelationshipGraph(bool val)
        {
            if(val == true)
            {
                _isGraph = true;
            }
        }
        /// <summary>
        /// Placeholder implementation that throws a NotImplementedException.
        /// </summary>
        public byte[] GetRelationshipGraph( List<string> removableNamespaces )
        {
            if (_isGraph == false)
            {
                return Array.Empty<byte>();
            }
            else
            {
                string testDirectory = Directory.GetParent( Environment.CurrentDirectory ).Parent.Parent.FullName;
                string imagePath = Path.Combine( testDirectory , "TestImage\\test_image.jpg" );
                Image image = Image.FromFile( imagePath );
                // Create a MemoryStream
                MemoryStream ms = new();
                // Save the image to the MemoryStream with the desired format (e.g., JPEG)
                image.Save( ms , ImageFormat.Jpeg );
                // Convert the MemoryStream to byte[]
                byte[] byteStream = ms.ToArray(); ;
                return byteStream;
            }
        }

        /// <summary>
        /// Loads the paths of DLL files provided by a student for analysis.
        /// </summary>
        /// <param name="PathOfDLLFilesOfStudent">The list of paths to DLL files submitted by the student.</param>
        public void LoadDLLFileOfStudent(List<string> PathOfDLLFilesOfStudent)
        {
            _dllFilePath = PathOfDLLFilesOfStudent;
        }

        /// <summary>
        /// Retrieves the list of DLL file paths loaded by the mock analyzer.
        /// </summary>
        /// <returns>The list of paths to DLL files loaded by the analyzer.</returns>
        public List<string> GetDllFilePath()
        {
            return _dllFilePath;
        }

        /// <summary>
        /// Loads the DLL files of custom analyzers in the MockAnalyzer class.
        /// </summary>
        /// <param name="PathOfDLLFilesOfCustomAnalyzers">
        /// List of paths to DLL files of custom analyzers.</param>
        public void LoadDLLOfCustomAnalyzers(List<string> PathOfDLLFilesOfCustomAnalyzers)
        {
            _dllFilePathCustom = PathOfDLLFilesOfCustomAnalyzers;
        }

        /// <summary>
        /// Gets the paths of DLL files of custom analyzers in the 
        /// MockAnalyzer class.
        /// </summary>
        /// <returns>List of paths to DLL files of custom analyzers.</returns>
        public List<string> GetDLLOfCustomAnalyzers()
        {
            return _dllFilePathCustom;
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
