using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Parsing
{
    public class ParsedDLLFiles
    {
        public List<ParsedClass> classObjList = new();
        public List<ParsedInterface> interfaceObjList = new();




        // MONO.CECIL objects lists (considering single module assembly)
        public List<ParsedClassMonoCecil> classObjListMC = new();



        public Dictionary<Type, ParsedClass> mapTypeToParsedClass= new();      

        //public List<ParsedInterface> interfaceObjList = new();
        /// <summary>
        /// function to parse the dll files
        /// </summary>
        /// <param name="paths"></param>
        public ParsedDLLFiles(List<string> paths) // path of dll files
        {
            // take the input of all the dll files
            // it merge the all the ParsedNamespace
            foreach (var path in paths)
            {
                // REFLECTION PARSING
                Assembly assembly = Assembly.LoadFrom(path);

                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();

                    foreach (Type type in types)
                    {
                        if (type.Namespace != null)
                        {
                            if (type.Namespace.StartsWith("System.") || type.Namespace.StartsWith("Microsoft."))
                            {
                                continue;
                            }

                            if (type.IsClass && type.IsValueType)
                            {
                                ParsedClass classObj = new ParsedClass(type);
                                classObjList.Add(classObj);
                                mapTypeToParsedClass[type] = classObj;
                            }
                            else if (type.IsInterface)
                            {
                                ParsedInterface interfaceObj = new ParsedInterface(type);
                                interfaceObjList.Add(interfaceObj);
                            }
                            else
                            {
                                // there can be enums , structs , delegates , events etc.. other than classes and interfaces when used getTypes()
                                // TODO : Handle these other types
                            }
                        }
                        else
                        {
                            // code written outside all namespaces may have namespace as null
                            // TODO : Handle outside namespace types later
                        }
                    }
                }


                // MONO.CECIL PARSING
                AssemblyDefinition assemblyDef = AssemblyDefinition.ReadAssembly(path);

                if (assemblyDef != null)
                {
                    // considering only single module programs
                    ModuleDefinition mainModule = assemblyDef.MainModule;

                    if (mainModule != null)
                    {
                         foreach(TypeDefinition type in mainModule.Types)
                        {
                            if (type.Namespace != "")
                            {
                                if(type.Namespace.StartsWith("System") || type.Namespace.StartsWith("Microsoft"))
                                {
                                    continue;
                                }

                                if(type.IsClass && type.IsValueType)
                                {
                                    ParsedClassMonoCecil classObj = new ParsedClassMonoCecil(type);
                                    classObjListMC.Add(classObj);
                                }
                                else if (type.IsInterface)
                                {
                                    
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }

            }

        }

    }
}


// it will call the constructor of the ParsedNamespace for each dll file