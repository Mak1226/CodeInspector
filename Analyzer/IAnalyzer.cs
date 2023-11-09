using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    /// <summary>
    /// Interface for analyzing and comparing files.
    /// </summary>
    public interface IAnalyzer
    {
        /// <summary>
        /// Configures the analyzer with options for the teacher and a flag indicating if it's a teacher.
        /// </summary>
        /// <param name="TeacherOptions">A dictionary of teacher-specific options.</param>
        /// <param name="TeacherFlag">A flag indicating whether the analyzer is being extended by a teacher.</param>
        public void Configure(IDictionary<int, bool> TeacherOptions, bool TeacherFlag);

        /// <summary>
        /// Loads DLL files from the student and teacher for analysis.
        /// </summary>
        /// <param name="PathOfDLLFilesOfStudent">A list of file paths for the student's DLL files.</param>
        /// <param name="PathOfDLLFileOfTeacher">The file path for the teacher's DLL file.</param>
        public void LoadDLLFile(List<string> PathOfDLLFilesOfStudent, string? PathOfDLLFileOfTeacher);

        /// <summary>
        /// Runs the analysis and returns a list of <see cref="AnalyzerResult"/> objects.
        /// </summary>
        /// <returns>
        /// A list of <see cref="AnalyzerResult"/> objects representing the results of the analysis.
        /// </returns>
        public List<AnalyzerResult> Run();

        // TODO : Decide on return type

        /// <summary>
        /// Generates a relationship graph based on the loaded files.
        /// </summary>
        public void GetRelationshipGraph();
    }
}