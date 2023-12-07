/******************************************************************************
 * Filename    = TestDispose.cs
 * 
 * Author      = Arun Sankar
 *
 * Product     = Analyzer
 * 
 * Project     = AnalyzerTests
 *
 * Description = Unit Tests for DisposableFieldsShouldBeDisposedRule class
 *****************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Analyzer.Parsing;
using Analyzer;

namespace AnalyzerTests.Pipeline
{

    /// <summary>
    /// Test class for the DisposableFieldsShouldBeDisposedRule.
    /// </summary>
    [TestClass()]
    public class TestDispose
    {

        //disposetestviolated.dll is generated from below code

        //public class ViolatingDisposableClass : IDisposable
        //{
        //    private IDisposable disposableField = new SomeDisposableObject();

        //    public void DoSomething()
        //    {
        //        // Some logic here
        //    }

        //    public void Dispose()
        //    {
        //        // Missing Dispose call for disposableField
        //    }
        //}

        //public class SomeDisposableObject : IDisposable
        //{
        //    public void Dispose()
        //    {
        //        Console.WriteLine( "SomeDisposableObject disposed" );
        //    }
        //}

        /// <summary>
        /// Test method for DisposableFieldsShouldBeDisposedRule when violation is present.
        /// </summary>
        [TestMethod()]
        public void TestViolation()
        {
            // Specify the path to the DLL file
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\disposetestviolated.dll";

            // Create a list of DLL paths
            ParsedDLLFile dllFile = new(path);

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            DisposableFieldsShouldBeDisposedRule analyzer = new(dllFiles);

            // RenderImageBytes the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                Trace.WriteLine(dll.Key);

                AnalyzerResult res = dll.Value;

                Assert.AreEqual( res.Verdict , 0 );

                Trace.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
            }

        }

        //disposetestold.dll is generated from below code

        //public class DisposableTestClassWithDispose : IDisposable
        //{
        //    private IDisposable disposableField = new DummyDisposable(); // Disposable field

        //    public void SomeMethod()
        //    {
        //        // Accessing the disposable field, but no need for Dispose
        //        Console.WriteLine( disposableField.ToString() );
        //    }

        //    public void Dispose()
        //    {
        //        // Properly disposing the field
        //        if (disposableField != null)
        //        {
        //            disposableField.Dispose();
        //        }
        //    }
        //}

        //public class DummyDisposable : IDisposable
        //{
        //    public void Dispose()
        //    {
        //        // Dummy Dispose method
        //    }
        //}


        /// <summary>
        /// Test method for DisposableFieldsShouldBeDisposedRule when no violation is present.
        /// </summary>
        [TestMethod()]
        public void TestNoViolation()
        {
            // Specify the path to the DLL file
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\disposetestold.dll";

            // Create a list of DLL paths
            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            DisposableFieldsShouldBeDisposedRule analyzer = new( dllFiles );

            // RenderImageBytes the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Assert.AreEqual( res.Verdict , 1 );

                Trace.WriteLine( res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage );
            }

        }

        //MultipleDisposableFieldsTestClass.dll is generated from below code

        //public class MultipleDisposableFieldsTestClass : IDisposable
        //{
        //    private StreamReader fileReader1; // Disposable field 1
        //    private StreamReader fileReader2; // Disposable field 2

        //    public MultipleDisposableFieldsTestClass( string filePath1 , string filePath2 )
        //    {
        //        fileReader1 = new StreamReader( filePath1 );
        //        fileReader2 = new StreamReader( filePath2 );
        //    }

        //    public void ReadFileContents()
        //    {
        //        // Reading file contents, but missing Dispose call for both fields
        //        Console.WriteLine( fileReader1.ReadToEnd() );
        //        Console.WriteLine( fileReader2.ReadToEnd() );
        //    }

        //    public void Dispose()
        //    {
        //        // Properly disposing both fields
        //        fileReader1?.Dispose();
        //        fileReader2?.Dispose();
        //    }

        /// <summary>
        /// Test method for DisposableFieldsShouldBeDisposedRule when there are multiple disposable fields.
        /// </summary>
        [TestMethod()]
        public void TestMultipleDisposableFields()
        {
            // Specify the path to the DLL file
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\MultipleDisposableFieldsTestClass.dll";

            // Create a list of DLL paths
            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            DisposableFieldsShouldBeDisposedRule analyzer = new( dllFiles );

            // RenderImageBytes the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Assert.AreEqual( res.Verdict , 0 );

                Trace.WriteLine( res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage );
            }

        }

        /// <summary>
        /// Test method for DisposableFieldsShouldBeDisposedRule on a DLL with no disposable fields.
        /// </summary>
        [TestMethod()]
        public void TestRandom()
        {
            // Specify the path to the DLL file
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\depthofinh.dll";

            // Create a list of DLL paths
            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            DisposableFieldsShouldBeDisposedRule analyzer = new( dllFiles );

            // RenderImageBytes the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Assert.AreEqual( res.Verdict , 1 );

                Trace.WriteLine( res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage );
            }

        }

        //branchcoveragedispose.dll is generated from below code

        //public interface ICustomDisposable : IDisposable
        //{
        //    void CustomDispose();
        //}

        //public class BaseDisposable : ICustomDisposable
        //{
        //    public void Dispose()
        //    {
        //        Console.WriteLine( "BaseDisposable Dispose" );
        //    }

        //    public void CustomDispose()
        //    {
        //        Console.WriteLine( "BaseDisposable CustomDispose" );
        //    }
        //}

        //public class DerivedClass : BaseDisposable
        //{
        //    // This class directly implements IDisposable through interfaces
        //}

        /// <summary>
        /// Test method for DisposableFieldsShouldBeDisposedRule when there is a derived class.
        /// </summary>
        [TestMethod()]
        public void TestBaseAndDerived()
        {
            // Specify the path to the DLL file
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\branchcoveragedispose.dll";

            // Create a list of DLL paths
            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            DisposableFieldsShouldBeDisposedRule analyzer = new( dllFiles );

            // RenderImageBytes the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Assert.AreEqual( res.Verdict , 1 );

                Trace.WriteLine( res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage );
            }

        }
    }
}
