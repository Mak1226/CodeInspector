using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Parsing
{
    public class ParsedInterface
    {
        private readonly Type _typeObj;
        private readonly string? _name;
        private readonly MethodInfo[]? _methods;
        private readonly Type[]? _parentInterfaces;

        public ParsedInterface(Type type)
        {
            _typeObj = type;
            _name = type.FullName;
            _methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            _parentInterfaces = type.GetInterfaces();

            // Add more logic specific to interfaces here if needed.
        }

        public Type TypeObj
        {
            get { return _typeObj; }
        }

        public string Name
        {
            get { return _name; }
        }

        public MethodInfo[] Methods
        {
            get { return _methods; }
        }

        public Type[] ParentInterfaces
        {
            get { return _parentInterfaces; }
        }

        // Add additional methods or properties as required.
    }
}
