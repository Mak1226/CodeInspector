using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Parsing
{
    /// <summary>
    /// Parses the most used information from the interface object using System.Reflection
    /// </summary>
    public class ParsedInterface
    {
        private readonly Type _typeObj;     // type object to access interface related information
        private readonly string _name;     // Name of Interface. (Doesn't include namespace name in it)
        private readonly MethodInfo[]? _methods;

        /// <summary>
        /// Contains interfaces implemented by the class only the ones specifically mentioned 
        /// Does not include interfaces implemented by the parent class/ implemented interface
        /// This is useful for creation of class relational diagram
        /// </summary>
        private readonly Type[]? _parentInterfaces;    

        public ParsedInterface(Type type)
        {
            _typeObj = type;
            _name = type.Name;
            _methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);


            // Finding interfaces which are only implemented by the class and declares specifically in the class
            _parentInterfaces = type.GetInterfaces();

            if(_parentInterfaces?.Length > 0)
            {
                HashSet<string> removableInterfaceNames = new();

                foreach (var i in _parentInterfaces)
                {
                    foreach (var x in i.GetInterfaces())
                    {
                        removableInterfaceNames.Add(x.FullName);
                    }
                }

                List<Type> ifaceList = new();

                foreach (var iface in _parentInterfaces)
                {
                    if (!removableInterfaceNames.Contains(iface.FullName))
                    {
                        ifaceList.Add(iface);
                    }
                }

                _parentInterfaces = ifaceList.ToArray();
            }
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
