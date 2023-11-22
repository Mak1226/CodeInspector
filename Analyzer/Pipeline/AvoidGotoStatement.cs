using Analyzer.Parsing;
using Mono.Cecil.Cil;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// Analyzer rule for detecting goto statements in methods.
    /// </summary>
    public class AvoidGotoStatementsAnalyzer : AnalyzerBase
    {
        private readonly List<string> _errorMessages;
        private int _verdict;
        private readonly string _analyzerID;

        public AvoidGotoStatementsAnalyzer(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            _errorMessages = new List<string>();
            _verdict = 1;
            _analyzerID = "117";
        }

        /// <summary>
        /// Runs the analysis to check for the presence of goto statements in methods.
        /// </summary>
        /// <returns>An <see cref="AnalyzerResult"/> based on the analysis.</returns>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            CheckForGotoStatements(parsedDLLFile);

            // Concatenate all error messages into a single string
            string errorMessageString = string.Join(", ", _errorMessages);

            // If no errors, add a message indicating everything looks fine
            if (string.IsNullOrEmpty(errorMessageString))
            {
                errorMessageString = "Everything looks fine. No goto statements found.";
            }
            else
            {
                errorMessageString = $"Goto statements found in functions: {errorMessageString}.";
                _verdict = 0;
            }

            return new AnalyzerResult(_analyzerID, _verdict, errorMessageString);
        }

        /// <summary>
        /// Checks each method for the presence of goto statements.
        /// </summary>
        private void CheckForGotoStatements(ParsedDLLFile parsedDLLFile)
        {
            foreach (ParsedClassMonoCecil cls in parsedDLLFile.classObjListMC)
            {
                foreach (MethodDefinition method in cls.MethodsList)
                {
                    if (method.HasBody)
                    {
                        if (MethodContainsGotoStatement(method.Body.Instructions))
                        {
                            // Collect the method name if a goto statement is found
                            _errorMessages.Add(method.FullName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the method contains a goto statement.
        /// </summary>
        private bool MethodContainsGotoStatement(IEnumerable<Instruction> instructions)
        {
            return instructions.Any(instruction => instruction.OpCode == OpCodes.Br || instruction.OpCode == OpCodes.Br_S);
        }
    }
}
