/******************************************************************************
* Filename    = AvoidSwitchStatements.cs
* 
* Author      = Thanmayee
* 
* Project     = Analyzer
*
* Description = Analyzer to detect the presence of switch statements 
*****************************************************************************/


using Analyzer.Parsing;
using Mono.Cecil.Cil;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// Analyzer rule for detecting switch statements in methods.
    /// </summary>
    public class AvoidSwitchStatementsAnalyzer : AnalyzerBase
    {
        private List<string> _errorMessages;
        private int _verdict;
        private readonly string _analyzerID;

        public AvoidSwitchStatementsAnalyzer(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            _errorMessages = new List<string>();
            _verdict = 1;
            _analyzerID = "107";
        }

        /// <summary>
        /// Runs the analysis to check for the presence of switch statements in methods.
        /// </summary>
        /// <returns>An <see cref="AnalyzerResult"/> based on the analysis.</returns>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            _errorMessages = new List<string>();
            _verdict = 1;

            CheckForSwitchStatements(parsedDLLFile);

            // Concatenate all error messages into a single string
            string errorMessageString = string.Join(", ", _errorMessages);

            // If no errors, add a message indicating everything looks fine
            if (string.IsNullOrEmpty(errorMessageString))
            {
                errorMessageString = "No switch statements found.";
            }
            else
            {
                errorMessageString = $"Switch statements found in functions: {errorMessageString}.";
                _verdict = 0;
            }

            return new AnalyzerResult(_analyzerID, _verdict, errorMessageString);
        }

        /// <summary>
        /// Checks each method for the presence of switch statements.
        /// </summary>
        private void CheckForSwitchStatements(ParsedDLLFile parsedDLLFile)
        {
            foreach (ParsedClassMonoCecil cls in parsedDLLFile.classObjListMC)
            {
                foreach (MethodDefinition method in cls.MethodsList)
                {
                    if (method.HasBody)
                    {
                        if (MethodContainsSwitchStatement(method.Body.Instructions))
                        {
                            // Collect the method name if a switch statement is found
                            _errorMessages.Add(method.FullName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the method contains a switch statement.
        /// </summary>
        private bool MethodContainsSwitchStatement(IEnumerable<Instruction> instructions)
        {
            return instructions.Any(instruction => instruction.OpCode == OpCodes.Switch);
        }
    }
}