/******************************************************************************
* Filename    = NotImplementedChecker.cs
* 
* Author      = Kaustubh Sapkale
* 
* Project     = Analyzer
*
* Description = 
*****************************************************************************/
using System;
using Analyzer.Parsing;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Analyzer.Pipeline.Analyzers
{
    /// <summary>
    /// This class represents an analyzer for counting unimplemented methods in DLL files.
    /// </summary>
    public class NotImplementedChecker : AnalyzerBase
    {
        private string _errorMessage;
        private int _verdict;
        private readonly string _analyzerID;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotImplementedChecker"/> class.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files to analyze.</param>
        public NotImplementedChecker(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            _errorMessage = "";
            _verdict = 1;
            _analyzerID = "120";
        }

        /// <summary>
        /// Analyzes the provided DLL files and counts the number of unimplemented methods.
        /// </summary>
        /// <param name="parsedDLLFile">The parsed DLL file to analyze.</param>
        private void Analyze(ParsedDLLFile parsedDLLFile)
        {
            foreach (ParsedClassMonoCecil cls in parsedDLLFile.classObjListMC)
            {
                foreach (MethodDefinition method in cls.MethodsList)
                {
                    if (!IsImplemented(method))
                    {
                        _errorMessage += $"{cls.Name}.{method.Name} ";
                        _verdict = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a method is implemented by examining its IL code for a System.NotImplementedException.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns>True if the method is implemented; false if it appears to be unimplemented.</returns>
        private static bool IsImplemented(MethodDefinition method)
        {
            if (method.HasBody)
            {
                foreach (Instruction instruction in method.Body.Instructions)
                {
                    if (instruction.OpCode == OpCodes.Newobj)
                    {
                        MethodReference constructor = (MethodReference)instruction.Operand;
                        if (constructor.DeclaringType.FullName == "System.NotImplementedException")
                        {
                            return false; // Unimplemented method
                        }
                    }
                }
            }

            return true; // Implemented method
        }

        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            _errorMessage = "";
            _verdict = 1;

            Analyze(parsedDLLFile);

            if (_verdict == 1)
            {
                _errorMessage = "No violation found";
            }
            return new AnalyzerResult(_analyzerID, _verdict, _errorMessage);
        }
    }
}
