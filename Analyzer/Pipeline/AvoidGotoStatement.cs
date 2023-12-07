/******************************************************************************
* Filename    = AvoidGotoStatement.cs
* 
* Author      = Thanmayee
* 
* Project     = Analyzer
*
* Description = Analyzer designed to identify and report the presence of goto statements
*****************************************************************************/

using Analyzer.Parsing;
using Mono.Cecil.Cil;
using Mono.Cecil;
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

        public AvoidGotoStatementsAnalyzer(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            _errorMessages = new List<string>();
            analyzerID = "117";
        }

        /// <summary>
        /// Runs the analysis to check for the presence of goto statements in methods.
        /// </summary>
        /// <returns>An <see cref="AnalyzerResult"/> based on the analysis.</returns>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            CheckForGotoStatements(parsedDLLFile);

            // If no errors, add a message indicating everything looks fine
            if (_errorMessages.Count == 0)
            {
                return new AnalyzerResult(analyzerID, 1, "No goto statements found.");
            }

            // Create an error message string with the names of classes that have errors
            string errorMessageString = $"Goto statements found in classes: {string.Join(", ", _errorMessages)}.";

            return new AnalyzerResult(analyzerID, 0, errorMessageString);
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
                            // Collect the class name if a goto statement is found
                            _errorMessages.Add(cls.TypeObj.Name);
                            break; // Break after finding the first goto statement in the method
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
