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
    public class AbstractClassNamingChecker : AnalyzerBase
    {

        private string _errorMessage;
        private int _verdict;
        private readonly string _analyzerID;

        /// <summary>
        /// Initializes a new instance of the AbstractClassNamingChecker analyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files.</param>
        public AbstractClassNamingChecker(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            // TODO if required
            _errorMessage = "";
            _verdict = 1;
            _analyzerID = "111";
        }

        /// <summary>
        /// Returns 1 if all abstract classes meet the criteria, otherwise 0.
        /// </summary>
        /// <returns>The score for the analyzer.</returns>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            _errorMessage = "No Violation Found";
            _verdict = 1;

            // Check if there is at least one abstract class that does not meet the criteria
            if (IncorrectAbstractClassName(parsedDLLFile))
            {
                _verdict = 0; // If there is any abstract class not meeting the criteria, set the score to 0
            }

            else
            {
                _verdict = 1; // If all abstract classes meet the criteria, set the score to 1
            }

            return new AnalyzerResult(_analyzerID, _verdict, _errorMessage);
        }

        /// <summary>
        /// Checks if a string is in Pascal case.
        /// </summary>
        /// <param name="s">The string to check.</param>
        /// <returns>True if the string is in Pascal case, false otherwise.</returns>
        private bool IsPascalCase(string s)
        {
            if (string.IsNullOrEmpty( s ))
            {
                return false;
            }

            return char.IsLower( s[0]);
        }

        /// <summary>
        /// Checks if there is at least one abstract class that does not meet the criteria.
        /// </summary>
        /// <returns>True if there is any abstract class not meeting the criteria, false if all meet the criteria.</returns>
        private bool IncorrectAbstractClassName(ParsedDLLFile parsedDLLFile)
        {
            int flag = 0;

            foreach (ParsedClass classObj in parsedDLLFile.classObjList)
            {
                Type classType = classObj.TypeObj;

                if (classType.GetTypeInfo().IsAbstract)
                {
                    string className = classType.Name;

                    // Check if the class name is not in Pascal case or does not end with 'Base'
                    if (!IsPascalCase(className) || !className.EndsWith("Base"))
                    {
                        Console.WriteLine($"Incorrect Abstract Class Naming : {className}");
                        _errorMessage += "Incorrect Abstract Class Naming : " + className;
                        flag = 1;// If any abstract class does not meet the criteria, return true
                    }
                }
            }

            if(flag == 1)
            {
                return true;
            }

            return false; // If all abstract classes meet the criteria, return false
        }
    }
}