using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// This class represents an analyzer for reviewing and detecting useless control flow in IL code.
    /// </summary>
    internal class ReviewUselessControlFlowRule : AnalyzerBase
    {
        public ReviewUselessControlFlowRule(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
        }
        public override AnalyzerResult Run()
        {
            int uselessControlFlowCount = 0;

            foreach (ParsedClassMonoCecil classObj in parsedDLLFiles.classObjListMC)
            {
                foreach (MethodDefinition method in classObj.TypeObj.Methods)
                {
                    int methodUselessControlFlowCount = ReviewUselessControlFlowInMethod(method);
                    uselessControlFlowCount += methodUselessControlFlowCount;
                }
            }

            return new AnalyzerResult("ReviewUselessControlFlowRule", uselessControlFlowCount, null);
        }


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

        private static bool IsJumpToNextInstruction(Instruction instruction)
        {
            return instruction.OpCode.FlowControl == FlowControl.Cond_Branch || instruction.OpCode.FlowControl == FlowControl.Branch;
        }
    }
}
