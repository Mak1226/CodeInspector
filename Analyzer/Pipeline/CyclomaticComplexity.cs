/******************************************************************************
* Filename    = CyclomaticComplexity.cs
* 
* Author      = 
* 
* Project     = Analyzer
*
* Description = 
*****************************************************************************/

using Analyzer.Parsing;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Analyzer.Pipeline
{
    public class CyclomaticComplexity : AnalyzerBase
    {
        private readonly int _maxAllowedComplexity;
        private readonly string _analyzerID ;
        public CyclomaticComplexity(List<ParsedDLLFile> dllFiles , int maxAllowedComplexity=10) : base(dllFiles)
        {
            _maxAllowedComplexity = maxAllowedComplexity;
            _analyzerID = "113";
        }


        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            StringBuilder errorMessageBuilder = new();
            int verdict = 1;    // no error

            List<ParsedClassMonoCecil> parsedClasses = parsedDLLFile.classObjListMC;

            foreach (ParsedClassMonoCecil parsedClass in parsedClasses)
            {
                List<MethodDefinition> methods_constructors = parsedClass.MethodsList;
                methods_constructors.AddRange(parsedClass.Constructors);

                foreach(MethodDefinition method in methods_constructors)
                {
                    int methodComplexity = GetMethodCyclomaticComplexity(method);

                    if(methodComplexity > _maxAllowedComplexity)
                    {
                        verdict = 0;
                        errorMessageBuilder.AppendLine($"{method.FullName} : Cyclomatic Complexity = {methodComplexity}");
                    }
                }     
            }

            if(verdict == 1)
            {
                errorMessageBuilder.AppendLine($"No methods have cyclomatic complexity greater than {_maxAllowedComplexity}");
                errorMessageBuilder.AppendLine($"[NOTE: Switch case complexity is not accurate]");
            }
            else
            {
                errorMessageBuilder.Insert(0, $"Methods having cyclomatic complexity greater than {_maxAllowedComplexity}:\n[NOTE: Switch case complexity is not accurate]\n");
            }

            AnalyzerResult analyzerResult = new(_analyzerID , verdict , errorMessageBuilder.ToString());

            return analyzerResult;
        }




        public int GetMethodCyclomaticComplexity(MethodDefinition method)
        {
            //Console.WriteLine($"\n\n METHOD Name: {method.FullName}");

            List<Instruction> targets = new();
            int cyclomaticComplexity = 1;

            // non empty methods
            if (method.HasBody)
            {
                foreach (Instruction instruction in method.Body.Instructions)
                {
                    //Console.WriteLine($"Inst: {instruction.ToString()}");
                    switch (instruction.OpCode.FlowControl)
                    {
                        case FlowControl.Cond_Branch:
                            cyclomaticComplexity += CalculateCondBranchCaseComplexity(instruction, targets);
                            break;
                        case FlowControl.Branch:
                            //CalculateBranchCaseComplexity(instruction , targets);
                            break;
                        default:
                            break;
                    }
                }
            }

            cyclomaticComplexity += targets.Count();
            //Console.WriteLine("\n\n TARGET ARRAY:");
            //foreach(var t in targets)
            //{
            //    Console.WriteLine(t.ToString());
            //}
            //Console.WriteLine("\n\n");


            return cyclomaticComplexity;
        }


        private int CalculateCondBranchCaseComplexity(Instruction instruction , List<Instruction> targets)
        {
            int complexity = 0;

            if (instruction.OpCode.Code == Code.Switch)
            {
                FindSwitchTargetLabels(instruction, targets);
            }
            else
            {
                Instruction target = instruction.Operand as Instruction;

                //if (!targets.Contains(target) && instruction.Previous?.OpCode.Code != Code.Dup)
                if (!targets.Contains(target))
                {
                    complexity++;
                    //Console.WriteLine(instruction.ToString());
                    //Console.WriteLine(instruction.Previous.ToString());
                    //Console.WriteLine(instruction.Next.ToString());

                    //Console.WriteLine($"cond target - {target.OpCode} - {target.Operand} - {target.Offset}\n");
                }
            }

            return complexity;
        }


        //private void CalculateBranchCaseComplexity(Instruction instruction , HashSet<Instruction> targets) 
        //{
        //    Instruction prevInstruction = instruction.Previous;     // depends on previous Instruction

        //    if(prevInstruction != null)
        //    {
        //        while(prevInstruction.OpCode.Code == Code.Nop) 
        //        {
        //            prevInstruction = prevInstruction.Previous;
        //        }

        //        if(prevInstruction.OpCode.FlowControl == FlowControl.Cond_Branch) 
        //        {
        //            Instruction branch = instruction.Operand as Instruction;

        //            if (branch != null)
        //            {
        //                targets.Add(branch);
        //            }
        //        }
        //    }
        //}


        //private int IsLoadInstruction(Instruction instruction)
        //{
        //    int isLoadInst = 0;

        //    List<OpCode> loadOpCodes = new() { OpCodes.Ldc_I4, OpCodes.Ldc_I4_0 , OpCodes.Ldc_I4_1 , OpCodes.Ldc_I4_2 , 
        //                                       OpCodes.Ldc_I4_3 , OpCodes.Ldc_I4_4 , OpCodes.Ldc_I4_5 , OpCodes.Ldc_I4_6 , 
        //                                       OpCodes.Ldc_I4_7 , OpCodes.Ldc_I4_8 , OpCodes.Ldc_I4_M1 , OpCodes.Ldc_I4_S , 
        //                                       OpCodes.Ldc_I8 , OpCodes.Ldc_R4 , OpCodes.Ldc_R8 , OpCodes.Ld };


        //    //OpCodes.Ldarg, OpCodes.Ldarga, OpCodes.Ldarg_0, OpCodes.Ldarg_1, OpCodes.Ldarg_2, OpCodes.Ldarg_3,
        //    //OpCodes.Ldloc, OpCodes.Ldloca, OpCodes.Ldloc_0, OpCodes.Ldloc_1, OpCodes.Ldloc_2, OpCodes.Ldloc_3,
        //    //OpCodes.Ldc_I4, OpCodes.Ldc_I4_0, OpCodes.Ldc_I4_1, OpCodes.Ldc_I4_2, OpCodes.Ldc_I4_3,
        //    //OpCodes.Ldc_I4_4, OpCodes.Ldc_I4_5, OpCodes.Ldc_I4_6, OpCodes.Ldc_I4_7, OpCodes.Ldc_I4_8,
        //    //OpCodes.Ldc_R4, OpCodes.Ldc_R8, OpCodes.Ldstr, OpCodes.Ldfld, OpCodes.Ldsfld
        //    // Add more load opcodes as needed

        //    return isLoadInst;
        //}



        private void FindSwitchTargetLabels(Instruction instruction , List<Instruction> targets)
        {
            // Analysing the cases of the switch statement to calculate complexity
            Instruction[] cases = instruction.Operand as Instruction[];

            foreach (Instruction caseLabel in cases)
            {
                targets.Add(caseLabel);
                //Console.WriteLine($"label - {caseLabel.ToString()}");
            }

            // Default case can be present or need not be present
            // Default case will be the first unconditional target
        }

    }
}
