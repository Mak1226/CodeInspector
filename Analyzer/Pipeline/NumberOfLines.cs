using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using System.Text.RegularExpressions;
using Analyzer.Parsing;
using System.Reflection;

namespace Analyzer.Pipeline.Analyzers
{
    public class NumberOfLines : BaseAnalyzer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberOfLines"/> class.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files to analyze.</param>
        public NumberOfLines(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            // The constructor can be used for any necessary setup or initialization.
        }

        /// <summary>
        /// Counts the total number of IL instructions in all methods of the provided DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files to analyze.</param>
        /// <returns>The total number of IL instructions in all methods.</returns>
        public static int CountFunctionLength(ParsedDLLFiles dllFiles)
        {
            int functionLength = 0;

            foreach (var classObjMC in dllFiles.classObjListMC)
            {
                foreach (var method in classObjMC.Methods)
                {
                    // Count the number of lines in the method's IL code
                    int ilLineCount = CountILLinesInMethod((MethodDefinition)method);

                    functionLength += ilLineCount;
                }
            }

            return functionLength;
        }

        /// <summary>
        /// Counts the number of IL instructions in the method body.
        /// </summary>
        /// <param name="method">The method to count IL instructions for.</param>
        /// <returns>The count of IL instructions in the method.</returns>
        private static int CountILLinesInMethod(MethodDefinition method)
        {
            if (method.Body != null)
            {
                // Count the IL instructions in the method body
                int ilLineCount = method.Body.Instructions.Count;

                return ilLineCount;
            }

            return 0;
        }
    }
}