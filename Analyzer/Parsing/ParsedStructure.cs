//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Reflection;
//using System.Collections;

//namespace Analyzer.Parsing
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class ParsedStructure
//    {
//        private readonly Type _typeObj;
//        private readonly string? _name;
//        private readonly ConstructorInfo[]? _constructors;
//        private readonly MethodInfo[]? _methods;
//        private readonly FieldInfo[]? _fields;
//        private readonly PropertyInfo[]? _properties;

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="type"></param>
//        public ParsedStructure(Type type)
//        {
//            _typeObj = type;
//            _name = type.FullName;
//            _constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
//            _methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
//            _fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
//            _properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

//            Dictionary<MethodInfo, ParameterInfo[]> dict = GetFunctionParameters();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public Type TypeObj
//        {
//            get { return _typeObj; }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public string Name
//        {
//            get { return _name; }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public ConstructorInfo[] Constructors
//        {
//            get { return _constructors; }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public MethodInfo[] Methods
//        {
//            get { return _methods; }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public FieldInfo[] Fields
//        {
//            get { return _fields; }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public PropertyInfo[] Properties
//        {
//            get { return _properties; }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public Dictionary<MethodInfo, ParameterInfo[]> GetFunctionParameters()
//        {
//            Dictionary<MethodInfo, ParameterInfo[]> dict = new();

//            if (_methods != null)
//            {
//                foreach (MethodInfo method in _methods)
//                {
//                    dict.Add(method, method.GetParameters());
//                }
//            }

//            return dict;
//        }
//    }
//}
