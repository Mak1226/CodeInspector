//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Analyzer.Parsing;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Mono.Cecil;

//namespace AnalyzerTests.Pipeline
//{
//    /// <summary>
//    /// Testing whether parsed attributes using System.Reflection are correctly parsed as per requirements
//    /// </summary>
//    [TestClass]
//    public class TestParsingReflection
//    {
//        static readonly List<string> dllFilesList1 = new List<string>() { "..\\..\\..\\..\\Analyzer\\TestDLLs\\BridgePattern.dll" };
//        static readonly ParsedDLLFile parsedDLLs1 = new(dllFilesList1);

//        // Other dlls to check interfaces of parent class are coming under interfaces list or not
//        static readonly List<string> dllFilesList2 = new List<string>() { "..\\..\\..\\..\\Analyzer\\TestDLLs\\demo.dll" };
//        static readonly ParsedDLLFile parsedDLLs2 = new(dllFilesList2);

//        /// <summary>
//        /// Checking whether all the classes were identified and parsed or not 
//        /// </summary>
//        [TestMethod]
//        public void CheckClassNames()
//        {
//            List<ParsedClass> parsedClasses = parsedDLLs1.classObjList;

//            // Checking number of classes
//            Assert.AreEqual(5, parsedClasses.Count);

//            // Checking classes retrieved
//            List<string> expectedClassFullNames = new(){ "BridgePattern.Shapes" , "BridgePattern.Square" , "BridgePattern.BriefView" , "BridgePattern.DetailedView" , "BridgePattern.Circle" };
//            List<string> expectedClassNames = new() { "Shapes", "Square", "BriefView", "DetailedView", "Circle" };

//            List<string> retrievedClassFullNames = new();
//            List<string> retrievedClassNames = new();
            
//            foreach( ParsedClass parsedClass in parsedClasses )
//            {
//                retrievedClassFullNames.Add(parsedClass.TypeObj.FullName);
//                retrievedClassNames.Add(parsedClass.Name);
//            }

//            expectedClassFullNames.Sort();
//            retrievedClassFullNames.Sort();

//            expectedClassNames.Sort();
//            retrievedClassNames.Sort();

//            CollectionAssert.AreEqual(expectedClassFullNames, retrievedClassFullNames);
//            CollectionAssert.AreEqual(expectedClassNames, retrievedClassNames);
//        }


//        /// <summary>
//        /// 1. Checking whether all the classes were identified and parsed or not
//        /// 2. Checking whether interfaces implemented by a class or interface were parsed correctly into parsedclass & parsedinterface objects or not
//        /// </summary>
//        [TestMethod]
//        public void CheckInterfacesImplemented()
//        {
//            List<ParsedInterface> parsedInterfaces = parsedDLLs1.interfaceObjList;
//            List<ParsedClass> parsedClasses = parsedDLLs1.classObjList;

//            Assert.AreEqual(parsedInterfaces.Count, 1);
//            Assert.AreEqual(parsedInterfaces[0].Name, "IDrawingView");
//            Assert.AreEqual(parsedInterfaces[0].ParentInterfaces.Length, 0);

//            // Checking "Interfaces" property of parsed class object
//            foreach (ParsedClass parsedClass in parsedClasses)
//            {
//                if (parsedClass.Name == "BriefView" || parsedClass.Name == "DetailedView")
//                {
//                    Assert.AreEqual(parsedClass.Interfaces.Length, 1);
//                    Assert.AreEqual(parsedClass.Interfaces[0].Name, "IDrawingView");
//                }
//                else
//                {
//                    Assert.AreEqual(parsedClass.Interfaces.Length, 0);
//                }
//            }

//            // Dll file2 tests
//            List<ParsedClass> otherParsedClasses = parsedDLLs2.classObjList;
//            List<ParsedInterface> otherParsedInterfaces = parsedDLLs2.interfaceObjList;

//            Assert.AreEqual(otherParsedInterfaces.Count, 2);
//            otherParsedInterfaces.Sort((x,y)=>x.Name.CompareTo(y.Name));

//            List<string> interfaceNames = new() { otherParsedInterfaces[0].Name, otherParsedInterfaces[1].Name };
//            CollectionAssert.AreEqual(interfaceNames, new List<string>() { "IApp", "ISample" });


//            // Checking "ParentInterfaces" property of an parsedinterface object
//            Assert.AreEqual(otherParsedInterfaces[0].ParentInterfaces.Length, 1);
//            Assert.AreEqual(otherParsedInterfaces[0].ParentInterfaces[0].Name, "ISample");
//            Assert.AreEqual(otherParsedInterfaces[1].ParentInterfaces.Length, 0);


//            // Checking "Interfaces" field of parsed class object when the interface implemented by it also have a parent interface
//            otherParsedClasses.Sort((x,y) => x.Name.CompareTo(y.Name));

//            List<string> classesNames = new();

//            foreach(ParsedClass parsedClass in otherParsedClasses)
//            {
//                classesNames.Add(parsedClass.Name);
//            }

//            CollectionAssert.AreEqual(classesNames, new List<string>() {"App1" , "Cleanup", "Cleanup1" , "Program" , "SampleClass" , "StartProgram" , "UploadFiles"});

//            // App1 implements IApp & IApp implements ISample => But we need only App1 interfaces shown as IApp for graph purpose
//            Assert.AreEqual(otherParsedClasses[0].Interfaces.Length, 1);
//            Assert.AreEqual(otherParsedClasses[0].Interfaces[0].Name, "IApp");

//            // Cleanup inherits from Cleanup1 & Cleanup1 implements ISample => But we need only Cleanup interfaces shown as Empty for graph purpose
//            Assert.AreEqual(otherParsedClasses[1].Interfaces.Length, 0);

//            // CleanUp1 
//            Assert.AreEqual(otherParsedClasses[2].Interfaces.Length, 1);
//            Assert.AreEqual(otherParsedClasses[2].Interfaces[0].Name, "ISample");

//            // Program , SampleClass , StartProgram , UploadFiles
//            Assert.AreEqual(otherParsedClasses[3].Interfaces.Length, 0);
//            Assert.AreEqual(otherParsedClasses[4].Interfaces.Length, 0);
//            Assert.AreEqual(otherParsedClasses[5].Interfaces.Length, 0);
//            Assert.AreEqual(otherParsedClasses[6].Interfaces.Length, 0);
//        }

//        //[TestMethod]
//        //public void Check
//    }
//}