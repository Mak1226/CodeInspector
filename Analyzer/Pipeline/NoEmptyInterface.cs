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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

namespace Analyzer.Pipeline
{
    /// <summary>
    ///
    /// </summary>
    public class NoEmptyInterface : AnalyzerBase
    {
        private string _errorMessage;
        private int _verdict;
        private readonly string _analyzerID;

        /// <summary>
        ///
        /// </summary>
        /// <param name="dllFiles"></param>
        public NoEmptyInterface(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            _errorMessage = "";
            _verdict = 1;
            _analyzerID = "104";
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<Type> FindEmptyInterfaces(ParsedDLLFile parsedDLLFile)
        {
            List<Type> emptyInterfaceList = new();

            foreach (ParsedInterface interfaceObj in parsedDLLFile.interfaceObjList)
            {
                Type interfaceType = interfaceObj.TypeObj;

                //
                if (interfaceObj.Methods.Length == 0)
                {
                    emptyInterfaceList.Add(interfaceType);
                }
            }

            return emptyInterfaceList;
        }

        private string ErrorMessage(List<Type> emptyInterfaceList)
        {
            var errorLog = new System.Text.StringBuilder("The following Interfaces are empty:\r\n");

            foreach (Type type in emptyInterfaceList)
            {
                // sanity check
                errorLog.AppendLine(type.FullName);
            }
            return errorLog.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            List<Type> emptyInterfaces;
            try
            {
                emptyInterfaces = FindEmptyInterfaces( parsedDLLFile );
                if (emptyInterfaces.Count > 0)
                {
                    _verdict = 0;
                    _errorMessage = ErrorMessage( emptyInterfaces );
                }
                else
                {
                    _errorMessage = "No violation found.";
                }
            }
            catch (NullReferenceException ex)
            {
                _verdict = 0;
                _errorMessage = "";
                Trace.WriteLine("NullReferenceException in DLL files" + ex.Message);
            }

            return new AnalyzerResult(_analyzerID, _verdict, _errorMessage);
        }
    }
}

