using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

namespace Analyzer
{
    public class ParsedClass
    {
        private Type _typeObj;
        private readonly string? _name;
        private readonly ConstructorInfo[]? _constructors;
        private readonly Type[]? _interfaces;
        private readonly MethodInfo[]? _methods;
        private readonly FieldInfo[]? _fields;
        private readonly PropertyInfo[]? _properties;
        private readonly Type? _parentClass;
        private readonly Type[]? _compositionList;
        private readonly Type[]? _aggregationList;
        private readonly List<Type>? _usingList;

        public ParsedClass(Type type)
        {
            _typeObj = type;
            _name = type.FullName;

            _constructors = type.GetConstructors(BindingFlags.DeclaredOnly);
            _interfaces = type.GetInterfaces();
            //_methods = type.GetMethods(BindingFlags.DeclaredOnly);
            _methods = type.GetMethods().Where(m => m.DeclaringType != typeof(object) &&
                m.Name != "GetType" &&
                m.Name != "Equals" &&
                m.Name != "ToString" &&
                m.Name != "GetHashCode").ToArray();

            _fields = type.GetFields(BindingFlags.DeclaredOnly);
            _properties = type.GetProperties(BindingFlags.DeclaredOnly);
            _parentClass = type.BaseType;

            // UsingList
            Dictionary<MethodInfo, ParameterInfo[]> dict = GetFunctionParameters();
            foreach (KeyValuePair<MethodInfo, ParameterInfo[]> pair in dict)
            {
                foreach (ParameterInfo argument in pair.Value)
                {
                    if (argument.ParameterType.ToString() != "System.Object" && argument.ParameterType.IsClass)
                    {
                        Type relatedClass = argument.ParameterType;
                        //adding to using list
                        _usingList.Add(relatedClass);
                    }
                }
            }

            // Aggregation List
            // check local variables

            // Composition List
        }

        //public Type TypeObj
        //{ 
        //   get { return _typeObj; } 
        //}

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