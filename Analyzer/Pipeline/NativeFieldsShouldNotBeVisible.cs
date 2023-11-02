using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Analyzer.Pipeline
{
    internal class NativeFieldsShouldNotBeVisible : BaseAnalyzer
    {
        public NativeFieldsShouldNotBeVisible(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            // TODO if required
        }

        public List<Type> FindVisibleNativeFields()
        {
            List<Type> visibleNativeFieldsList = new List<Type>();

            foreach (ParsedClassMonoCecil classObj in parsedDLLFiles.classObjListMC)
            {
                TypeDefinition classType = classObj.TypeObj;
                // rule does not apply to interface, enumerations and delegates or to types without fields
                if (!classType.IsValueType)
                {
                    // TODO
                    // For now implementing just for classes
                    // Classes and namespaces can have structures
                    // Structures are recommended to be immutable too
                    // Otherwise they should also have getter and setter
                    // NOTE: parsedDLLFiles.classObjListMC provides only classes
                    continue;
                }

                if (!classType.HasFields)
                {
                    continue;
                }

                foreach (FieldDefinition field in classType.Fields)
                {
                    if (!field.IsVisible())
                    {
                        continue;
                    }

                    //not readonly native fields or arrays of native fields
                    if ((field.FieldType.IsNative() && !field.IsInitOnly) ||
                        (field.FieldType.IsArray && field.FieldType.GetElementType().IsNative()))
                    {

                        //
                    }
                }
            }

            return visibleNativeFieldsList;

        }
    }
}
