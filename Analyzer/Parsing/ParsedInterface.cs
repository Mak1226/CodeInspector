/******************************************************************************
* Filename    = ParsedInterface.cs
* 
* Author      = 
* 
* Project     = Analyzer
*
* Description = Parses the most used information from the interface object using System.Reflection
*****************************************************************************/

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
        public Type TypeObj { get; }     // type object to access interface related information
        public string Name { get; }    // Name of Interface. (Doesn't include namespace name in it)
        public MethodInfo[] Methods { get; }

        /// <summary>
        /// Contains interfaces implemented by the class only the ones specifically mentioned 
        /// Does not include interfaces implemented by the parent class/ implemented interface
        /// This is useful for creation of class relational diagram
        /// </summary>
        public Type[] ParentInterfaces;    

        public ParsedInterface(Type type)
        {
            TypeObj = type;
            Name = type.Name;
            Methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);


            // Finding interfaces which are only implemented by the class and declares specifically in the class
            ParentInterfaces = type.GetInterfaces();

            if(ParentInterfaces.Length > 0)
            {
                HashSet<string> removableInterfaceNames = new();

                foreach (Type i in ParentInterfaces)
                {
                    foreach (Type x in i.GetInterfaces())
                    {
                        removableInterfaceNames.Add(x.FullName);
                    }
                }

                List<Type> ifaceList = new();

                foreach (Type iface in ParentInterfaces)
                {
                    if (!removableInterfaceNames.Contains(iface.FullName))
                    {
                        ifaceList.Add(iface);
                    }
                }

                ParentInterfaces = ifaceList.ToArray();
            }
        }
    }
}
