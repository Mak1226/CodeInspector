/******************************************************************************
* Filename    = TestNativeFieldsShouldNotBeVisible.cs
* 
* Author      = Sneha Bhattacharjee
*
* Product     = Analyzer
* 
* Project     = AnalyzerTests
*
* Description = Unit Tests for NoVisibleInstanceFields.cs
*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer.Parsing;
using System.Reflection;

namespace TestNoVisibleInstanceFields
{
    public class PublicClassWithPublicField
    {
        public int _publicField;
    }

    public class HasProtectedNativeField
    {
        protected int _native;
    }

    public class HasInternalNativeField
    {
        internal int _native;
    }

    public class HasPublicReadonlyNativeField
    {
        public readonly int _native;
    }

    public class HasPublicNativeFieldArray
    {
        public IntPtr[]? _native;
    }

    public class HasPrivateNativeField
    {
        private IntPtr _native;
    }
    
    public class ClassHasProperty
    {
        public int _value {  get; }        
    }

    internal class IsPrivateClass
    {
        public int native;
    }
        
}

namespace Analyzer.Pipeline.Tests
{
    [TestClass()]
    public class TestNoVisibleInstanceFields
    {
        private readonly string _dllFile;
        private readonly ParsedDLLFile _parsedDLL;
        public TestNoVisibleInstanceFields()
        {
            _dllFile = Assembly.GetExecutingAssembly().Location;
            _parsedDLL = new(_dllFile);
        }

        [TestMethod()]
        public void TestHasPublicTypeWithNoFields()
        {
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\NativeFieldsShouldNotBeVisible1.dll";

            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };
            NoVisibleInstanceFields noVisibleInstanceFields = new( dllFiles );

            Dictionary<string , AnalyzerResult> result = noVisibleInstanceFields.AnalyzeAllDLLs();
            Console.WriteLine( result[dllFile.DLLFileName].ErrorMessage );
            Assert.AreEqual( 1 , result[dllFile.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestPublicClassWithPublicField()
        {
            _parsedDLL.classObjListMC.RemoveAll(cls => cls.TypeObj.FullName != "TestNoVisibleInstanceFields.PublicClassWithPublicField" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            NoVisibleInstanceFields nativeFieldsShouldNotBeVisible = new(parseddllFiles);
            Dictionary<string, AnalyzerResult> result = nativeFieldsShouldNotBeVisible.AnalyzeAllDLLs();

            Console.WriteLine(result[_parsedDLL.DLLFileName].ErrorMessage);
            Assert.AreEqual(0, result[_parsedDLL.DLLFileName].Verdict);
        }

        [TestMethod()]
        public void TestHasProtectedNativeField()
        {
            _parsedDLL.classObjListMC.RemoveAll(cls => cls.TypeObj.FullName != "TestNoVisibleInstanceFields.HasProtectedNativeField" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            NoVisibleInstanceFields nativeFieldsShouldNotBeVisible = new(parseddllFiles);
            Dictionary<string, AnalyzerResult> result = nativeFieldsShouldNotBeVisible.AnalyzeAllDLLs();

            Console.WriteLine(result[_parsedDLL.DLLFileName].ErrorMessage);
            Assert.AreEqual(0, result[_parsedDLL.DLLFileName].Verdict);
        }

        [TestMethod()]
        public void TestHasInternalNativeField()
        {
            _parsedDLL.classObjListMC.RemoveAll(cls => cls.TypeObj.FullName != "TestNoVisibleInstanceFields.HasInternalNativeField" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            NoVisibleInstanceFields nativeFieldsShouldNotBeVisible = new(parseddllFiles);
            Dictionary<string, AnalyzerResult> result = nativeFieldsShouldNotBeVisible.AnalyzeAllDLLs();

            Console.WriteLine(result[_parsedDLL.DLLFileName].ErrorMessage);
            Assert.AreEqual(1, result[_parsedDLL.DLLFileName].Verdict);
        }

        [TestMethod()]
        public void TestHasPublicReadonlyNativeField()
        {
            _parsedDLL.classObjListMC.RemoveAll(cls => cls.TypeObj.FullName != "TestNoVisibleInstanceFields.HasPublicReadonlyNativeField" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            NoVisibleInstanceFields nativeFieldsShouldNotBeVisible = new(parseddllFiles);
            Dictionary<string, AnalyzerResult> result = nativeFieldsShouldNotBeVisible.AnalyzeAllDLLs();

            Console.WriteLine(result[_parsedDLL.DLLFileName].ErrorMessage);
            Assert.AreEqual(1, result[_parsedDLL.DLLFileName].Verdict);
        }

        [TestMethod()]
        public void TestHasPublicNativeFieldArray()
        {
            _parsedDLL.classObjListMC.RemoveAll(cls => cls.TypeObj.FullName != "TestNoVisibleInstanceFields.HasPublicNativeFieldArray" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            NoVisibleInstanceFields nativeFieldsShouldNotBeVisible = new(parseddllFiles);
            Dictionary<string, AnalyzerResult> result = nativeFieldsShouldNotBeVisible.AnalyzeAllDLLs();

            Console.WriteLine(result[_parsedDLL.DLLFileName].ErrorMessage);
            Assert.AreEqual(0, result[_parsedDLL.DLLFileName].Verdict);
        }

        [TestMethod()]
        public void TestHasPrivateNativeField()
        {
            _parsedDLL.classObjListMC.RemoveAll( cls => cls.TypeObj.FullName != "TestNoVisibleInstanceFields.HasPrivateNativeField" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            NoVisibleInstanceFields nativeFieldsShouldNotBeVisible = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = nativeFieldsShouldNotBeVisible.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 1 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestClassHasProperty()
        {
            _parsedDLL.classObjListMC.RemoveAll( cls => cls.TypeObj.FullName != "TestNoVisibleInstanceFields.ClassHasProperty" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            NoVisibleInstanceFields nativeFieldsShouldNotBeVisible = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = nativeFieldsShouldNotBeVisible.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 1 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestIsPrivateClass()
        {
            _parsedDLL.classObjListMC.RemoveAll( cls => cls.TypeObj.FullName != "TestNoVisibleInstanceFields.IsPrivateClass" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            NoVisibleInstanceFields nativeFieldsShouldNotBeVisible = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = nativeFieldsShouldNotBeVisible.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 1 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestException()
        {
            List<ParsedDLLFile> parseddllFiles = new() { null };
            NoVisibleInstanceFields nativeFieldsShouldNotBeVisible = new(parseddllFiles);
            Assert.ThrowsException<NullReferenceException>( () => nativeFieldsShouldNotBeVisible.AnalyzeAllDLLs() );
        }
    }
}
