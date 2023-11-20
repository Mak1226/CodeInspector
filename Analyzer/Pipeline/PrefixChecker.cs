using System;
using Analyzer.Parsing;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// An analyzer that checks the correctness of type name prefixes in parsed DLL files.
    /// </summary>
    public class PrefixCheckerAnalyzer : AnalyzerBase
    {
       
        private string _errorMessage;
        private int _verdict;
        private readonly string _analyzerID;
       
        /// <summary>
        /// Initializes a new instance of the BaseAnalyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files for analysis.</param>
        public PrefixCheckerAnalyzer(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            // The constructor can be used for any necessary setup or initialization.
            _errorMessage = "";
            _verdict = 1;
            _analyzerID = "115";
        }



        /// <summary>
        /// Analyzes the DLL files to check type name prefixes for correctness.
        /// </summary>
        /// <returns>The number of errors found during the analysis.</returns>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            _errorMessage = "";
            _verdict = 1;
            int errorCount = 0;

            foreach (ParsedClass classObj in parsedDLLFile.classObjList)
            {
                if (!IsCorrectTypeName(classObj.Name))
                {
                    Console.WriteLine($"INCORRECT TYPE PREFIX : {classObj.Name}");
                    _errorMessage = "INCORRECT TYPE PREFIX : " + classObj.Name;
                    errorCount++;
                }
            }

            // To check interfaces
            foreach (ParsedInterface interfaceObj in parsedDLLFile.interfaceObjList)
            {
                if (!IsCorrectInterfaceName(interfaceObj.Name))
                {
                    Console.WriteLine($"INCORRECT INTERFACE PREFIX : {interfaceObj.Name}");
                    _errorMessage = "INCORRECT INTERFACE PREFIX : " + interfaceObj.Name;
                    errorCount++;
                }
            }

            foreach (ParsedStructure structObj in parsedDLLFile.structureObjList)
            {
                    if (!IsCorrectGenericParameterName(structObj.Name))
                    {
                        Console.WriteLine($"INCORRECT PARAMETER PREFIX : {structObj.Name}");
                        _errorMessage = "INCORRECT PARAMETER PREFIX : " + structObj.Name;
                        errorCount++;
                    }
            }

            if (errorCount == 0)
            {
                _verdict = 1;
            }
            else
            {
                _verdict = 0;
            }

            return new AnalyzerResult(_analyzerID, _verdict, _errorMessage);
        }


        /// <summary>
        /// Checks if a type name follows the correct interface prefix.
        /// </summary>
        /// <param name="name">The type name to check.</param>
        /// <returns>True if the type name has the correct interface prefix, otherwise false.</returns>
        private bool IsCorrectInterfaceName(string name)
        {
            return name.Length > 2 && name[0] == 'I' && char.IsUpper(name[1]);
        }

   /// <summary>
        /// Checks if a type name follows the correct type prefix.
        /// </summary>
        /// <param name="name">The type name to check.</param>
        /// <returns>True if the type name has the correct type prefix, otherwise false.</returns>
        private bool IsCorrectTypeName(string name)
        {
            if (name.Length < 3)
            {
                return true;
            }

            return name[0] switch
            {
                'I' => char.IsLower( name[1] ) || char.IsUpper( name[2] ),
                _ => true,
            };
        }
       
        /// <summary>
        /// Checks if a type name follows the correct generic parameter prefix.
        /// </summary>
        /// <param name="name">The type name to check.</param>
        /// <returns>True if the type name has the correct type prefix, otherwise false.</returns>
        private bool IsCorrectGenericParameterName (string name)
        {
            return (((name.Length > 1) && (name [0] != 'T')) || char.IsLower (name [0]));
        }
    }
}
