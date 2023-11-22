//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Analyzer.Parsing;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace AnalyzerTests.Parsing
//{
//    internal class TestParsingClassReflection
//    {
//        public class TestParsingDLL
//        {
//            static readonly List<string> dllFilesList1 = new List<string>() { "..\\..\\..\\..\\Analyzer\\TestDLLs\\BridgePattern.dll" };
//            static readonly ParsedDLLFile parsedDLLs1 = new( dllFilesList1 );

//            // Other dlls to check interfaces of parent class are coming under interfaces list or not
//            static readonly List<string> dllFilesList2 = new List<string>() { "..\\..\\..\\..\\Analyzer\\TestDLLs\\demo.dll" };
//            static readonly ParsedDLLFile parsedDLLs2 = new( dllFilesList2 );

//            /// <summary>
//            /// Checking whether all the classes were identified and parsed or not 
//            /// </summary>
//            [TestMethod]
//            public void CheckClassNames()
//            {
//                List<ParsedClass> parsedClasses = parsedDLLs1.classObjList;

//                // Checking number of classes
//                Assert.AreEqual( 5 , parsedClasses.Count );

//                // Checking classes retrieved
//                List<string> expectedClassFullNames = new() { "BridgePattern.Shapes" , "BridgePattern.Square" , "BridgePattern.BriefView" , "BridgePattern.DetailedView" , "BridgePattern.Circle" };
//                List<string> expectedClassNames = new() { "Shapes" , "Square" , "BriefView" , "DetailedView" , "Circle" };

//                List<string> retrievedClassFullNames = new();
//                List<string> retrievedClassNames = new();

//                foreach (ParsedClass parsedClass in parsedClasses)
//                {
//                    retrievedClassFullNames.Add( parsedClass.TypeObj.FullName );
//                    retrievedClassNames.Add( parsedClass.Name );
//                }

//                expectedClassFullNames.Sort();
//                retrievedClassFullNames.Sort();

//                expectedClassNames.Sort();
//                retrievedClassNames.Sort();

//                CollectionAssert.AreEqual( expectedClassFullNames , retrievedClassFullNames );
//                CollectionAssert.AreEqual( expectedClassNames , retrievedClassNames );
//            }


//            /// <summary>
//            /// 1. Checking whether all the classes were identified and parsed or not
//            /// 2. Checking whether interfaces implemented by a class or interface were parsed correctly into parsedclass & parsedinterface objects or not
//            /// </summary>
//            [TestMethod]
//            public void CheckInterfacesImplemented()
//            {
//                List<ParsedInterface> parsedInterfaces = parsedDLLs1.interfaceObjList;
//                List<ParsedClass> parsedClasses = parsedDLLs1.classObjList;

//                Assert.AreEqual( parsedInterfaces.Count , 1 );
//                Assert.AreEqual( parsedInterfaces[0].Name , "IDrawingView" );
//                Assert.AreEqual( parsedInterfaces[0].ParentInterfaces.Length , 0 );

//                // Checking "Interfaces" property of parsed class object
//                foreach (ParsedClass parsedClass in parsedClasses)
//                {
//                    if (parsedClass.Name == "BriefView" || parsedClass.Name == "DetailedView")
//                    {
//                        Assert.AreEqual( parsedClass.Interfaces.Length , 1 );
//                        Assert.AreEqual( parsedClass.Interfaces[0].Name , "IDrawingView" );
//                    }
//                    else
//                    {
//                        Assert.AreEqual( parsedClass.Interfaces.Length , 0 );
//                    }
//                }

//                // Dll file2 tests
//                List<ParsedClass> otherParsedClasses = parsedDLLs2.classObjList;
//                List<ParsedInterface> otherParsedInterfaces = parsedDLLs2.interfaceObjList;

//                Assert.AreEqual( otherParsedInterfaces.Count , 2 );
//                otherParsedInterfaces.Sort( ( x , y ) => x.Name.CompareTo( y.Name ) );

//                List<string> interfaceNames = new() { otherParsedInterfaces[0].Name , otherParsedInterfaces[1].Name };
//                CollectionAssert.AreEqual( interfaceNames , new List<string>() { "IApp" , "ISample" } );


//                // Checking "ParentInterfaces" property of an parsedinterface object
//                Assert.AreEqual( otherParsedInterfaces[0].ParentInterfaces.Length , 1 );
//                Assert.AreEqual( otherParsedInterfaces[0].ParentInterfaces[0].Name , "ISample" );
//                Assert.AreEqual( otherParsedInterfaces[1].ParentInterfaces.Length , 0 );


//                // Checking "Interfaces" field of parsed class object when the interface implemented by it also have a parent interface
//                otherParsedClasses.Sort( ( x , y ) => x.Name.CompareTo( y.Name ) );

//                List<string> classesNames = new();

//                foreach (ParsedClass parsedClass in otherParsedClasses)
//                {
//                    classesNames.Add( parsedClass.Name );
//                }

