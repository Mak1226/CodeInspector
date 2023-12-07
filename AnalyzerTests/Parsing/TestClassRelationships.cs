/******************************************************************************
* Filename    = TestClassRelationships.cs
* 
* Author      = Yukta Salunkhe
* 
* Project     = Analyzer
*
* Description =  Test class to verify the functionality of retrieving class relationships from given dll file.
*****************************************************************************/
using Analyzer.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Analyzer;
using System.Drawing;

namespace AnalyzerTests.Pipeline
{
    /// <summary>
    /// Test class to verify the functionality of retrieving class relationships from given dll file.
    /// </summary>
    [TestClass()]
    public class TestClassRelationships
    {
        /// <summary>
        /// Testing all 4 types of relationships existing between classes of TypeRelationships.dl
        /// </summary>
        [TestMethod()]
        public void CheckRelationshipsList()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\TypeRelationships.dll";
            var parsedDllObj = new ParsedDLLFile( path );
            DllFileObjs.Add( parsedDllObj );

            int classes = 0;
            foreach (ParsedDLLFile dllFileObj in DllFileObjs)
            {
                foreach (ParsedClassMonoCecil cls in dllFileObj.classObjListMC)
                {
                    classes++;
                }
            }

            //check diff Relationship Lists
            Assert.AreEqual( 5 , classes );

            Dictionary<string , List<string>> InheritanceRel = new();
            Dictionary<string , List<string>> CompositionRel = new();
            Dictionary<string , List<string>> AggregationRel = new();
            Dictionary<string , List<string>> UsingRel = new();

            foreach (ParsedDLLFile dllFileObj in DllFileObjs)
            {
                foreach (ParsedClassMonoCecil cls in dllFileObj.classObjListMC)
                {
                    foreach (string inhCls in cls.InheritanceList)
                    {
                        if (!InheritanceRel.ContainsKey( cls.Name ))
                        {
                            InheritanceRel[cls.Name] = new List<string>();
                        }
                        InheritanceRel[cls.Name].Add( inhCls );
                    }
                    foreach (string compCls in cls.CompositionList)
                    {
                        if (!CompositionRel.ContainsKey( cls.Name ))
                        {
                            CompositionRel[cls.Name] = new List<string>();
                        }
                        CompositionRel[cls.Name].Add( compCls );
                    }
                    foreach (string aggCls in cls.AggregationList)
                    {
                        if (!AggregationRel.ContainsKey( cls.Name ))
                        {
                            AggregationRel[cls.Name] = new List<string>();
                        }
                        AggregationRel[cls.Name].Add( aggCls );
                    }

                    foreach (string useCls in cls.UsingList)
                    {
                        if (!UsingRel.ContainsKey( cls.Name ))
                        {
                            UsingRel[cls.Name] = new List<string>();
                        }
                        UsingRel[cls.Name].Add( useCls );
                    }
                }
            }

            Dictionary<string , List<string>> InheritanceExp = new();
            Dictionary<string , List<string>> CompositionExp = new();
            Dictionary<string , List<string>> AggregationExp = new();
            Dictionary<string , List<string>> UsingExp = new();

            InheritanceExp["Student"] = new List<string> { "CTypeRelationships.Person" };
            CompositionExp["Car"] = new List<string> { "CTypeRelationships.Engine" };
            AggregationExp["StudentCar"] = new List<string> { "CTypeRelationships.Car" };
            UsingExp["StudentCar"] = new List<string> { "CTypeRelationships.Student" };

            foreach (string key in InheritanceRel.Keys)
            {
                CollectionAssert.AreEqual( InheritanceExp[key] , InheritanceRel[key] );
            }
            foreach (string key in CompositionRel.Keys)
            {
                CollectionAssert.AreEqual( CompositionExp[key] , CompositionRel[key] );
            }
            foreach (string key in AggregationRel.Keys)
            {
                CollectionAssert.AreEqual( AggregationExp[key] , AggregationRel[key] );
            }
            foreach (string key in UsingRel.Keys)
            {
                CollectionAssert.AreEqual( UsingExp[key] , UsingRel[key] );
            }
        }

