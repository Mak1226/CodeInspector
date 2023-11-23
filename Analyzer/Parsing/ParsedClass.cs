using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

namespace Analyzer.Parsing
{
    /// <summary>
    /// Parses the most used information from the class object using System.Reflection
    /// </summary>
    public class ParsedClass
    {
        public Type TypeObj { get; }   // type object to access class related information
        public string Name { get; }    // Name of Class. (Doesn't include namespace name in it)
        public ConstructorInfo[] Constructors { get;  }      // Includes Default Constructors also if created

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

            Constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            Methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            Fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            Properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            //// Properties can come into fields and methods. Currently here trying to remove as much as possible from fields (Auto properties)
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

            FindInterfacesImplemented();
        }

        /// <summary>
        /// Finding interfaces which are only implemented by the class and declares specifically in the class
        /// </summary>
        private void FindInterfacesImplemented()
        {
            if (ParentClass != null && ParentClass.GetInterfaces() != null)
            {
                Interfaces = TypeObj.GetInterfaces().Except( ParentClass.GetInterfaces() ).ToArray();
            }
            else
            {
                Interfaces = TypeObj.GetInterfaces();
            }

            if (Interfaces.Length > 0)
            {
                HashSet<string> removableInterfaceNames = new();

                foreach (Type i in Interfaces)
                {
                    foreach (Type x in i.GetInterfaces())
                    {
                        removableInterfaceNames.Add( x.FullName );
                    }
                }

                List<Type> ifaceList = new();

                foreach (Type iface in Interfaces)
                {
                    if (!removableInterfaceNames.Contains( iface.FullName ))
                    {
                        ifaceList.Add( iface );
                    }
                }

                Interfaces = ifaceList.ToArray();
            }
        }
    }
}
