/******************************************************************************
* Filename    = ParsedClass.cs
* 
* Author      = Nikhitha Atyam, Yukta Salunkhe
* 
* Product     = Analyzer
* 
* Project     = Analyzer
*
* Description = Parses the most used information from the class object using Mono.Cecil package
*****************************************************************************/

using System.Diagnostics;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;


namespace Analyzer.Parsing
{
    /// <summary>
    /// Parses most used information from the class object using Mono.Cecil
    /// </summary>
    public class ParsedClassMonoCecil
    {
        public TypeDefinition TypeObj { get; }   // type object to access class related information
        public string Name { get; }      // Name of Class. (Doesn't include namespace name in it)
        public List<MethodDefinition> Constructors { get; }     // Includes Default Constructors also 

        /// <summary>
        /// Contains interfaces implemented by the class only the ones at the lower level (direct implementation)
        /// i.e Does not include interfaces implemented by the parent class/ implemented interface
        /// This is useful for creation of class relational diagram
        /// </summary>
        public List<InterfaceImplementation> Interfaces { get; private set; }

        public List<MethodDefinition> MethodsList { get; }   // Methods declared only by the class
        public TypeDefinition? ParentClass;                  // ParentClass - does not contain classes starting with System/Microsoft
        public List<FieldDefinition> FieldsList;             // Fields declared only in the class
        public List<PropertyDefinition> PropertiesList;      // Properties declared only in the class


        // To store class relationships
        public HashSet<string> CompositionList { get; }
        public HashSet<string> AggregationList { get; }
        public HashSet<string> UsingList { get; }
        public HashSet<string> InheritanceList { get; }

        public ParsedClassMonoCecil(TypeDefinition type)
        {
            Trace.WriteLine("Creating ParsedMonoCecil Obj for " + type.FullName);
            TypeObj = type;
            Name = type.Name;

            Constructors = new List<MethodDefinition>();
            MethodsList = new List<MethodDefinition>();
            Interfaces = new List<InterfaceImplementation>();
            FieldsList = new List<FieldDefinition>();
            PropertiesList = new List<PropertyDefinition>();
            InheritanceList = new HashSet<string>();
            UsingList = new HashSet<string>();
            CompositionList = new HashSet<string>();
            AggregationList = new HashSet<string>();

            // type.Methods will include constructors of the class & will not give methods of parent class
            foreach (MethodDefinition method in type.Methods)
            {
                if (method.IsConstructor)
                {
                    Constructors.Add(method);
                }
                else
                {
                    MethodsList.Add(method);
                }
            }

            // Finding parent class declared in the project - does not contain classes starting with System/Microsoft
            if (type.BaseType?.Namespace != null || type.BaseType?.Namespace == "")
            {
                if (!(type.BaseType.Namespace.StartsWith( "System" ) || type.BaseType.Namespace.StartsWith( "Microsoft" )))
                {
                    ParentClass = TypeObj.BaseType.Resolve();
                }
            }
            else
            {
                ParentClass = TypeObj.BaseType?.Resolve();
            }

            // Finding interfaces which are only implemented by the class and declared specifically in the class
            FindInterfacesImplemented();

            // Finding fields and properties declared by the class
            FieldsList = TypeObj.Fields.ToList();
            PropertiesList = TypeObj.Properties.ToList();

            Trace.WriteLine( "Extracting Relationships List for " + type.FullName );
            //Extracting the relationships between different classes and the current ParsedClassMonoCecil Type obj
            if (!TypeObj.GetType().IsGenericType)
            {
                UpdateInheritanceList();
                UpdateRelationshipsListFromCtors();
                UpdateAggregationList();
                UpdateUsingList();
            }
            Trace.WriteLine( "Updated the Relationships List for " + type.FullName );
            Trace.WriteLine( "ParsedMonoCecil Obj creation Completed for " + type.FullName );


            // Properties,events handlers will be coming to methods list when used GetMethods() as described above
            List<EventDefinition> events = new();
            if(TypeObj?.Events != null)
            {
                events = TypeObj.Events.ToList();
            }

            if (PropertiesList.Count > 0 || events.Count > 0)
            {
                List<MethodDefinition> methodInfos = new(MethodsList);
                List<string> propertiesNames = new( PropertiesList.Select( property => property.Name ) );
                List<string> eventHandlersNames = new( events.Select( eventInfo => eventInfo.Name ) );

                foreach (MethodDefinition method in MethodsList)
                {
                    string methodName = method.Name;

                    if ((methodName.StartsWith( "get_" ) || methodName.StartsWith( "set_" )) && propertiesNames.Contains( methodName.Substring( 4 , methodName.Length - 4 ) ))
                    {
                        methodInfos.Remove( method );
                    }

                    if (methodName.StartsWith( "add_" ) && eventHandlersNames.Contains( methodName.Substring( 4 , methodName.Length - 4 ) ))
                    {
                        methodInfos.Remove( method );
                    }

                    if (methodName.StartsWith( "remove_" ) && eventHandlersNames.Contains( methodName.Substring( 7 , methodName.Length - 7 ) ))
                    {
                        methodInfos.Remove( method );
                    }
                }

                MethodsList = methodInfos;
            }


            //// This is commented so that it will be used later if required
            //// Properties can come into fields and methods. Currently here trying to remove those fields from fields list (Auto properties)
            //if (PropertiesList.Count > 0)
            //{
            //    List<FieldDefinition> fieldsToRemove = new();

            //    List<string> propertiesNames = new();

            //    foreach (PropertyDefinition property in PropertiesList)
            //    {
            //        propertiesNames.Add(property.Name);
            //    }

            //    foreach (FieldDefinition field in FieldsList)
            //    {
            //        if ((field.Name.StartsWith("<")) && (field.Name.EndsWith(">k__BackingField")) && (propertiesNames.Contains(field.Name.Substring(1, field.Name.Length - 17)))){
            //            fieldsToRemove.Add(field);
            //        }
            //    }

            //    foreach(FieldDefinition field in fieldsToRemove)
            //    {
            //       FieldsList.Remove(field);
            //    }
            //}
        }


