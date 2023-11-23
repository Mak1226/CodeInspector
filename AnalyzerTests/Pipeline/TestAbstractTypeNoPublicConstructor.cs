/******************************************************************************
* Filename    = TestAbstractTypeNoPublicConstructor.cs
* 
* Author      = Sneha Bhattacharjee
*
* Product     = Analyzer
* 
* Project     = AnalyzerTests
*
* Description = Unit Tests for AbstractTypeNoPublicConstructor.cs
*****************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Parsing;
using System.IO;
using System.Reflection;

namespace AbstractClassWithPublicConstructor
{
    public abstract class AbstractClassWithPublicConstructor
    {
        private readonly int _sampleVar;
        private int _sampleVar2;
        public AbstractClassWithPublicConstructor()
        {
            _sampleVar = 100;
        }

        private void SampleFunction( int sampleVar )
        {
            _sampleVar2 = sampleVar + _sampleVar;
        }
    }

    public abstract class AbstractClassWithProtectedConstructor
    {
        private readonly int _sampleVar;
        private readonly int _sampleVar2;
        protected AbstractClassWithProtectedConstructor()
        {
            _sampleVar = 100;
        }
    }

    public abstract class AbstractClassWithInternalConstructor
    {
        private readonly int _sampleVar;
        private readonly int _sampleVar2;
        internal AbstractClassWithInternalConstructor()
        {
            _sampleVar = 100;
            _sampleVar2 = 2 * _sampleVar;
        }
    }

    internal abstract class InternalAbstractClassWithPublicConstructor
    {
        private readonly int _sampleVar;
        private readonly int _sampleVar2;
        public InternalAbstractClassWithPublicConstructor()
        {
            _sampleVar = 100;
            _sampleVar2 = 2 * _sampleVar;
        }
    }

    public abstract class AbstractClassWithNoDefaultConstructor
    {
        private readonly int _sampleVar;
        private readonly int _sampleVar2;
    }

    internal abstract class InternalAbstractClassWithNoDefaultConstructor
    {
        private readonly int _sampleVar;
        private readonly int _sampleVar2;
    }

    public class PublicClassWithNoDefaultConstructor
    {
        private readonly int _sampleVar;
        private readonly int _sampleVar2;
    }

    public abstract class AbstractClassWithEmptyConstructor
    {
        private readonly int _sampleVar;
        private readonly int _sampleVar2;
        public AbstractClassWithEmptyConstructor()
        {

        }
    }

    public abstract class AbstractClassWithProtectedInternalConstructor
    {
        private readonly int _sampleVar;
        private readonly int _sampleVar2;
        protected internal AbstractClassWithProtectedInternalConstructor()
        {

        }
    }
}

namespace Analyzer.Pipeline.Tests
{
    [TestClass()]
    public class TestAbstractTypeNoPublicConstructor
    {
        private readonly string _dllFile;
        private readonly ParsedDLLFile _parsedDLL;
        public TestAbstractTypeNoPublicConstructor()
        {
            _dllFile = Assembly.GetExecutingAssembly().Location;
            _parsedDLL = new( _dllFile );
        }

        [TestMethod()]
        public void TestAbstractConstructorPresent()
        {
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\AbstractTypeNoPublicConstructor1.dll";

            ParsedDLLFile dLLFile = new (path);

            List<ParsedDLLFile> dllFiles = new() { dLLFile };
            AbstractTypeNoPublicConstructor abstractTypeNoPublicConstructor = new(dllFiles);

            Dictionary<string, AnalyzerResult> result = abstractTypeNoPublicConstructor.AnalyzeAllDLLs();
            Console.WriteLine(result[dLLFile.DLLFileName].ErrorMessage);
            Assert.AreEqual(0, result[dLLFile.DLLFileName].Verdict);
        }

        [TestMethod()]
        public void TestAbstractClassWithPublicConstructor()
        {
            _parsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.FullName != "AbstractClassWithPublicConstructor.AbstractClassWithPublicConstructor" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            AbstractTypeNoPublicConstructor abstractTypeNoPublicConstructor = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = abstractTypeNoPublicConstructor.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 0 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestAbstractClassWithProtectedConstructor()
        {
            _parsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.FullName != "AbstractClassWithPublicConstructor.AbstractClassWithProtectedConstructor" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            AbstractTypeNoPublicConstructor abstractTypeNoPublicConstructor = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = abstractTypeNoPublicConstructor.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 1 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestAbstractClassWithInternalConstructor()
        {
            _parsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.FullName != "AbstractClassWithPublicConstructor.AbstractClassWithProtectedConstructor" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            AbstractTypeNoPublicConstructor abstractTypeNoPublicConstructor = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = abstractTypeNoPublicConstructor.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 1 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestInternalAbstractClassWithPublicConstructor()
        {
            _parsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.FullName != "AbstractClassWithPublicConstructor.InternalAbstractClassWithPublicConstructor" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            AbstractTypeNoPublicConstructor abstractTypeNoPublicConstructor = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = abstractTypeNoPublicConstructor.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 0 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestAbstractClassWithNoDefaultConstructor()
        {
            _parsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.FullName != "AbstractClassWithPublicConstructor.AbstractClassWithNoDefaultConstructor" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            AbstractTypeNoPublicConstructor abstractTypeNoPublicConstructor = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = abstractTypeNoPublicConstructor.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 1 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestInternalAbstractClassWithNoDefaultConstructor()
        {
            _parsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.FullName != "AbstractClassWithPublicConstructor.InternalAbstractClassWithNoDefaultConstructor" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            AbstractTypeNoPublicConstructor abstractTypeNoPublicConstructor = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = abstractTypeNoPublicConstructor.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 1 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestPublicClassWithNoDefaultConstructor()
        {
            _parsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.FullName != "AbstractClassWithPublicConstructor.PublicClassWithNoDefaultConstructor" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            AbstractTypeNoPublicConstructor abstractTypeNoPublicConstructor = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = abstractTypeNoPublicConstructor.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 1 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestAbstractClassWithEmptyConstructor()
        {
            _parsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.FullName != "AbstractClassWithPublicConstructor.AbstractClassWithEmptyConstructor" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            AbstractTypeNoPublicConstructor abstractTypeNoPublicConstructor = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = abstractTypeNoPublicConstructor.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 0 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestAbstractClassWithProtectedInternalConstructor()
        {
            _parsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.FullName != "AbstractClassWithPublicConstructor.AbstractClassWithProtectedInternalConstructor" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            AbstractTypeNoPublicConstructor abstractTypeNoPublicConstructor = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = abstractTypeNoPublicConstructor.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 0 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestException()
        {
            _parsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.FullName != "AbstractClassWithPublicConstructor.AbstractClassWithProtectedInternalConstructor" );
            List<ParsedDLLFile> parseddllFiles = new() { null };

            AbstractTypeNoPublicConstructor abstractTypeNoPublicConstructor = new( parseddllFiles );

            Assert.ThrowsException<NullReferenceException>(() => abstractTypeNoPublicConstructor.AnalyzeAllDLLs());
        }
    }
}
