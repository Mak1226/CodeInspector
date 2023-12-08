/******************************************************************************
* Filename    = TestParsingDLL.cs
* 
* Author      = Nikhitha Atyam
* 
* Product     = Analyzer
* 
* Project     = AnalyzerTests
*
* Description = UnitTests for Analyzer.Parsing.ParsedDLL.cs 
*               (checks whether parsing assembly using System.Reflection, Mono.Cecil is correct)
*****************************************************************************/

using Analyzer.Parsing;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace AnalyzerTests.Parsing
{
    /// <summary>
    /// Checks the parsed class,interface object lists from the parsedDLL object
    /// </summary>
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
            // current DLL is being used
            string currentDLLPath = Assembly.GetExecutingAssembly().Location;   
            ParsedDLLFile parsedDLL = new(currentDLLPath);
            Assert.AreEqual(parsedDLL.DLLFileName, "AnalyzerTests.dll");

            parsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.Namespace != "TestParsingDLL_BridgePattern" );
            parsedDLL.interfaceObjList.RemoveAll( iface => iface.TypeObj.Namespace != "TestParsingDLL_BridgePattern" );
            parsedDLL.classObjListMC.RemoveAll( cls => cls.TypeObj.Namespace != "TestParsingDLL_BridgePattern" );
            

            // Check for classObjList of ParsedDLL object
            List<string> expectedClassNames = new() { "Shapes", "Square", "BriefView", "DetailedView", "Circle" };
            List<string> retrievedClassNames = new();

            foreach(ParsedClass parsedClass in parsedDLL.classObjList)
            {
                retrievedClassNames.Add(parsedClass.TypeObj.Name);
            }
            CollectionAssert.AreEquivalent( expectedClassNames, retrievedClassNames );


            // Check for interfaceObjList of ParsedDLL object
            List<string> expectedInterfaceNames = new() { "IDrawingView" };
            List<string> retrievedInterfaceNames = new();

            foreach(ParsedInterface parsedInterface in parsedDLL.interfaceObjList)
            {
                retrievedInterfaceNames.Add(parsedInterface.TypeObj.Name);
            }
            CollectionAssert.AreEquivalent(expectedInterfaceNames, retrievedInterfaceNames);


            // Check for classObjListMC of ParsedDLL object
            List<string> retrievedClassNamesMC = new();

            foreach(ParsedClassMonoCecil parsedClass in parsedDLL.classObjListMC)
            {
                retrievedClassNamesMC.Add(parsedClass.TypeObj.Name);
            }
            CollectionAssert.AreEquivalent(expectedClassNames , retrievedClassNamesMC);
        }


        /// <summary>
        /// Testing the parsing of DLL completely without filtering
        /// </summary>
        [TestMethod]
        public void CheckCompleteDLL()
        {
            // BridgePatternDLL structure is same as "TestParsingDLL_BridgePattern" namespace
            string dllPath = "..\\..\\..\\TestDLLs\\BridgePattern.dll";
            ParsedDLLFile parsedDLL = new( dllPath );

            Assert.AreEqual(5, parsedDLL.classObjList.Count);
            Assert.AreEqual(5, parsedDLL.classObjListMC.Count);
            Assert.AreEqual(1, parsedDLL.interfaceObjList.Count);
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
