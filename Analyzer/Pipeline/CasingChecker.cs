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
    /// An analyzer that checks the correctness of casing in parsed DLL files.
    /// </summary>
    public class CasingChecker : BaseAnalyzer
    {
        
        private string errorMessage;
        private int verdict;
        private readonly string analyzerID;

        /// <summary>
        /// Initializes a new instance of the BaseAnalyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files for analysis.</param>
        public CasingChecker(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            // The constructor can be used for any necessary setup or initialization.
            errorMessage = "";
            verdict = 1;
            analyzerID = "Custom2";
        }

        /// <summary>
        /// Analyzes the DLL files to check casing for correctness.
        /// </summary>
        /// <returns>the verdict if the casing is right or not</returns>
        public override AnalyzerResult Run()
        {
            // Flag to track if any casing mistake is found
            int hasMistake = 0;

            // Check namespace names for PascalCasing
            foreach (string ns in parsedDLLFiles.GetNamespaces())
            {
                if (!IsPascalCase(ns))
                {
                    hasMistake = 1;
                    break; // If a mistake is found, exit the loop
                }
            }

            if (!hasMistake)
            {
                // Check type names for PascalCasing
                foreach (var type in parsedDLLFiles.GetTypes())
                {
                    if (!IsPascalCase(type))
                    {
                        hasMistake = 1;
                        break;
                    }
                }
            }

            if (!hasMistake)
            {
                // Check method names for PascalCasing and parameter names for camelCasing
                foreach (var method in parsedDLLFiles.GetMethods())
                {
                    if (!IsPascalCase(method.Name))
                    {
                        hasMistake = 1;
                        break;
                    }

                    if (!AreParametersCamelCased(method.Parameters))
                    {
                        hasMistake = 1;
                        break;
                    }
                }
            }
	
	    // Return an AnalyzerResult with a verdict (0 for mistakes, 1 for correct casing)            
            Verdict = hasMistake ? 0 : 1

            return new AnalyzerResult(analyzerID, verdict, errorMessage);
        }

        // check if name is PascalCased
	private static int IsPascalCase (string name)
	{
		if (String.IsNullOrEmpty (name))
			return 1;

		return Char.IsUpper (name [0]);
	}
	
	// check if name is camelCased
	private static int IsCamelCase (string name)
	{
		if (String.IsNullOrEmpty (name))
			return 1;

		return Char.IsLower (name [0]);
	}
	
	private int AreParametersCamelCased(Analyzer.Parsing.ParameterInfo[] parameters)
        {
            foreach (var param in parameters)
            {
                if (!IsCamelCase(param.Name))
                {
                    return 0;
                }
            }
            return 1;
        }

    }
}
