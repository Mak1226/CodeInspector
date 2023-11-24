/******************************************************************************
* Filename    = ParsedClass.cs
* 
* Author      = Nikhitha Atyam, Yukta Salunkhe
* 
* Product     = Analyzer
* 
* Project     = Analyzer
*
* Description = Parses the most used information from the class object using System.Reflection
*****************************************************************************/

using System.Reflection;


namespace Analyzer.Parsing
{
    /// <summary>
    /// Parses the most used information from the class object using System.Reflection
    /// System.Reflection Documentation: https://learn.microsoft.com/en-us/dotnet/api/system.reflection?view=net-6.0
    /// </summary>
    public class ParsedClass
    {
        public Type TypeObj { get; }   // type object to access class related information
        public string Name { get; }    // Name of Class (Doesn't include namespace name in it)
        public ConstructorInfo[] Constructors { get;  }      // Includes default constructors also

        /// <summary>
        /// Contains interfaces implemented by the class only the ones at the lower level (direct implementation)
        /// i.e Does not include interfaces implemented by the parent class/ implemented interface
        /// This is useful for creation of class relational diagram
        /// </summary>
        public Type[] Interfaces { get; private set; }

        public MethodInfo[] Methods { get; }     // Methods declared only by the class (it include other things like properties etc.. as per documentation)
        public FieldInfo[] Fields { get; }      // Fields declared only by the class
        public PropertyInfo[] Properties { get; }   // Properties declared only by the class
        public Type? ParentClass { get; }        // ParentClass - does not contain classes starting with System/Microsoft

        /// <summary>
        /// Parses the most used information from the class object
        /// </summary>
        /// <param name="type">class object when parsed using System.reflection</param>
        public ParsedClass(Type type)
        {
            TypeObj = type;
            Name = type.Name;
            Interfaces = Array.Empty<Type>();

            // Using BindingFlag: DeclaredOnly to limit to the declared members of the class
            Constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            Methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            Fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            Properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            
            // Finding parent class declared in the project - does not contain classes starting with System/Microsoft
            if (type.BaseType?.Namespace != null) 
            {
                if (!(type.BaseType.Namespace.StartsWith("System") || type.BaseType.Namespace.StartsWith("Microsoft")))
                {
                    ParentClass = TypeObj.BaseType;
                }
            }
            else
            {
                ParentClass = TypeObj.BaseType;
            }

            // Finding interfaces implemented as per class diagram requirements 
            FindInterfacesImplemented();


            //// This is commented so that it will be used later if required
            //// Properties can come into fields and methods. Currently here trying to remove Auto Implemented Properties from fields (Auto properties)
            //if(Properties.Length > 0)
            //{
            //    List<FieldInfo> fields = Fields.ToList();

            //    List<string> propertiesNames = new();

            //    foreach(PropertyInfo property in Properties)
            //    {
            //        propertiesNames.Add(property.Name);
            //    }

            //    foreach(FieldInfo field in Fields)
            //    {
            //        if (field.Name.StartsWith("<") && field.Name.EndsWith(">k__BackingField") && propertiesNames.Contains(field.Name.Substring(1,field.Name.Length - 17)))
            //        {
            //            fields.Remove(field);
            //        }
            //    }

            //    Fields = fields.ToArray();
            //}
        }


        /// <summary>
        /// Finding interfaces which are implemented by the class and not implemented by parent class or parent interface
        /// </summary>
        private void FindInterfacesImplemented()
        {
            // Finding interfaces implemented by class & not implemented by parent class
            if (ParentClass != null && ParentClass.GetInterfaces() != null)
            {
                Interfaces = TypeObj.GetInterfaces().Except( ParentClass.GetInterfaces() ).ToArray();
            }
            else
            {
                Interfaces = TypeObj.GetInterfaces();
            }

            // Removing interfaces implemented by parent interfaces
            if (Interfaces.Length > 0)
            {
                HashSet<string> removableInterfaceNames = new();

                foreach (Type parentIface in Interfaces)
                {
                    foreach (Type parentImplIFace in parentIface.GetInterfaces())
                    {
                        removableInterfaceNames.Add(parentImplIFace.FullName);
                    }
                }

                List<Type> implIfaceList = new();

                foreach (Type iface in Interfaces)
                {
                    if (!removableInterfaceNames.Contains(iface.FullName))
                    {
                        implIfaceList.Add( iface );
                    }
                }

                Interfaces = implIfaceList.ToArray();
            }
        }
    }
}