        /// <summary>
        /// Finding interfaces which are implemented by the class and not implemented by parent class or parent interface
        /// </summary>
        private void FindInterfacesImplemented()
        {
            if (TypeObj.HasInterfaces)
            {
                // Finding all interfaces implemented by a class
                Interfaces = TypeObj.Interfaces.ToList();

                // Finding interfaces implemented by class & not implemented by parent class
                if (ParentClass?.Interfaces != null)
                {
                    Interfaces = TypeObj.Interfaces.Except( ParentClass.Interfaces ).ToList();
                }

                // Removing interfaces implemented by parent interfaces
                HashSet<string> removableInterfaceNames = new();

                foreach (InterfaceImplementation parentIface in Interfaces)
                {
                    foreach (InterfaceImplementation parentImplIFace in parentIface.InterfaceType.Resolve().Interfaces)
                    {
                        removableInterfaceNames.Add( parentImplIFace.InterfaceType.FullName );
                    }
                }

                List<InterfaceImplementation> implIfaceList = new();

                foreach (InterfaceImplementation iface in Interfaces)
                {
                    if (!removableInterfaceNames.Contains( iface.InterfaceType.FullName ))
                    {
                        implIfaceList.Add( iface );
                    }
                }

                Interfaces = implIfaceList;
            }
        }


        // Finding if an element is part of collection of sets 
        private bool SetsContainElement<T>( T element , params HashSet<T>[] sets )
        {
            return sets.Any( set => set.Contains( element ) );
        }


        /// <summary>
        /// UpdateInheritanceList updates the Inheritance List
        /// </summary>
        private void UpdateInheritanceList()
        {
            //Adding the parent class (if exist) in the inheritance list
            if (ParentClass != null)
            {
                if (!ParentClass.FullName.StartsWith("System"))
                {
                    InheritanceList.Add( "C" + ParentClass.FullName );
                }
            }

            //adding all interfaces from which the class inherits, in the inheritance list
            foreach (InterfaceImplementation? iface in Interfaces)
            {
                if (!iface.InterfaceType.FullName.StartsWith("System"))
                {
                    InheritanceList.Add( "I" + iface.InterfaceType.FullName );
                }
            }
        }


