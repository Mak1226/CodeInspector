/******************************************************************************
 * Filename    = AsyncMethodAnalyzer.cs
 * 
 * Author      = Arun Sankar
 *
 * Product     = Analyzer
 * 
 * Project     = Analyzer
 *
 * Description = Class that implements Async Method Analyser
 *****************************************************************************/

using Analyzer.Parsing;
using System.Collections.Generic;
using Mono.Cecil;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// Analyzer for detecting asynchronous methods in a DLL.
    /// </summary>
    public class AsyncMethodAnalyzer : AnalyzerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncMethodAnalyzer"/> class with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files to analyze.</param>
        public AsyncMethodAnalyzer( List<ParsedDLLFile> dllFiles ) : base( dllFiles )
        {
        }

        /// <summary>
        /// Analyzes a single DLL for asynchronous methods.
        /// </summary>
        /// <param name="parsedDLLFile">The parsed DLL file.</param>
        /// <returns>An <see cref="AnalyzerResult"/> containing the analysis results.</returns>
        protected override AnalyzerResult AnalyzeSingleDLL( ParsedDLLFile parsedDLLFile )
        {
            int asyncMethodCount = 0;

            foreach (ParsedClassMonoCecil classObj in parsedDLLFile.classObjListMC)
            {
                foreach (MethodDefinition method in classObj.TypeObj.Methods)
                {
                    if (IsAsyncMethod( method ))
                    {
                        asyncMethodCount++;
                    }
                }
            }

            string errorString = asyncMethodCount > 0
                ? $"Detected {asyncMethodCount} async methods."
                : "No async methods found.";
            int verdict = asyncMethodCount > 0 ? 0 : 1;
            return new AnalyzerResult( "110" , verdict , errorString );
        }

        /// <summary>
        /// Checks if a method is asynchronous by examining its custom attributes.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns>True if the method is asynchronous; otherwise, false.</returns>
        private static bool IsAsyncMethod( MethodDefinition method )
        {
            // Check if the method has the AsyncStateMachineAttribute custom attribute
            return method.CustomAttributes.Any( attribute =>
                attribute.AttributeType.Name == "AsyncStateMachineAttribute" );
        }
    }
}