        /// <summary>
        /// Checking the case where the constructor takes in an object as parameter, but is then assigned to local variable
        /// </summary>
        [TestMethod()]
        public void CheckParameterInCtorCase()
        {
            string dllFile = Assembly.GetExecutingAssembly().Location;

            ParsedDLLFile parsedDLL = new( dllFile );

            foreach (ParsedClassMonoCecil parsedClass in parsedDLL.classObjListMC)
            {
                if ((parsedClass.TypeObj.Namespace == "ClassRelTestCase1") && (parsedClass.Name == "Square"))
                {
                    HashSet<string> expectedUsingList = new() { "CClassRelTestCase1.Circle" };
                    bool areEqual = parsedClass.UsingList.SetEquals( expectedUsingList );
                    Assert.AreEqual( true , areEqual );
                }

                if ((parsedClass.TypeObj.Namespace == "ClassRelTestCase1") && (parsedClass.Name == "Circle"))
                {
                    HashSet<string> expectedUsingList = new() { "IClassRelTestCase1.IColor" };
                    bool areEqual = parsedClass.UsingList.SetEquals( expectedUsingList );
                    Assert.AreEqual( true , areEqual );
                }
            }
        }

        /// <summary>
        /// Checking the case where 1.the constructor takes in an object as parameter but is then assigned to a field (using case)
        /// 2. the field in a constructor is assigned to an object returned by another function (aggregation case)
        /// </summary>
        [TestMethod()]
        public void CheckParameterAssignedToField()
        {
            string dllFile = Assembly.GetExecutingAssembly().Location;

            ParsedDLLFile parsedDLL = new( dllFile );

            foreach (ParsedClassMonoCecil parsedClass in parsedDLL.classObjListMC)
            {
                if ((parsedClass.TypeObj.Namespace == "ClassRelTestCase1") && (parsedClass.Name == "Triangle"))
                {
                    HashSet<string> expectedUsingList = new() { "CClassRelTestCase1.Circle" };
                    bool areEqual = parsedClass.UsingList.SetEquals( expectedUsingList );
                    Assert.AreEqual( true , areEqual );

                    HashSet<string> expectedAggregationList = new() { "CClassRelTestCase1.RedColour" , "IClassRelTestCase1.IColor"};
                    foreach(string x in parsedClass.AggregationList)
                    {
                        Console.WriteLine( x );
                    }
                    areEqual = parsedClass.AggregationList.SetEquals( expectedAggregationList );
                    Assert.AreEqual( true , areEqual );
                }
            }
        }

        /// <summary>
        /// Testing the case where an interface is present as parameter of a method in a class.  
        /// </summary>
        [TestMethod()]
        public void CheckInterfaceParameter()
        {
            string dllFile = Assembly.GetExecutingAssembly().Location;

            ParsedDLLFile parsedDLL = new( dllFile );

            foreach (ParsedClassMonoCecil parsedClass in parsedDLL.classObjListMC)
            {
                if ((parsedClass.TypeObj.Namespace == "ClassRelTestCase1") && (parsedClass.Name == "Rectangle"))
                {
                    HashSet<string> expectedUsingList = new() { "IClassRelTestCase1.IColor" };
                    bool areEqual = parsedClass.UsingList.SetEquals( expectedUsingList );
                    Assert.AreEqual( true , areEqual );
                }
            }
        }
    }
}
    namespace ClassRelTestCase1
    {
        public interface IColor
        {
            
        }

        public class RedColour : IColor
        {
            public RedColour()
            {
            Console.WriteLine( "Red Color" );
            }
        }

        public class BlueColor : IColor
        {
            public BlueColor()
            {
                Console.WriteLine( "Blue Color" );
            }
        }
    public interface IDrawable
        {
            void Draw();
        }

        public class Circle : IDrawable
        {
            public Circle(IColor clr)
            {
                IColor rClr = clr;        
                Console.WriteLine(rClr.ToString());
            }
            public void Draw()
            {
                //add some lambda function
                Console.WriteLine("Drawing a circle.");
            }
            
        }

        public class Square : IDrawable
        {
            public Square(Circle c1)
            {
                //parameter is assigned to local variable
                //using expected betn square and circle
                Circle cc1 = c1;
                Console.WriteLine(cc1);
            }
            public void Draw()
            {
                Console.WriteLine("Drawing a square.");
            }
        }
        public class Rectangle: IDrawable
        {
            public void Draw()
            {
                Console.WriteLine("Drawing a square.");
            }
            public void AddColour(IColor clr)
            {
                Console.WriteLine("Using some Clr ");
            }
        }
        public class Triangle : IDrawable
        {
            public Circle _inscribed;
            public RedColour _color;
            public IColor _c1;
            public Triangle(Circle c1 )
            {
                _inscribed = c1;
                _color = getRedColor();
                _c1 = GetColor.getBlueColor();                
            }
            public void Draw()
            {
                Console.WriteLine( "Drawing a Triangle." );
            }
            
            public RedColour getRedColor()
            {
                return new RedColour();
            }
        }

    static class GetColor
    {
        public static IColor getBlueColor()
        {
            return new BlueColor();
        }

    }
}