//                CollectionAssert.AreEqual( classesNames , new List<string>() { "App1" , "Cleanup" , "Cleanup1" , "Program" , "SampleClass" , "StartProgram" , "UploadFiles" } );

//                // App1 implements IApp & IApp implements ISample => But we need only App1 interfaces shown as IApp for graph purpose
//                Assert.AreEqual( otherParsedClasses[0].Interfaces.Length , 1 );
//                Assert.AreEqual( otherParsedClasses[0].Interfaces[0].Name , "IApp" );

//                // Cleanup inherits from Cleanup1 & Cleanup1 implements ISample => But we need only Cleanup interfaces shown as Empty for graph purpose
//                Assert.AreEqual( otherParsedClasses[1].Interfaces.Length , 0 );

//                // CleanUp1 
//                Assert.AreEqual( otherParsedClasses[2].Interfaces.Length , 1 );
//                Assert.AreEqual( otherParsedClasses[2].Interfaces[0].Name , "ISample" );

//                // Program , SampleClass , StartProgram , UploadFiles
//                Assert.AreEqual( otherParsedClasses[3].Interfaces.Length , 0 );
//                Assert.AreEqual( otherParsedClasses[4].Interfaces.Length , 0 );
//                Assert.AreEqual( otherParsedClasses[5].Interfaces.Length , 0 );
//                Assert.AreEqual( otherParsedClasses[6].Interfaces.Length , 0 );
//            }

//            //[TestMethod]
//            //public void Check
//        }
//    }
//}


//namespace TestParsingDLL1
//{
//    public interface IDrawingView
//    {
//        List<string> DisplaySquare( double length );
//        List<string> DisplayCircle( double radius );
//    }


//    public class BriefView : IDrawingView
//    {
//        public List<string> DisplayCircle( double radius )
//        {
//            List<string> result = new() { "Brief View of Circle" , $"Drawing Circle with radius {radius} units" };
//            return result;
//        }

//        public List<string> DisplaySquare( double length )
//        {
//            List<string> result = new() { "Brief View of Square" , $"Drawing Square with length {length} units" };
//            return result;
//        }
//    }


//    public class DetailedView : IDrawingView
//    {
//        public List<string> DisplayCircle( double radius )
//        {
//            List<string> result = new() { "Detailed View of Circle" };

//            double area = Math.PI * Math.Pow( radius , 2 );
//            double perimeter = 2 * Math.PI * radius;
//            result.Add( $"Drawing Circle with radius {radius} units" );
//            result.Add( $"Area of Circle = {area} sq.units" );
//            result.Add( $"Perimeter of Circle = {perimeter} units" );
//            return result;
//        }


//        public List<string> DisplaySquare( double length )
//        {
//            List<string> result = new() { "Detailed View of Square" };

//            double area = Math.Pow( length , 2 );
//            double perimeter = 4 * length;
//            result.Add( $"Drawing Square with length {length} units" );
//            result.Add( $"Area of Square = {area} sq.units" );
//            result.Add( $"Perimeter of Square = {perimeter} units" );
//            return result;
//        }
//    }


//    public abstract class Shapes
//    {
//        protected IDrawingView view;
//        public Shapes( IDrawingView view )
//        {
//            this.view = view;
//        }
//        public abstract List<string> Display();
//    }


//    public class Circle : Shapes
//    {
//        private readonly double _radius;

//        public Circle( IDrawingView _view , double radius ) : base( _view )
//        {
//            if (radius > 0)
//            {
//                _radius = radius;
//            }
//            else
//            {
//                throw new ArgumentException( "Invalid radius of circle" );
//            }
//        }

//        public Circle( double radius ) : base( new DetailedView() )
//        {
//            if (radius > 0)
//            {
//                _radius = radius;
//            }
//            else
//            {
//                throw new ArgumentException( "Invalid radius of circle" );
//            }
//        }

//        public override List<string> Display()
//        {
//            List<string> result;

//            result = base.view.DisplayCircle( _radius );
//            foreach (string str in result)
//            {
//                Console.WriteLine( str );
//            }
//            return result;
//        }
//    }


//    public class Square : Shapes
//    {
//        private readonly double _length;

//        public Square( IDrawingView _view , double length ) : base( _view )
//        {
//            if (length > 0)
//            {
//                _length = length;
//            }
//            else
//            {
//                throw new ArgumentException( "Invalid length of square" );
//            }
//        }

//        public Square( double length ) : base( new DetailedView() )
//        {
//            if (length > 0)
//            {
//                _length = length;
//            }
//            else
//            {
//                throw new ArgumentException( "Invalid length of square" );
//            }
//        }

//        public override List<string> Display()
//        {
//            List<string> result;
//            result = base.view.DisplaySquare( _length );

//            foreach (string str in result)
//            {
//                Console.WriteLine( str );
//            }
//            return result;
//        }
//    }
//}
