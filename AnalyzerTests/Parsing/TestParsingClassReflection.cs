using System;
using System.Collections.Generic;
using Analyzer.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;


namespace AnalyzerTests.Parsing
{
    [TestClass]
    public class TestParsingClassReflection
    {
        readonly ParsedClass squareClass_Bridge = new(typeof(TestParsingClass_BridgePattern.Square));
        readonly ParsedClass shapesClass_Bridge = new(typeof(TestParsingClass_BridgePattern.Shapes));
        readonly ParsedClass briefViewClass_Bridge = new(typeof(TestParsingClass_BridgePattern.BriefView));

        readonly ParsedClass App1Class_Demo = new(typeof(TestParsingClass_DemoProject.App1));
        readonly ParsedClass App2Class_Demo = new(typeof(TestParsingClass_DemoProject.App2));
        readonly ParsedClass App3Class_Demo = new(typeof(TestParsingClass_DemoProject.App3));
        readonly ParsedClass App4Class_Demo = new(typeof(TestParsingClass_DemoProject.App4));
        readonly ParsedClass App5Class_Demo = new(typeof(TestParsingClass_DemoProject.App5));
        readonly ParsedClass App6Class_Demo = new(typeof(TestParsingClass_DemoProject.App6));
        readonly ParsedClass App7Class_Demo = new(typeof(TestParsingClass_DemoProject.App7));

        readonly ParsedClass SampleClass1_Demo2 = new(typeof(TestParsingClass_DemoProject2.SampleClass1));
        readonly ParsedClass SampleClass2_Demo2 = new(typeof(TestParsingClass_DemoProject2.SampleClass2));
        readonly ParsedClass SampleClass3_Demo2 = new(typeof(TestParsingClass_DemoProject2.SampleClass3));
        readonly ParsedClass SampleClass4_Demo2 = new(typeof(TestParsingClass_DemoProject2.SampleClass4));
        readonly ParsedClass SampleClass5_Demo2 = new(typeof(TestParsingClass_DemoProject2.SampleClass5));
        readonly ParsedClass CalculatorClass_Demo2 = new(typeof(TestParsingClass_DemoProject2.Calculator_OverloadCase));


        [TestMethod]
        public void CheckBasicFields()
        {
            Assert.AreEqual(typeof(TestParsingClass_BridgePattern.Square), squareClass_Bridge.TypeObj);
            Assert.AreEqual("Square", squareClass_Bridge.Name);
        }


        [TestMethod]
        public void CheckParentClass()
        {
            // no parent class
            Assert.AreEqual(null, shapesClass_Bridge.ParentClass);

            // inheritance
            Assert.AreEqual(typeof(TestParsingClass_BridgePattern.Shapes), squareClass_Bridge.ParentClass);

            // parent class having namespace null
            Assert.AreEqual(typeof(SampleGlobalClass), SampleClass5_Demo2.ParentClass);
        }


        [TestMethod]
        public void CheckConstructors()
        {
            // Even if constructor is not explicitly declared, there will be default constructor
            Assert.AreEqual(1, briefViewClass_Bridge.Constructors.Length);

            // Explicit declaration of constructor
            Assert.AreEqual(2, squareClass_Bridge.Constructors.Length);
        }


