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

        private string _errorMessage;
        private int _verdict;
        private readonly string _analyzerID;


        /// <summary>
        /// Initializes a new instance of the AvoidUnusedPrivateFieldsRule class.
        /// </summary>
        /// <param name="dllFiles">The ParsedDLLFiles object containing the parsed DLL information.</param>
        public AvoidUnusedPrivateFieldsRule(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            _errorMessage = "";
            _verdict = 1;
            _analyzerID = "103";
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

                    foreach (Instruction? ins in method.Body.Instructions)
                    {
                        if (ins.OpCode == OpCodes.Ldfld)
                        {

                            FieldReference fieldReference = (FieldReference) ins.Operand;

                            string fieldName = fieldReference.Name.ToString();

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

                    foreach (Instruction? ins in method.Body.Instructions)
                    {
                        if (ins.OpCode == OpCodes.Ldfld)
                        {
                            FieldReference fieldReference = (FieldReference) ins.Operand;

                            string fieldName = fieldReference.Name.ToString();

                            if (UnusedFields.Contains(fieldName))
                            {
                                UnusedFields.Remove(fieldName);
                            }
                        }
                    }
                }

                if (UnusedFields.Count > 0)
                {

                    _errorMessage += cls.Name.ToString() + " -> ";

                    foreach (string filedName in UnusedFields)
                    {
                        _errorMessage += filedName + " ";
                    }
                    _errorMessage += ", ";

                    _verdict = 0;
                }

            }

            if(_verdict == 0)
            {
                _errorMessage += "these are unused private field";
                return;
            }

            _verdict = 1;
        }

        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            _errorMessage = "";
            _verdict = 1;

            Check(parsedDLLFile);
            return new AnalyzerResult(_analyzerID, _verdict, _errorMessage == "" ? "No violations found." : _errorMessage);
        }
    }
}
