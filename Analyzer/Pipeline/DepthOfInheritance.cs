using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// This class represents an analyzer for calculating the depth of inheritance for classes in DLL files.
    /// </summary>
    internal class DepthOfInheritance : BaseAnalyzer
    {
        /// <summary>
        /// Initializes a new instance of the DepthOfInheritance analyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files to analyze.</param>
        public DepthOfInheritance(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            // The constructor can be used for any necessary setup or initialization.
            // In this case, it sets the parsedDLLFiles field with the provided DLL files.
        }

        /// <summary>
        /// Calculates the depth of inheritance for classes in the parsed DLL files.
        /// </summary>
        /// <returns>
        /// A dictionary that maps class types to their depth of inheritance.
        /// </returns>
        public Dictionary<Type, int> CalculateDepthOfInheritance()
        {
            // Create a dictionary to store class types and their corresponding depth of inheritance
            Dictionary<Type, int> depthOfInheritanceMap = new();

            // Iterate through the ParsedClass objects in the DLL files
            foreach (ParsedClass classObj in parsedDLLFiles.classObjList)
            {
                // Calculate the depth of inheritance for the current class
                int depth = CalculateDepth(classObj.TypeObj);

                // Store the depth in the dictionary with the class type as the key
                depthOfInheritanceMap[classObj.TypeObj] = depth;
            }

            // Return the dictionary containing class types and their depth of inheritance
            return depthOfInheritanceMap;
        }

        /// <summary>
        /// Helper method to calculate the depth of inheritance for a given class type.
        /// </summary>
        /// <param name="type">The class type for which to calculate the depth of inheritance.</param>
        /// <returns>
        /// The depth of inheritance for the specified class type.
        /// </returns>
        private static int CalculateDepth(Type type)
        {
            int depth = 0;
            while (type != null)
            {
                depth++;
                // Update the type variable to be the base class of the current class
                type = type.BaseType!;
            }

            // Subtract 1 to exclude System.Object, which is at depth 0
            if (depth > 0)
            {
                depth--;
            }
            return depth;
        }
    }
}
