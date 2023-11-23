/******************************************************************************
* Filename    = TestNoEmptyInterface.cs
* 
* Author      = Sneha Bhattacharjee
*
* Product     = Analyzer
* 
* Project     = AnalyzerTests
*
* Description = Unit Tests for NoEmptyInterface.cs
*****************************************************************************/

using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NoEmptyInterface
{
    public interface IInterfaceEmpty
    {

    }

    public interface IInterfaceNotEmpty
    {
        public void ContractImplement();
    }

    public interface IEmptyInterfaceInheritEmptyInterface : IInterfaceEmpty
    {

    }

    public interface IInterfaceInheritNotEmptyInterface : IInterfaceNotEmpty
    {
        public void ContractImplement2();
    }

    public interface IEmptyInterfaceInheritNonEmptyInterface : IInterfaceNotEmpty
    {

    }

    public interface IInterfaceInheritEmptyInterface : IInterfaceEmpty
    {
        public void ContractImplement2();
    }
}

namespace Analyzer.Pipeline.Tests
{
    [TestClass()]
    public class TestNoEmptyInterface
    {
        private readonly string _dllFile;
        private readonly ParsedDLLFile _parsedDLL;
        public TestNoEmptyInterface()
        {
            _dllFile = Assembly.GetExecutingAssembly().Location;
            _parsedDLL = new( _dllFile );
        }

        [TestMethod()]
        public void TestEmptyInterfacePresent()
        {
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\NoEmptyInterfaces1.dll";

            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };
            NoEmptyInterface noEmptyInterfaces = new( dllFiles );

            Dictionary<string , AnalyzerResult> result = noEmptyInterfaces.AnalyzeAllDLLs();
            Console.WriteLine( result[dllFile.DLLFileName].ErrorMessage );
            Assert.AreEqual( 0 , result[dllFile.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestEmptyInterfaceExists()
        {
            _parsedDLL.interfaceObjList.RemoveAll( iface => iface.TypeObj.FullName != "NoEmptyInterface.IInterfaceEmpty" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            NoEmptyInterface noEmptyInterface = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = noEmptyInterface.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 0 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestNoEmptyInterfaceExists()
        {
            _parsedDLL.interfaceObjList.RemoveAll( iface => iface.TypeObj.FullName != "NoEmptyInterface.IInterfaceNotEmpty" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            NoEmptyInterface noEmptyInterface = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = noEmptyInterface.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 1 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestEmptyInterfaceInheritEmptyInterface()
        {
            _parsedDLL.interfaceObjList.RemoveAll( iface => (iface.TypeObj.FullName != "NoEmptyInterface.IEmptyInterfaceInheritEmptyInterface" && 
                                                             iface.TypeObj.FullName != "NoEmptyInterface.IInterfaceEmpty" ));
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            NoEmptyInterface noEmptyInterface = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = noEmptyInterface.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 0 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestInterfaceInheritNotEmptyInterface()
        {
            _parsedDLL.interfaceObjList.RemoveAll( iface => (iface.TypeObj.FullName != "NoEmptyInterface.IInterfaceInheritNotEmptyInterface" && 
                                                             iface.TypeObj.FullName != "NoEmptyInterface.IInterfaceNotEmpty" ));
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            NoEmptyInterface noEmptyInterface = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = noEmptyInterface.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 1 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestEmptyInterfaceInheritNonEmptyInterface()
        {
            _parsedDLL.interfaceObjList.RemoveAll( iface => (iface.TypeObj.FullName != "NoEmptyInterface.IEmptyInterfaceInheritNonEmptyInterface" &&
                                                             iface.TypeObj.FullName != "NoEmptyInterface.IInterfaceNotEmpty") );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            NoEmptyInterface noEmptyInterface = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = noEmptyInterface.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 0 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        public void TestInterfaceInheritEmptyInterface()
        {
            _parsedDLL.interfaceObjList.RemoveAll( iface => (iface.TypeObj.FullName != "NoEmptyInterface.IInterfaceInheritEmptyInterface" &&
                                                             iface.TypeObj.FullName != "NoEmptyInterface.IInterfaceEmpty") );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            NoEmptyInterface noEmptyInterface = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = noEmptyInterface.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 0 , result[_parsedDLL.DLLFileName].Verdict );
        }

        [TestMethod()]
        [ExpectedException( typeof( NullReferenceException ) )]
        public void TestException()
        {
            _parsedDLL.interfaceObjList.RemoveAll( iface => iface.TypeObj.FullName != "AbstractClassWithPublicConstructor.AbstractClassWithProtectedInternalConstructor" );
            List<ParsedDLLFile> parseddllFiles = new() { null };

            AbstractTypeNoPublicConstructor abstractTypeNoPublicConstructor = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = abstractTypeNoPublicConstructor.AnalyzeAllDLLs();

            // If control flow reaches this point, then no exception was raised.
            Assert.Fail( "Exception was not raised" );
        }
    }
}
