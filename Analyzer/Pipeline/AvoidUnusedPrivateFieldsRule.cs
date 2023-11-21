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
    /// <summary>
    /// This class represents an analyzer rule for detecting unused private fields in a DLL.
    /// </summary>
    public class AvoidUnusedPrivateFieldsRule : AnalyzerBase
    {

        private string errorMessage;
        private int verdict;
        private readonly string analyzerID;


        /// <summary>
        /// Initializes a new instance of the AvoidUnusedPrivateFieldsRule class.
        /// </summary>
        /// <param name="dllFiles">The ParsedDLLFiles object containing the parsed DLL information.</param>
        public AvoidUnusedPrivateFieldsRule(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            errorMessage = "";
            verdict = 1;
            analyzerID = "103";
        }

        /// <summary>
        /// Checks for unused private fields in the parsed DLL.
        /// </summary>
        /// <returns>True if no unused private fields are found; otherwise, false.</returns>
        private void Check(ParsedDLLFile parsedDLLFile)
        {
            foreach (ParsedClassMonoCecil cls in parsedDLLFile.classObjListMC)
            {
                List<string> UnusedFields = new();

                foreach (FieldDefinition field in cls.FieldsList)
                {
                    UnusedFields.Add(field.Name.ToString()); 
                }

                if (cls.FieldsList.Count == 0)
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

                    errorMessage += cls.Name.ToString() + " -> ";

                    foreach (var filedName in UnusedFields)
                    {
                        errorMessage += filedName + " ";
                    }
                    errorMessage += ", ";

                    verdict = 0;
                }

            }

            if(verdict == 0)
            {
                errorMessage += "these are unused private field";
                return;
            }

            verdict = 1;
        }

        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            errorMessage = "";
            verdict = 1;

            Check(parsedDLLFile);
            return new AnalyzerResult(analyzerID, verdict, errorMessage == "" ? "No violations found." : errorMessage);
        }
    }
}
