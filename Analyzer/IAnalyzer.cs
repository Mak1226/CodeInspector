/******************************************************************************
* Filename    = IAnalyzer.cs
*
* Author      = Mangesh Dalvi
* 
* Roll No     = 112001010
*
* Product     = Code Inspector
* 
* Project     = Analyzer
*
* Description = Represents the main Analyzer interface responsible for orchestrating the analysis process.
******************************************************************************/

namespace Analyzer
{
    /// <summary>
    /// Represents the main Analyzer interface responsible for orchestrating the analysis process.
    /// </summary>
    public interface IAnalyzer
    {
        /// <summary>
        /// Configures the Analyzer with teacher options.
        /// </summary>
        /// <param name="teacherOptions">Dictionary of teacher options.</param>
        public void Configure(IDictionary<int, bool> teacherOptions);

        /// <summary>
        /// Loads DLL files provided by the student for analysis.
        /// </summary>
        /// <param name="pathOfDLLFilesOfStudent">List of paths to DLL files.</param>
        public void LoadDLLFileOfStudent(List<string> pathOfDLLFilesOfStudent);

        /// <summary>
        /// Loads DLL files of custom analyzers for additional analysis.
        /// </summary>
        /// <param name="pathOfDLLFilesOfCustomAnalyzers">List of paths to DLL files of custom analyzers.</param>
        public void LoadDLLOfCustomAnalyzers(List<string> pathOfDLLFilesOfCustomAnalyzers);

        /// <summary>
        /// Runs the main analysis pipeline and returns the results.
        /// </summary>
        /// <returns>Dictionary of analysis results.</returns>
        public Dictionary<string, List<AnalyzerResult>> Run();

        /// <summary>
        /// Generates a relationship graph based on the analysis results.
        /// </summary>
        /// <param name="removableNamespaces">List of namespaces to be excluded from the graph.</param>
        /// <returns>Byte array representing the generated relationship graph.</returns>
        public byte[] GetRelationshipGraph(List<string> removableNamespaces);

        /// <summary>
        /// Runs custom analyzers specified by the teacher.
        /// </summary>
        /// <returns>Dictionary of analysis results from custom analyzers.</returns>
        public Dictionary<string, List<AnalyzerResult>> RnuCustomAnalyzers();
    }
}
