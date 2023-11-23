using Analyzer.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;


namespace AnalyzerTests.Parsing
{
    [TestClass]
    public class TestParsingInterfaceReflection
    {
        readonly ParsedInterface IDrawingView_Bridge = new(typeof(TestParsingInterface_BridgePattern.IDrawingView));
        readonly ParsedInterface IApp_Demo = new(typeof(TestParsingInterface_DemoProject.IApp));
        readonly ParsedInterface IApp1_Demo = new(typeof(TestParsingInterface_DemoProject.IApp1));
        readonly ParsedInterface IApp2_Demo = new(typeof(TestParsingInterface_DemoProject.IApp2));



        [TestMethod]
        public void CheckBasicFields()
        {
            Assert.AreEqual(typeof(TestParsingInterface_BridgePattern.IDrawingView), IDrawingView_Bridge.TypeObj);
            Assert.AreEqual("IDrawingView", IDrawingView_Bridge.Name);
        }


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