/******************************************************************************
* Filename    = NoEmptyInterface.cs
* 
* Author      = Sneha Bhattacharjee
*
* Product     = Analyzer
* 
* Project     = Analyzer
*
* Description = The interface does not declare any members. 
*               A type implements an interface by providing implementations for the members of the interface. 
*               An empty interface does not define any members. 
*               Therefore, it does not define a contract that can be implemented.
*****************************************************************************/

using Analyzer.Parsing;
using Logging;
using System.Diagnostics;
using System.Text;

namespace Analyzer.Pipeline
{
    /// <summary>
    /// Given the list of all interfaces, find those which are empty.
    /// </summary>
    public class NoEmptyInterface : AnalyzerBase
    {
        private string _errorMessage;   // Output message returned by the analyzer.
        private int _verdict;   // Verdict if the analyzer has passed or failed.

        /// <summary>
        /// Initializes a new instance of the <see cref="NoEmptyInterface"/> analyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles">List of ParsedDLL files to analyze.</param>
        public NoEmptyInterface(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            _errorMessage = "";
            _verdict = 1;
            analyzerID = "104";
            Logger.Inform( $"[Analyzer][NoEmptyInterface.cs] Created instance of analyzer NoEmptyInterface" );

        }

        /// <summary>
        /// Finds empty interfaces in the DLL.
        /// </summary>
        /// <returns>List of empty interfaces.</returns>
        /// <param name="parsedDLLFile">DLL file to be analyzed.</param>
        public List<Type> FindEmptyInterfaces(ParsedDLLFile parsedDLLFile)
        {
            Logger.Inform( $"[Analyzer][NoEmptyInterface.cs] FindEmptyInterfaces: Running analyzer on {parsedDLLFile.DLLFileName} " );

            List<Type> emptyInterfaceList = new();

            foreach (ParsedInterface interfaceObj in parsedDLLFile.interfaceObjList)
            {
                Type interfaceType = interfaceObj.TypeObj;

                if (interfaceObj.Methods.Length == 0)
                {
                    emptyInterfaceList.Add(interfaceType);
                }
            }
            return emptyInterfaceList;
        }

        /// <summary>
        /// Helper function to form the error message.
        /// </summary>
        /// <param name="emptyInterfaceList">List of all violating types.</param>
        /// <returns>String with all the violating types.</returns>
        private string ErrorMessage(List<Type> emptyInterfaceList)
        {
            StringBuilder errorLog = new("The following interfaces are empty:\r\n");

            foreach (Type type in emptyInterfaceList)
            {
                errorLog.AppendLine(type.FullName);
            }
            return errorLog.ToString();
        }


        /// <summary>
        /// Analyzes each DLL file for interfaces with no contract
        /// And reports if the DLL violates the above.
        /// </summary>
        /// <param name="parsedDLLFile">Parsed DLL file.</param>
        /// <returns><see cref="AnalyzerResult"/> containing the analysis results.</returns>
        /// <exception cref="NullReferenceException">If the file object is null.</exception>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            List<Type> emptyInterfaces;
            try
            {
                emptyInterfaces = FindEmptyInterfaces(parsedDLLFile);
                if (emptyInterfaces.Count > 0)
                {
                    _verdict = 0;
                    _errorMessage = ErrorMessage(emptyInterfaces);
                }
                else
                {
                    _errorMessage = "No violation found.\r\n";
                }
            }
            catch (Exception ex)
            {
                Logger.Error( $"[Analyzer][NoEmptyInterface.cs] AnalyzeSingleDLL: Exception while analyzing {parsedDLLFile.DLLFileName} " + ex.Message );
                throw;
            }

            Logger.Debug( $"[Analyzer][NoEmptyInterface.cs] AnalyzeSingleDLL: Successfully finished analyzing {parsedDLLFile.DLLFileName} " );
            return new AnalyzerResult(analyzerID, _verdict, _errorMessage);
        }
    }
}

