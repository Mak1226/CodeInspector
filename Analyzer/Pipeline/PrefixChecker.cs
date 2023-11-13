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
        
        private string errorMessage;
        private int verdict;
        private readonly string analyzerID;
        
        /// <summary>
        /// Initializes a new instance of the BaseAnalyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files for analysis.</param>
        public PrefixCheckerAnalyzer(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            // The constructor can be used for any necessary setup or initialization.
            errorMessage = "";
            verdict = 1;
            analyzerID = "Custom4";
        }

        /// <summary>
        /// Analyzes the DLL files to check type name prefixes for correctness.
        /// </summary>
        /// <returns>The number of errors found during the analysis.</returns>
        public override AnalyzerResult Run()
        {
            int errorCount = 0;

            foreach (var classObj in parsedDLLFiles.classObjList)
            {
                if (!IsCorrectTypeName(classObj.Name))
                {
                    //Console.WriteLine($"[Error] Incorrect type prefix: {classObj.Name}");
                    errorCount++;
                }
            }

            // To check interfaces
            foreach (var interfaceObj in parsedDLLFiles.interfaceObjList)
            {
                if (!IsCorrectInterfaceName(interfaceObj.Name))
                {
                    //Console.WriteLine($"[Error] Incorrect interface prefix: {interfaceObj.Name}");
                    errorCount++;
                }
            }

            foreach (var structObj in parsedDLLFiles.structureObjList)
            {
                    if (!IsCorrectGenericParameterName(structObj.Name))
                    {
                        //Console.WriteLine($"[Error] Incorrect generic parameter prefix: {structObj.Name}");
                        errorCount++;
                    }
            }

            if (errorCount == 0)
            {
                verdict = 1;
            }
            else
            {
                verdict = 0;
            }

            return new AnalyzerResult(analyzerID, verdict, errorMessage);
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

		switch (name [0]) {	
		case 'I':	
			return Char.IsLower (name [1]) ? true : Char.IsUpper (name [2]);
		default:
			return true;
		}
        }
        
        /// <summary>
        /// Checks if a type name follows the correct generic parameter prefix.
        /// </summary>
        /// <param name="name">The type name to check.</param>
        /// <returns>True if the type name has the correct type prefix, otherwise false.</returns>
        private bool IsCorrectGenericParameterName (string name)
	{
		return (((name.Length > 1) && (name [0] != 'T')) || Char.IsLower (name [0]));
	}
    }
}

