/******************************************************************************
 * Filename    = RemoveUnusedLocalVariablesRule.cs
 * 
 * Author      = Arun Sankar
 *
 * Product     = Analyzer
 * 
 * Project     = Analyzer
 *
 * Description = Class that implements Unused Local Variable Analyser
 *****************************************************************************/

using Analyzer.Parsing;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using System.Collections;
using Logging;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// This class represents an analyzer for detecting unused, unassigned local variables in methods using Mono.Cecil.
    /// </summary>
    public class RemoveUnusedLocalVariablesRule : AnalyzerBase
    {
        /// <summary>
        /// Initializes a new instance of the RemoveUnusedLocalVariablesRule with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files to analyze.</param>
        public RemoveUnusedLocalVariablesRule( List<ParsedDLLFile> dllFiles ) : base( dllFiles )
        {
            analyzerID = "109";
            // The constructor sets the parsedDLLFiles field with the provided DLL files.
        }

        /// <summary>
        /// Gets the result of the analysis, which includes the number of unused local variables removed.
        /// </summary>
        /// <returns>An AnalyzerResult containing the analysis results.</returns>
        protected override AnalyzerResult AnalyzeSingleDLL( ParsedDLLFile parsedDLLFile )
        {
            int totalUnusedLocals = 0;

            foreach (ParsedClassMonoCecil classObj in parsedDLLFile.classObjListMC)
            {
                foreach (MethodDefinition method in classObj.TypeObj.Methods)
                {
                    int unusedLocalsCount = RemoveUnusedLocalVariables(method);
                    //Console.WriteLine( "Unused" );
                    totalUnusedLocals += unusedLocalsCount;
                }
            }

            string errorString = totalUnusedLocals > 0
                ? $"There are {totalUnusedLocals} unused local variables"
                : "No unused local variables found.";
            int verdict = totalUnusedLocals > 0 ? 0 : 1;
            return new AnalyzerResult(analyzerID , verdict , errorString );
        }

        /// <summary>
        /// Gets the VariableDefinition associated with an Instruction in a MethodDefinition.
        /// </summary>
        /// <param name="self">The Instruction for which to retrieve the VariableDefinition.</param>
        /// <param name="method">The MethodDefinition containing the Instruction.</param>
        /// <returns>The VariableDefinition associated with the Instruction, or null if not found.</returns>
        public static VariableDefinition? GetVariable( Instruction self , MethodDefinition method )
        {
            if ((self == null) || (method == null) || !method.HasBody)
            {
                return null;
            }

            int index;
            switch (self.OpCode.Code)
            {
                case Code.Ldloc_0:
                case Code.Ldloc_1:
                case Code.Ldloc_2:
                case Code.Ldloc_3:
                    index = self.OpCode.Code - Code.Ldloc_0;
                    break;
                case Code.Stloc_0:
                case Code.Stloc_1:
                case Code.Stloc_2:
                case Code.Stloc_3:
                    index = self.OpCode.Code - Code.Stloc_0;
                    break;
                case Code.Ldloc:
                case Code.Ldloc_S:
                case Code.Ldloca:
                case Code.Ldloca_S:
                case Code.Stloc:
                case Code.Stloc_S:
                    return (self.Operand as VariableDefinition);
                default:
                    return null;
            }
            return method.Body.Variables[index];
        }

        /// <summary>
        /// Detects unused local variables from a method and returns its count.
        /// </summary>
        /// <param name="method">The method to analyze and remove unused local variables from.</param>
        /// <param name="unusedVariableNames">A list to store the names of unused variables.</param>
        /// <returns>The count of unused local variables removed from the method.</returns>
        private static int RemoveUnusedLocalVariables(MethodDefinition method)
        {
            Logger.Log("Inside function RemoveUnusedLocalVariables" , LogLevel.INFO );
            const int DefaultLength = (16 << 3);

            if (!method.HasBody)
            {
                return 0;
            }

            MethodBody body = method.Body;
            Collection<VariableDefinition> variables = body.Variables;
            int count = variables.Count;
            BitArray used = new( Math.Max( DefaultLength , count ) );
            used.SetAll( false );

            foreach (Instruction ins in body.Instructions)
            {
                VariableDefinition? vd = GetVariable( ins , method );
                if (vd != null)
                {
                    used[vd.Index] = true;
                }
            }
            int unusedLocalsCount = 0;
            for (int i = 0; i < count; i++)
            {
                if (!used[i])
                {
                    unusedLocalsCount += 1;
                }
            }
            return unusedLocalsCount;
        }
    }
}
