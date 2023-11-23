using Mono.Cecil;
using System.IO;
using System.Reflection;

namespace Analyzer.Parsing
{
    public class ParsedDLLFile
    {
        private string _dllPath { get; }
        public string DLLFileName { get; }

        public List<ParsedClass> classObjList = new();
        public List<ParsedInterface> interfaceObjList = new();

        // MONO.CECIL objects lists (considering single module assembly)
        public List<ParsedClassMonoCecil> classObjListMC = new();

        /// <summary>
        /// function to parse the dll files
        /// </summary>
        /// <param name="path"></param>
        public ParsedDLLFile(string path) // path of dll files
        {
            _dllPath = path;
            DLLFileName = Path.GetFileName(path);
            
            ReflectionParsingDLL();
            MonoCecilParsingDLL();
        }

        private void ReflectionParsingDLL()
        {
            Assembly assembly = Assembly.Load( File.ReadAllBytes(_dllPath) );

            if (assembly != null)
            {
                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    if (type.Namespace != null)
                    {
                        if (type.Namespace.StartsWith( "System." ) || type.Namespace.StartsWith( "Microsoft." ))
                        {
                            continue;
                        }
                    }
                    
                    if (type.IsClass)
                    {
                        // To avoid structures and delegates
                        if (!type.IsValueType && !typeof(Delegate).IsAssignableFrom(type))
                        {
                            ParsedClass classObj = new(type);
                            classObjList.Add( classObj );
                        }
                    }
                    else if (type.IsInterface)
                    {
                        ParsedInterface interfaceObj = new(type);
                        interfaceObjList.Add( interfaceObj );
                    }
                }
            }
        }

        private void MonoCecilParsingDLL()
        {
            AssemblyDefinition assemblyDef = AssemblyDefinition.ReadAssembly(_dllPath);

            if (assemblyDef != null)
            {
                // considering only single module programs
                ModuleDefinition mainModule = assemblyDef.MainModule;

                if (mainModule != null)
                {
                    foreach (TypeDefinition type in mainModule.Types)
                    {
                        if (type.Namespace != null)
                        {
                            if (type.Namespace.StartsWith( "System" ) || type.Namespace.StartsWith( "Microsoft" ))
                            {
                                continue;
                            }
                        }

                        if (type.IsClass && !type.IsValueType && type.BaseType?.FullName != "System.MulticastDelegate")
                        {
                            ParsedClassMonoCecil classObj = new( type );
                            classObjListMC.Add( classObj );
                        }
                    }
                }
                assemblyDef.Dispose();
            }
        }
    }
}
