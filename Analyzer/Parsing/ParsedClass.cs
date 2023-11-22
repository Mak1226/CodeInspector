using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

namespace Analyzer.Parsing
{
    /// <summary>
    /// Parses the most used information from the class object using System.Reflection
    /// </summary>
    public class ParsedClass
    {
        public Type TypeObj { get; }   // type object to access class related information
        public string Name { get; }    // Name of Class. (Doesn't include namespace name in it)
        public ConstructorInfo[]? Constructors { get;  }      // Includes Default Constructors also if created

        /// <summary>
        /// Contains interfaces implemented by the class only the ones specifically mentioned 
        /// Does not include interfaces implemented by the parent class/ implemented interface
        /// This is useful for creation of class relational diagram
        /// </summary>
        public Type[]? Interfaces { get; }
        public MethodInfo[]? Methods { get; }     // Methods declared only by the class
        public FieldInfo[]? Fields { get; }      // Fields declared only by the class
        public PropertyInfo[]? Properties { get; }   // Properties declared only by the class
        public Type? ParentClass { get; }        // ParentClass - does not contain classes starting with System/Microsoft

        // Storing information related to methods and can be used to get local variables rather methodinfo
        public List<MethodBase> MethodBaseList { get; }

        /// <summary>
        /// Parses the most used information from the class object
        /// </summary>
        /// <param name="type">class object when parsed using System.reflection</param>
        public ParsedClass(Type type)
        {
            TypeObj = type;
            Name = type.Name;

            Constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            Methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            Fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            Properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            MethodBaseList = new List<MethodBase>();

            // Finding parent class declared in the project - does not contain classes starting with System/Microsoft
            if (type.BaseType.Namespace != null) 
            {
                if (!(type.BaseType.Namespace.StartsWith("System") || type.BaseType.Namespace.StartsWith("Microsoft")))
                {
                    ParentClass = TypeObj.BaseType;
                }
            }
            else
            {
                ParentClass = TypeObj.BaseType;
            }

            // Finding interfaces which are only implemented by the class and declares specifically in the class
            if (ParentClass != null && ParentClass.GetInterfaces() != null)
            {
                Interfaces = type.GetInterfaces().Except(ParentClass.GetInterfaces()).ToArray();
            }
            else
            {
                Interfaces = type.GetInterfaces();
            }

            if(Interfaces?.Length > 0)
            {
                HashSet<string> removableInterfaceNames = new();

                foreach (Type i in Interfaces)
                {
                    foreach (Type x in i.GetInterfaces())
                    {
                        removableInterfaceNames.Add(x.FullName);
                    }
                }

                List<Type> ifaceList = new();

                foreach (Type iface in Interfaces)
                {
                    if (!removableInterfaceNames.Contains(iface.FullName))
                    {
                        ifaceList.Add(iface);
                    }
                }

                Interfaces = ifaceList.ToArray();
            }


            // Finding method bases for methods of the class found earlier
            foreach (MethodInfo methodinfo in Methods)
            {
                MethodBase methodBase = TypeObj.GetMethod(methodinfo.Name);

                if (methodBase != null)
                {
                    MethodBaseList.Add(methodBase);
                }
            }
        }

        /// <summary>
        /// Provides information related to all methods of class
        /// </summary>
        /// <returns></returns>
        public Dictionary<MethodInfo, ParameterInfo[]> GetFunctionParameters()
        {
            Dictionary<MethodInfo, ParameterInfo[]> dict = new();

            if (Methods != null)
            {
                foreach (MethodInfo method in Methods)
                {
                    dict.Add(method, method.GetParameters());
                }
            }

            return dict;
        }
    }
}
