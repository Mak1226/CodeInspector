using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                        if (constructor.IsPublic)
                        {
                            abstractTypesWithPublicConstructors.Add(classType);
                            break;
                        }
                    }
                }
            }
            return abstractTypesWithPublicConstructors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abstractTypesWithPublicConstructor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private string ErrorMessage(List<Type> abstractTypesWithPublicConstructor)
        {
            var errorLog = new System.Text.StringBuilder("The following abstract classes have public constructors:");
                        
            foreach (Type type in abstractTypesWithPublicConstructor)
            {
                try
                {
                    // sanity check
                    errorLog.AppendLine(type.Name.ToString());
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    throw new ArgumentOutOfRangeException("Invalid Argument ", ex);
                }
                
            }
            return errorLog.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            _errorMessage = "";
            _verdict = 1;

            List<Type> abstractTypesWithPublicConstructor = FindAbstractTypeWithPublicConstructor(parsedDLLFile);
            if (abstractTypesWithPublicConstructor.Count > 0)
            {
                _verdict = 0;
                _errorMessage = ErrorMessage(abstractTypesWithPublicConstructor);
            }
            return new AnalyzerResult(_analyzerID, _verdict, _errorMessage);
        }
    }
}
