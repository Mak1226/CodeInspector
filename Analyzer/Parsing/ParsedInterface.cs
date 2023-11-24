/******************************************************************************
* Filename    = ParsedDLLFile.cs
* 
* Author      = Nikhitha Atyam, Thanmayee
* 
* Product     = Analyzer
* 
* Project     = Analyzer
*
* Description = Parses the most used information from the interface object using System.Reflection
*****************************************************************************/


using System.Reflection;


namespace Analyzer.Parsing
{
    /// <summary>
    /// Parses the most used information from the interface object using System.Reflection
    /// </summary>
    public class ParsedInterface
    {
        public Type TypeObj { get; }     // type object to access interface related information
        public string Name { get; }    // Name of Interface. (Doesn't include namespace name in it)
        public MethodInfo[] Methods { get; }    // Methods declared only by the interface

        /// <summary>
        /// Contains interfaces implemented by the interface only the ones at the lower level (direct implementation)
        /// i.e Does not include interfaces implemented by the parent interface
        /// This is useful for creation of class relational diagram
        /// </summary>
        public Type[] ParentInterfaces;


        // Parses the interface object (parameter - type)
        public ParsedInterface(Type type)
        {
            TypeObj = type;
            Name = type.Name;

            // Using BindingFlag: DeclaredOnly to limit to the declared members of the interface
            Methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            // Finding interfaces which are only implemented by the class and declares specifically in the class
            ParentInterfaces = type.GetInterfaces();

            if(ParentInterfaces.Length > 0)
            {
                HashSet<string> removableInterfaceNames = new();

                foreach (Type parentIface in ParentInterfaces)
                {
                    foreach (Type parentImplIFace in parentIface.GetInterfaces())
                    {
                        removableInterfaceNames.Add( parentImplIFace.FullName);
                    }
                }

                List<Type> implIfaceList = new();

                foreach (Type iface in ParentInterfaces)
                {
                    if (!removableInterfaceNames.Contains(iface.FullName))
                    {
                        implIfaceList.Add(iface);
                    }
                }

                ParentInterfaces = implIfaceList.ToArray();
            }
        }
    }
}
