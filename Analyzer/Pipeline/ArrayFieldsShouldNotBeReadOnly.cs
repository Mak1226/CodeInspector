using Analyzer.Parsing;
using Mono.Cecil.Cil;
using Mono.Cecil;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// Analyzer rule to detect and report readonly array fields in classes.
    /// </summary>
    public class ArrayFieldsShouldNotBeReadOnlyRule : AnalyzerBase
    {
        // Fields to store analysis results
        private string errorMessage;
        private int verdict;
        private readonly string analyzerID;

        /// <summary>
        /// Initializes a new instance of the ArrayFieldsShouldNotBeReadOnlyRule class.
        /// </summary>
        /// <param name="dllFiles">The ParsedDLLFiles object containing the parsed DLL information.</param>
        public ArrayFieldsShouldNotBeReadOnlyRule(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            // Initialize fields
            errorMessage = "";
            verdict = 1;
            analyzerID = "arrayFieldsShouldNotBeReadOnly";
        }

        /// <summary>
        /// Runs the analysis to check for readonly array fields in classes.
        /// </summary>
        /// <returns>An AnalyzerResult object indicating the analysis result.</returns>
        public override AnalyzerResult Run()
        {
            CheckForReadOnlyArrayFields();
            return new AnalyzerResult(analyzerID, verdict, errorMessage);
        }

        /// <summary>
        /// Checks each class for the presence of readonly array fields.
        /// </summary>
        private void CheckForReadOnlyArrayFields()
        {
            // Iterate through each class and its fields
            foreach (ParsedClassMonoCecil cls in parsedDLLFiles.classObjListMC)
            {
                foreach (FieldDefinition field in cls.FieldsList)
                {
                    // Check if the field is an array and is marked as readonly
                    if (field.IsInitOnly && field.IsPublic && field.FieldType.IsArray)
                    {
                        verdict = 0;
                        errorMessage = "Readonly array field found.";
                        return; // You can return early if a readonly array field is found.
                    }
                }
            }
        }
    }
}
