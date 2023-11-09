using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// A base class providing a common structure for various analyzers.
    /// </summary>
    abstract public class AnalyzerBase
    {
        /// <summary>
        /// The parsed DLL files to be used for analysis.
        /// </summary>
        public ParsedDLLFiles parsedDLLFiles;

        /// <summary>
        /// Initializes a new instance of the BaseAnalyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">The parsed DLL files for analysis.</param>
        public AnalyzerBase(ParsedDLLFiles dllFiles)
        {
            // Set the parsedDLLFiles field with the provided DLL files
            parsedDLLFiles = dllFiles;
        }

        public abstract AnalyzerResult Run();
    }
}

/*
abstract class 
pascal case
'Base' should be suffix
*/