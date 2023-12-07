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
using Logging;

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
        private int _customAnalyzersID;
        private readonly IDictionary<int, InvokeCustomAnalyzers> _customAnalyzers;
        private readonly List<Tuple<int , string>> _allConfigurationOptions;

        /// <summary>
        /// Initializes a new instance of the Analyzer class.
        /// </summary>
        public Analyzer()
        {
            _pathOfDLLFilesOfStudent = new List<string>();
            _teacherOptions = new Dictionary<int, bool>();
            _pathOfDLLFilesOfCustomAnalyzers = new List<string>();
            _customAnalyzersID = 201;
            _customAnalyzers = new Dictionary<int, InvokeCustomAnalyzers>();
            _allConfigurationOptions = AnalyzerFactory.GetAllConfigurationOptions();
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
                Logger.Inform( "[Analyzer.cs] Configure: Teacher Options " + kvp.Key + " " + kvp.Value );
            }
            _teacherOptions = TeacherOptions;
        }

        /// <summary>
        /// Loads DLL files provided by the student for analysis.
        /// </summary>
        /// <param name="PathOfDLLFilesOfStudent">List of paths to DLL files.</param>
        public void LoadDLLFileOfStudent(List<string> PathOfDLLFilesOfStudent)
        {
            Logger.Inform( "[Analyzer.cs] LoadDLLFileOfStudent: Loaded students " + string.Join( " " , _pathOfDLLFilesOfStudent));
            _pathOfDLLFilesOfStudent = PathOfDLLFilesOfStudent;
        }

        /// <summary>
        /// Loads DLL files of custom analyzers for additional analysis.
        /// </summary>
        /// <param name="PathOfDLLFilesOfCustomAnalyzers">List of paths to DLL files of custom analyzers.</param>
        public List<Tuple<int , string>> LoadDLLOfCustomAnalyzers(List<string> PathOfDLLFilesOfCustomAnalyzers)
        {
            _pathOfDLLFilesOfCustomAnalyzers = PathOfDLLFilesOfCustomAnalyzers;

            foreach(string path in PathOfDLLFilesOfCustomAnalyzers)
            {

                try
                {
                    InvokeCustomAnalyzers customAnalyzer = new(new List<string> { path });
                    _customAnalyzers[_customAnalyzersID] = customAnalyzer;
                    _allConfigurationOptions.Add(Tuple.Create(_customAnalyzersID++, path));
                    Logger.Inform("[Analyzer.cs] LoadDLLOfCustomAnalyzers: Loaded custom analyzers " + path);
                }
                catch
                {
                    Logger.Debug("[Analyzer.cs] LoadDLLOfCustomAnalyzers: Failed to Load custom analyzers " + path);
                }
            }

            return _allConfigurationOptions;
        }

        /// <summary>
        /// Runs the main analysis pipeline and returns the results.
        /// </summary>
        /// <returns>Dictionary of analysis results.</returns>
        public Dictionary<string, List<AnalyzerResult>> Run()
        {
            Logger.Inform( "[Analyzer.cs] Run: Started" + string.Join( " " , _pathOfDLLFilesOfStudent ) );
            MainPipeline _mainAnalyzerPipeline = new();
            _mainAnalyzerPipeline.AddDLLFiles(_pathOfDLLFilesOfStudent);
            _mainAnalyzerPipeline.AddTeacherOptions(_teacherOptions);
            Dictionary<string, List<AnalyzerResult>> result = _mainAnalyzerPipeline.Start();
            Logger.Inform( "[Analyzer.cs] Run: Completed main analyzer" + string.Join( " " , _pathOfDLLFilesOfStudent ) );

            Dictionary<string , List<AnalyzerResult>> customAnalyzerResults = RnuCustomAnalyzers();

            result = MergeDictionaries(result, customAnalyzerResults);

            foreach (KeyValuePair<string , List<AnalyzerResult>> keyValuePair in result)
            {
                foreach (AnalyzerResult analyzerResult in keyValuePair.Value)
                {
                    Logger.Inform($"Analyzer : Key: {keyValuePair.Key}");
                    Logger.Inform($"Analyzer : {analyzerResult}");
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
            try
            {
                Logger.Inform( "[Analyzer.cs] GetRelationshipGraph: Started" + string.Join( " " , _pathOfDLLFilesOfStudent ) + "By removing " + string.Join( " " , removableNamespaces ) );
                MainPipeline _customAnalyzerPipeline = new();
                _customAnalyzerPipeline.AddDLLFiles( _pathOfDLLFilesOfStudent );
                _customAnalyzerPipeline.AddTeacherOptions( _teacherOptions );
                Logger.Inform( "[Analyzer.cs] GetRelationshipGraph: Completed main analyzer" + string.Join( " " , _pathOfDLLFilesOfStudent ) + "By removing " + string.Join( " " , removableNamespaces ) );
                return _customAnalyzerPipeline.GenerateClassDiagram( removableNamespaces );
            }
            catch
            {
                Logger.Debug("[Analyzer.cs] GetRelationshipGraph: Failed main analyzer" + string.Join( " " , _pathOfDLLFilesOfStudent ) + "By removing " + string.Join( " " , removableNamespaces)); 
                return default;
            }
        }

        /// <summary>
        /// Runs custom analyzers specified by the teacher.
        /// </summary>
        /// <returns>Dictionary of analysis results from custom analyzers.</returns>
        public Dictionary<string, List<AnalyzerResult>> RnuCustomAnalyzers()
        {

            Dictionary<string , List<AnalyzerResult>> result = new();

            foreach (KeyValuePair<int , InvokeCustomAnalyzers> analyzer in _customAnalyzers)
            {
                if (_teacherOptions[analyzer.Key]==true)
                {
                    try
                    {
                        InvokeCustomAnalyzers current = analyzer.Value;
                        current.AddStudentDllFiles( _pathOfDLLFilesOfStudent );
                        Dictionary<string , List<AnalyzerResult>> currentResult = current.Start();
                        UpdateAnalyzerId( currentResult , analyzer.Key );
                        result = MergeDictionaries( result , currentResult );
                        Logger.Inform( "[Analyzer.cs] RunCustomAnalyzers: Completed custom analyzer " + analyzer.Key + " " + string.Join( " " , _pathOfDLLFilesOfStudent ) );
                    } catch
                    {
                        Logger.Debug( "[Analyzer.cs] RunCustomAnalyzers: Failed custom analyzer " + analyzer.Key + " " + string.Join( " " , _pathOfDLLFilesOfStudent ) );
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Helper function to Update Analyzer Id
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="newAnalyzerId"></param>
        private void UpdateAnalyzerId(Dictionary<string , List<AnalyzerResult>> dictionary , int newAnalyzerId)
        {
            foreach (string key in dictionary.Keys)
            {
                List<AnalyzerResult> resultList = dictionary[key];

                foreach (AnalyzerResult result in resultList)
                {
                    result.AnalyserID = newAnalyzerId.ToString();
                }
            }
        }

        /// <summary>
        /// Helper function to merge two dictionaries based on their key
        /// </summary>
        /// <param name="dictionary1"></param>
        /// <param name="dictionary2"></param>
        /// <returns></returns>
        private Dictionary<string , List<AnalyzerResult>> MergeDictionaries(Dictionary<string , List<AnalyzerResult>> dictionary1, Dictionary<string , List<AnalyzerResult>> dictionary2 )
        {
            Dictionary<string , List<AnalyzerResult>> mergedDictionary = new();

            foreach (string? key in dictionary1.Keys.Concat( dictionary2.Keys ).Distinct())
            {
                List<AnalyzerResult> resultList = new();

                if (dictionary1.TryGetValue( key , out List<AnalyzerResult>? list1 ))
                {
                    resultList.AddRange( list1 );
                }

                if (dictionary2.TryGetValue( key , out List<AnalyzerResult>? list2 ))
                {
                    resultList.AddRange( list2 );
                }

                mergedDictionary[key] = resultList;
            }

            return mergedDictionary;

            // write lambda for writing console writeline

            Action action = () => { Console.WriteLine( "Hello World" ); };  


        }
    }
}
