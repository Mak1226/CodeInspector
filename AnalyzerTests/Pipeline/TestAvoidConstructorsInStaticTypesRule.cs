using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Analyzer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace AnalyzerTests.Pipeline
{
    /// <summary>
    /// Test class for testing the analyzer- AvoidConstructorsInStaticTypesRule.
    /// </summary>
    [TestClass()]
    public class TestAvoidConstructorsInStaticTypesRule
    {
        /// <summary>
        /// Test method for a case in which all classes follow the rule of not having visible non-static constructor 
        /// for classes having only static methods and fields.
        /// </summary>
        [TestMethod()]
        public void TestGoodExample()
        {
            string dllFile = Assembly.GetExecutingAssembly().Location;
            ParsedDLLFile parsedDLL = new( dllFile );

            parsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.Namespace != "AcistTestCase1" );
            List<ParsedDLLFile> parseddllFiles = new() { parsedDLL };

            AvoidConstructorsInStaticTypes avoidConstructorInStaticTypes = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> resultObj = avoidConstructorInStaticTypes.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["AnalyzerTests.dll"];
            Console.WriteLine(result.ErrorMessage );
            Assert.AreEqual( 1 , result.Verdict );
            Assert.AreEqual( "No violation found" , result.ErrorMessage );
        }

        /// <summary>
        /// Test method for a case in which classes don't follow the above mentioned rule.
        /// </summary>
        [TestMethod()]
        public void TestBadExample()
        {
            string dllFile = Assembly.GetExecutingAssembly().Location;

            ParsedDLLFile parsedDLL = new( dllFile );

            parsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.Namespace != "AcistTestCase2" );
            List<ParsedDLLFile> parseddllFiles = new() { parsedDLL };

            AvoidConstructorsInStaticTypes avoidConstructorInStaticTypes = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> resultObj = avoidConstructorInStaticTypes.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["AnalyzerTests.dll"];
            Assert.AreEqual( 0 , result.Verdict );
            Console.WriteLine( result.ErrorMessage );
            string expectedErrorMsg = "Classes AcistTestCase2.BadExample contains only static fields and methods, but has non-static, visible constructor. Try changing it to private or make it static.";
            Assert.AreEqual( expectedErrorMsg , result.ErrorMessage );
        }


        [TestMethod()]
        public void TestMultipleClassViolations()
        {
            string dllFile = Assembly.GetExecutingAssembly().Location;

            ParsedDLLFile parsedDLL = new(dllFile);

            parsedDLL.classObjList.RemoveAll(cls => cls.TypeObj.Namespace != "AcistTestCase3");
            List<ParsedDLLFile> parseddllFiles = new() { parsedDLL };

            AvoidConstructorsInStaticTypes avoidConstructorInStaticTypes = new(parseddllFiles);
            Dictionary<string, AnalyzerResult> resultObj = avoidConstructorInStaticTypes.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["AnalyzerTests.dll"];
            Assert.AreEqual(0, result.Verdict);
            string expectedErrorMsg = "Classes AcistTestCase3.Logger, AcistTestCase3.WarningLogger contains only static fields and methods, but has non-static, visible constructor. Try changing it to private or make it static.";
            Assert.AreEqual(expectedErrorMsg, result.ErrorMessage);
        }

        [TestMethod()]
        public void TestDllsOnAcist()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\Proxy.dll";
            string path2 = "..\\..\\..\\TestDLLs\\ACIST1.dll";
            string path3 = "..\\..\\..\\TestDLLs\\BridgePattern.dll";
            var parsedDllObj = new ParsedDLLFile(path);
            var parsedDllObj1 = new ParsedDLLFile(path2);
            var parsedDllObj2 = new ParsedDLLFile(path3);


            DllFileObjs.Add(parsedDllObj);
            DllFileObjs.Add(parsedDllObj1);
            DllFileObjs.Add(parsedDllObj2);


            AvoidConstructorsInStaticTypes avoidConstructorInStaticTypes = new(DllFileObjs);

            Dictionary<string, Analyzer.AnalyzerResult> resultObj = avoidConstructorInStaticTypes.AnalyzeAllDLLs();

            Assert.AreEqual(1, resultObj["Proxy.dll"].Verdict);
            Assert.AreEqual(0, resultObj["ACIST1.dll"].Verdict);
            Assert.AreEqual(1, resultObj["BridgePattern.dll"].Verdict);

        }

    }
}


namespace AcistTestCase1
{
    public interface IExample
    {
        public static int x = 4;
    }
    public class GoodExample: IExample
    {
        public static int counter = 0;
        private GoodExample() { }
        public static void Calculate()
        {
            Console.WriteLine( "Static Method" );
            Func<int, int, int> addFunction = (a, b) => a + b;

            // Using the lambda function
            int result = addFunction(5, 3);
        }
    }

    public class GoodExample2
    {
        public int counter = 0;
        public GoodExample2() { }
        public static void Calculate()
        {
            Console.WriteLine( "Static Method" );
        }
    }
}

namespace AcistTestCase2
{
    public class BadExampleBase
    {
        public static int val = 14;
        private BadExampleBase()
        {
            // Private constructor logic
        }

        protected BadExampleBase( int parameter )
        {
            // Public constructor logic
        }
    }

    public class BadExample : BadExampleBase
    {
        public BadExample() : base( 0 )
        {
            // Child class constructor logic
        }

        public static int counter = 0;

        public static void Calculate()
        {
            Console.WriteLine( "Static Method" );
        }
    }
    public class GoodExample
    {
        public static int counter = 0;
        public int counter2 = 0;
        private GoodExample() { }
        public static void Calculate()
        {
            Console.WriteLine( "Static Method" );
        }
    }

}

namespace AcistTestCase3
{
    public class Logger
    {
        public static int val = 14;
    }

    public class WarningLogger : Logger
    {
        public static int warningCount = 1;
        public WarningLogger() { }
    }
}