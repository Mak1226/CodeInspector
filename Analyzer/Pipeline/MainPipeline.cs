using Analyzer.Parsing;
using Analyzer.UMLDiagram;
using Microsoft.VisualBasic.FileIO;
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
        private readonly List<ParsedDLLFile> _parsedDLLFiles;
        private readonly Dictionary<string, List<AnalyzerResult>> _results;
        private readonly object _lock;

        public MainPipeline()
        {
            _allAnalyzers = new();
            _teacherOptions = new Dictionary<int, bool> ();
            _studentDLLFiles = new List<string>();
            _parsedDLLFiles = new List<ParsedDLLFile> ();
            _results = new();
            _lock = new object();
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
            foreach (string file in _studentDLLFiles)
            {
                _parsedDLLFiles.Add(new ParsedDLLFile(file));
            }

            _allAnalyzers[101] = new AbstractTypeNoPublicConstructor(_parsedDLLFiles);
            _allAnalyzers[102] = new AvoidConstructorsInStaticTypes(_parsedDLLFiles);
            _allAnalyzers[103] = new AvoidUnusedPrivateFieldsRule(_parsedDLLFiles);
            _allAnalyzers[104] = new NoEmptyInterface(_parsedDLLFiles);
            _allAnalyzers[105] = new DepthOfInheritance(_parsedDLLFiles);
            _allAnalyzers[106] = new ArrayFieldsShouldNotBeReadOnlyRule(_parsedDLLFiles);
            _allAnalyzers[107] = new AvoidSwitchStatementsAnalyzer(_parsedDLLFiles);
            _allAnalyzers[108] = new DisposableFieldsShouldBeDisposedRule(_parsedDLLFiles);
            _allAnalyzers[109] = new RemoveUnusedLocalVariablesRule(_parsedDLLFiles);
            _allAnalyzers[110] = new ReviewUselessControlFlowRule(_parsedDLLFiles);
            _allAnalyzers[111] = new AbstractClassNamingChecker(_parsedDLLFiles);
            _allAnalyzers[112] = new CasingChecker(_parsedDLLFiles);
            _allAnalyzers[113] = new CyclomaticComplexity(_parsedDLLFiles);
            _allAnalyzers[114] = new NewLineLiteralRule(_parsedDLLFiles);
            _allAnalyzers[115] = new PrefixCheckerAnalyzer(_parsedDLLFiles);
            _allAnalyzers[116] = new SwitchStatementDefaultCaseChecker(_parsedDLLFiles);
            _allAnalyzers[117] = new AvoidGotoStatementsAnalyzer(_parsedDLLFiles);
            _allAnalyzers[118] = new NativeFieldsShouldNotBeVisible(_parsedDLLFiles);
        }

        private void RunAnalyzer(int analyzerID)
        {
            Dictionary<string, AnalyzerResult> currentAnalyzerResult;

            try
            {
                currentAnalyzerResult = _allAnalyzers[analyzerID].AnalyzeAllDLLs();
            }
            catch (Exception e)
            {
                currentAnalyzerResult = new Dictionary<string, AnalyzerResult>();

                string errorMsg = "Internal error, analyzer failed to execute";

                // check analyzerID is present in _allAnalyzers as key
                if (!_allAnalyzers.ContainsKey(analyzerID))
                {
                    errorMsg = "Analyser does not exists";
                }

                foreach (ParsedDLLFile dllFile in _parsedDLLFiles)
                {
                    currentAnalyzerResult[dllFile.DLLFileName] = new AnalyzerResult(analyzerID.ToString(), 1, errorMsg);
                }
            }

            foreach (KeyValuePair<string, AnalyzerResult> dllResult in currentAnalyzerResult)
            {
                lock (_lock) {
                    _results[dllResult.Key].Add(dllResult.Value);
                }
            }
        }

        /// <summary>
        /// Starts the pipeline and runs all of the analyzers that have been selected by teacher
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<AnalyzerResult>> Start()
        {

            List<Thread> threads = new();

            foreach (ParsedDLLFile file in _parsedDLLFiles)
            {
                _results[file.DLLFileName] = new List<AnalyzerResult>();
            }

            foreach(KeyValuePair<int,bool> option in _teacherOptions)
            {
                if(option.Value == true)
                {
                    Thread WorkerThread = new(() => RunAnalyzer(option.Key));
                    WorkerThread.Start();
                    threads.Add(WorkerThread);
                }
            }    

            foreach(Thread workerThread in threads)
            {
                workerThread.Join();
            }

            return _results;
        }

        public byte[] GenerateClassDiagram(List<string> removableNamespaces)
        {
            // TODO: Call ClassDiagram.Run() after modifications
            ClassDiagram classDiag = new(_parsedDLLFiles);
            byte[] bytes = classDiag.Run(removableNamespaces).Result;
            return bytes;
        }
    }
}
