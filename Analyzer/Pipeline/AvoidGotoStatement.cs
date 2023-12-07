/******************************************************************************
* Filename    = AvoidGotoStatements.cs
* 
* Author      = Thanmayee
* 
* Project     = Analyzer
*
* Description = Analyzer designed to identify and report the presence of goto statements
*****************************************************************************/

using Analyzer.Parsing;
using Mono.Cecil.Cil;
using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;

namespace Analyzer.Pipeline
{
    public class AvoidGotoStatementsAnalyzer : AnalyzerBase
    {
        private readonly List<string> _errorMessages;
        private readonly string _analyzerID;

        public AvoidGotoStatementsAnalyzer( List<ParsedDLLFile> dllFiles ) : base( dllFiles )
        {
            _errorMessages = new List<string>();
            _analyzerID = "117";
        }

        protected override AnalyzerResult AnalyzeSingleDLL( ParsedDLLFile parsedDLLFile )
        {
            CheckForGotoStatements( parsedDLLFile );

            if (_errorMessages.Count == 0)
            {
                return new AnalyzerResult( _analyzerID , 1 , "No goto statements found." );
            }

            string errorMessageString = $"Goto statements found in classes: {string.Join( ", " , _errorMessages )}.";
            return new AnalyzerResult( _analyzerID , 0 , errorMessageString );
        }

        private void CheckForGotoStatements( ParsedDLLFile parsedDLLFile )
        {
            foreach (ParsedClassMonoCecil cls in parsedDLLFile.classObjListMC)
            {
                foreach (MethodDefinition method in cls.MethodsList)
                {
                    if (method.HasBody)
                    {
                        if (MethodContainsGotoStatement( method.Body.Instructions ))
                        {
                            _errorMessages.Add( cls.TypeObj.Name );
                            break;
                        }
                    }
                }
            }
        }

        private bool MethodContainsGotoStatement( IEnumerable<Instruction> instructions )
        {
            return instructions.Any( instruction => instruction.OpCode == OpCodes.Br || instruction.OpCode == OpCodes.Br_S );
        }
    }
}
