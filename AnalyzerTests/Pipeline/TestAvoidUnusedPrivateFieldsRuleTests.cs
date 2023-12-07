/******************************************************************************
* Filename    = TestAvoidUnusedPrivateFieldsRuleTests.cs
*
* Author      = Mangesh Dalvi
* 
* Roll No     = 112001010
*
* Product     = Code Inspector
* 
* Project     = AnalyzerTests
*
* Description = Unit tests for the UnusedPrivateFieldsRuleTests class.
******************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Analyzer.Parsing;
using Analyzer.Pipeline;
using Mono.Cecil;
using Analyzer;

namespace AnalyzerTests.Pipeline
{
    /// <summary>
    /// Test class for test TestClassWithUnusedPrivateFields
    /// </summary>
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

    /// <summary>
    /// Test class for test TestClassWithUnusedPrivateFields
    /// </summary>
    public class ClassWithNoFields
    {

    }

    /// <summary>
    /// Test class for the TestClassWithNoFields.
    /// </summary>
    [TestClass()]
    public class TestAvoidUnusedPrivateFieldsRuleTests
    {
        /// <summary>
        /// Test method for analyzing a DLL with unused private fields.
        /// </summary>
        [TestMethod()]
        public void Test1()
        {
            //UnusedPrivateFields.cs

            //namespace ClassLibrary1
            //{
            //    public abstract class TeacherAnalyzer
            //    {
            //        public int _something; //unused
            //        public string _some;  // unused
            //    }
            //}

            string path = "..\\..\\..\\TestDLLs\\UnusedPrivateFields.dll";

            ParsedDLLFile dllFile = new(path);
            List<ParsedDLLFile> dllFiles = new() { dllFile };
            AvoidUnusedPrivateFieldsRule avoidUnusedPrivateFieldsRule = new(dllFiles);
            Dictionary<string , AnalyzerResult> result = avoidUnusedPrivateFieldsRule.AnalyzeAllDLLs();

            AnalyzerResult expected = new ("103" , 0 , "TeacherAnalyzer : _something _some , are unused private field.");
           
            Assert.AreEqual( expected.AnalyserID, result["UnusedPrivateFields.dll"].AnalyserID);
            Assert.AreEqual( expected.Verdict , result["UnusedPrivateFields.dll"].Verdict );
            Assert.AreEqual( expected.ErrorMessage , result["UnusedPrivateFields.dll"].ErrorMessage );

        }

        /// <summary>
        /// Test method for analyzing a class with unused private fields.
        /// </summary>
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

        /// <summary>
        /// Test method for analyzing a class with no fields.
        /// </summary>
        [TestMethod()]
        public void TestClassWithNoFields()
        {

            string dllFile = Assembly.GetExecutingAssembly().Location;

            AvoidUnusedPrivateFieldsRule avoidUnusedPrivateFieldsRule = new(new List<ParsedDLLFile> { });

            ModuleDefinition module = ModuleDefinition.ReadModule(dllFile);
            TypeReference typeReference = module.ImportReference(typeof(ClassWithNoFields));
            TypeDefinition typeDefinition = typeReference.Resolve();

            List<string> unusedFields = avoidUnusedPrivateFieldsRule.HandleClass(new ParsedClassMonoCecil(typeDefinition));
            List<string> originalUnusedFields = new() {};

            Assert.AreEqual(unusedFields.ToString(), originalUnusedFields.ToString());
        }

    }
}
