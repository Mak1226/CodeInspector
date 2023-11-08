using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil;

namespace AnalyzerTests.Pipeline
{
    [TestClass]
    public class TestParsedClassReflection
    {
        static readonly List<string> dllFiles = new List<string>() { "..\\..\\..\\..\\Analyzer\\TestDLLs\\BridgePattern.dll" };

        static readonly ParsedDLLFiles parsedDLLs = new(dllFiles);
        List<ParsedClass> parsedClasses = parsedDLLs.classObjList;
        List<ParsedInterface> parsedInterfaces = parsedDLLs.interfaceObjList;


        [TestMethod]
        public void CheckClassNames()
        {
            // Checking number of classes
            Assert.AreEqual(5, parsedClasses.Count);

            // Checking classes retrieved
            List<string> expectedClassFullNames = new(){ "BridgePattern.Shapes" , "BridgePattern.Square" , "BridgePattern.BriefView" , "BridgePattern.DetailedView" , "BridgePattern.Circle" };
            List<string> expectedClassNames = new() { "Shapes", "Square", "BriefView", "DetailedView", "Circle" };

            List<string> retrievedClassFullNames = new();
            List<string> retrievedClassNames = new();
            
            foreach( ParsedClass parsedClass in parsedClasses )
            {
                retrievedClassFullNames.Add(parsedClass.TypeObj.FullName);
                retrievedClassNames.Add(parsedClass.Name);
            }

            expectedClassFullNames.Sort();
            retrievedClassFullNames.Sort();

            expectedClassNames.Sort();
            retrievedClassNames.Sort();

            CollectionAssert.AreEqual(expectedClassFullNames, retrievedClassFullNames);
            CollectionAssert.AreEqual(expectedClassNames, retrievedClassNames);
        }

        [TestMethod]
        public void CheckInterfacesImplemented()
        {
            Assert.AreEqual(parsedInterfaces.Count, 1);
            Assert.AreEqual(parsedInterfaces[0].Name, "IDrawingView");
            Assert.AreEqual(parsedInterfaces[0].ParentInterfaces.Length, 0);

            foreach (ParsedClass parsedClass in parsedClasses)
            {
                if (parsedClass.Name == "BriefView" || parsedClass.Name == "DetailedView")
                {
                    Assert.AreEqual(parsedClass.Interfaces.Length, 1);
                    Assert.AreEqual(parsedClass.Interfaces[0].Name, "IDrawingView");
                }
                else
                {
                    Assert.AreEqual(parsedClass.Interfaces.Length, 0);
                }
            }

            // Other dlls to check interfaces of parent class are coming under interfaces list or not
            List<string> otherdllFiles = new List<string>() { "..\\..\\..\\..\\Analyzer\\TestDLLs\\demo.dll" };

            ParsedDLLFiles otherParsedDLL = new(otherdllFiles);
            List<ParsedClass> otherParsedClasses = otherParsedDLL.classObjList;
            List<ParsedInterface> otherParsedInterfaces = otherParsedDLL.interfaceObjList;

            //Console.WriteLine(otherParsedInterfaces[0].Name);
            Assert.AreEqual(otherParsedInterfaces.Count, 2);

            List<string> interfaceNames = new List<string>() { otherParsedInterfaces[0].Name, otherParsedInterfaces[1].Name };
            interfaceNames.Sort();
            CollectionAssert.AreEqual(interfaceNames, new List<string>() { "IApp", "ISample" });

            foreach(ParsedClass parsedClass in otherParsedClasses) 
            {
                
            }
        }


    }
}