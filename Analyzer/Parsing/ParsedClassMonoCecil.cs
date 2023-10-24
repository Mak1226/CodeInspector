using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;

namespace Analyzer.Parsing
{
    public class ParsedClassMonoCecil
    {
        private readonly TypeDefinition _typeObj;
        private readonly string? _name;
        private readonly List<MethodDefinition> _constructors;
        private readonly List<MethodDefinition> _methods;
        private readonly TypeDefinition? _parentClass;
        private readonly List<InterfaceImplementation> _interfaces;
        private readonly List<FieldDefinition> _fields;

        public ParsedClassMonoCecil(TypeDefinition type)
        {
            _typeObj = type;
            _name = type.FullName;
            _constructors = new List<MethodDefinition>();
            _methods = new List<MethodDefinition>();
            _interfaces = new List<InterfaceImplementation>();
            _fields = new List<FieldDefinition>();

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

            if(type.BaseType != null)
            {
                _parentClass = type.BaseType.Resolve();
            }

            //_interfaces = type.Interfaces?.ToList().Except(_parentClass?.Interfaces?.ToList());
            if(type.HasInterfaces)
            {
                _interfaces = type.Interfaces.ToList();
                
                if(_parentClass?.Interfaces != null && _parentClass.Interfaces != null)
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

        }
    }
}
