/******************************************************************************
* Filename    = ArrayFieldsShouldNotBeReadOnly.cs
* 
* Author      = Thanmayee
* 
* Project     = Analyzer
*
* Description = Analyzer to identify readonly array fields 
*****************************************************************************/

using Analyzer.Parsing;
using Mono.Cecil;


namespace Analyzer.Pipeline
{
    /// <summary>
    /// Analyzer rule for detecting readonly array fields in classes.
    /// </summary>
    public class ArrayFieldsShouldNotBeReadOnlyRule : AnalyzerBase
    {
        private string _errorMessage;
        private int _verdict;

        public ArrayFieldsShouldNotBeReadOnlyRule(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            _errorMessage = "";
            _verdict = 1;
            analyzerID = "106";
        }

        /// <summary>
        /// Runs the analysis to check for readonly array fields in classes.
        /// </summary>
        /// <returns>An <see cref="AnalyzerResult"/> based on the analysis.</returns>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            _errorMessage = "";
            _verdict = 1;

            CheckForReadOnlyArrayFields(parsedDLLFile);

            // If no errors, add a message indicating everything looks fine
            if (string.IsNullOrEmpty(_errorMessage))
            {
                _errorMessage = "No readonly array fields found.";
            }

            return new AnalyzerResult(analyzerID, _verdict, _errorMessage);
        }

        /// <summary>
        /// Checks each class for readonly array fields.
        /// </summary>
        private void CheckForReadOnlyArrayFields(ParsedDLLFile parsedDLLFile)
        {
            foreach (ParsedClassMonoCecil cls in parsedDLLFile.classObjListMC)
            {
                foreach (FieldDefinition field in cls.FieldsList)
                {
                    // Check if the field is an array and is marked as readonly
                    if (field.IsInitOnly && field.IsPublic && field.FieldType.IsArray)
                    {
                        // Modify the errorMessage to include information about the read-only array field
                        _errorMessage += $"Readonly array field found in class {field.DeclaringType.FullName}, field {field.Name}.{Environment.NewLine}";
                    }
                }
            }

            // After checking all classes and fields, if any read-only array fields were found, set verdict to 0.
            if (!string.IsNullOrEmpty(_errorMessage))
            {
                _verdict = 0;
            }
        }
    }
}
