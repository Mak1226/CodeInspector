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

using System.Reflection;
using Analyzer;
using Analyzer.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoEmptyInterface
{
    /// <summary>
    /// Empty interface.
    /// </summary>
    public interface IInterfaceEmpty
    {

    }

    /// <summary>
    /// Non empty interface.
    /// </summary>
    public interface IInterfaceNotEmpty
    {
        public void ContractImplement();
    }

    /// <summary>
    /// Empty interface that inherits from an empty interface.
    /// </summary>
    public interface IEmptyInterfaceInheritEmptyInterface : IInterfaceEmpty
    {

    }

    /// <summary>
    /// Non Empty interface that inherits from an non empty interface.
    /// </summary>
    public interface IInterfaceInheritNotEmptyInterface : IInterfaceNotEmpty
    {
        public void ContractImplement2();
    }

    /// <summary>
    /// Empty interface that inherits from an non empty interface.
    /// </summary>
    public interface IEmptyInterfaceInheritNonEmptyInterface : IInterfaceNotEmpty
    {

    }

    /// <summary>
    /// Non Empty interface that inherits from an empty interface.
    /// </summary>
    public interface IInterfaceInheritEmptyInterface : IInterfaceEmpty
    {
        public void ContractImplement2();
    }
}

namespace AnalyzerTests.Pipeline
{
    /// <summary>
    /// Unit tests for NoEmptyInterface analyzer.
    /// </summary>
    [TestClass()]
    public class TestNoEmptyInterface
    {
        private readonly string _dllFile;
        private readonly ParsedDLLFile _parsedDLL;
        /// <summary>
        /// Loads the current file and all its types.
        /// </summary>
        public TestNoEmptyInterface()
        {
            _dllFile = Assembly.GetExecutingAssembly().Location;
            _parsedDLL = new( _dllFile );
        }

        /// <summary>
        /// Fails since empty interface exists.
        /// <see cref = "NoEmptyInterface.IInterfaceEmpty"/>
        /// </summary>
        [TestMethod()]
        public void TestEmptyInterfaceExists()
        {
            _parsedDLL.interfaceObjList.RemoveAll( iface => iface.TypeObj.FullName != "NoEmptyInterface.IInterfaceEmpty" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            Analyzer.Pipeline.NoEmptyInterface noEmptyInterface = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = noEmptyInterface.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 0 , result[_parsedDLL.DLLFileName].Verdict );
        }

        /// <summary>
        /// Passes since no empty interface.
        /// <see cref = "NoEmptyInterface.IInterfaceNotEmpty"/>
        /// </summary>
        [TestMethod()]
        public void TestNoEmptyInterfaceExists()
        {
            _parsedDLL.interfaceObjList.RemoveAll( iface => iface.TypeObj.FullName != "NoEmptyInterface.IInterfaceNotEmpty" );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            Analyzer.Pipeline.NoEmptyInterface noEmptyInterface = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = noEmptyInterface.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 1 , result[_parsedDLL.DLLFileName].Verdict );
        }

        /// <summary>
        /// Fails since empty interface exists.
        /// <see cref = "NoEmptyInterface.IEmptyInterfaceInheritEmptyInterface"/>
        /// </summary>
        [TestMethod()]
        public void TestEmptyInterfaceInheritEmptyInterface()
        {
            _parsedDLL.interfaceObjList.RemoveAll( iface => (iface.TypeObj.FullName != "NoEmptyInterface.IEmptyInterfaceInheritEmptyInterface" && 
                                                             iface.TypeObj.FullName != "NoEmptyInterface.IInterfaceEmpty" ));
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            Analyzer.Pipeline.NoEmptyInterface noEmptyInterface = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = noEmptyInterface.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 0 , result[_parsedDLL.DLLFileName].Verdict );
        }

        /// <summary>
        /// Passes since no empty interface.
        /// <see cref = "NoEmptyInterface.IInterfaceInheritNotEmptyInterface"/>
        /// </summary>
        [TestMethod()]
        public void TestInterfaceInheritNotEmptyInterface()
        {
            _parsedDLL.interfaceObjList.RemoveAll( iface => (iface.TypeObj.FullName != "NoEmptyInterface.IInterfaceInheritNotEmptyInterface" && 
                                                             iface.TypeObj.FullName != "NoEmptyInterface.IInterfaceNotEmpty" ));
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            Analyzer.Pipeline.NoEmptyInterface noEmptyInterface = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = noEmptyInterface.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 1 , result[_parsedDLL.DLLFileName].Verdict );
        }

        /// <summary>
        /// Fails since empty interface exists.
        /// <see cref = "NoEmptyInterface.IEmptyInterfaceInheritNonEmptyInterface"/>
        /// </summary>
        [TestMethod()]
        public void TestEmptyInterfaceInheritNonEmptyInterface()
        {
            _parsedDLL.interfaceObjList.RemoveAll( iface => (iface.TypeObj.FullName != "NoEmptyInterface.IEmptyInterfaceInheritNonEmptyInterface" &&
                                                             iface.TypeObj.FullName != "NoEmptyInterface.IInterfaceNotEmpty") );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            Analyzer.Pipeline.NoEmptyInterface noEmptyInterface = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = noEmptyInterface.AnalyzeAllDLLs();

            Console.WriteLine( result[_parsedDLL.DLLFileName].ErrorMessage );
            Assert.AreEqual( 0 , result[_parsedDLL.DLLFileName].Verdict );
        }

        /// <summary>
        /// Fails since empty interface exists.
        /// <see cref = "NoEmptyInterface.IInterfaceInheritEmptyInterface"/>
        /// </summary>
        [TestMethod()]
        public void TestInterfaceInheritEmptyInterface()
        {
            _parsedDLL.interfaceObjList.RemoveAll( iface => (iface.TypeObj.FullName != "NoEmptyInterface.IInterfaceInheritEmptyInterface" &&
                                                             iface.TypeObj.FullName != "NoEmptyInterface.IInterfaceEmpty") );
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            Analyzer.Pipeline.NoEmptyInterface noEmptyInterface = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> result = noEmptyInterface.AnalyzeAllDLLs();

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

            Analyzer.Pipeline.NoEmptyInterface noEmptyInterface = new( parseddllFiles );
            Assert.ThrowsException<NullReferenceException>( () => noEmptyInterface.AnalyzeAllDLLs() );
        }
    }
}
