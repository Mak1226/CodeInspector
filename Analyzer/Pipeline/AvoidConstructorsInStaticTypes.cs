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
    /// <summary>
    /// This class represents an analyzer which check if a class containing all static fields and methods has a public not static constructor
    /// </summary>
    public class AvoidConstructorsInStaticTypes : AnalyzerBase
    {
        // Unique identifier for the analyzer
        private string AnalyzerId = "";

        // Dictionary to store whether types have been checked for violations
        private Dictionary<ParsedClass, bool> checkedTypes = new();
        
        // List to store classes violating the rule
        public List<ParsedClass> violatingClasses = new();

        /// <summary>
        /// Initializes a new instance of the AvoidConstructorsWithStaticTypes analyzer with parsed DLL files.
        /// </summary>
        /// <param name="dllFiles"></param>
        public AvoidConstructorsInStaticTypes(ParsedDLLFiles dllFiles) : base(dllFiles)
        {

            foreach (ParsedClass cls in parsedDLLFiles.classObjList)
            {
                if (cls.TypeObj.IsEnum || cls.TypeObj.IsInterface || cls.TypeObj.IsValueType || cls.TypeObj.IsSubclassOf(typeof(Delegate)))
                    continue;
                if (cls.TypeObj.IsDefined(typeof(CompilerGeneratedAttribute), false)){
                    continue;
                }

                // Check if all methods and fields in the class are static
                bool isAllStatic = CheckAllStatic(cls);

                // If all methods and fields are static, check constructors
                if (isAllStatic)
                {
                    foreach(ConstructorInfo constructor in cls.Constructors)
                    {
                        //default constructor should be declared static or private to avoid violations
                        if(!constructor.IsStatic && !constructor.IsPrivate)
                        {
                            violatingClasses.Add(cls);
                            Console.WriteLine("CTOR NOT STATIC, NOT PRIVATE");
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Checks if all methods and fields of a class are static or not
        /// </summary>
        /// <param name="cls"></param>
        /// <returns>True if all methods and fields in the class are static, else returns False</returns>
        public bool CheckAllStatic (ParsedClass cls) { 
            if(cls == null)
            {
                return false;
            }

            // Check if the class has already been checked
            if (checkedTypes.ContainsKey(cls))
                return checkedTypes[cls];

            // Check Methods in the class
            if (cls.Methods.Length != 0)
            {
                foreach(MethodInfo method in cls.Methods)
                {
                    if(method.IsConstructor)
                    {
                        //parameterized constructor not declared as static do not violate the rule
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

            // Check fields in the class
            if (cls.Fields.Length != 0)
            {
                foreach(FieldInfo field in cls.Fields)
                {
                    if(!field.IsStatic) { 
                        return checkedTypes[cls] = false;}
                }
            }

            // Check the parent class, if it exists
            Type parent = cls.ParentClass;

            if(parent  == null)
            {
                return true;
            }

            //Recursively check the parent class. If paprent class is "System.Object" return True.
            if(parent.FullName == "System.Object")
            {
                return checkedTypes[cls] = true;
            }
            return checkedTypes[cls] = CheckAllStatic(parsedDLLFiles.mapTypeToParsedClass[parent]);
        }

        /// <summary>
        /// Constructs the AnalyzerResult object based on the analysis.
        /// </summary>
        /// <param name="dllFiles"></param>
        public override AnalyzerResult Run()
        {
            //If any class violates the rule, return verdict as 0 (Failed), else 1 (Passed)
            int violations = violatingClasses.Count;
            int verdict = 1;
            if(violations > 0)
            {
                verdict = 0;
            }

            if(verdict == 1)
            {
                return new AnalyzerResult(AnalyzerId, verdict, "");
            }

            //adding error message
            string errorMsg = "Classes ";

            int sz = errorMsg.Length;
            for(var i=0; i<sz; i++)
            {
                var cls = errorMsg[i];
                errorMsg += cls.GetType().FullName;
                if(i != sz - 1)
                {
                    errorMsg += ", ";
                }
            }

            errorMsg += " contains only static fields and methods, but has non-static, visible constructor. Try changing it to private or make it static.";

            //Return the AnalyzerResult object, with appropriate error mesaage.
            AnalyzerResult resultObj = new AnalyzerResult(AnalyzerId,verdict, errorMsg);
            return resultObj;
        }
    }
}
