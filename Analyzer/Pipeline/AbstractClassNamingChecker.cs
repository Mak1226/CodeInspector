using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// Analyzer to check whether abstract classes are Pascal cased and have a 'Base' suffix or not.
    /// </summary>
    public class AbstractClassNamingChacker : BaseAnalyzer
    {
        
        private string errorMessage;
        private int verdict;
        private readonly string analyzerID;
        
        /// <summary>
        /// Initializes a new instance of the AbstractClassNamingChecker analyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files.</param>
        public AbstractClassNamingChecker(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            // TODO if required
            errorMessage = "";
            verdict = 1;
            analyzerID = "Custom3";
        }

        /// <summary>
        /// Returns 1 if all abstract classes meet the criteria, otherwise 0.
        /// </summary>
        /// <returns>The score for the analyzer.</returns>
        public override AnalyzerResult Run()
        {
            // Check if there is at least one abstract class that does not meet the criteria
            if (IncorrectAbstractClassName())
            {
                verdict = 0; // If there is any abstract class not meeting the criteria, set the score to 0
            }

            else
            {
            	verdict = 1; // If all abstract classes meet the criteria, set the score to 1
            }
            
            return new AnalyzerResult(analyzerID, verdict, errorMessage);
        }

        /// <summary>
        /// Checks if a string is in Pascal case.
        /// </summary>
        /// <param name="s">The string to check.</param>
        /// <returns>True if the string is in Pascal case, false otherwise.</returns>
        private bool IsPascalCase(string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
                return false;

            for (int i = 1; i < s.Length; i++)
            {
                if (!char.IsLetter(s[i]) || char.IsLower(s[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if there is at least one abstract class that does not meet the criteria.
        /// </summary>
        /// <returns>True if there is any abstract class not meeting the criteria, false if all meet the criteria.</returns>
        private bool IncorrectAbstractClassName()
        {
            foreach (ParsedClass classObj in parsedDLLFiles.classObjList)
            {
                Type classType = classObj.TypeObj;

                if (classType.GetTypeInfo().IsAbstract)
                {
                    string className = classType.Name;

                    // Check if the class name is not in Pascal case or does not end with 'Base'
                    if (!IsPascalCase(className) || !className.EndsWith("Base"))
                    {
                        return true; // If any abstract class does not meet the criteria, return true
                    }
                }
            }

            return false; // If all abstract classes meet the criteria, return false
        }
    }
}

