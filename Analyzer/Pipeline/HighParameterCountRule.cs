/******************************************************************************
 * Filename    = HighParameterCountRule.cs
 * 
 * Author      = Arun Sankar
 *
 * Product     = Analyzer
 * 
 * Project     = Analyzer
 *
 * Description = Class that implements High Parameter Count Check Analyser
 *****************************************************************************/

using System.Diagnostics;
using Analyzer.Parsing;
using Mono.Cecil;
using Logging;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// This class represents an analyzer for detecting methods with a high number of parameters.
    /// </summary>
    public class HighParameterCountRule : AnalyzerBase
    {
        private const int ParameterCountThreshold = 5; // We can adjust the threshold as needed

        /// <summary>
        /// Initializes a new instance of the HighParameterCountRule with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files to analyze.</param>
        public HighParameterCountRule(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            analyzerID = "119";
        }

        /// <summary>
        /// Gets the result of the analysis, which includes the count of methods with a high number of parameters.
        /// </summary>
        /// <returns>An AnalyzerResult containing the analysis results.</returns>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            int highParameterCountMethods = 0;

            foreach (ParsedClassMonoCecil classObj in parsedDLLFile.classObjListMC)
            {
                foreach (MethodDefinition method in classObj.TypeObj.Methods)
                {
                    if (method.Parameters.Count > ParameterCountThreshold)
                    {
                        highParameterCountMethods++;
                        //Trace.WriteLine($"Method {method.Name} has a high number of parameters: {method.Parameters.Count}");
                        Logger.Log( $"Method {method.Name} has a high number of parameters: {method.Parameters.Count}" , LogLevel.INFO );
                    }
                }
            }

            string errorString = highParameterCountMethods > 0
                ? $"Detected {highParameterCountMethods} methods with a high number of parameters."
                : "No methods with a high number of parameters found.";
            int verdict = highParameterCountMethods > 0 ? 0 : 1;
            return new AnalyzerResult(analyzerID, verdict, errorString);
        }
    }
}
