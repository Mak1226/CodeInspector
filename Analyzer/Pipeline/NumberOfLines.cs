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
        public NumberOfLines(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            // The constructor can be used for any necessary setup or initialization.
        }
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