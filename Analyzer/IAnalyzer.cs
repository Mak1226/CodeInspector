using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    /// <summary>
    /// Represents the configuration options for the analyzer.
    /// </summary>
    public struct ConfigureOptions
    {
        public int AnalysisID;
        public List<string> PathOfDLLFilesOfStudent;
    }

    /// <summary>
    /// Represents the configuration options for a teacher.
    /// </summary>
    public struct ConfigureOptionsForTeacher
    {
        public string PathOfTeacherDLLFile;
        public List<string> PathOfDLLFilesOfStudent;
    }

    /// <summary>
    /// Represents the result of an analysis.
    /// </summary>
    public struct AnalysisResult
    {
        public bool Flag;
    }

    /// <summary>
    /// Defines the contract for an analyzer.
    /// </summary>
    public interface IAnalyzer
    {
        /// <summary>
        /// Performs the analysis based on the provided configurations.
        /// </summary>
        /// <param name="Configurations">The configuration options for analysis.</param>
        /// <returns>The result of the analysis.</returns>
        public AnalysisResult GetAnalysis(ConfigureOptions Configurations);

        /// <summary>
        /// Performs custom analysis for a teacher based on the provided configurations.
        /// </summary>
        /// <param name="ConfigureOptionsForTeacher">The configuration options for a teacher's custom analysis.</param>
        /// <returns>The result of the custom analysis.</returns>
        public AnalysisResult CustomAnalysis(ConfigureOptionsForTeacher Configurations);

        // TODO : Decide on return type

        /// <summary>
        /// Generates a relationship graph based on the loaded files.
        /// </summary>
        /// <param name="PathOfDLLFilesOfStudent">The list of paths to DLL files of students.</param>
        public void GetRelationshipGraph(List<string> PathOfDLLFilesOfStudent);
    }
}