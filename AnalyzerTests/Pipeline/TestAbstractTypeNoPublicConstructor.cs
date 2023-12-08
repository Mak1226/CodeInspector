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

using System.Reflection;
using Analyzer;
using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractClassWithPublicConstructor
{
    /// <summary>
    /// Abstract Class With Public Constructor.
    /// </summary>
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

    /// <summary>
    /// Abstract Class With Protected Constructor.
    /// </summary>
    public abstract class AbstractClassWithProtectedConstructor
    {
        private readonly int _sampleVar;
        private readonly int _sampleVar2;
        protected AbstractClassWithProtectedConstructor()
        {
            _sampleVar = 100;
        }
    }

    /// <summary>
    /// Abstract Class With Internal Constructor.
    /// </summary>
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

    /// <summary>
    /// Internal Abstract Class With Public Constructor.
    /// Not an externally visible type.
    /// </summary>
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

    /// <summary>
    /// Abstract Class With No Explicitly Defined Constructor.
    /// Empty private constructor will be created at compile time.
    /// </summary>
    public abstract class AbstractClassWithNoDefaultConstructor
    {
        private readonly int _sampleVar;
        private readonly int _sampleVar2;
    }

    /// <summary>
    /// Internal Abstract Class With No Explicitly Defined Constructor.
    /// Empty private constructor will be created at compile time.
    /// </summary>
    internal abstract class InternalAbstractClassWithNoDefaultConstructor
    {
        private readonly int _sampleVar;
        private readonly int _sampleVar2;
    }

    /// <summary>
    /// Public Class With No Explicitly Defined Constructor.
    /// Empty public constructor will be created at compile time.
    /// </summary>
    public class PublicClassWithNoDefaultConstructor
    {
        private readonly int _sampleVar;
        private readonly int _sampleVar2;
    }

    /// <summary>
    /// Public Abstract Class With empty public constructor.
    /// </summary>
    public abstract class AbstractClassWithEmptyConstructor
    {
        private readonly int _sampleVar;
        private readonly int _sampleVar2;
        public AbstractClassWithEmptyConstructor()
        {

        }
    }

    /// <summary>
    /// Abstract class with protected internal constructor.
    /// protected internal is a visible instance type.
    /// </summary>
    public abstract class AbstractClassWithProtectedInternalConstructor
    {
        private readonly int _sampleVar;
        private readonly int _sampleVar2;
        protected internal AbstractClassWithProtectedInternalConstructor()
        {

        }
    }
}

namespace AnalyzerTests.Pipeline
{
    /// <summary>
    /// Unit tests for AbstractTypeNoPublicConstructor analyzer.
    /// </summary>
    [TestClass()]
    public class TestAbstractTypeNoPublicConstructor
    {
        private readonly string _dllFile;
        private readonly ParsedDLLFile _parsedDLL;
        /// <summary>
        /// Loads the current file and all its types.
        /// </summary>
        public TestAbstractTypeNoPublicConstructor()
        {
            _dllFile = Assembly.GetExecutingAssembly().Location;
            _parsedDLL = new( _dllFile );
        }

        /// <summary>
        /// Fails since abstract type with a public constructor.
        /// <see cref = "AbstractClassWithPublicConstructor.AbstractClassWithPublicConstructor"/>
        /// </summary>
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

        /// <summary>
        /// Passes since protected and internal constructors are allowed.
        /// <see cref = "AbstractClassWithPublicConstructor.AbstractClassWithProtectedConstructor"/>
        /// </summary>
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

        /// <summary>
        /// Passes since protected and internal constructors are allowed.
        /// <see cref = "AbstractClassWithPublicConstructor.AbstractClassWithInternalConstructor"/>
        /// </summary>
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

        /// <summary>
        /// Fails since class is still abstract.
        /// <see cref = "AbstractClassWithPublicConstructor.InternalAbstractClassWithPublicConstructor"/>
        /// </summary>
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

        /// <summary>
        /// Passes since default private constructor gets created at compile time.
        /// <see cref = "AbstractClassWithPublicConstructor.AbstractClassWithNoDefaultConstructor"/>
        /// </summary>
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

        /// <summary>
        /// Still an abstract class, but private constructor.
        /// <see cref = "AbstractClassWithPublicConstructor.InternalAbstractClassWithNoDefaultConstructor"/>
        /// </summary>
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

        /// <summary>
        /// Passes since not abstract class.
        /// <see cref = "AbstractClassWithPublicConstructor.PublicClassWithNoDefaultConstructor"/>
        /// </summary>
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

        /// <summary>
        /// Fails since explicitly defined public constructor.
        /// <see cref = "AbstractClassWithPublicConstructor.AbstractClassWithEmptyConstructor"/>
        /// </summary>
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

        /// <summary>
        /// Fails since constructor is not private or internal
        /// <see cref = "AbstractClassWithPublicConstructor.AbstractClassWithProtectedInternalConstructor"/>
        /// </summary>
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

        /// <summary>
        /// Testing Exception when parsed file object is null.
        /// <exception cref="NullReferenceException"> was thrown.
        /// </summary>
        [TestMethod()]
        public void TestException()
        {
            List<ParsedDLLFile> parseddllFiles = new() { null };

            AbstractTypeNoPublicConstructor abstractTypeNoPublicConstructor = new( parseddllFiles );

            Assert.ThrowsException<NullReferenceException>(() => abstractTypeNoPublicConstructor.AnalyzeAllDLLs());
        }
    }
}
