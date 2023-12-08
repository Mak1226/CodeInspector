/******************************************************************************
* Filename    = TestParsingClassReflection.cs
* 
* Author      = Nikhitha Atyam
* 
* Product     = Analyzer
* 
* Project     = AnalyzerTests
*
* Description = UnitTests for Analyzer.Parsing.ParsedClass.cs 
*               (checks the parsed members from the class object using System.Reflection)
*****************************************************************************/

using Analyzer.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;


namespace AnalyzerTests.Parsing
{
    /// <summary>
    /// Checks whether parsed attributes of class object are per requirements 
    /// so that other modules like analyzers and class diagram work correctly
    /// </summary>
    [TestClass]
    public class TestParsingClassReflection
    {
        // parsed class objects required for checking the correctness of parsed attributes of class object
        public readonly ParsedClass squareClass_Bridge = new(typeof(TestParsingClass_BridgePattern.Square));
        public readonly ParsedClass shapesClass_Bridge = new(typeof(TestParsingClass_BridgePattern.Shapes));
        public readonly ParsedClass briefViewClass_Bridge = new(typeof(TestParsingClass_BridgePattern.BriefView));

        public readonly ParsedClass App1Class_Demo = new(typeof(TestParsingClass_DemoProject.App1));
        public readonly ParsedClass App2Class_Demo = new(typeof(TestParsingClass_DemoProject.App2));
        public readonly ParsedClass App3Class_Demo = new(typeof(TestParsingClass_DemoProject.App3));
        public readonly ParsedClass App4Class_Demo = new(typeof(TestParsingClass_DemoProject.App4));
        public readonly ParsedClass App5Class_Demo = new(typeof(TestParsingClass_DemoProject.App5));
        public readonly ParsedClass App6Class_Demo = new(typeof(TestParsingClass_DemoProject.App6));
        public readonly ParsedClass App7Class_Demo = new(typeof(TestParsingClass_DemoProject.App7));

        public readonly ParsedClass SampleClass1_Demo2 = new(typeof(TestParsingClass_DemoProject2.SampleClass1));
        public readonly ParsedClass SampleClass2_Demo2 = new(typeof(TestParsingClass_DemoProject2.SampleClass2));
        public readonly ParsedClass SampleClass3_Demo2 = new(typeof(TestParsingClass_DemoProject2.SampleClass3));
        public readonly ParsedClass SampleClass4_Demo2 = new(typeof(TestParsingClass_DemoProject2.SampleClass4));
        public readonly ParsedClass SampleClass5_Demo2 = new(typeof(TestParsingClass_DemoProject2.SampleClass5));
        public readonly ParsedClass CalculatorClass_Demo2 = new(typeof(TestParsingClass_DemoProject2.Calculator_OverloadCase));


        /// <summary>
        /// Checks the basic attributes parsed of parsedClass
        /// </summary>
        [TestMethod]
        public void CheckBasicAttributes()
        {
            // TypeObj and Name Check
            Assert.AreEqual(typeof(TestParsingClass_BridgePattern.Square), squareClass_Bridge.TypeObj);
            Assert.AreEqual("Square", squareClass_Bridge.Name);
        }


        /// <summary>
        /// Test for parent class
        /// </summary>
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


        /// <summary>
        /// Tests for constructors (default constructors should also be checked)
        /// </summary>
        [TestMethod]
        public void CheckConstructors()
        {
            // Even if constructor is not explicitly declared, there will be default constructor
            Assert.AreEqual(1, briefViewClass_Bridge.Constructors.Length);

            // Explicit declaration of constructor
            Assert.AreEqual(2, squareClass_Bridge.Constructors.Length);
        }


        /// <summary>
        /// Tests for interfaces implemented by a class
        /// Interfaces list should be following requirements as per class diagram in different cases
        /// </summary>
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
            CollectionAssert.AreEquivalent( new Type[1] { typeof(TestParsingClass_DemoProject.IApp2) } , 
                                            App6Class_Demo.Interfaces);

            // App7: IApp, IApp2  - Normal Multiple interfaces implementation case
            Assert.AreEqual(2, App7Class_Demo.Interfaces.Length);
        }


        /// <summary>
        /// Tests for methods declared only in the class
        /// </summary>
        [TestMethod]
        public void CheckMethods()
        {
            // General Case: Counting number of declared only methods with no inheritance / interface implementation
            Assert.AreEqual(3, SampleClass1_Demo2.Methods.Length);

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


        /// <summary>
        /// Tests for fields declared only in the class
        /// </summary>
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
            Assert.AreEqual(0, App1Class_Demo.Fields.Length);

            // Will change according to implementation of fields followed
            // i.e. Allowing/Disallowing of Auto Implemented Properties
            Assert.AreEqual(1, briefViewClass_Bridge.Fields.Length);

            // public + static  : here inherited class fields should not come (i.e. sampleValue21(public) from sampleClass2)
            CollectionAssert.AreEquivalent(new FieldInfo[2] {
                                                                typeof(TestParsingClass_DemoProject2.SampleClass3).GetField("sampleValue1"),
                                                                typeof(TestParsingClass_DemoProject2.SampleClass3).GetField("sampleValue2")
                                                            },
                                           SampleClass3_Demo2.Fields);
        }


