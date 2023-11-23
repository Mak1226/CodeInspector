using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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
                Console.WriteLine(interfaceObj.Name);
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
                try
                {
                    // sanity check
                    errorLog.AppendLine(type.FullName);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    throw new ArgumentOutOfRangeException("Invalid Argument ", ex);
                }
            }
            return errorLog.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override AnalyzerResult AnalyzeSingleDLL(ParsedDLLFile parsedDLLFile)
        {
            List<Type> emptyInterfaces = FindEmptyInterfaces(parsedDLLFile);
            if (emptyInterfaces.Count > 0)
            {
                _verdict = 0;
                _errorMessage = ErrorMessage(emptyInterfaces);
            }
            return new AnalyzerResult(_analyzerID, _verdict, _errorMessage);
        }
    }
}

