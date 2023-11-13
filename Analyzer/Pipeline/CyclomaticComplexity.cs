using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Analyzer.Pipeline
{
    public class CyclomaticComplexity : AnalyzerBase
    {
        public CyclomaticComplexity(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            List<ParsedClassMonoCecil> parsedClasses = dllFiles.classObjListMC;
        }

        private HashSet<Instruction> targets = new();

        private int GetMethodCyclomaticComplexity(MethodDefinition method)
        {
            // non empty methods
            if(method.HasBody)
            {
                int cyclomaticComplexity = 1;

                foreach(Instruction instruction in method.Body.Instructions)
                {
                    switch(instruction.OpCode.FlowControl) 
                    {
                        case FlowControl.Cond_Branch:
                            if(instruction.OpCode.Code == Code.Switch)
                            {
                                FindSwitchTargetLabels(instruction);
                            }
                            break;

                        case FlowControl.Branch:

                            break;

                        default:
                            break;
                    }
                }


                return cyclomaticComplexity;  
            }
            else
            {
                return 1;
            }
        }


        private void FindSwitchTargetLabels(Instruction instruction)
        {
            // Analysing the cases of the switch statement to calculate complexity
            Instruction[] cases = instruction.Operand as Instruction[];

            foreach(Instruction caseLabel in cases)
            {
                targets.Add(caseLabel);
            }

            // Default case can be present or need not be present
            // Default case will be the first 
        }


        public override AnalyzerResult Run()
        {
            throw new NotImplementedException();
        }
    }
}
