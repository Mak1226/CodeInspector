/******************************************************************************
* Filename    = CasingChecker.cs
*
* Author      = Monesh Vanga 
* 
* Product     = Analyzer
* 
* Project     = Analyzer
*
* Description = An analyzer that checks the correctness of casing of classes , namesspaces, parameters and methods in parsed DLL files.
*****************************************************************************/

using System;
using Analyzer.Parsing;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using Mono.Cecil;

namespace Analyzer.Pipeline
{
    public class CasingChecker : AnalyzerBase
    {
        private string _errorMessage;
        private int _verdict;

        /// <summary>
        /// Initializes a new instance of the BaseAnalyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files for analysis.</param>
        public CasingChecker(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            // The constructor can be used for any necessary setup or initialization.
            _errorMessage = "";
            _verdict = 1;
            analyzerID = "112";
        }

        /// <summary>
        /// Analyzes the DLL files to check casing for correctness.
        /// </summary>
        /// <returns>the verdict if the casing is right or not</returns>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            _errorMessage = "";
            _verdict = 1;

            // Return an AnalyzerResult with a verdict (0 for mistakes, 1 for correct casing)            
            if (casecheck(parsedDLLFile))
            {
                _verdict = 0;
            }

            else
            {
                _errorMessage = "No Violations Found";
                _verdict = 1;
            }

            return new AnalyzerResult(analyzerID, _verdict, _errorMessage);
        }

        //method for checking casing
        public bool casecheck(ParsedDLLFile parsedDLLFile)
        {
            // Flag to track if any casing mistake is found
            bool hasMistake = false;
            
            // Check namespace names for PascalCasing
            foreach(ParsedInterface interfaceObj in parsedDLLFile.interfaceObjList)
            {
                string? s = interfaceObj.TypeObj.Namespace;
                if (!IsPascalCase(s))
                {
                    hasMistake = true;
                    Console.WriteLine($"Incorrect Namespace Naming : {s}");
                    _errorMessage += "Incorrect Namespace Naming : " + s + " ";
                }
            }

            //Checking Class names for Pascal Casing
            foreach (ParsedClassMonoCecil cls in parsedDLLFile.classObjListMC)
            {
                if((cls.Name[0] >= 'a' && cls.Name[0] <= 'z') || (cls.Name[0] >= 'A' && cls.Name[0] <= 'Z'))
                {
                    if(!IsPascalCase(cls.Name))
                    {
                        hasMistake = true;
                        Console.WriteLine( $"Incorrect Class Naming : {cls.Name}" );
                        _errorMessage += "Incorrect Class Naming : " + cls.Name + " ";
                    }
                }
                
                // Check method names for PascalCasing and parameter names for camelCasing
                foreach (MethodDefinition method in cls.MethodsList)
                {
                    if(method.Name[0] != '.')
                    {
                        if (!IsPascalCase(method.Name))
                        {
                            hasMistake = true;
                            Console.WriteLine($"Incorrect Method Naming : {method.Name}");
                            _errorMessage += "Incorrect Method Naming : " + method.Name + " ";
                        }
                    }

                    if (!AreParametersCamelCased(method))
                    {
                        hasMistake = true;
                    }
                }
            }
            return hasMistake;
        }

        // check if name is PascalCased
        private static bool IsPascalCase( string name )
        {
            if (string.IsNullOrEmpty( name ))
            {  
                return true;
            }

            return char.IsUpper (name [0]);
        }

        // check if name is camelCased
        private static bool IsCamelCase (string name)
        {
            if (string.IsNullOrEmpty( name ))
            {
                return true;
            }

            return char.IsLower (name [0]);
        }

        //method for checking parameters casing
        private bool AreParametersCamelCased(MethodDefinition method)
        {
                int flag = 0;
                   
                foreach (ParameterDefinition param in method.Parameters)
                {
                    //if the parameter starts with underscore
                    if (param.Name[0] != '_')
                    {
                        if (!IsCamelCase( param.Name ))
                        {
                            Console.WriteLine( $"Incorrect Parameter Naming : {param.Name}" );
                            _errorMessage += "Incorrect Parameter Naming : " + param.Name + " ";
                            flag = 1;
                        }
                    }

                    //if the parameter does not start with underscore
                    else
                    {
                        if (!char.IsLower(param.Name[1]))
                        {
                            Console.WriteLine( $"Incorrect Parameter Naming : {param.Name}" );
                            _errorMessage += "Incorrect Parameter Naming : " + param.Name + " ";    
                            flag = 1;
                        }
                    }
                }

                //if mistake is found
                if(flag==1)
                {
                    return false;
                }

                return true;
        }

    }
}
