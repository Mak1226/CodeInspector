using Analyzer.Parsing;
using Mono.Cecil.Cil;
using Mono.Cecil;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// Analyzer rule to detect the presence of switch statements in methods.
    /// </summary>
    public class AvoidSwitchStatementsAnalyzer : AnalyzerBase
    {
        // Fields to store analysis results
        private string errorMessage;
        private int verdict;
        private readonly string analyzerID;

        /// <summary>
        /// Initializes a new instance of the AvoidSwitchStatementsAnalyzer class.
        /// </summary>
        /// <param name="dllFiles">The ParsedDLLFiles object containing the parsed DLL information.</param>
        public AvoidSwitchStatementsAnalyzer(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            // Initialize fields
            errorMessage = "";
            verdict = 1;
            analyzerID = "avoidSwitchStatements";
        }

        /// <summary>
        /// Runs the analysis to check for switch statements in methods.
        /// </summary>
        /// <returns>An AnalyzerResult object indicating the analysis result.</returns>
        public override AnalyzerResult Run()
        {
            CheckForSwitchStatements();
            return new AnalyzerResult(analyzerID, verdict, errorMessage);
        }

        /// <summary>
        /// Checks each method for the presence of switch statements.
        /// </summary>
        private void CheckForSwitchStatements()
        {
            // Iterate through each class and its methods
            foreach (ParsedClassMonoCecil cls in parsedDLLFiles.classObjListMC)
            {
                foreach (MethodDefinition method in cls.MethodsList)
                {
                    // Check if the method has a body
                    if (method.HasBody)
                    {
                        // Iterate through each instruction in the method's body
                        foreach (Instruction instruction in method.Body.Instructions)
                        {
                            // Check if the instruction is a switch statement
                            if (instruction.OpCode == OpCodes.Switch)
                            {
                                verdict = 0;
                                errorMessage = "Switch statement found.";
                                return; // You can return early if a switch statement is found.
                            }
                        }
                    }
                }
            }
        }
    }
}
