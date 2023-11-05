using Analyzer.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Pipeline
{
    public class AvoidConstructorsInStaticTypes : BaseAnalyzer
    {
        private Dictionary<ParsedClass, bool> checkedTypes = new();
        public List<ParsedClass> violatingClasses = new();

        public AvoidConstructorsInStaticTypes(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            //analysis in constructor or getScore?

            foreach (ParsedClass cls in parsedDLLFiles.classObjList)
            {
                if (cls.TypeObj.IsEnum || cls.TypeObj.IsInterface || cls.TypeObj.IsValueType || cls.TypeObj.IsSubclassOf(typeof(Delegate)))
                    continue;
                if (cls.TypeObj.IsDefined(typeof(CompilerGeneratedAttribute), false)){
                    continue;
                }
                
                bool isAllStatic = CheckAllStatic(cls);
                if (isAllStatic)
                {
                    foreach(ConstructorInfo constructor in cls.Constructors)
                    {
                        //default constructor should be declared static or should not be visible
                        //parameterised constructor with non static flag will not reach here, since this is accepted
                        if(!constructor.IsStatic && !constructor.IsPrivate)
                        {
                            violatingClasses.Add(cls);
                            Console.WriteLine("CTOR NOT STATIC, NOT PRIVATE");
                        }
                    }
                }

            }
        }

        public bool CheckAllStatic (ParsedClass cls) { 
            //list of types which are violating

            if(cls == null)
            {
                return false;
            }

            if(checkedTypes.ContainsKey(cls))
                return checkedTypes[cls];

            if(cls.Methods.Length != 0)
            {
                foreach(MethodInfo method in cls.Methods)
                {
                    if(method.IsConstructor)
                    {
                        //parameterized constructor not declared as static does not violate the rule
                        if(!method.IsStatic && method.GetParameters().Length >0) { 
                            return checkedTypes[cls] = false;}
                    }
                    else
                    {
                        if(!method.IsStatic )
                        {
                            return checkedTypes[cls] = false;
                        }
                    }
                }
            }
            if(cls.Fields.Length != 0)
            {
                foreach(FieldInfo field in cls.Fields)
                {
                    if(!field.IsStatic) { 
                        return checkedTypes[cls] = false;}
                }
            }

            Type parent = cls.ParentClass;
            //verify if this works 
            if(parent  == null)
            {
                return true;
            }
            if(parent.FullName == "System.Object")
            {
                return checkedTypes[cls] = true;
            }
            return checkedTypes[cls] = CheckAllStatic(parsedDLLFiles.mapTypeToParsedClass[parent]);
        }

        public override int GetScore()
        {
            //CHECK
            int violations = violatingClasses.Count;
            //score?
            //Add to analyzerResultsDictionary

            if(violations > 0)
            {
                return 0;
            }
            return 1;
        }
    }
}
