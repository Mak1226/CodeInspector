/******************************************************************************
* Filename    = TestParsingInterfaceReflection.cs
* 
* Author      = Nikhitha Atyam
* 
* Product     = Analyzer
* 
* Project     = AnalyzerTests
*
* Description = UnitTests for Analyzer.Parsing.ParsedInterface.cs 
*               (checks the parsed members from the interface object using System.Reflection)
*****************************************************************************/

using Analyzer.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;


namespace AnalyzerTests.Parsing
{
    /// <summary>
    /// Checks whether parsed attributes of interface object are per requirements 
    /// so that other modules like analyzers and class diagram work correctly
    /// </summary>
    [TestClass]
    public class TestParsingInterfaceReflection
    {
        // parsed interface objects required for checking the correctness of parsed attributes of interface object
        public readonly ParsedInterface IDrawingView_Bridge = new(typeof(TestParsingInterface_BridgePattern.IDrawingView));
        public readonly ParsedInterface IApp_Demo = new(typeof(TestParsingInterface_DemoProject.IApp));
        public readonly ParsedInterface IApp1_Demo = new(typeof(TestParsingInterface_DemoProject.IApp1));
        public readonly ParsedInterface IApp2_Demo = new(typeof(TestParsingInterface_DemoProject.IApp2));


        /// <summary>
        /// Checks the basic attributes parsed of parsedInterface
        /// </summary>
        [TestMethod]
        public void CheckBasicFields()
        {
            // TypeObj and Name Check
            Assert.AreEqual(typeof(TestParsingInterface_BridgePattern.IDrawingView), IDrawingView_Bridge.TypeObj);
            Assert.AreEqual("IDrawingView", IDrawingView_Bridge.Name);
        }


        /// <summary>
        /// Tests for methods declared only in the interface
        /// </summary>
        [TestMethod]
        public void CheckMethods()
        {
            // General Case
            Assert.AreEqual(1, IApp_Demo.Methods.Length);
            Assert.AreEqual(2, IDrawingView_Bridge.Methods.Length);
            CollectionAssert.AreEquivalent(new MethodInfo[2] {
                                                                typeof(TestParsingInterface_BridgePattern.IDrawingView).GetMethod("DisplaySquare"),
                                                                typeof(TestParsingInterface_BridgePattern.IDrawingView).GetMethod("DisplayCircle"),
                                                             } , 
                                           IDrawingView_Bridge.Methods);

            // Implementing another interface case
            Assert.AreEqual(1, IApp1_Demo.Methods.Length);
            CollectionAssert.AreEquivalent(new MethodInfo[1] { typeof(TestParsingInterface_DemoProject.IApp1).GetMethod("Sample1_Func1") },
                                           IApp1_Demo.Methods);

            // Inheritance + no methods
            Assert.AreEqual(0, IApp2_Demo.Methods.Length);
        }


        /// <summary>
        /// Tests for interfaces implemented by a interface
        /// Interfaces list should be following requirements as per class diagram in different cases
        /// </summary>
        [TestMethod]
        public void CheckInterfacesImplemented()
        {
            // No inheritance
            Assert.AreEqual(0, IDrawingView_Bridge.ParentInterfaces.Length);

            // Single inheritance
            CollectionAssert.AreEquivalent(new Type[1] { typeof(TestParsingInterface_DemoProject.IApp) }, 
                                           IApp1_Demo.ParentInterfaces);

            // IApp2: IApp, IApp1, IApp3     IApp1: IApp
            CollectionAssert.AreEquivalent(new Type[2] {
                                                            typeof(TestParsingInterface_DemoProject.IApp1) ,
                                                            typeof(TestParsingInterface_DemoProject.IApp3)
                                                        },
                                           IApp2_Demo.ParentInterfaces);
        }
    }
}


// TEST CASE NAMESPACES FOR TESTING PARSEDINTERFACE
namespace TestParsingInterface_BridgePattern
{
    public interface IDrawingView
    {
        List<string> DisplaySquare(double length);
        List<string> DisplayCircle(double radius);
    }
}


namespace TestParsingInterface_DemoProject
{
    public interface IApp
    {
        public void Sample_Func1();
    }

    public interface IApp1 : IApp
    {
        public void Sample1_Func1();
    }

    public interface IApp2 : IApp1, IApp, IApp3
    {
    }

    public interface IApp3
    { 
    }
}
