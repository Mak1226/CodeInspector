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
    /// <summary>
    /// An analyzer that checks the correctness of casing in parsed DLL files.
    /// </summary>
    public class CasingChecker : AnalyzerBase
    {
       
        private string errorMessage;
        private int verdict;
        private readonly string analyzerID;

        /// <summary>
        /// Initializes a new instance of the BaseAnalyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files for analysis.</param>
        public CasingChecker(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            errorMessage = "";
            verdict = 1;
            // The constructor can be used for any necessary setup or initialization.
            analyzerID = "112";
        }

        /// <summary>
        /// Analyzes the DLL files to check casing for correctness.
        /// </summary>
        /// <returns>the verdict if the casing is right or not</returns>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            errorMessage = "";
            verdict = 1;

            // Return an AnalyzerResult with a verdict (0 for mistakes, 1 for correct casing)            
            if (casecheck(parsedDLLFile))
            {
                verdict = 0;
            }

            else
            {
                verdict = 1;
            }

            return new AnalyzerResult(analyzerID, verdict, errorMessage);
        }

        public bool casecheck(ParsedDLLFile parsedDLLFile)
        {
            // Flag to track if any casing mistake is found
            bool hasMistake = false;

            // Check namespace names for PascalCasing
            foreach(var classObj in parsedDLLFile.classObjListMC)
            {
                if (!IsPascalCase(classObj.TypeObj.BaseType.Namespace))
                {
                    hasMistake = true;
                    Console.WriteLine($"INCORRECT NAMESPACE NAMING : {classObj.TypeObj.BaseType.Namespace}");
                }
            }

            if (!hasMistake)
            {
                foreach (ParsedClassMonoCecil cls in parsedDLLFile.classObjListMC)
                {
                    // Check method names for PascalCasing and parameter names for camelCasing
                    foreach (MethodDefinition method in cls.MethodsList)
                    {
                        if (!IsPascalCase(method.Name))
                        {
                            hasMistake = true;
                            Console.WriteLine($"INCORRECT METHOD NAMING : {method.Name}");
                        }

                        if (!AreParametersCamelCased(method))
                        {
                            hasMistake = true;
                        }
                    }
                }
            }
            return hasMistake;
        }

        // check if name is PascalCased
        private static bool IsPascalCase(string name)
        {
           if (String.IsNullOrEmpty (name))
           return true;

           return Char.IsUpper (name [0]);
           }

           // check if name is camelCased
           private static bool IsCamelCase (string name)
           {
           if (String.IsNullOrEmpty (name))
           return true;

           return Char.IsLower (name [0]);
           }

           private bool AreParametersCamelCased(MethodDefinition method)
           {
                    int flag = 0;
                    
                    foreach (var param in method.Parameters)
                    {
                        if (!IsCamelCase(param.Name))
                        {
                            Console.WriteLine($"INCORRECT PARAMETER NAMING : {param.Name}");                            
                            flag = 1;
                        }
                    }

                    if(flag==1)
                    {
                        return false;
                    }

                    return true;
           }

        }
}