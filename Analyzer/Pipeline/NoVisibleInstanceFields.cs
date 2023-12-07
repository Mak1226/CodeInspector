/******************************************************************************
* Filename    = NoVisibleInstanceFields.cs
* 
* Author      = Sneha Bhattacharjee
*
* Product     = Analyzer
* 
* Project     = Analyzer
*
* Description = Class should not have non-private instance field. 
*               The primary use of a field should be as an implementation detail. 
*               Fields should be private or internal and should be exposed by using properties. 
*****************************************************************************/

using Analyzer.Parsing;
using System.Text;
using Mono.Cecil;
using System.Diagnostics;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// Instance ields inside a class must not be visible outside.
    /// </summary>
    public class NoVisibleInstanceFields : AnalyzerBase
    {
        private string _errorMessage;   // Output message returned by the analyzer.
        private int _verdict;   // Verdict if the analyzer has passed or failed.
        private readonly string _analyzerID;    // Unique ID for the analyzer.

        /// <summary>
        /// Initializes a new instance of the <see cref="NoVisibleInstanceFields"/> analyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">List of ParsedDLL files to analyze.</param>
        public NoVisibleInstanceFields(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            _errorMessage = "";
            _verdict = 1;
            _analyzerID = "118";
        }

        /// <summary>
        /// Finds all visible instance fields in that DLL.
        /// </summary>
        /// <param name="parsedDLLFile">DLL file to be analyzed.</param>
        /// <returns>List of violating fields in the DLL.</returns>
        private List<FieldDefinition> FindVisibleNativeFields(ParsedDLLFile parsedDLLFile)
        {
            List<FieldDefinition> visibleNativeFieldsList = new();

            foreach (ParsedClassMonoCecil classobj in parsedDLLFile.classObjListMC)
            {
                TypeDefinition classtype = classobj.TypeObj;

                /*
                // rule does not apply to interface, enumerations and delegates or to types without fields
                if (classtype.IsValueType)
                {
                    // todo
                    // for now implementing just for classes
                    // classes and namespaces can have structures
                    // structures are recommended to be immutable too
                    // otherwise they should also have getter and setter
                    // note: parseddllfiles.classobjlistmc provides only classes
                    continue;
                }
                */

                // By default, this rule only looks at externally visible types.
                if (!classtype.IsPublic)
                {
                    continue;
                }

                foreach (FieldDefinition field in classtype.Fields)
                {
                    // IsFamilyOrAssembly for protected internal.
                    // IsFamily           for protected.
                    // IsAssembly         for internal.
                    if (field.IsPrivate || 
                        (field.IsAssembly && !field.IsFamilyOrAssembly))
                    {
                        continue;
                    }
                    else
                    {
                        // IsInitOnly for readonly.
                        if (field.IsPublic && field.IsInitOnly)
                        {
                            continue;
                        }
                        else
                        {
                            visibleNativeFieldsList.Add(field);
                        }
                    }
                }
            }
            return visibleNativeFieldsList;
        }

        /// <summary>
        /// Helper function to form the error message.
        /// </summary>
        /// <param name="visibleNativeFieldsList">List of all violating types.</param>
        /// <returns>String with all the violating types.</returns>
        private string ErrorMessage(List<FieldDefinition> visibleNativeFieldsList)
        {
            StringBuilder errorLog = new ("The following native fields are visible:\r\n");

            foreach (FieldDefinition field in visibleNativeFieldsList)
            {
                errorLog.AppendLine(field.FullName);
            }
            return errorLog.ToString();
        }

        /// <summary>
        /// Analyzes each DLL file for externally visible instance fields
        /// And reports if the DLL violates the above.
        /// </summary>
        /// <param name="parsedDLLFile">Parsed DLL file.</param>
        /// <returns><see cref="AnalyzerResult"/> containing the analysis results.</returns>
        /// <exception cref="NullReferenceException">If the file object is null.</exception>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            List<FieldDefinition> visibleNativeFieldsList = FindVisibleNativeFields(parsedDLLFile);
            try
            {
                if (visibleNativeFieldsList.Count > 0)
                {
                    _verdict = 0;
                    _errorMessage = ErrorMessage(visibleNativeFieldsList);
                }
                else
                {
                    _errorMessage = "No violation found.";
                }
            }
            catch (NullReferenceException ex)
            {
                throw;
            }

            return new AnalyzerResult(_analyzerID, _verdict, _errorMessage);
        }
    }
}
