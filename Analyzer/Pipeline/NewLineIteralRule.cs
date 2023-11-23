/******************************************************************************
* Filename    = NewLineIteralRule.cs
* 
* Author      = Kaustubh Sapkale
* 
* Project     = Analyzer
*
* Description = 
*****************************************************************************/
using System;
using System.Globalization;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Analyzer.Parsing;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// This rule warns if methods, including properties, are using the literal 
    /// \r and/or \n for new lines. This isn't portable across operating systems.
    /// To ensure correct cross-platform functionality they should be replaced by 
    /// System.Environment.NewLine.
    /// </summary>
    public class NewLineLiteralRule : AnalyzerBase
    {
        private string _errorMessage;
        private int _verdict;
        private readonly string _analyzerID;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewLineLiteralRule"/> class.
        /// </summary>
        /// <param name="dllFiles">The ParsedDLLFiles object containing the parsed DLL information.</param>
        public NewLineLiteralRule(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            _errorMessage = "";
            _verdict = 1;
            _analyzerID = "114";
        }

        /// <summary>
        /// Checks for the usage of literal \r and/or \n for new lines in methods and properties.
        /// </summary>
        /// <param name="parsedDLLFile">The ParsedDLLFile to analyze.</param>
        private void Check(ParsedDLLFile parsedDLLFile)
        {
            foreach (ParsedClassMonoCecil cls in parsedDLLFile.classObjListMC)
            {
                foreach (MethodDefinition method in cls.MethodsList)
                {
                    CheckMethod(method);
                }

                foreach (PropertyDefinition property in cls.PropertiesList)
                {
                    if (property.GetMethod != null)
                    {
                        CheckMethod(property.GetMethod);
                    }

                    if (property.SetMethod != null)
                    {
                        CheckMethod(property.SetMethod);
                    }
                }
            }
        }

        /// <summary>
        /// Checks a method for the usage of literal \r and/or \n for new lines.
        /// </summary>
        /// <param name="method">The method to check.</param>
        private void CheckMethod(MethodDefinition method)
        {
            // methods can be empty (e.g., p/invoke declarations)
            if (!method.HasBody)
            {
                return;
            }

            foreach (Instruction ins in method.Body.Instructions)
            {
                // look for a string load
                if (ins.OpCode.Code != Code.Ldstr)
                {
                    continue;
                }

                // check the string being referenced by the instruction
                string? s = (ins.Operand as string);
                if (string.IsNullOrEmpty(s))
                {
                    continue;
                }

                if (s.IndexOfAny(new[] { '\r', '\n' }) >= 0)
                {
                    _errorMessage += method.Name.ToString() + " ";
                    _verdict = 0;
                    return;
                }
            }
        }

        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            _errorMessage = "";
            _verdict = 1;

            Check(parsedDLLFile);
            return new AnalyzerResult(_analyzerID, _verdict, _errorMessage);
        }
    }
}
