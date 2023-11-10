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
        private string errorMessage;
        private int verdict;
        private readonly string analyzerID;

        /// <summary>
        ///
        /// </summary>
        /// <param name="dllFiles"></param>
        public NoEmptyInterface(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            errorMessage = "";
            verdict = 1;
            analyzerID = "Custom3";
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<Type> FindEmptyInterfaces()
        {
            List<Type> emptyInterfaceList = new List<Type>();

            foreach (ParsedInterface interfaceObj in parsedDLLFiles.interfaceObjList)
            {
                Type interfaceType = interfaceObj.TypeObj;

                //
                if (interfaceObj.Methods == null)
                {
                    emptyInterfaceList.Add(interfaceType);
                }
            }

            return emptyInterfaceList;
        }


        private string ErrorMessage(List<Type> emptyInterfaceList)
        {
            var errorLog = new System.Text.StringBuilder("The following Interfaces are empty:");

            foreach (Type type in emptyInterfaceList)
            {
                try
                {
                    // sanity check
                    errorLog.AppendLine(type.FullName.ToString());
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
        public override AnalyzerResult Run()
        {
            List<Type> emptyInterfaces = FindEmptyInterfaces();
            if (emptyInterfaces.Count > 0)
            {
                verdict = 1;
                errorMessage = ErrorMessage(emptyInterfaces);
            }
            return new AnalyzerResult(analyzerID, verdict, errorMessage);
        }
    }
}

