//using Analyzer.Parsing;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Reflection;

//namespace Analyzer.Pipeline
//{
//    /// <summary>
//    ///
//    /// </summary>
//    internal class NoEmptyInterface : AnalyzerBase
//    {
//        private string errorMessage;
//        private int verdict;
//        private readonly string analyzerID;

//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="dllFiles"></param>
//        public NoEmptyInterface(ParsedDLLFiles dllFiles) : base(dllFiles)
//        {
//            errorMessage = "";
//            verdict = 1;
//            analyzerID = "Custom3";
//        }

//        /// <summary>
//        ///
//        /// </summary>
//        /// <returns></returns>
//        public List<Type> FindEmptyInterfaces()
//        {
//            List<Type> emptyInterfaceList = new List<Type>();

//            foreach (ParsedInterface interfaceObj in parsedDLLFiles.interfaceObjList)
//            {
//                Type interfaceType = interfaceObj.TypeObj;

//                //
//                if (interfaceObj.Methods == null)
//                {
//                    emptyInterfaceList.Add(interfaceType);
//                }
//            }

//            return emptyInterfaceList;
//        }


//        private string ErrorMessage(List<Type> abstractTypes)
//        {
//            foreach (Type type in abstractTypes)
//            {
//                // sanity check
//                if (type.GetTypeInfo().IsAbstract)
//                {

//                }
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public override AnalyzerResult Run()
//        {
//            List<Type> abstractTypes = FindAbstractTypeWithPublicConstructor();
//            if (abstractTypes.Count > 0)
//            {
//                verdict = 1;
//                errorMessage = ErrorMessage(abstractTypes);
//            }
//            return new AnalyzerResult(analyzerID, verdict, errorMessage);
//        }
//    }
//}

