using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Analyzer.Parsing
{
    /// <summary>
    /// Parses most used information from Class Object using Mono.Cecil
    /// </summary>
    public class ParsedClassMonoCecil
    {
        private readonly TypeDefinition _typeObj;   // type object to access class related information
        private readonly string _name;      // Name of Class. (Doesn't include namespace name in it)
        private readonly List<MethodDefinition> _constructors;      // Includes Default Constructors also if created

        /// <summary>
        /// Contains interfaces implemented by the class only the ones specifically mentioned 
        /// Does not include interfaces implemented by the parent class/ implemented interface
        /// This is useful for creation of class relational diagram
        /// </summary>
        private readonly List<InterfaceImplementation> _interfaces;

        public readonly List<MethodDefinition> _methods;   // Methods declared only by the class
        private readonly TypeDefinition? _parentClass;      // ParentClass - does not contain classes starting with System/Microsoft
        public readonly List<FieldDefinition> _fields;     
        private readonly List<PropertyDefinition> _properties;


        // To store class relationship
        private readonly HashSet<string> _compositionList;
        private readonly HashSet<string> _aggregationList;
        private readonly HashSet<string> _usingList;
        private readonly HashSet<string> _inheritanceList;

        public ParsedClassMonoCecil(TypeDefinition type)
        {
            _typeObj = type;
            _name = type.Name;

            _constructors = new List<MethodDefinition>();
            _methods = new List<MethodDefinition>();
            _interfaces = new List<InterfaceImplementation>();
            _fields = new List<FieldDefinition>();
            _properties = new List<PropertyDefinition>();

            // type.Methods will include constructors of the class & will not give methods of parent class
            foreach (MethodDefinition method in type.Methods)
            {
                if (method.IsConstructor)
                {
                    _constructors.Add(method);
                }
                else
                {
                    _methods.Add(method);
                }
            }


            // Finding parent class declared in the project - does not contain classes starting with System/Microsoft
            if (type.BaseType.Namespace != null)
            {
                if (!(type.BaseType.Namespace.StartsWith("System") || type.BaseType.Namespace.StartsWith("Microsoft")))
                {
                    _parentClass = _typeObj.BaseType.Resolve();
                }
            }
            else
            {
                _parentClass = _typeObj.BaseType.Resolve();
            }


            // Finding interfaces which are only implemented by the class and declares specifically in the class
            if (type.HasInterfaces)
            {
                _interfaces = type.Interfaces.ToList();

                if (_parentClass?.Interfaces != null)
                {
                    _interfaces = type.Interfaces.Except(_parentClass.Interfaces).ToList();
                }


                HashSet<string> removableInterfaceNames = new();

                foreach (var i in _interfaces)
                {
                    foreach (var x in i.InterfaceType.Resolve().Interfaces)
                    {
                        removableInterfaceNames.Add(x.InterfaceType.FullName);
                    }
                }

                List<InterfaceImplementation> ifaceList = new();

                foreach (var iface in _interfaces)
                {
                    if (!removableInterfaceNames.Contains(iface.InterfaceType.FullName))
                    {
                        ifaceList.Add(iface);
                    }
                }

                _interfaces = ifaceList;
            }

            _fields = _typeObj.Fields.ToList();
            _properties = _typeObj.Properties.ToList();


            _usingList = new HashSet<string>();
            _compositionList = new HashSet<string>();
            _aggregationList = new HashSet<string>();


            //Inheritance List
            //Adding the parent class (if exist) in the inheritance list
            _inheritanceList = new HashSet<string>();
            if (_parentClass != null)
            {
                if (!_parentClass.FullName.StartsWith("System"))
                {
                    _inheritanceList.Add("C"+_parentClass.FullName);
                }
            }
            
            //adding all interfaces from which the class inherits, in the inheritance list
            foreach (var iface in _interfaces)
            {
                if (!iface.InterfaceType.FullName.StartsWith("System"))
                {
                    _inheritanceList.Add("I"+iface.InterfaceType.FullName);
                }
            }
            
            //Composition List
            //Cases: 1. If any parameter of constructor is assigned to a field of the class, then it is composition relationship.
            //2. If any new object is instantiated inside a constructor, and is assigned to any class field, then there exist composition relationship.
            foreach (MethodDefinition ctor in _constructors)
            {
                List<ParameterDefinition> parameterList = ctor.Parameters.ToList();
                if (ctor.HasBody)
                {
                    //iterating over all instructions, to check if any class field (of reference type) is assigned a value (be it from parameter or by instantiating a new object)
                    for (int i = 0; i < ctor.Body.Instructions.Count; i++)
                    {
                        var inst = ctor.Body.Instructions[i];
                        if (inst != null && inst.OpCode == OpCodes.Stfld)
                        {
                            var fieldReference = (FieldReference)inst.Operand;
                            var fieldType = fieldReference.FieldType;
                            var objType = fieldType.Resolve();

                            // Check if the field type is of reference type (not a value type), not a Generic type, and does not start with "System"
                            if (!fieldType.IsValueType && !objType.IsGenericInstance && !objType.FullName.StartsWith("System"))
                            {
                                if (objType.IsClass && !SetsContainElement("C"+ objType.FullName, _inheritanceList))
                                {
                                    _compositionList.Add("C" + objType.FullName);
                                }
                                else if(objType.IsInterface && !SetsContainElement("I" + objType.FullName, _inheritanceList))
                                {
                                    _compositionList.Add("I" + objType.FullName);
                                }
                            }
                        }
                    }
                }


                // Handling Case 2 of using relationship, where the parameter of constructor is assigned to a local variable.
                // If between 2 classes composition and using relation exist, giving priority to composition relation.
                foreach (ParameterDefinition parameter in parameterList)
                {
                    var parameterType = parameter.ParameterType.Resolve();
                    string parameterTypeName = parameter.ParameterType.FullName;

                    if (!parameterTypeName.StartsWith("System") && !parameterType.GetType().IsGenericType)
                    {
                        Console.WriteLine(parameterType);

                        Console.WriteLine(parameterType.IsClass);
                        if (parameterType.IsClass && !SetsContainElement("C" + parameter.ParameterType.FullName, _inheritanceList, _compositionList))
                        {
                            Console.WriteLine("2: C"+ parameter.ParameterType.FullName);
                            _usingList.Add("C"+parameter.ParameterType.FullName);
                        }
                        else if (parameterType.IsInterface && !SetsContainElement("I" + parameter.ParameterType.FullName, _inheritanceList, _compositionList))
                        {
                            Console.WriteLine("2: I" + parameter.ParameterType.FullName);

                            _usingList.Add("I" + parameter.ParameterType.FullName);
                        }
                    }
                }

                //Handling Case 2 of aggregation relationship, where new object is instantiated inside a constructor and is assigned to its local variable.
                //If between 2 classes composition and aggregation relation exists, giving priority to composition relation.
                foreach (MethodDefinition ctr in _constructors)
                {
                    if (ctr.HasBody)
                    {
                        foreach (var inst in ctr.Body.Instructions)
                        {
                            if (inst != null && inst.OpCode == OpCodes.Newobj)
                            {
                                var constructorReference = (MethodReference)inst.Operand;
                                var objectType = constructorReference.DeclaringType.Resolve();

                                // adding to aggregation list, if object is not of generic type and is not in composition list (i.e either the object is assigned to a local variable
                                // or if not, since we have decided on the priority of composition over aggreagation, we can check if the composition list has that particular class object or not).
                                if (!objectType.IsGenericInstance && !objectType.FullName.StartsWith("System"))
                                {
                                    if (objectType.IsClass && !SetsContainElement("C" + objectType.FullName, _inheritanceList, _compositionList))
                                    {
                                        _aggregationList.Add("C" + objectType.FullName);
                                        _usingList.Remove("C" + objectType.FullName);

                                    }
                                    else if(objectType.IsInterface && !SetsContainElement("I" + objectType.FullName, _inheritanceList, _compositionList))
                                    {
                                        _aggregationList.Add("I" + objectType.FullName);
                                        _usingList.Remove("I" + objectType.FullName);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Aggregation List
            // Cases: 1. If a new class object is created and/or instantiated inside any method (other than constructor), its aggregation.
            // 2. If a new class object is instantiated inside a constructor, but is not assigned to any class field, its aggregation. 
            // check if new opcode is present in method body and get its type
            foreach (MethodDefinition method in _methods)
            {
                if (method.HasBody)
                {
                    foreach (var inst in method.Body.Instructions)
                    {
                        if (inst != null && inst.OpCode == OpCodes.Newobj)
                        {
                            var constructorReference = (MethodReference)inst.Operand;
                            var objectType = constructorReference.DeclaringType;

                            // adding to aggrgation list, if object is not of generic type
                            if (!objectType.IsGenericInstance && !objectType.FullName.StartsWith("System") && !SetsContainElement("C" + objectType.FullName, _inheritanceList, _compositionList))
                            {
                                _aggregationList.Add("C" + objectType.FullName);
                            }
                        }
                    }
                }
            }

            // Using Class Relationship 
            // Cases considering: 1. if any method (other than constructors) contain other class as parameter
            // 2.If its a parameter for a constructor, and is not assigned to any field inside the constructor, then it is considered as using relationship. 

            //Get all the methods and its parameters into a dictionary, which can be iterated over to check its types.
            Dictionary<MethodDefinition, List<ParameterDefinition>> dict = GetFunctionParameters();
            foreach (KeyValuePair<MethodDefinition, List<ParameterDefinition>> pair in dict)
            {
                foreach (ParameterDefinition argument in pair.Value)
                {
                    var objType = argument.ParameterType.Resolve();

                    //adding to using list, if the parameter is of class type and is not of generic class (list, dict,etc.)
                    if (objType != _typeObj && objType!= null && !(objType.GetType().IsGenericType))
                    {
                        if (pair.Key.IsConstructor)
                        {
                            continue;
                        }
                        else
                        {
                            //ignoring the classes those start with "System"
                            if (!argument.ParameterType.FullName.StartsWith("System"))
                            {
                                if (objType.IsClass && !SetsContainElement("C" + argument.ParameterType.FullName, _inheritanceList, _compositionList, _aggregationList))
                                {
                                    Console.WriteLine("1: " + argument.ParameterType.FullName);

                                    _usingList.Add("C" + argument.ParameterType.FullName);
                                }
                                else if (objType.IsInterface && !SetsContainElement("I" + argument.ParameterType.FullName, _inheritanceList, _compositionList, _aggregationList))
                                {
                                    Console.WriteLine("1: " + argument.ParameterType.FullName);
                                    _usingList.Add("I" + argument.ParameterType.FullName);
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool SetsContainElement<T>(T element, params HashSet<T>[] sets)
        {
            return sets.Any(set => set.Contains(element));
        }

        public Dictionary<MethodDefinition, List<ParameterDefinition>> GetFunctionParameters()
        {
            Dictionary<MethodDefinition, List<ParameterDefinition>> dict = new();

            if (_methods != null)
            {
                foreach (MethodDefinition method in _methods)
                {
                    dict.Add(method, method.Parameters.ToList());
                }
            }

            return dict;
        }

        public List<string> CompositionList
        {
            get { return new List<string>(_compositionList); }
        }

        public List<string> AggregationList
        {
            get { return new List<string>(_aggregationList); }
        }

        public List<string> InheritanceList
        {
            get { return new List<string>(_inheritanceList); }
        }

        public List<string> UsingList
        {
            get { return new List<string>(_usingList); }
        }

        public List<FieldDefinition> FieldsList
        {
            get { return _fields; }
        }

        public List<MethodDefinition> MethodsList
        {
            get { return _methods; }
        }

        public List<PropertyDefinition> PropertiesList
        {
            get {  return _properties;}
        }
        public TypeDefinition TypeObj
        {
            get { return _typeObj; }
        }

        public string Name
        {
            get { return _name; }
        }

        public List<MethodDefinition> Constructors
        {
            get { return _constructors; }
        }

        public List<InterfaceImplementation> Interfaces
        {
            get { return _interfaces; }
        }

        public List<MethodDefinition> Methods
        {
            get { return _methods; }

        }

        public List<FieldDefinition> Fields
        {
            get { return _fields; }
        }

        public TypeDefinition ParentClass
        {
            get { return _parentClass; }
        }
    }
}