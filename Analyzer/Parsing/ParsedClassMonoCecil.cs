using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;

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

        private readonly List<MethodDefinition> _methods;   // Methods declared only by the class
        private readonly TypeDefinition? _parentClass;      // ParentClass - does not contain classes starting with System/Microsoft
        private readonly List<FieldDefinition> _fields;    
        private readonly List<PropertyDefinition> _properties;

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
                if(method.IsConstructor)
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
                
                if(_parentClass?.Interfaces != null)
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
        }
    }
}