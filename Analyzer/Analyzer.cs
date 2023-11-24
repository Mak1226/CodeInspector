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
* 
* Features:
* - Configures the Analyzer with teacher options.
* - Loads DLL files provided by the student for analysis.
* - Loads DLL files of custom analyzers for additional analysis.
* - Runs the main analysis pipeline of 20 Analyzers and returns the results.
* - Generates a relationship graph based on the analysis results with support of removable namespaces.
* - Runs multiple custom analyzers specified by the teacher on students dll files.
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
            Trace.WriteLine("Teacher Options\n");
            foreach (KeyValuePair<int , bool> kvp in TeacherOptions)
            {
                Trace.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}\n");
            }
            _teacherOptions = TeacherOptions;
        }

        /// <summary>
        /// Loads DLL files provided by the student for analysis.
        /// </summary>
        /// <param name="PathOfDLLFilesOfStudent">List of paths to DLL files.</param>
        public void LoadDLLFileOfStudent(List<string> PathOfDLLFilesOfStudent)
        {
            Trace.Write("Analyzer : Loaded students " + string.Join(" ", _pathOfDLLFilesOfStudent) + "\n");
            _pathOfDLLFilesOfStudent = PathOfDLLFilesOfStudent;
        }

        /// <summary>
        /// Loads DLL files of custom analyzers for additional analysis.
        /// </summary>
        /// <param name="PathOfDLLFilesOfCustomAnalyzers">List of paths to DLL files of custom analyzers.</param>
        public void LoadDLLOfCustomAnalyzers(List<string> PathOfDLLFilesOfCustomAnalyzers)
        {
            Trace.Write("Analyzer : Loaded custom analyzers " + string.Join(" ", _pathOfDLLFilesOfStudent) + "\n");
            _pathOfDLLFilesOfCustomAnalyzers = PathOfDLLFilesOfCustomAnalyzers;
        }

        /// <summary>
        /// Runs the main analysis pipeline and returns the results.
        /// </summary>
        /// <returns>Dictionary of analysis results.</returns>
        public Dictionary<string, List<AnalyzerResult>> Run()
        {
            Trace.Write("Analyzer : MainPipeline is starting with " + string.Join(" ", _pathOfDLLFilesOfStudent) + "\n");
            MainPipeline _customAnalyzerPipeline = new();
            _customAnalyzerPipeline.AddDLLFiles(_pathOfDLLFilesOfStudent);
            _customAnalyzerPipeline.AddTeacherOptions(_teacherOptions);
            Dictionary<string, List<AnalyzerResult>> result = _customAnalyzerPipeline.Start();
            Trace.Write("Analyzer : MainPipeline is over for " + string.Join(" ", _pathOfDLLFilesOfStudent) + "\n");

            foreach (KeyValuePair<string , List<AnalyzerResult>> keyValuePair in result)
            {
                foreach (AnalyzerResult analyzerResult in keyValuePair.Value)
                {
                    Trace.Write($"Analyzer : Key: {keyValuePair.Key}");
                    Trace.Write($"  {analyzerResult}\n");
                }
            }

            return result;
        }

        /// <summary>
        /// Generates a relationship graph based on the analysis results.
        /// </summary>
        /// <param name="removableNamespaces">List of namespaces to be excluded from the graph.</param>
        /// <returns>Byte array representing the generated relationship graph.</returns>
        public byte[] GetRelationshipGraph(List<string> removableNamespaces)
        {
            Trace.Write( "Analyzer : Starting Relationship Graph with " + string.Join( " ", _pathOfDLLFilesOfStudent ) + "By removing " + string.Join( " " , removableNamespaces));
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
            Trace.Write("Analyzer : Invoking custom Analysers " + string.Join( " ", _pathOfDLLFilesOfCustomAnalyzers ) + "on students dll file " + string.Join( " ", _pathOfDLLFilesOfStudent));
            Dictionary<string , List<AnalyzerResult>> result = new InvokeCustomAnalyzers( _pathOfDLLFilesOfCustomAnalyzers , _pathOfDLLFilesOfStudent ).Start();
            Trace.Write( "Analyzer : Completed custom Analysers " + string.Join( " " , _pathOfDLLFilesOfCustomAnalyzers ) + "on students dll file " + string.Join( " " , _pathOfDLLFilesOfStudent ) );

            foreach (KeyValuePair<string , List<AnalyzerResult>> keyValuePair in result)
            {
                foreach (AnalyzerResult analyzerResult in keyValuePair.Value)
                {
                    Trace.Write( $"Analyzer : Key: {keyValuePair.Key}");
                    Trace.Write( $"  {analyzerResult}\n" );
                }
            }

            return result;
        }
    }
}
