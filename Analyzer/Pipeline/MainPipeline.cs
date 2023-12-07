/******************************************************************************
* Filename    = MainPipeline.cs
*
* Author      = Mangesh Dalvi, Nikhitha
* 
* Roll No     = 112001010, 112001009
*
* Product     = Code Inspector
* 
* Project     = Analyzer
*
* Description = Represents the main pipeline for orchestrating the analysis process.
******************************************************************************/

using Analyzer.Parsing;
using Analyzer.UMLDiagram;
using System.Diagnostics;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// Represents the main pipeline for orchestrating the analysis process.
    /// </summary>
    public class MainPipeline
    {
        private IDictionary<int, bool> _teacherOptions;
        private List<string> _studentDLLFiles;
        private readonly Dictionary<int, AnalyzerBase> _allAnalyzers;
        private readonly List<ParsedDLLFile> _parsedDLLFiles;
        private readonly Dictionary<string, List<AnalyzerResult>> _results;
        private readonly object _lock;

        /// <summary>
        /// Initializes a new instance of the MainPipeline class.
        /// </summary>
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
        /// Adds teacher options to the pipeline.
        /// </summary>
        /// <param name="TeacherOptions">Dictionary of teacher options.</param>
        public void AddTeacherOptions(IDictionary<int, bool> TeacherOptions)
        {
            _teacherOptions = TeacherOptions;
        }

        /// <summary>
        /// Adds DLL files to the pipeline for analysis.
        /// </summary>
        /// <param name="PathOfDLLFilesOfStudent">List of paths to DLL files.</param>
        public void AddDLLFiles(List<string> PathOfDLLFilesOfStudent)
        {
            _studentDLLFiles = PathOfDLLFilesOfStudent;
            GenerateAnalysers();
        }

        /// <summary>
        /// Initializes the analyzers based on the provided DLL files.
        /// </summary>
        private void GenerateAnalysers()
        {
            foreach (string file in _studentDLLFiles)
            {
                try
                {
                    _parsedDLLFiles.Add(new ParsedDLLFile(file));
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"MainPipeline : Failed to parse {Path.GetFileName(file)}. Exception {ex.GetType().Name} : {ex.Message}");
                }
            }

            Trace.Write("MainPipeline : Generating instance of Analyzers\n");
            _allAnalyzers[101] = new AbstractTypeNoPublicConstructor(_parsedDLLFiles);
            _allAnalyzers[102] = new AvoidConstructorsInStaticTypes(_parsedDLLFiles);
            _allAnalyzers[103] = new AvoidUnusedPrivateFieldsRule(_parsedDLLFiles);
            _allAnalyzers[104] = new NoEmptyInterface(_parsedDLLFiles);
            _allAnalyzers[105] = new DepthOfInheritance(_parsedDLLFiles);
            _allAnalyzers[106] = new ArrayFieldsShouldNotBeReadOnlyRule(_parsedDLLFiles);
            _allAnalyzers[107] = new AvoidSwitchStatementsAnalyzer(_parsedDLLFiles);
            _allAnalyzers[108] = new DisposableFieldsShouldBeDisposedRule(_parsedDLLFiles);
            _allAnalyzers[109] = new RemoveUnusedLocalVariablesRule(_parsedDLLFiles);
            _allAnalyzers[110] = new AsyncMethodAnalyzer( _parsedDLLFiles);
            _allAnalyzers[111] = new AbstractClassNamingChecker(_parsedDLLFiles);
            _allAnalyzers[112] = new CasingChecker(_parsedDLLFiles);
            _allAnalyzers[113] = new CyclomaticComplexity(_parsedDLLFiles);
            _allAnalyzers[114] = new NewLineLiteralRule(_parsedDLLFiles);
            _allAnalyzers[115] = new PrefixCheckerAnalyzer(_parsedDLLFiles);
            _allAnalyzers[116] = new SwitchStatementDefaultCaseChecker(_parsedDLLFiles);
            _allAnalyzers[117] = new AvoidGotoStatementsAnalyzer(_parsedDLLFiles);
            _allAnalyzers[118] = new NoVisibleInstanceFields(_parsedDLLFiles);
            _allAnalyzers[119] = new HighParameterCountRule(_parsedDLLFiles);
            _allAnalyzers[120] = new NotImplementedChecker(_parsedDLLFiles);
            Trace.Write("MainPipeline : All Analyzers Generated\n");
        }

        /// <summary>
        /// Runs the specified analyzer on all parsed DLL files.
        /// </summary>
        /// <param name="analyzerID">Identifier of the analyzer to run.</param>
        private void RunAnalyzer(int analyzerID)
        {
            Trace.WriteLine("MainPipeline : Calling analyzer " + analyzerID);

            Dictionary<string, AnalyzerResult> currentAnalyzerResult;

            try
            {
                currentAnalyzerResult = _allAnalyzers[analyzerID].AnalyzeAllDLLs();
                Trace.WriteLine("MainPipeline : Succeed analyzer " + analyzerID);
            }
            catch (KeyNotFoundException)
            {
                currentAnalyzerResult = new Dictionary<string, AnalyzerResult>();
                string errorMsg = "Analyser does not exist";

                foreach (ParsedDLLFile dllFile in _parsedDLLFiles)
                {
                    currentAnalyzerResult[dllFile.DLLFileName] = new AnalyzerResult(analyzerID.ToString(), 1, errorMsg);
                }
            }

            foreach (KeyValuePair<string, AnalyzerResult> dllResult in currentAnalyzerResult)
            {
                lock (_lock) 
                {
                    _results[dllResult.Key].Add(dllResult.Value);
                }
            }
        }

        /// <summary>
        /// Starts the analysis process using multiple threads.
        /// </summary>
        /// <returns>Dictionary of analysis results.</returns>
        public Dictionary<string, List<AnalyzerResult>> Start()
        {

            List<Thread> threads = new();

            foreach (ParsedDLLFile file in _parsedDLLFiles)
            {
                _results[file.DLLFileName] = new List<AnalyzerResult>();
            }
            
            if (_parsedDLLFiles.Count != 0)
            {
                foreach (KeyValuePair<int , bool> option in _teacherOptions)
                {
                    if (option.Value == true)
                    {
                        Thread WorkerThread = new( () => RunAnalyzer( option.Key ) );
                        WorkerThread.Start();
                        threads.Add( WorkerThread );
                    }
                }
            }    

            foreach(Thread workerThread in threads)
            {
                workerThread.Join();
            }

            // errors during the parsing of dlls
            foreach(string filepath in _studentDLLFiles)
            {
                string fileName = Path.GetFileName(filepath);
                if(!_results.ContainsKey(fileName))
                {
                    _results[fileName] = new List<AnalyzerResult>();

                    foreach (KeyValuePair<int , bool> option in _teacherOptions)
                    {
                        if (option.Value == true)
                        {
                            _results[fileName].Add( new AnalyzerResult( option.Key.ToString() , 0 , $"Failed to parse {fileName}" ) );
                        }
                    }
                }
            }

            return _results;
        }

        /// <summary>
        /// Generates a class diagram based on the analysis results.
        /// </summary>
        /// <param name="removableNamespaces">List of namespaces to be excluded from the diagram.</param>
        /// <returns>Byte array representing the generated class diagram.</returns>
        public byte[] GenerateClassDiagram(List<string> removableNamespaces)
        {
            ClassDiagram classDiag = new(_parsedDLLFiles);
            byte[] bytes = classDiag.Run(removableNamespaces).Result;
            Trace.WriteLine("MainPipeline : Created Class Relationship graph with removing " + string.Join( " " , removableNamespaces));
            return bytes;
        }
    }
}