        [TestMethod]
        public void CheckInterfacesImplemented()
        {
            // Case: No interface implementation by the class
            Assert.AreEqual(0, squareClass_Bridge.Interfaces.Length);

            // Case: Single Direct Interface implementation
            Assert.AreEqual(1, briefViewClass_Bridge.Interfaces.Length);
            Assert.AreEqual(1, App3Class_Demo.Interfaces.Length);
            Assert.AreEqual(typeof(TestParsingClass_BridgePattern.IDrawingView), briefViewClass_Bridge.Interfaces[0]);

            // Case: No interface implementation by the class but parent class implementing interface
            // App4: App3, App3: IApp => App4 will not be shown that IApp is being implemented by it in class Diagram
            // This kind of relationships may be helpful in reducing the size of class diagram
            Assert.AreEqual(0, App4Class_Demo.Interfaces.Length);

            // Similar to above case but here an interface implemented by class is implementing one more interface rather than parent class
            // App1: IApp1  ,  IApp1: IApp
            Assert.AreEqual(1, App1Class_Demo.Interfaces.Length);
            Assert.AreEqual(typeof(TestParsingClass_DemoProject.IApp1), App1Class_Demo.Interfaces[0]);

            // App2: IApp2,IApp1  ,  IApp1: IApp 
            Assert.AreEqual(2, App2Class_Demo.Interfaces.Length);
            CollectionAssert.AreEquivalent(new Type[2] { typeof(TestParsingClass_DemoProject.IApp1), typeof(TestParsingClass_DemoProject.IApp2) } ,
                                           App2Class_Demo.Interfaces);

            // App5: App4,    App4: App3,  App3: IApp
            Assert.AreEqual(0, App5Class_Demo.Interfaces.Length);

            // App6: App3,IApp2,IApp  , App3: IApp  (class , interface combination)
            Assert.AreEqual(1, App6Class_Demo.Interfaces.Length);
            Assert.AreEqual(typeof(TestParsingClass_DemoProject.IApp2), App6Class_Demo.Interfaces[0]);

            // App7: IApp, IApp2  - Normal Multiple interfaces implementation case
            Assert.AreEqual(2, App7Class_Demo.Interfaces.Length);
        }


        [TestMethod]
        public void CheckMethods()
        {
            // General Case: Counting number of declared only methods with no inheritance / interface implementation
            Assert.AreEqual(11, SampleClass1_Demo2.Methods.Length);

            // Case: Class when implementing interface
            Assert.AreEqual(3, SampleClass2_Demo2.Methods.Length);

            // Case: Class when inheriting another class
            Assert.AreEqual(1, SampleClass3_Demo2.Methods.Length);
            CollectionAssert.AreEquivalent(new MethodInfo[1] { typeof(TestParsingClass_DemoProject2.SampleClass3).GetMethod("Sample_C3_Func1", BindingFlags.NonPublic | BindingFlags.Instance) },
                                           SampleClass3_Demo2.Methods);

            // Overloading Case 
            Assert.AreEqual(3, CalculatorClass_Demo2.Methods.Length);
            CollectionAssert.AreEquivalent(new MethodInfo[3] { 
                                                               typeof(TestParsingClass_DemoProject2.Calculator_OverloadCase).GetMethod( "Add", new Type[] {typeof(int), typeof(int)} ) ,
                                                               typeof(TestParsingClass_DemoProject2.Calculator_OverloadCase).GetMethod( "Add", new Type[] {typeof(int), typeof(int), typeof(int)} ) ,
                                                               typeof(TestParsingClass_DemoProject2.Calculator_OverloadCase).GetMethod( "Add", new Type[] {typeof(double) , typeof(double)} ) 
                                                             },
                                           CalculatorClass_Demo2.Methods);



            // Overriding Case
            Assert.AreEqual(1, squareClass_Bridge.Methods.Length);
            Assert.AreEqual(typeof(TestParsingClass_BridgePattern.Square).GetMethod("Display"), squareClass_Bridge.Methods[0]);

            // No method case
            Assert.AreEqual(0, App4Class_Demo.Methods.Length);
        }


        [TestMethod]
        public void CheckFields()
        {
            // General case fields - public, protected, private
            // protected + instance
            CollectionAssert.AreEquivalent(new FieldInfo[1] { typeof(TestParsingClass_BridgePattern.Shapes).GetField("view", BindingFlags.NonPublic | BindingFlags.Instance) } ,
                                      shapesClass_Bridge.Fields);

            // private + instance  : here inherited class fields should not come (i.e. view(protected) from shapes)
            CollectionAssert.AreEquivalent(new FieldInfo[1] { typeof(TestParsingClass_BridgePattern.Square).GetField("_length", BindingFlags.NonPublic | BindingFlags.Instance) },
                                      squareClass_Bridge.Fields);

            // no field 
            Assert.AreEqual(0, briefViewClass_Bridge.Fields.Length);


            // public + static  : here inherited class fields should not come (i.e. sampleValue21(public) from sampleClass2)
            CollectionAssert.AreEquivalent(new FieldInfo[2] {
                                                                typeof(TestParsingClass_DemoProject2.SampleClass3).GetField("sampleValue1"),
                                                                typeof(TestParsingClass_DemoProject2.SampleClass3).GetField("sampleValue2")
                                                            },
                                           SampleClass3_Demo2.Fields);
        }


