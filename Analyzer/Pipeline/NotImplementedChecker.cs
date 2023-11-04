using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Parsing;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Analyzer.Pipeline.Analyzers
{
    internal class NotImplementedChecker:BaseAnalyzer
    {
        public NotImplementedChecker(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            // The constructor can be used for any necessary setup or initialization.
        }
        public int CountUnimplementedMethods(ParsedDLLFiles dllFiles)
        {
            int unimplementedMethodCount = 0;

            foreach (var classObjMC in dllFiles.classObjListMC)
            {
                foreach (var method in classObjMC.Methods)
                {
                    if (!IsImplemented((MethodDefinition)method))
                    {
                        unimplementedMethodCount++;
                    }
                }
            }

            return unimplementedMethodCount;
        }

        private static bool IsImplemented(MethodDefinition method)
        {
            if (method.HasBody)
            {
                foreach (Instruction instruction in method.Body.Instructions)
                {
                    if (instruction.OpCode == OpCodes.Newobj)
                    {
                        MethodReference constructor = (MethodReference)instruction.Operand;
                        if (constructor.DeclaringType.FullName == "System.NotImplementedException")
                        {
                            return false; // Unimplemented method
                        }
                    }
                }
            }

            return true; // Implemented method
        }
    }
}
