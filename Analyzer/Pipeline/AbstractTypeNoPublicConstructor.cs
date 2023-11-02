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
    /// Given the list of all classes find abstract classes with at least one public constructor.
    /// Classes are the only type that can be abstract and allow constrctors.
    /// Enums
    /// </summary>
    internal class AbstractTypeNoPublicConstructor : BaseAnalyzer
    {
        /// <summary>
        /// Initializes a new instance of the AbstractTypeNoPublicConstructor analyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles"></param>
        public AbstractTypeNoPublicConstructor(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            // TODO if required
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>List of all abstract types that have public constructors.</returns>
        public List<Type> FindAbstractTypeWithPublicConstructor() 
        {
            List<Type> abstractTypesWithPublicConstructors = new List<Type>();  // List which stores all abstract types that have public constructors
            // Loop over all classes in the provided DLLs
            foreach (ParsedClass classObj in parsedDLLFiles.classObjList)
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
    }
}