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
                errorMessageBuilder.Append($"No methods have cyclomatic complexity greater than {_maxAllowedComplexity}\n");
                errorMessageBuilder.Append($"[NOTE: Switch case complexity is not accurate]");
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
            List<Instruction> targets = new();
            int cyclomaticComplexity = 1;

            // non empty methods
            if (method.HasBody)
            {
                foreach (Instruction instruction in method.Body.Instructions)
                {
                    if(instruction.OpCode.FlowControl == FlowControl.Cond_Branch)
                    {
                        cyclomaticComplexity += CalculateCondBranchCaseComplexity(instruction, targets);
                    }
                }
            }

            cyclomaticComplexity += targets.Count();

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
                }
            }

            return complexity;
        }


        private void FindSwitchTargetLabels(Instruction instruction , List<Instruction> targets)
        {
            // Analysing the cases of the switch statement to calculate complexity
            Instruction[] cases = instruction.Operand as Instruction[];

            foreach (Instruction caseLabel in cases)
            {
                targets.Add(caseLabel);
            }
        }

    }
}
