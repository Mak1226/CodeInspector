using Analyzer.Parsing;
using System.Collections.Generic;
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
        public ReviewUselessControlFlowRule( List<ParsedDLLFile> dllFiles ) : base( dllFiles )
        {
        }

        /// <summary>
        /// Gets the result of the analysis, which includes the count of useless control flow occurrences.
        /// </summary>
        /// <returns>An AnalyzerResult containing the analysis results.</returns>
        protected override AnalyzerResult AnalyzeSingleDLL( ParsedDLLFile parsedDLLFile )
        {
            int uselessControlFlowCount = 0;

            foreach (ParsedClassMonoCecil classObj in parsedDLLFile.classObjListMC)
            {
                foreach (MethodDefinition method in classObj.TypeObj.Methods)
                {
                    if (!method.HasBody)
                    {
                        continue;
                    }

                    int methodUselessControlFlowCount = ReviewUselessControlFlowInMethod( method );
                    uselessControlFlowCount += methodUselessControlFlowCount;
                }
            }

            string errorString = uselessControlFlowCount > 0
                ? $"Detected {uselessControlFlowCount} occurrences of useless control flow."
                : "No occurrences of useless control flow found.";
            int verdict = uselessControlFlowCount > 0 ? 0 : 1;
            return new AnalyzerResult( "110" , verdict , errorString );
        }

        /// <summary>
        /// Reviews and detects useless control flow in a method.
        /// </summary>
        /// <param name="method">The method to analyze.</param>
        /// <returns>The count of useless control flow occurrences in the method.</returns>
        private static int ReviewUselessControlFlowInMethod( MethodDefinition method )
        {
            int methodUselessControlFlowCount = 0;
            Collection<Instruction> instructions = method.Body.Instructions;

            for (int i = 0; i < instructions.Count; i++)
            {
                Instruction instruction = instructions[i];
                if (IsJumpToNextInstruction( instruction ))
                {
                    // Check if the next instruction is a no-op (useless control flow)
                    if (i + 1 < instructions.Count && instructions[i + 1].OpCode == OpCodes.Nop)
                    {
                        methodUselessControlFlowCount++;
                        i++; // Skip the next instruction
                    }
                }
                else if (IsEmptyBlock( instructions , i ))
                {
                    methodUselessControlFlowCount++;
                }
            }

            return methodUselessControlFlowCount;
        }

        /// <summary>
        /// Checks if an instruction is a jump to the next instruction.
        /// </summary>
        /// <param name="instruction">The instruction to check.</param>
        /// <returns>True if the instruction is a jump to the next instruction; otherwise, false.</returns>
        private static bool IsJumpToNextInstruction( Instruction instruction )
        {
            if (instruction.OpCode.FlowControl == FlowControl.Cond_Branch || instruction.OpCode.FlowControl == FlowControl.Branch)
            {
                // Check if the target of the branch is the next instruction or a NOP, which indicates useless control flow
                Instruction targetInstruction = (Instruction)instruction.Operand;
                return targetInstruction == instruction.Next || targetInstruction.OpCode == OpCodes.Nop;
            }
            return false;
        }

        /// <summary>
        /// Checks if an instruction represents an empty block.
        /// </summary>
        /// <param name="instructions">The collection of instructions.</param>
        /// <param name="index">The index of the current instruction.</param>
        /// <returns>True if the instruction represents an empty block; otherwise, false.</returns>
        private static bool IsEmptyBlock( Collection<Instruction> instructions , int index )
        {
            // Check if the instruction is an unconditional jump to the next instruction
            if (instructions[index].OpCode == OpCodes.Br
                && index + 1 < instructions.Count
                && instructions[index + 1].Offset == (int)instructions[index].Operand)
            {
                // Check if the next instruction is a no-op
                return index + 2 < instructions.Count && instructions[index + 2].OpCode == OpCodes.Nop;
            }

            return false;
        }

    }
}
