/*******************************************************************************
* Filename    = CyclomaticComplexity.cs
* 
* Author      = Nikhitha Atyam
* 
* Product     = Analyzer
* 
* Project     = Analyzer
*
* Description = Measures the cyclomatic complexity of methods in the dll files and flags the complex methods
*********************************************************************************/

using Analyzer.Parsing;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// CYCLOMATIC COMPLEXITY: Quantitative measure of number of linearly-independent paths through a program module 
    /// Programs with lower cyclomatic complexity are easier to understand and less risky to modify
    /// Higher cyclomatic complexity => complex method => Such methods are flagged by this analyzer
    /// </summary>
    public class CyclomaticComplexity : AnalyzerBase
    {
        // maximum allowed complexity for each method 
        private readonly int _maxAllowedComplexity;    


        /// <summary>
        /// Measures cyclomatic complexity of all methods in the files
        /// </summary>
        /// <param name="dllFiles">List of parsed DLL objects which are to be analyzed</param>
        /// <param name="maxAllowedComplexity">Maximum allowed complexity for each method(default = 10)</param>
        public CyclomaticComplexity(List<ParsedDLLFile> dllFiles , int maxAllowedComplexity=10) : base(dllFiles)
        {
            _maxAllowedComplexity = maxAllowedComplexity;
            analyzerID = "113";
        }


        // Analyzing all methods of one DLL file
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            StringBuilder errorMessageBuilder = new();
            int verdict = 1;    // no violating method

            List<ParsedClassMonoCecil> parsedClasses = parsedDLLFile.classObjListMC;

            // Traversing over all class objects for finding all methods of the file
            foreach (ParsedClassMonoCecil parsedClass in parsedClasses)
            {
                // Constructors are also included in this analysis
                List<MethodDefinition> methodsAndConstructorsList = parsedClass.MethodsList;
                methodsAndConstructorsList.AddRange(parsedClass.Constructors);

                foreach(MethodDefinition method in methodsAndConstructorsList)
                {
                    int methodComplexity = GetMethodCyclomaticComplexity(method);

                    // Flags method if complexity of it is more than allowed complexity
                    if(methodComplexity > _maxAllowedComplexity)
                    {
                        verdict = 0;    // there exists complex methods
                        errorMessageBuilder.AppendLine($"{method.FullName} : Cyclomatic Complexity = {methodComplexity}");
                    }
                }     
            }

            // No violating method
            if(verdict == 1)
            {
                errorMessageBuilder.AppendLine($"No methods have cyclomatic complexity greater than {_maxAllowedComplexity}");
                errorMessageBuilder.AppendLine($"[NOTE: Switch case complexity is not accurate]");
            }
            else
            {
                errorMessageBuilder.Insert(0, $"Methods having cyclomatic complexity greater than {_maxAllowedComplexity}:\n[NOTE: Switch case complexity is not accurate]\n");
            }

            AnalyzerResult analyzerResult = new(analyzerID , verdict , errorMessageBuilder.ToString());

            return analyzerResult;
        }


        /// <summary>
        /// Finding cyclomatic complexity of a method
        /// </summary>
        /// <param name="method">Method for which complexity needs to be found</param>
        /// <returns>complexity of method</returns>
        public int GetMethodCyclomaticComplexity(MethodDefinition method)
        {
            List<Instruction> targets = new();      // stores branch targets
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


        // Calculating complexity in case of conditional branch 
        private int CalculateCondBranchCaseComplexity(Instruction instruction , List<Instruction> targets)
        {
            int complexity = 0;

            if (instruction.OpCode.Code == Code.Switch)
            {
                FindSwitchTargetLabels(instruction, targets);
            }
            else
            {
                //Instruction target = instruction.Operand as Instruction;

                //if (!targets.Contains(target) && instruction.Previous?.OpCode.Code != Code.Dup)
                //if (!targets.Contains(target))
                //{
                    complexity++;
                //}
            }

            return complexity;
        }


        // Finding the number of switch case labels
        private void FindSwitchTargetLabels(Instruction instruction , List<Instruction> targets)
        {
            // Analysing the cases of the switch statement to calculate complexity
            Instruction[] cases = instruction.Operand as Instruction[];

            foreach (Instruction caseLabel in cases)
            {
                targets.Add(caseLabel);
            }

            // default case in case may exist or not
            // default case would be generally unconditional branch statement next to switch instruction
            // But in case of no default => this way may not be the appropriate way for finding existence of default case
        }

    }
}
