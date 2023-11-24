/******************************************************************************
* Filename    = DiagramBase.cs
* 
* Author      = Sneha Bhattacharjee
*
* Product     = Analyzer
* 
* Project     = Analyzer
*
* Description = A base class providing a common structure for UML Diagram.
*****************************************************************************/

using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.UMLDiagram
{
    /// <summary>
    /// A base class providing a common structure for UML Diagram.
    /// </summary>
    public abstract class DiagramBase
    {
        /// <summary>
        /// The parsed DLL files to be used for analysis.
        /// </summary>
        public List<ParsedDLLFile> parsedDLLFiles;

        /// <summary>
        /// Initializes a new instance of the DiagramBase with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files for drawing.</param>
        public DiagramBase(List<ParsedDLLFile> dllFiles)
        {
            // Set the parsedDLLFiles field with the provided DLL files
            parsedDLLFiles = dllFiles;
        }
    }
}
