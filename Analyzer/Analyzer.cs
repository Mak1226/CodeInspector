/******************************************************************************
* Filename    = Analyzer.cs
*
* Author      = Mangesh Dalvi
* 
* Roll No     = 112001010
*
* Product     = Code Inspector
* 
* Project     = Analyzer
*
* Description = Represents the main Analyzer class responsible for orchestrating the analysis process.
******************************************************************************/

using Analyzer.DynamicAnalyzer;
using Analyzer.Pipeline;
using System.Diagnostics;

namespace Analyzer
{
    /// <summary>
    /// Represents the main Analyzer class responsible for orchestrating the analysis process.
    /// </summary>
    public class Analyzer : IAnalyzer
    {
        private List<string> _pathOfDLLFilesOfStudent;
        private IDictionary<int, bool> _teacherOptions;
        private List<string> _pathOfDLLFilesOfCustomAnalyzers;

        /// <summary>
        /// Initializes a new instance of the Analyzer class.
        /// </summary>
        public Analyzer()
        {
            _pathOfDLLFilesOfStudent = new List<string>();
            _teacherOptions = new Dictionary<int, bool>();
            _pathOfDLLFilesOfCustomAnalyzers = new List<string>();
        }

        /// <summary>
        /// Configures the Analyzer with teacher options.
        /// </summary>
        /// <param name="TeacherOptions">Dictionary of teacher options.</param>
        public void Configure(IDictionary<int, bool> TeacherOptions)
        {
            _teacherOptions = TeacherOptions;
        }

        /// <summary>
        /// Loads DLL files provided by the student for analysis.
        /// </summary>
        /// <param name="PathOfDLLFilesOfStudent">List of paths to DLL files.</param>
        public void LoadDLLFileOfStudent(List<string> PathOfDLLFilesOfStudent)
        {
            _pathOfDLLFilesOfStudent = PathOfDLLFilesOfStudent;
        }

        /// <summary>
        /// Loads DLL files of custom analyzers for additional analysis.
        /// </summary>
        /// <param name="PathOfDLLFilesOfCustomAnalyzers">List of paths to DLL files of custom analyzers.</param>
        public void LoadDLLOfCustomAnalyzers(List<string> PathOfDLLFilesOfCustomAnalyzers)
        {
            _pathOfDLLFilesOfCustomAnalyzers = PathOfDLLFilesOfCustomAnalyzers;
        }

        /// <summary>
        /// Runs the main analysis pipeline and returns the results.
        /// </summary>
        /// <returns>Dictionary of analysis results.</returns>
        public Dictionary<string, List<AnalyzerResult>> Run()
        {
            Trace.Write("Analyzers MainPipeline is starting\n");

            MainPipeline _customAnalyzerPipeline = new();
            _customAnalyzerPipeline.AddDLLFiles(_pathOfDLLFilesOfStudent);
            _customAnalyzerPipeline.AddTeacherOptions(_teacherOptions);

            Trace.Write("Analyzers MainPipeline is over\n");

            return _customAnalyzerPipeline.Start();
        }

        /// <summary>
        /// Generates a relationship graph based on the analysis results.
        /// </summary>
        /// <param name="removableNamespaces">List of namespaces to be excluded from the graph.</param>
        /// <returns>Byte array representing the generated relationship graph.</returns>
        public byte[] GetRelationshipGraph(List<string> removableNamespaces)
        {

            MainPipeline _customAnalyzerPipeline = new();
            _customAnalyzerPipeline.AddDLLFiles(_pathOfDLLFilesOfStudent);
            _customAnalyzerPipeline.AddTeacherOptions(_teacherOptions);

            return _customAnalyzerPipeline.GenerateClassDiagram(removableNamespaces);
        }

        /// <summary>
        /// Runs custom analyzers specified by the teacher.
        /// </summary>
        /// <returns>Dictionary of analysis results from custom analyzers.</returns>
        public Dictionary<string, List<AnalyzerResult>> RnuCustomAnalyzers()
        {
            return new InvokeCustomAnalyzers(_pathOfDLLFilesOfCustomAnalyzers, _pathOfDLLFilesOfStudent).Start();
        }
    }
}
