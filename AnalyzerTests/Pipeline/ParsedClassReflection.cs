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
    public class ParsedClassReflection
    {
        [TestMethod]
        public void CheckClassNames()
        {
            List<string> dllFiles = new List<string>() { "..\\..\\..\\..\\Analyzer\\TestDLLs\\BridgePattern.dll" };

            ParsedDLLFiles parsedDLLs = new(dllFiles);
            List<ParsedClass> parsedClasses = parsedDLLs.classObjList;

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
    }
}