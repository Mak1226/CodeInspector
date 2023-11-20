using Analyzer.Parsing;
using Mono.Cecil.Cil;
using Mono.Cecil;
using System;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// Analyzer rule for detecting readonly array fields in classes.
    /// </summary>
    public class ArrayFieldsShouldNotBeReadOnlyRule : AnalyzerBase
    {
        private string errorMessage;
        private int verdict;
        private readonly string analyzerID;

        public ArrayFieldsShouldNotBeReadOnlyRule(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            errorMessage = "";
            verdict = 1;
            analyzerID = "106";
        }

        /// <summary>
        /// Runs the analysis to check for readonly array fields in classes.
        /// </summary>
        /// <returns>An <see cref="AnalyzerResult"/> based on the analysis.</returns>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            errorMessage = "";
            verdict = 1;

            CheckForReadOnlyArrayFields(parsedDLLFile);

            // If no errors, add a message indicating everything looks fine
            if (string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = "Everything looks fine. No readonly array fields found.";
            }

            return new AnalyzerResult(analyzerID, verdict, errorMessage);
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
                        errorMessage += $"Readonly array field found in class {field.DeclaringType.FullName}, field {field.Name}.{Environment.NewLine}";
                    }
                }
            }

            // After checking all classes and fields, if any read-only array fields were found, set verdict to 0.
            if (!string.IsNullOrEmpty(errorMessage))
            {
                verdict = 0;
            }
        }
    }
}
