using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly List<string> _compositionList;
        private readonly List<string> _aggregationList;
        private readonly List<string> _usingList;
        private readonly List<string> _inheritanceList;

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


            // Using Class Relationship 
            // Cases considering: 1. if any method (other than constructors) contain other class as parameter
            // 2.If its a parameter for a constructor, and is not assigned to any field inside the constructor, then it is considered as using relationship. 
            _usingList = new List<string>();
            _compositionList = new List<string>();
            _aggregationList = new List<string>();

            //Get all the methods and its parameters into a dictionary, which can be iterated over to check its types.
            Dictionary<MethodDefinition, List<ParameterDefinition>> dict = GetFunctionParameters();
            foreach (KeyValuePair<MethodDefinition, List<ParameterDefinition>> pair in dict)
            {
                foreach (ParameterDefinition argument in pair.Value)
                {
                    Type relatedClass = argument.GetType();

                    //adding to using list, if the parameter is of class type and is not of generic class (list, dict,etc.)
                    if (relatedClass.IsClass && relatedClass != _typeObj.GetType() && !relatedClass.IsGenericType)
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
                                _usingList.Add(argument.ParameterType.FullName);
                            }
                        }
                    }
                }
            }

            //Inheritance List
            //Adding the parent class (if exist) in the inheritance list
            _inheritanceList = new List<string>();
            if (_parentClass != null)
            {
                if (!_parentClass.FullName.StartsWith("System"))
                {
                    _inheritanceList.Add(_parentClass.FullName);
                }
            }
            
            //adding all interfaces from which the class inherits, in the inheritance list
            foreach (var iface in _interfaces)
            {
                if (!iface.InterfaceType.FullName.StartsWith("System"))
                {
                    _inheritanceList.Add(iface.InterfaceType.FullName);
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
                            if (!objectType.IsGenericInstance && !objectType.FullName.StartsWith("System"))
                            {
                                _aggregationList.Add(objectType.FullName);
                            }
                        }
                    }
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
                            var classType = fieldType.Resolve();

                            // Check if the field type is of reference type (not a value type), not a Generic type, and does not start with "System"
                            if (!fieldType.IsValueType && classType.IsClass && !classType.IsGenericInstance && !classType.FullName.StartsWith("System"))
                            {
                                _compositionList.Add(classType.FullName);
                            }
                        }
                    }
                }


                // Handling Case 2 of using relationship, where the parameter of constructor is assigned to a local variable.
                // If between 2 classes composition and using relation exist, giving priority to composition relation.
                foreach (ParameterDefinition parameter in parameterList)
                {
                    Type parameterType = parameter.Resolve().GetType();
                    if (parameterType.IsClass && !parameterType.IsGenericType && !_compositionList.Contains(parameterType.Name))
                    {
                        string parameterTypeName = parameter.ParameterType.FullName;
                        if (!parameterTypeName.StartsWith("System"))
                        {
                            _usingList.Add(parameter.ParameterType.FullName);
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
                                var objectType = constructorReference.DeclaringType;
                                // adding to aggregation list, if object is not of generic type and is not in composition list (i.e either the object is assigned to a local variable
                                // or if not, since we have decided on the priority of composition over aggreagation, we can check if the composition list has that particular class object or not).
                                if (!objectType.IsGenericInstance && !objectType.FullName.StartsWith("System") && !_compositionList.Contains(objectType.FullName))
                                {
                                    _aggregationList.Add(objectType.FullName);
                                }
                            }
                        }
                    }
                }
            }
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

        public List<string> CompositionList
        {
            get { return _compositionList; }
        }

        public List<string> AggregationList
        {
            get { return _aggregationList; }
        }

        public List<string> InheritanceList
        {
            get { return _inheritanceList; }
        }

        public List<string> UsingList
        {
            get { return _usingList; }
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
    }
}