        /// <summary>
        /// Tests for properties declared only in the class
        /// </summary>
        [TestMethod]
        public void CheckProperties()
        {
            // protected/public/private + instance (auto + explicit)
            Assert.AreEqual(4, SampleClass1_Demo2.Properties.Length);
            CollectionAssert.AreEquivalent(new PropertyInfo[4] {
                                                                   typeof(TestParsingClass_DemoProject2.SampleClass1).GetProperty("SampleX"),
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

        

        /// <summary>
        /// Checking events
        /// </summary>
        [TestMethod]
        public void CheckEvents()
        {
            ParsedClass eventClass2_Demo = new( typeof( EventExample.TemperatureChangedEventArgs ) );
            ParsedClass eventClass3_Demo = new( typeof( EventExample.TemperatureSensor ) );
            
            Assert.AreEqual(0, eventClass2_Demo.TypeObj.GetEvents().Length);
            Assert.AreEqual(1, eventClass3_Demo.TypeObj.GetEvents().Length );
        }

        //// can be removed later
        //[TestMethod]
        //public void NoUseSampleMethod()
        //{
        //Console.WriteLine( SampleClass1_Demo2.Properties[0].Name );
        //Console.WriteLine( SampleClass1_Demo2.Properties[0].GetType().Name );
        //Console.WriteLine( SampleClass1_Demo2.Methods[0].Name );
        //Console.WriteLine( SampleClass1_Demo2.Methods[0].IsSpecialName );

        //Console.WriteLine( SampleClass4_Demo2.Properties[0].Name );
        //Console.WriteLine( SampleClass4_Demo2.Properties[0].GetType().Name );

        //Console.WriteLine( "=============" );
        //foreach (MemberInfo member in briefViewClass_Bridge.TypeObj.GetMembers())
        //{
        //    Console.WriteLine( member.Name );
        //    Console.WriteLine( member.MemberType );
        //}

        //string dashboardDLL = "C:\\Users\\nikhi\\source\\repos\\nikhi9603\\Analyzer\\Dashboard\\bin\\Debug\\net6.0-windows\\Dashboard.dll";
        //ParsedDLLFile parsedDLLFile = new(dashboardDLL);
        //Console.WriteLine(parsedDLLFile.classObjList.Count);

        //foreach (ParsedClass parsedClass in parsedDLLFile.classObjList)
        //{
        //    Console.WriteLine(parsedClass.Name);
        //    Console.WriteLine( parsedClass.TypeObj.GetEvents().Length );
        //}


        //ParsedClass eventClass1_Demo = new( typeof( EventExample.Program ) );
        //ParsedClass eventClass2_Demo = new( typeof( EventExample.TemperatureChangedEventArgs ) );
        //ParsedClass eventClass3_Demo = new( typeof( EventExample.TemperatureSensor ) );
        //List<ParsedClass> parsedClassesList = new() { eventClass1_Demo, eventClass2_Demo, eventClass3_Demo, eventClass4_Demo};
        //foreach (ParsedClass parsedClass in parsedClassesList)
        //{
        //    Console.WriteLine( parsedClass.Name );
        //    Console.WriteLine( parsedClass.TypeObj.GetEvents().Length );

        //    foreach(EventInfo e in parsedClass.TypeObj.GetEvents())
        //    {
        //        Console.WriteLine(e.Name);
        //    }
        //}
        //}
    }
}


// TEST CASE NAMESPACES FOR TESTING PARSEDCLASS
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
        private readonly string _name;

        public int x;
        public int SampleX
        {
            get { return x; } 
            set { x = value; }
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




namespace EventExample
{
    class TemperatureChangedEventArgs : EventArgs
    {
        public int NewTemperature { get; }

        public TemperatureChangedEventArgs( int newTemperature )
        {
            NewTemperature = newTemperature;
        }
    }

    class TemperatureSensor
    {
        private int _temperature;

        // Define the event using the EventHandler delegate
        public event EventHandler<TemperatureChangedEventArgs> TemperatureChanged;

        public int Temperature
        {
            get { return _temperature; }
            set
            {
                if (value != _temperature)
                {
                    _temperature = value;
                    OnTemperatureChanged( new TemperatureChangedEventArgs( value ) );
                }
            }
        }

        // Raise the TemperatureChanged event
        protected virtual void OnTemperatureChanged( TemperatureChangedEventArgs e )
        {
            TemperatureChanged?.Invoke( this , e );
        }
    }

    class Program
    {
        static void Main1()
        {
            TemperatureSensor sensor = new();

            // Subscribe to the event
            sensor.TemperatureChanged += TemperatureChangedHandler;

            // Change the temperature, triggering the event
            sensor.Temperature = 25;

            // Unsubscribe from the event
            sensor.TemperatureChanged -= TemperatureChangedHandler;

            // Change the temperature again, but this time no event is triggered
            sensor.Temperature = 30;
        }

        // Event handler method
        private static void TemperatureChangedHandler( object sender , TemperatureChangedEventArgs e )
        {
            Console.WriteLine( $"Temperature changed: {e.NewTemperature} degrees Celsius" );
        }
    }
}
