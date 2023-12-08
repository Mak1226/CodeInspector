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

using System.Reflection;
using System.Text;
using Logging;
using Analyzer.Parsing;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// Given the list of all classes find abstract classes with at least one public constructor.
    /// Classes are the only type that can be abstract and allow constructor.
    /// This rule does not apply to Enums, Interfaces, Structs and Delegates.
    /// </summary>
    public class AbstractTypeNoPublicConstructor : AnalyzerBase
    {
        private string _errorMessage;   // Output message returned by the analyzer.
        private int _verdict;   // Verdict if the analyzer has passed or failed.

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractTypeNoPublicConstructor"/> analyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">List of ParsedDLL files to analyze.</param>
        public AbstractTypeNoPublicConstructor(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            _errorMessage = "";
            _verdict = 1;
            analyzerID = "101";
            Logger.Inform( $"[Analyzer][AbstractTypeNoPublicConstructor.cs] Created instance of analyzer AbstractTypeNoPublicConstructor" );
        }

        /// <summary>
        /// Finds all abstract classes with public or protected internal constructors.
        /// </summary>
        /// <param name="parsedDLLFile">ParsedDLL file to be analyzed.</param>
        /// <returns>List of all abstract types that have public constructors.</returns>
        private List<Type> FindAbstractTypeWithPublicConstructor(ParsedDLLFile parsedDLLFile) 
        {
            Logger.Inform( $"[Analyzer][AbstractTypeNoPublicConstructor.cs] FindAbstractTypeWithPublicConstructor: Running analyzer on {parsedDLLFile.DLLFileName} " );
            List<Type> abstractTypesWithPublicConstructors = new();  // List which stores all abstract types that have public constructors.
            // Loop over all classes in the provided DLLs.
            foreach (ParsedClass classObj in parsedDLLFile.classObjList)
            {
                Type classType = classObj.TypeObj;
                if (classType.GetTypeInfo().IsAbstract)
                {
                    ConstructorInfo[] allConstructors = classObj.Constructors;  
                    foreach (ConstructorInfo constructor in allConstructors)
                    {
                        // If is Public or Protected Internal.
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

        /// <summary>
        /// Helper function to form the error message.
        /// </summary>
        /// <param name="abstractTypesWithPublicConstructor">List of all violating types.</param>
        /// <returns>String with all the violating types.</returns>
        private string ErrorMessage(List<Type> abstractTypesWithPublicConstructor)
        {
            StringBuilder errorLog = new ("The following abstract classes have public constructors:\r\n");
                        
            foreach (Type type in abstractTypesWithPublicConstructor)
            {
                errorLog.AppendLine(type.FullName);
            }
            return errorLog.ToString();
        }

        /// <summary>
        /// Analyzes each DLL file for abstract classes with at least one public constructor
        /// And reports if the DLL violates the above.
        /// </summary>
        /// <param name="parsedDLLFile">Parsed DLL file.</param>
        /// <returns><see cref="AnalyzerResult"/> containing the analysis results.</returns>
        /// <exception cref="NullReferenceException">If the file object is null.</exception>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            List<Type> abstractTypesWithPublicConstructor;
            try
            {
                abstractTypesWithPublicConstructor = FindAbstractTypeWithPublicConstructor(parsedDLLFile);
                if (abstractTypesWithPublicConstructor.Count > 0)
                {
                    _verdict = 0;
                    _errorMessage = ErrorMessage(abstractTypesWithPublicConstructor);
                }
                else
                {
                    _errorMessage = "No violation found.";
                }
            }
            catch (Exception ex)
            {
                Logger.Error( $"[Analyzer][AbstractTypeNoPublicConstructor.cs] AnalyzeSingleDLL: Exception while analyzing {parsedDLLFile.DLLFileName} " + ex.Message);
                throw;
            }
            Logger.Debug( $"[Analyzer][AbstractTypeNoPublicConstructor.cs] AnalyzeSingleDLL: Successfully finished analyzing {parsedDLLFile.DLLFileName} " );
            return new AnalyzerResult(analyzerID, _verdict, _errorMessage);
        }
    }
}
