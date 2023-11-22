using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// This class represents an analyzer for reviewing and detecting useless control flow in IL code.
    /// </summary>
    public class ReviewUselessControlFlowRule : AnalyzerBase
    {
        /// <summary>
        /// Initializes a new instance of the ReviewUselessControlFlowRule with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files to analyze.</param>
        public ReviewUselessControlFlowRule(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
        }

        /// <summary>
        /// Gets the result of the analysis, which includes the count of useless control flow occurrences.
        /// </summary>
        /// <returns>An AnalyzerResult containing the analysis results.</returns>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            int uselessControlFlowCount = 0;

            foreach (ParsedClassMonoCecil classObj in parsedDLLFile.classObjListMC)
            {
                foreach (MethodDefinition method in classObj.TypeObj.Methods)
                {
                    if(!method.HasBody)
                    {
                        continue;
                    }

                    int methodUselessControlFlowCount = ReviewUselessControlFlowInMethod(method);
                    uselessControlFlowCount += methodUselessControlFlowCount;
                }
            }

            string errorString = uselessControlFlowCount > 0
                ? $"Detected {uselessControlFlowCount} occurrences of useless control flow."
                : "No occurrences of useless control flow found.";

            return new AnalyzerResult("110", uselessControlFlowCount, errorString);
        }

        /// <summary>
        /// Reviews and detects useless control flow in a method.
        /// </summary>
        /// <param name="method">The method to analyze.</param>
        /// <returns>The count of useless control flow occurrences in the method.</returns>
        private static int ReviewUselessControlFlowInMethod(MethodDefinition method)
        {
            int methodUselessControlFlowCount = 0;
            Collection<Instruction> instructions = method.Body.Instructions;

            for (int i = 0; i < instructions.Count; i++)
            {
                Instruction instruction = instructions[i];
                if (IsJumpToNextInstruction(instruction))
                {
                    // Check if the next instruction is a no-op (useless control flow)
                    if (i + 1 < instructions.Count && instructions[i + 1].OpCode == OpCodes.Nop)
                    {
                        methodUselessControlFlowCount++;
                        i++; // Skip the next instruction
                    }
                }
            }

            return methodUselessControlFlowCount;
        }

        /// <summary>
        /// Checks if an instruction is a jump to the next instruction.
        /// </summary>
        /// <param name="instruction">The instruction to check.</param>
        /// <returns>True if the instruction is a jump to the next instruction; otherwise, false.</returns>
        private static bool IsJumpToNextInstruction(Instruction instruction)
        {
            return instruction.OpCode.FlowControl == FlowControl.Cond_Branch || instruction.OpCode.FlowControl == FlowControl.Branch;
        }
    }
}
