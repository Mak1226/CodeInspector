using System;
using Analyzer.Parsing;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace AnalyzerTests.Parsing
{
    [TestClass] 
    public class TestParsingDLL
    {
        /// <summary>
        /// Testing whether valid types are coming in the class and interface object lists while parsing
        /// While Parsing DLL, currently only these two types are considered. Remaining types like structures, delegates etc.. are ignored
        /// </summary>
        [TestMethod]
        public void TestValidParsingTypes()
        {
            string currentDLLPath = Assembly.GetExecutingAssembly().Location;   
            ParsedDLLFile parsedDLL = new(currentDLLPath);
            Assert.AreEqual(parsedDLL.DLLFileName, "AnalyzerTests.dll");

            parsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.Namespace != "TestParsingDLL_BridgePattern" );
            parsedDLL.interfaceObjList.RemoveAll( iface => iface.TypeObj.Namespace != "TestParsingDLL_BridgePattern" );
            parsedDLL.classObjListMC.RemoveAll( cls => cls.TypeObj.Namespace != "TestParsingDLL_BridgePattern" );
            

            List<string> expectedClassNames = new() { "Shapes" , "Square" , "BriefView" , "DetailedView" , "Circle" };
            List<string> retrievedClassNames = new();

            foreach(ParsedClass parsedClass in parsedDLL.classObjList)
            {
                retrievedClassNames.Add(parsedClass.TypeObj.Name);
            }
            CollectionAssert.AreEquivalent( expectedClassNames , retrievedClassNames );


            List<string> expectedInterfaceNames = new() { "IDrawingView" };
            List<string> retrievedInterfaceNames = new();

            foreach(ParsedInterface parsedInterface in parsedDLL.interfaceObjList)
            {
                retrievedInterfaceNames.Add(parsedInterface.TypeObj.Name);
            }
            CollectionAssert.AreEquivalent(expectedInterfaceNames , retrievedInterfaceNames);
        }
    }
}


namespace TestParsingDLL_BridgePattern
{
    public struct SampleStructure
    {
        public int var1;
    }

    enum ShapesEnum 
    { 
        Circle , 
        Square ,
        Rectangle
    }

    public delegate void MyDelegate( string message );

    public interface IDrawingView
    {
    }

    public class BriefView : IDrawingView
    {
    }

    public class DetailedView : IDrawingView
    {
    }

    public abstract class Shapes
    { 
    }

    public class Circle : Shapes
    {
    }

    public class Square : Shapes
    {
    }
}
