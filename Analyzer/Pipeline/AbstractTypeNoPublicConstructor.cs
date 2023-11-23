/******************************************************************************
* Filename    = AbstractTypeNoPublicConstructor.cs
* 
* Author      = Sneha Bhattacharjee
*
* Product     = Analyzer
* 
* Project     = Analyzer
*
* Description = Abstract types must not have Public or Protected Internal constructors. 
*               Constructors on abstract types can be called only by derived types. 
*               Because public constructors create instances of a type and you cannot create instances of an abstract type, 
*               an abstract type that has a public constructor is incorrectly designed.
*****************************************************************************/

using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// Given the list of all classes find abstract classes with at least one public constructor.
    /// Classes are the only type that can be abstract and allow constructor.
    /// Enums
    /// </summary>
    public class AbstractTypeNoPublicConstructor : AnalyzerBase
    {
        private string _errorMessage;
        private int _verdict;
        private readonly string _analyzerID;

        /// <summary>
        /// Initializes a new instance of the AbstractTypeNoPublicConstructor analyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles"></param>
        public AbstractTypeNoPublicConstructor(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            _errorMessage = "";
            _verdict = 1;
            _analyzerID = "101";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>List of all abstract types that have public constructors.</returns>
        private List<Type> FindAbstractTypeWithPublicConstructor(ParsedDLLFile parsedDLLFile) 
        {
            List<Type> abstractTypesWithPublicConstructors = new();  // List which stores all abstract types that have public constructors
            // Loop over all classes in the provided DLLs
            foreach (ParsedClass classObj in parsedDLLFile.classObjList)
            {
                Type classType = classObj.TypeObj;
                if (classType.GetTypeInfo().IsAbstract)
                {
                    ConstructorInfo[] allConstructors = classObj.Constructors;  
                    // Loop over all constructors of that class
                    foreach (ConstructorInfo constructor in allConstructors)
                    {
                        if (constructor.IsPublic || constructor.IsFamilyOrAssembly)
                        {
                            abstractTypesWithPublicConstructors.Add(classType);
                            break;
                        }
                    }
                }
            }
            return abstractTypesWithPublicConstructors;
        }


        private string ErrorMessage(List<Type> abstractTypesWithPublicConstructor)
        {
            var errorLog = new System.Text.StringBuilder("The following abstract classes have public constructors:\r\n");
                        
            foreach (Type type in abstractTypesWithPublicConstructor)
            {
                errorLog.AppendLine(type.FullName);
                
            }
            return errorLog.ToString();
        }

        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            List<Type> abstractTypesWithPublicConstructor;
            try
            {
                abstractTypesWithPublicConstructor = FindAbstractTypeWithPublicConstructor( parsedDLLFile );
                if (abstractTypesWithPublicConstructor.Count > 0)
                {
                    _verdict = 0;
                    _errorMessage = ErrorMessage( abstractTypesWithPublicConstructor );
                }
                else
                {
                    _errorMessage = "No violation found.";
                }
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException("Encountered exception while processing.", ex);
            }

            return new AnalyzerResult(_analyzerID, _verdict, _errorMessage);
        }
    }
}
