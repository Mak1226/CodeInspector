using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

namespace Analyzer.Parsing
{
    public class ParsedClass
    {
        private readonly Type _typeObj; 
        private readonly string? _name;
        private readonly ConstructorInfo[]? _constructors;
        private readonly Type[]? _interfaces;
        private readonly MethodInfo[]? _methods;
        private readonly FieldInfo[]? _fields;
        private readonly PropertyInfo[]? _properties;
        private readonly Type? _parentClass;
        private readonly List<Type> _compositionList; 
        private readonly List<Type> _aggregationList; 
        private readonly List<Type> _usingList;

        // If needed for local variables
        private readonly List<MethodBase> _methodsBaseList;

        public ParsedClass(Type type)
        {
            _typeObj = type;
            _name = type.FullName;
            _constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            _methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            _fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            _properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            _parentClass = type.BaseType;

            if (_parentClass != null && _parentClass.GetInterfaces() != null)
            {
                _interfaces = type.GetInterfaces().Except(_parentClass.GetInterfaces()).ToArray();
            }

            foreach (MethodInfo methodinfo in _methods)
            {
                MethodBase methodBase = _typeObj.GetMethod(methodinfo.Name);

                if (methodBase != null)
                {
                    _methodsBaseList.Add(methodBase);
                }
            }

            //_methods = type.getmethods().where(m => m.declaringtype != typeof(object) &&
            //    m.name != "gettype" &&
            //    m.name != "equals" &&
            //    m.name != "tostring" &&
            //    m.name != "gethashcode").toarray();


            // Composition Class Relationship
            // Cases considering: 1. 

            // Getting local variables from MethodBase
            // _methods store MethodInfo objects but not its super class MethodBase

            // similar syntax for local variable
            //List<LocalVariableInfo> lvInfo = methodBase.GetMethodBody().LocalVariables.ToList();




            // Using Class Relationship 
            // Cases considering: 1. if some method contains other class as parameter
            // TODO : Check for other cases of Using if exists
            _usingList = new List<Type>();

            Dictionary<MethodInfo, ParameterInfo[]> dict = GetFunctionParameters();
            foreach (KeyValuePair<MethodInfo, ParameterInfo[]> pair in dict)
            {
                foreach (ParameterInfo argument in pair.Value)
                {
                    Type relatedClass = argument.ParameterType;

                    if (relatedClass.IsClass && relatedClass != _typeObj)
                    {
                        //adding to using list
                        _usingList.Add(relatedClass);
                    }
                }
            }

            // Aggregation List
            // check local variables


        }

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