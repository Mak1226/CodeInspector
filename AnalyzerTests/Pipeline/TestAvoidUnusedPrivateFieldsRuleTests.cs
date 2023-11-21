using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Analyzer.Parsing;
using Analyzer.Pipeline;
using Analyzer;
using Microsoft.VisualBasic.FileIO;
using Mono.Cecil.Cil;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using AnalyzerTests.Pipeline;

namespace Analyzer.Pipeline.Tests
{

    public class ClassWithUnusedPrivateFields
    {
        private readonly int _var1;
        private readonly int _var2;

        ClassWithUnusedPrivateFields()
        {
            _var1 = 1;
            _var2 = 2;
        }

        public int GetVar1()
        {
            return _var1;
        }

    }

    [TestClass()]
    public class TestAvoidUnusedPrivateFieldsRuleTests
    {
        [TestMethod()]
        public void Test1()
        {

            //List<string> DllFilePaths = new List<string>();

            //string path = "C:\\Users\\HP\\source\\repos\\Demo1\\ClassLibrary1\\bin\\Debug\\net6.0\\ClassLibrary1.dll";

            string path = "..\\..\\..\\..\\Analyzer\\TestDLLs\\UnusedPrivateFields.dll";
            ParsedDLLFile dllFile = new(path);

            //DllFilePaths.Add(path);

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            AvoidUnusedPrivateFieldsRule avoidUnusedPrivateFieldsRule = new(dllFiles);

            Dictionary<string , AnalyzerResult> result = avoidUnusedPrivateFieldsRule.AnalyzeAllDLLs();

            Console.WriteLine(result[dllFile.DLLFileName].ErrorMessage);

            Assert.AreEqual(1, result[dllFile.DLLFileName].Verdict);

        }

        [TestMethod()]
        public void TestClassWithUnusedPrivateFields()
        {

            string dllFile = Assembly.GetExecutingAssembly().Location;

            AvoidUnusedPrivateFieldsRule avoidUnusedPrivateFieldsRule = new(new List<ParsedDLLFile> { } );

            ModuleDefinition module = ModuleDefinition.ReadModule(dllFile);
            TypeReference typeReference = module.ImportReference(typeof(ClassWithUnusedPrivateFields));
            TypeDefinition typeDefinition = typeReference.Resolve();

            List<string> unusedFields = avoidUnusedPrivateFieldsRule.HandleClass(new ParsedClassMonoCecil(typeDefinition));
            List<string> originalUnusedFields = new() {"_var2"};

            Assert.AreEqual(unusedFields.ToString() ,originalUnusedFields.ToString());
        }


        /*        [TestMethod()]
                public void Test2()
                {

                    List<string> DllFilePaths = new List<string>();

                    DllFilePaths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\UnusedPrivateFields.dll");

                    ParsedDLLFiles dllFiles = new(DllFilePaths);

                    AvoidUnusedPrivateFieldsRule avoidUnusedPrivateFieldsRule = new(dllFiles);

                    var result = avoidUnusedPrivateFieldsRule.Run();

                    Console.WriteLine(result.ErrorMessage);

                    Assert.AreEqual(0, result.Verdict);

                }*/

        //Mono.Collections.Generic.Collection<FieldDefinition> fields = typeDefinition.Fields;

    }
}
