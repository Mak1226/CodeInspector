using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Analyzer.Parsing;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// This rule checks if switch statements must have a default case.
    /// </summary>
    public class SwitchStatementDefaultCaseChecker : AnalyzerBase
    {
        private string errorMessage;
        private int verdict;
        private readonly string analyzerID;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchStatementRule"/> class.
        /// </summary>
        /// <param name="dllFiles">The ParsedDLLFiles object containing the parsed DLL information.</param>
        public SwitchStatementDefaultCaseChecker(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            errorMessage = "";
            verdict = 1;
            analyzerID = "105";
        }

        /// <summary>
        /// Checks for switch statements without a default case in the parsed DLL.
        /// </summary>
        /// <param name="parsedDLLFile">The ParsedDLLFile to analyze.</param>
        private void CheckSwitchStatements(ParsedDLLFile parsedDLLFile)
        {
            foreach (ParsedClassMonoCecil cls in parsedDLLFile.classObjListMC)
            {
                foreach (MethodDefinition method in cls.MethodsList)
                {
                    CheckSwitchStatementsInMethod(method, cls);
                }
            }
        }

        /// <summary>
        /// Checks a method for switch statements without a default case.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <param name="cls">The class containing the method.</param>
        private void CheckSwitchStatementsInMethod(MethodDefinition method, ParsedClassMonoCecil cls)
        {
            // methods can be empty (e.g., p/invoke declarations)
            bool defaultcaseflag = false;
            if (!method.HasBody)
                return;

            foreach (var instruction in method.Body.Instructions)
            {
                if (instruction.OpCode == OpCodes.Switch)
                {

                    // Check if there is a default case
                    foreach (var target in (instruction.Operand as Instruction[]))
                    {
                        if (target.OpCode == OpCodes.Br)
                        {
                            // Default case found
                            defaultcaseflag = true;
                            return;
                        }
                    }

                    // No default case found
                    if (defaultcaseflag)
                    {
                        errorMessage += $"{cls.Name}.{method.Name} ";
                        verdict = 0;
                        return;
                    }
                }
            }
        }

        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            errorMessage = "";
            verdict = 1;

            CheckSwitchStatements(parsedDLLFile);
            return new AnalyzerResult(analyzerID, verdict, errorMessage);
        }
    }
}
