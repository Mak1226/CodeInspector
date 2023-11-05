using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Parsing;
using Mono.Cecil.Cil;
using Mono.Cecil;
using static System.Net.Mime.MediaTypeNames;

namespace Analyzer.Pipeline
{
    public class AvoidUnusedPrivateFieldsRule : BaseAnalyzer
    {
        public AvoidUnusedPrivateFieldsRule(ParsedDLLFiles dllFiles) : base(dllFiles)
        {

        }

        private bool Check()
        {
            foreach (ParsedClassMonoCecil cls in parsedDLLFiles.classObjListMC)
            {
                List<string> UnusedFields = new();

                foreach (FieldDefinition field in cls.FieldsList)
                {
                    UnusedFields.Add(field.Name.ToString()); 
                }

                if (cls._fields.Count == 0)
                {
                    continue;
                }

                foreach (MethodDefinition method in cls.MethodsList)
                {
                    if (!method.HasBody)
                    {
                        continue;
                    }

                    foreach (var ins in method.Body.Instructions)
                    {
                        if (ins.OpCode == OpCodes.Ldfld)
                        {

                            FieldReference fieldReference = (FieldReference) ins.Operand;

                            var fieldName = fieldReference.Name.ToString();

                            if (UnusedFields.Contains(fieldName))
                            {
                                UnusedFields.Remove(fieldName);
                            }

                        }
                    }
                }

                foreach (MethodDefinition method in cls.Constructors)
                {
                    if (!method.HasBody)
                    {
                        continue;
                    }

                    foreach (var ins in method.Body.Instructions)
                    {
                        if (ins.OpCode == OpCodes.Ldfld)
                        {
                            FieldReference fieldReference = (FieldReference) ins.Operand;

                            var fieldName = fieldReference.Name.ToString();

                            if (UnusedFields.Contains(fieldName))
                            {
                                UnusedFields.Remove(fieldName);
                            }
                        }
                    }
                }

                if (UnusedFields.Count > 0)
                {
                    return false;
                }

            }

            return true;
        }

        override
        public int GetScore()
        {
            if (Check())
            {
                return 1;
            }
            return 0;
        }
    }
}