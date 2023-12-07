/******************************************************************************
 * Filename    = DisposableFieldsShouldBeDisposedRule.cs
 * 
 * Author      = Arun Sankar
 *
 * Product     = Analyzer
 * 
 * Project     = Analyzer
 *
 * Description = Class that implements Disposable Fields Should Be Disposed Analyser
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Parsing;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// This class represents an analyzer for checking that fields of disposable types are properly disposed.
    /// </summary>
    public class DisposableFieldsShouldBeDisposedRule : AnalyzerBase
    {
        /// <summary>
        /// Initializes a new instance of the DisposableFieldsShouldBeDisposedRule analyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files to analyze.</param>
        public DisposableFieldsShouldBeDisposedRule(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            analyzerID = "108";
        }

        /// <summary>
        /// Gets the result of the analysis, which includes the number of violations found.
        /// </summary>
        /// <returns>An AnalyzerResult containing the analysis results.</returns>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            int violationCount = 0;

            // Iterate through the classes in the DLL files
            foreach (ParsedClassMonoCecil classObj in parsedDLLFile.classObjListMC)
            {
                // Check if the class implements IDisposable
                if (ImplementsIDisposable(classObj.TypeObj))
                {
                    // Check if there are missing Dispose calls for disposable fields
                    int missingDisposeCalls = CheckDisposeCalls(classObj.TypeObj);
                    violationCount += missingDisposeCalls;
                }
            }
            // Provide an error string with information about the violations
            string errorString = violationCount > 0
                ? $"{violationCount} violations found: Some disposable fields are not properly disposed."
                : "No violations found.";
            int verdict = violationCount > 0 ? 0 : 1;
            return new AnalyzerResult(analyzerID, verdict, errorString);
        }

        /// <summary>
        /// Gets a list of fields in a class that are of disposable types.
        /// </summary>
        /// <param name="type">The class to analyze.</param>
        /// <returns>A list of disposable fields in the class.</returns>
        private List<FieldDefinition> GetDisposableFields( TypeDefinition type )
        {
            List<FieldDefinition> disposableFields = new();

            // Iterate through the fields in the class
            foreach (FieldDefinition field in type.Fields)
            {
                TypeReference fieldType = field.FieldType;
                TypeDefinition fieldTypeDefinition = fieldType.Resolve();

                // Check if the field's type is "System.IDisposable" or its resolved type definition implements IDisposable
                if (fieldType.FullName == "System.IDisposable" || (fieldTypeDefinition != null && ImplementsIDisposable( fieldTypeDefinition )))
                {
                    disposableFields.Add( field );
                }
            }

            return disposableFields;
        }


        /// <summary>
        /// Checks if a class implements IDisposable by checking its interfaces and base types.
        /// </summary>
        /// <param name="type">The class to check.</param>
        /// <returns>True if the class implements IDisposable; otherwise, false.</returns>
        private bool ImplementsIDisposable(TypeDefinition type)
        {
            // Check if the class directly implements IDisposable through interfaces
            if (type.Interfaces != null)
            {
                foreach (InterfaceImplementation? iface in type.Interfaces)
                {
                    if (iface.InterfaceType.FullName == "System.IDisposable")
                    {
                        return true;
                    }
                }
            }

            // Check if any base type implements IDisposable (recursively)
            if (type.BaseType != null)
            {
                TypeDefinition baseType = type.BaseType.Resolve();
                if (baseType != null)
                {
                    return ImplementsIDisposable(baseType);
                }
            }

            return false;
        }

        /// <summary>
        /// Checks for missing Dispose calls for disposable fields in a class.
        /// </summary>
        /// <param name="type">The class to check.</param>
        /// <returns>The number of missing Dispose calls.</returns>
        private int CheckDisposeCalls(TypeDefinition type)
        {
            int missingDisposeCalls = 0;

            // Collect all disposable fields in the class
            List<FieldDefinition> disposableFields = GetDisposableFields(type);

            if (disposableFields.Count > 0)
            {
                // Check if the class has a Dispose method (manually search for it)
                MethodDefinition? disposeMethod = null;
                foreach (MethodDefinition method in type.Methods)
                {
                    if (method.Name == "Dispose" && method.Parameters.Count == 0)
                    {
                        disposeMethod = method;
                        break;
                    }
                }

                if (disposeMethod != null)
                {
                    // Find all the calls to Dispose within the Dispose method
                    Collection<Instruction> disposeMethodInstructions = disposeMethod.Body.Instructions;

                    foreach (FieldDefinition field in disposableFields)
                    {
                        if (!IsDisposeCalledForField(disposeMethodInstructions, field))
                        {
                            missingDisposeCalls++;
                        }
                    }
                }
            }

            return missingDisposeCalls;
        }

        /// <summary>
        /// Checks if the Dispose method is called for a specific field within the Dispose method.
        /// </summary>
        /// <param name="instructions">The instructions within the Dispose method.</param>
        /// <param name="field">The field to check for Dispose calls.</param>
        /// <returns>True if Dispose is called for the field; otherwise, false.</returns>
        private static bool IsDisposeCalledForField(Collection<Instruction> instructions, FieldDefinition field)
        {
            foreach (Instruction instruction in instructions)
            {
                if (instruction.OpCode == OpCodes.Callvirt && instruction.Operand is MethodReference methodReference)
                {
                    if (methodReference.DeclaringType?.FullName == "System.IDisposable" &&
                        methodReference.Name == "Dispose")
                    {
                        // Check if the Dispose method is called on the field
                        if (instruction.Previous is Instruction ldFld &&
                        ldFld.OpCode == OpCodes.Ldfld &&
                        ldFld.Operand is FieldReference fieldRef &&
                        fieldRef.Resolve() == field)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }

}