        [TestMethod]
        public void CheckProperties()
        {
            // protected/public/private + instance (auto + explicit)
            Assert.AreEqual(4, SampleClass1_Demo2.Properties.Length);
            CollectionAssert.AreEquivalent(new PropertyInfo[4] {
                                                                   typeof(TestParsingClass_DemoProject2.SampleClass1).GetProperty("Name"),
                                                                   typeof(TestParsingClass_DemoProject2.SampleClass1).GetProperty("SampleProp1"),
                                                                   typeof(TestParsingClass_DemoProject2.SampleClass1).GetProperty("SampleProp2", BindingFlags.NonPublic | BindingFlags.Instance),
                                                                   typeof(TestParsingClass_DemoProject2.SampleClass1).GetProperty("SampleProp3", BindingFlags.NonPublic | BindingFlags.Instance)
                                                               },
                                           SampleClass1_Demo2.Properties);

            // public + static + (checking properties while inheritance according to the needs)
            Assert.AreEqual(1, SampleClass4_Demo2.Properties.Length);
            CollectionAssert.AreEquivalent(new PropertyInfo[1] { typeof(TestParsingClass_DemoProject2.SampleClass4).GetProperty("sampleProp41") },
                                           SampleClass4_Demo2.Properties);

            // no property case
            Assert.AreEqual(0, SampleClass3_Demo2.Properties.Length);
        }
    }
}



namespace TestParsingClass_BridgePattern
{
    public interface IDrawingView
    {
        List<string> DisplaySquare(double length);
        List<string> DisplayCircle(double radius);
    }

    public class BriefView : IDrawingView
    {
        public string SProp { get; set; } 

        public string SProp1
        {
            get { return SProp; }
            set { SProp = value; }
        }
        public List<string> DisplayCircle(double radius)
        {
            List<string> result = new();
            return result;
        }

        public List<string> DisplaySquare(double length)
        {
            List<string> result = new();
            return result;
        }
    }


    public abstract class Shapes
    {
        protected IDrawingView view;
        public Shapes(IDrawingView view)
        {
            this.view = view;
        }
        public abstract List<string> Display();
    }


    public class Square : Shapes
    {
        private readonly double _length;

        public Square(IDrawingView _view, double length) : base(_view)
        {
        }

        public Square(double length) : base(new BriefView())
        {
        }

        public override List<string> Display()
        {
            List<string> result = new();
            return result;
        }
    }
}


namespace TestParsingClass_DemoProject
{
    public interface IApp
    {
    }

    public interface IApp1 : IApp
    { 
    }

    public interface IApp2
    { 
    }

    public class App1 : IApp1
    { 
    }

    public class App2 : IApp2, IApp1
    {
    }

    public class App3 : IApp
    {
    }

    public class App4 : App3
    { 
    }

    public class App5 : App4
    { 
    }

    public class App6 : App3, IApp2, IApp
    { 
    }

    public class App7 : IApp, IApp2
    { 
    }
}


namespace TestParsingClass_DemoProject2
{
    public interface ISample
    {
        public void Sample_IFunc1();
        public void Sample_IFunc2();
    }

    public class SampleClass1
    {
        private string _name;

        public string Name
        {
            get { return _name; } 
            set { _name = value; }
        }

        public string SampleProp1 { get; set; }
        private string SampleProp2 { get; set; }
        protected string SampleProp3 { get; set; }


        public void Sample_C1_Func1()
        { 
        }
        protected void Sample_C1_Func2()
        { 
        }

        private void Sample_C1_Func3()
        { 
        }
    }

    public class SampleClass2 : ISample
    {
        public string sampleValue21;

        public void Sample_IFunc1()
        {
        }

        public void Sample_IFunc2()
        {
        }

        public void Sample_C2_Func1()
        {
        }
    }

    public class SampleClass3 : SampleClass2
    {
        public static int sampleValue1;
        public string sampleValue2;

        private void Sample_C3_Func1()
        { 
        }
    }

    public class Calculator_OverloadCase
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Add(int a, int b, int c)
        {
            return a + b + c;
        }

        public double Add(double a, double b)
        {
            return a + b;
        }
    }

    public class SampleClass4 : SampleClass1
    {
        public string sampleProp41 { get; private set; }
    }


    public class SampleClass5 : SampleGlobalClass
    { 
    }
}



public class SampleGlobalClass
{
}
