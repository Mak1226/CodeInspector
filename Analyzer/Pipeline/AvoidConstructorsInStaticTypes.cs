using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// This class represents an analyzer which check if a class containing all static fields and methods has a public not static constructor
    /// </summary>
    public class AvoidConstructorsInStaticTypes : AnalyzerBase
    {
        // Unique identifier for the analyzer
        private readonly string _analyzerID;
        private string _errorMessage;
        private int _verdict;

        // Set to store classes violating the rule
        public HashSet<ParsedClass> violatingClasses;

        /// <summary>
        /// Initializes a new instance of the AvoidConstructorsWithStaticTypes analyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles"></param>
        public AvoidConstructorsInStaticTypes( List<ParsedDLLFile> dllFiles ) : base( dllFiles )
        {
            _analyzerID = "102";
        }

        /// <summary>
        /// Checks if all methods and fields of a class are static or not
        /// </summary>
        /// <param name="cls"></param>
        /// <returns>True if all methods and fields in the class are static, else returns False</returns>
        public bool CheckAllStatic( ParsedClass cls )
        {
            // Get all methods in the class
            MethodInfo[] methods = cls.TypeObj.GetMethods();

            //if any method or any parameterized constructor is not declared as static, return false immediately.
            if (methods.Length != 0)
            {
                foreach (MethodInfo method in methods)
                {
                    Type? declaringType = method.DeclaringType;
                    if ((declaringType != null) && (declaringType.ToString().StartsWith( "System.Object" )))
                    {
                        continue;
                    }

                    if (!method.IsStatic)
                    {
                        return false;
                    }

                }
            }

            // Get all fields in the class
            FieldInfo[] fields = cls.TypeObj.GetFields();

            if (fields.Length != 0)
            {
                foreach (FieldInfo field in fields)
                {
                    if (!field.IsStatic)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Constructs the AnalyzerResult object based on the analysis.
        /// </summary>
        /// <param name="parsedDLLFile"></param>
        protected override AnalyzerResult AnalyzeSingleDLL( ParsedDLLFile parsedDLLFile )
        {
            _errorMessage = "";
            _verdict = 1;
            violatingClasses = new HashSet<ParsedClass>();
            foreach (ParsedClass cls in parsedDLLFile.classObjList)
            {
                if (cls.TypeObj.IsDefined( typeof( CompilerGeneratedAttribute ) , false ))
                {
                    continue;
                }

                // Check if all methods and fields in the class are static
                bool isAllStatic = CheckAllStatic( cls );

                // If all methods and fields are static, check constructors
                if (isAllStatic)
                {
                    foreach (ConstructorInfo constructor in cls.Constructors)
                    {
                        //checking if any parameterized constructor is not declared static
                        if (constructor.GetParameters().Length > 0)
                        {
                            continue;
                        }
                        //default constructor should be declared static or private to avoid violations
                        if (!constructor.IsStatic && !constructor.IsPrivate)
                        {
                            violatingClasses.Add( cls );
                        }
                    }
                }
            }

            //If any class violates the rule, return verdict as 0 (Failed), else 1 (Passed)
            int violations = violatingClasses.Count;
            if (violations > 0)
            {
                _verdict = 0;
            }

            if (_verdict != 0)
            {
                _verdict = 1;
                _errorMessage = "No violation found";
                return new AnalyzerResult( _analyzerID , _verdict , _errorMessage );
            }

            //adding error message
            _errorMessage = "Classes ";
            foreach (ParsedClass cls in violatingClasses)
            {
                _errorMessage += cls.TypeObj.FullName;
                violations--;
                if (violations != 0)
                {
                    _errorMessage += ", ";
                }
            }
            _errorMessage += " contains only static fields and methods, but has non-static, visible constructor. Try changing it to private or make it static.";

            //Return the AnalyzerResult object, with appropriate error mesaage.
            AnalyzerResult resultObj = new( _analyzerID , _verdict , _errorMessage );
            return resultObj;
        }
    }
}
