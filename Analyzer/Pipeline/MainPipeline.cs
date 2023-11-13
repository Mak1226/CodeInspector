using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Pipeline
{

    /// <summary>
    /// The main pipeline for running analyzers
    /// </summary>
    public class MainPipeline
    {

        private IDictionary<int, bool> _teacherOptions;
        private List<string> _studentDLLFiles;
        private readonly Dictionary<int, AnalyzerBase> _allAnalyzers;

        public MainPipeline()
        {
            _allAnalyzers = new();
            _teacherOptions = new Dictionary<int, bool> ();
            _studentDLLFiles = new List<string>();
        }

        /// <summary>
        /// Adds the given teacher options to the pipeline.
        /// </summary>
        /// <param name="TeacherOptions">The teacher options to add</param>
        public void AddTeacherOptions(IDictionary<int, bool> TeacherOptions)
        {
            _teacherOptions = TeacherOptions;
        }

        /// <summary>
        /// Adds the given student DLL files to the pipeline.
        /// </summary>
        /// <param name="PathOfDLLFilesOfStudent">The paths to the student DLL files to add</param>
        public void AddDLLFiles(List<string> PathOfDLLFilesOfStudent)
        {
            _studentDLLFiles = PathOfDLLFilesOfStudent;
            GenerateAnalysers();
        }

        /// <summary>
        /// Generates the analyzers that will be run by the pipeline.
        /// </summary>
        private void GenerateAnalysers()
        {
            ParsedDLLFiles parsedDLLFiles = new(_studentDLLFiles);

            _allAnalyzers[101] = new AbstractTypeNoPublicConstructor(parsedDLLFiles);
            _allAnalyzers[102] = new AvoidConstructorsInStaticTypes(parsedDLLFiles);
            _allAnalyzers[103] = new AvoidUnusedPrivateFieldsRule(parsedDLLFiles);
            _allAnalyzers[104] = new NoEmptyInterface(parsedDLLFiles);
            _allAnalyzers[105] = new DepthOfInheritance(parsedDLLFiles);
            _allAnalyzers[106] = new ArrayFieldsShouldNotBeReadOnlyRule(parsedDLLFiles);
            _allAnalyzers[107] = new AvoidSwitchStatementsAnalyzer(parsedDLLFiles);
            _allAnalyzers[108] = new DisposableFieldsShouldBeDisposedRule(parsedDLLFiles);
            _allAnalyzers[109] = new RemoveUnusedLocalVariablesRule(parsedDLLFiles);
            _allAnalyzers[110] = new ReviewUselessControlFlowRule(parsedDLLFiles);
        }

        /// <summary>
        /// Starts the pipeline and runs all of the analyzers that have been selected by teacher
        /// </summary>
        /// <returns>A list of analyzer results, where each result represents the results of running one analyzer</returns>
        public List<AnalyzerResult> Start()
        {
            List<AnalyzerResult> results = new();

            foreach(var option in _teacherOptions)
            {
                if(option.Value == true)
                {
                    AnalyzerResult currentResult;

                    try
                    {
                        currentResult = _allAnalyzers[option.Key].Run();
                    }
                    catch(Exception _)
                    {
                        currentResult = new AnalyzerResult(option.Key.ToString(), 1, "Internal error, analyzer failed to execute");
                    }

                    results.Add(currentResult);
                }
            }

            return results;
        }

    }
}