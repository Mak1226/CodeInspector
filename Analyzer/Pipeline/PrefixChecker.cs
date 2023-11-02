using System;
using Analyzer.Parsing;

namespace Analyzer.Pipeline.Analyzers
{
    /// <summary>
    /// An analyzer that checks the correctness of type name prefixes in parsed DLL files.
    /// </summary>
    public class PrefixCheckerAnalyzer : BaseAnalyzer
    {
        /// <summary>
        /// Initializes a new instance of the BaseAnalyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files for analysis.</param>
        public PrefixCheckerAnalyzer(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            // The constructor can be used for any necessary setup or initialization.
        }

        /// <summary>
        /// Analyzes the DLL files to check type name prefixes for correctness.
        /// </summary>
        /// <returns>The number of errors found during the analysis.</returns>
        public override int GetScore()
        {
            int errorCount = 0;

            foreach (var type in parsedDLLFiles.Types)
            {
                if (type.IsInterface)
                {
                    if (!IsCorrectInterfaceName(type.Name))
                    {
                        Console.WriteLine($"[Error] Incorrect interface prefix: {type.Name}");
                        errorCount++;
                    }
                }
                else
                {
                    if (!IsCorrectTypeName(type.Name))
                    {
                        Console.WriteLine($"[Error] Incorrect type prefix: {type.Name}");
                        errorCount++;
                    }
                }
            }

            if(errorCount == 0)
            {
            	return 1;
            }
            
            else
            {
            	return 0;
            }
        }

        /// <summary>
        /// Checks if a type name follows the correct interface prefix.
        /// </summary>
        /// <param name="name">The type name to check.</param>
        /// <returns>True if the type name has the correct interface prefix, otherwise false.</returns>
        private bool IsCorrectInterfaceName(string name)
        {
            return name.Length >= 2 && name[0] == 'I' && char.IsUpper(name[1]);
        }

	/// <summary>
        /// Checks if a type name follows the correct type prefix.
        /// </summary>
        /// <param name="name">The type name to check.</param>
        /// <returns>True if the type name has the correct type prefix, otherwise false.</returns>
        private bool IsCorrectTypeName(string name)
        {
            return name.Length < 3 || char.IsLower(name[1]) || (char.IsUpper(name[0]) && char.IsUpper(name[2]));
        }
    }
}
