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
        private readonly Type _typeObj;     // type object to access class related information
        private readonly string _name;     // Name of Class. (Doesn't include namespace name in it)
        private readonly ConstructorInfo[]? _constructors;      // Includes Default Constructors also if created

        /// <summary>
        /// Contains interfaces implemented by the class only the ones specifically mentioned 
        /// Does not include interfaces implemented by the parent class/ implemented interface
        /// This is useful for creation of class relational diagram
        /// </summary>
        private readonly Type[]? _interfaces;       

        private readonly MethodInfo[]? _methods;     // Methods declared only by the class
        private readonly FieldInfo[]? _fields;      // Fields declared only by the class
        private readonly PropertyInfo[]? _properties;   // Properties declared only by the class
        private readonly Type? _parentClass;        // ParentClass - does not contain classes starting with System/Microsoft

        // Storing information related to methods and can be used to get local variables rather methodinfo
        private readonly List<MethodBase> _methodsBaseList;

        /// <summary>
        /// Parses the most used information from the class object
        /// </summary>
        /// <param name="type">class object when parsed using System.reflection</param>
        public ParsedClass(Type type)
        {
            _typeObj = type;
            _name = type.Name;
            _constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            _methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            _fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            _properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            _methodsBaseList = new List<MethodBase>();

            // Finding parent class declared in the project - does not contain classes starting with System/Microsoft
            if (type.BaseType.Namespace != null) 
            {
                if (!(type.BaseType.Namespace.StartsWith("System") || type.BaseType.Namespace.StartsWith("Microsoft")))
                {
                    _parentClass = _typeObj.BaseType;
                }
            }
            else
            {
                _parentClass = _typeObj.BaseType;
            }

            // Finding interfaces which are only implemented by the class and declares specifically in the class
            if (_parentClass != null && _parentClass.GetInterfaces() != null)
            {
                _interfaces = type.GetInterfaces().Except(_parentClass.GetInterfaces()).ToArray();
            }

            if(_interfaces?.Length > 0)
            {
                HashSet<string> removableInterfaceNames = new();

                foreach (var i in _interfaces)
                {
                    foreach (var x in i.GetInterfaces())
                    {
                        removableInterfaceNames.Add(x.FullName);
                    }
                }

                List<Type> ifaceList = new();

                foreach (var iface in _interfaces)
                {
                    if (!removableInterfaceNames.Contains(iface.FullName))
                    {
                        ifaceList.Add(iface);
                    }
                }

                _interfaces = ifaceList.ToArray();
            }


            // Finding method bases for methods of the class found earlier
            foreach (MethodInfo methodinfo in _methods)
            {
                MethodBase methodBase = _typeObj.GetMethod(methodinfo.Name);

                if (methodBase != null)
                {
                    _methodsBaseList.Add(methodBase);
                }
            }
        }


        /// <summary>
        /// Getters of private fields of this class
        /// Some of them can return null.(Refer: Nullable private fields of this class)
        /// </summary>
        public Type TypeObj
        {
            get { return _typeObj; }
        }

        public string Name
        {
            get { return _name; }
        }

        public ConstructorInfo[] Constructors
        {
            get { return _constructors; }
        }

        public Type[] Interfaces
        {
            get { return _interfaces; }
        }

        public MethodInfo[] Methods
        {
            get { return _methods; }

        }

        public FieldInfo[] Fields
        {
            get { return _fields; }
        }

        public PropertyInfo[] Properties
        {
            get { return _properties; }
        }

        public Type ParentClass
        {
            get { return _parentClass; }
        }


        /// <summary>
        /// Provides information related to all methods of class
        /// </summary>
        /// <returns></returns>
        public Dictionary<MethodInfo, ParameterInfo[]> GetFunctionParameters()
        {
            Dictionary<MethodInfo, ParameterInfo[]> dict = new();

            if (_methods != null)
            {
                foreach (MethodInfo method in _methods)
                {
                    dict.Add(method, method.GetParameters());
                }
            }

            return dict;
        }
    }
}