        /// <summary>
        /// UpdateRelationShipsFromCtorList updates the Relationships List by analyzing the objects and
        /// their method of instantiation specifically related to the constructor
        /// </summary>
        private void UpdateRelationshipsListFromCtors()
        {
            //Composition Relation:
            //Cases1: If any parameter of constructor is assigned to a field of the class, then it is composition relationship.
            //CAse2: If any new object is instantiated inside a constructor, and is assigned to any class field, then there exist composition relationship.
            foreach (MethodDefinition? ctor in Constructors)
            {
                List<ParameterDefinition>? parameterList = ctor.Parameters.ToList();
                if (ctor.HasBody)
                {
                    //iterating over all instructions, to check if any class field (of reference type) is assigned a value (be it from parameter or by instantiating a new object)
                    for (int i = 0; i < ctor.Body.Instructions.Count; i++)
                    {
                        Instruction? inst = ctor.Body.Instructions[i];
                        if (inst != null && inst.OpCode == OpCodes.Stfld)
                        {
                            FieldReference? fieldReference = (FieldReference)inst.Operand;
                            TypeReference? fieldType = fieldReference.FieldType;
                            TypeDefinition? objType = fieldType.Resolve();

                            // Check if the field type is of reference type (not a value type), not a Generic type, and does not start with "System"
                            if (!fieldType.IsValueType && !objType.GetType().IsGenericType && !objType.FullName.StartsWith( "System" ))
                            {
                                if (objType.IsClass && !SetsContainElement( "C" + objType.FullName , InheritanceList ))
                                {
                                    if (objType.Name.StartsWith( "<" ))
                                    {
                                        continue;
                                    }
                                    CompositionList.Add( "C" + objType.FullName );
                                }
                                else if (objType.IsInterface && !SetsContainElement( "I" + objType.FullName , InheritanceList ))
                                {
                                    CompositionList.Add( "I" + objType.FullName );
                                }
                            }
                        }
                    }
                }


                // Handling Case 2 of using relationship, where the parameter of constructor is assigned to a local variable.
                // If between 2 classes composition and using relation exist, giving priority to composition relation.
                foreach (ParameterDefinition? parameter in parameterList)
                {
                    TypeDefinition? parameterType = parameter.ParameterType.Resolve();
                    string? parameterTypeName = parameter.ParameterType.FullName;

                    if (!parameterTypeName.StartsWith( "System" ) && !parameterType.GetType().IsGenericType)
                    {
                        //Console.WriteLine( parameterType );
                        //Console.WriteLine( parameterType.IsClass );
                        if (parameterType.IsClass && !SetsContainElement( "C" + parameter.ParameterType.FullName , InheritanceList , CompositionList ))
                        {
                            if(parameter.ParameterType.Name.StartsWith( "<" ) )
                            {
                                continue;
                            }
                            UsingList.Add( "C" + parameter.ParameterType.FullName );
                        }
                        else if (parameterType.IsInterface && !SetsContainElement( "I" + parameter.ParameterType.FullName , InheritanceList , CompositionList ))
                        {
                            UsingList.Add( "I" + parameter.ParameterType.FullName );
                        }
                    }
                }

                //Handling Case 1 of aggregation relationship, where new object is instantiated inside a constructor and is assigned to its local variable.
                //If between 2 classes composition and aggregation relation exists, giving priority to composition relation.
                foreach (MethodDefinition? ctr in Constructors)
                {
                    if (ctr.HasBody)
                    {
                        foreach (Instruction inst in ctr.Body.Instructions)
                        {
                            if (inst != null && inst.OpCode == OpCodes.Newobj)
                            {
                                MethodReference? constructorReference = (MethodReference)inst.Operand;
                                TypeDefinition? objectType = constructorReference.DeclaringType.Resolve();

                                // adding to aggregation list, if object is not of generic type and is not in composition list (i.e either the object is assigned to a local variable
                                // or if not, since we have decided on the priority of composition over aggreagation, we can check if the composition list has that particular class object or not).
                                if (!objectType.GetType().IsGenericType && !objectType.FullName.StartsWith("System"))
                                {
                                    if (objectType.IsClass && !SetsContainElement( "C" + objectType.FullName , InheritanceList , CompositionList ))
                                    {
                                        if (objectType.Name.StartsWith( "<" ))
                                        {
                                            continue;
                                        }
                                        AggregationList.Add( "C" + objectType.FullName );
                                        UsingList.Remove( "C" + objectType.FullName );

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// UpdateAggreagationList is used to extract out the aggregation relationship existing between the current class and other classes
        /// </summary>
        private void UpdateAggregationList()
        {
            // Aggregation List:
            // Cases1: If a new class object is created and/or instantiated inside any method (other than constructor), its aggregation.
            // Cases2: If a new class object is instantiated inside a constructor, but is not assigned to any class field, its aggregation. 
            // check if new opcode is present in method body and get its type
            foreach (MethodDefinition? method in MethodsList)
            {
                if (method.HasBody)
                {
                    foreach (Instruction? inst in method.Body.Instructions)
                    {
                        if (inst != null && inst.OpCode == OpCodes.Newobj)
                        {
                            var constructorReference = (MethodReference)inst.Operand;
                            TypeReference? objectType = constructorReference.DeclaringType;
                        
                            // adding to aggrgation list, if object is not of generic type
                            if (!objectType.GetType().IsGenericType && !objectType.FullName.StartsWith( "System" ) && !SetsContainElement( "C" + objectType.FullName , InheritanceList , CompositionList ))
                            {
                                if(objectType.Name.StartsWith( "<" ))
                                {
                                    continue;
                                }
                                AggregationList.Add( "C" + objectType.FullName );
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// UpdateUsingList is used to extract out the using relationship existing between the current class and other classes
        /// </summary>
        private void UpdateUsingList()
        {
            // Using Class Relationship 
            // Cases2: If any method (other than constructors) contain other class as parameter

            //Get all the methods and its parameters into a dictionary, which can be iterated over to check its types.
            Dictionary<MethodDefinition , List<ParameterDefinition>> dict = GetFunctionParameters();
            foreach (KeyValuePair<MethodDefinition , List<ParameterDefinition>> pair in dict)
            {
                foreach (ParameterDefinition? argument in pair.Value)
                {
                    TypeDefinition? objType = argument.ParameterType.Resolve();
                    TypeReference objRef = argument.ParameterType;

                    //adding to using list, if the parameter is of class type and is not of generic class (list, dict,etc.)
                    if (objType != TypeObj && objType != null && !(objType.GetType().IsGenericType || objRef.IsGenericInstance))
                    {
                        if (pair.Key.IsConstructor)
                        {
                            continue;
                        }
                        else
                        {
                            //ignoring the classes those start with "System"
                            if (!argument.ParameterType.FullName.StartsWith( "System" ))
                            {
                                if (objType.IsClass && !SetsContainElement( "C" + argument.ParameterType.FullName , InheritanceList , CompositionList , AggregationList ))
                                {
                                    //Console.WriteLine( "1: " + argument.ParameterType.FullName );
                                    if (argument.ParameterType.Name.StartsWith( "<" ))
                                    {
                                        continue;
                                    }
                                    UsingList.Add( "C" + argument.ParameterType.FullName );
                                }
                                else if (objType.IsInterface && !SetsContainElement( "I" + argument.ParameterType.FullName , InheritanceList , CompositionList , AggregationList ))
                                {
                                    //Console.WriteLine( "1: " + argument.ParameterType.FullName );
                                    UsingList.Add( "I" + argument.ParameterType.FullName );
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Gets the all parameters information related to all methods in the class
        /// </summary>
        public Dictionary<MethodDefinition , List<ParameterDefinition>> GetFunctionParameters()
        {
            Dictionary<MethodDefinition , List<ParameterDefinition>>? dict = new();

            if (MethodsList != null)
            {
                foreach (MethodDefinition method in MethodsList)
                {
                    dict.Add( method , method.Parameters.ToList() );
                }
            }

            return dict;
        }
    }
}
