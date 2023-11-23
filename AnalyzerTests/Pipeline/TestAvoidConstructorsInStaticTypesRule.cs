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

            parsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.Namespace != "TestCase1" );
            List<ParsedDLLFile> parseddllFiles = new() { parsedDLL };

            AvoidConstructorsInStaticTypes avoidConstructorInStaticTypes = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> resultObj = avoidConstructorInStaticTypes.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["AnalyzerTests.dll"];
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

            parsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.Namespace != "TestCase2" );
            List<ParsedDLLFile> parseddllFiles = new() { parsedDLL };

            AvoidConstructorsInStaticTypes avoidConstructorInStaticTypes = new( parseddllFiles );
            Dictionary<string , AnalyzerResult> resultObj = avoidConstructorInStaticTypes.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["AnalyzerTests.dll"];
            Assert.AreEqual( 0 , result.Verdict );
            Console.WriteLine( result.ErrorMessage );
            string expectedErrorMsg = "Classes TestCase2.BadExample contains only static fields and methods, but has non-static, visible constructor. Try changing it to private or make it static.";
            Assert.AreEqual( expectedErrorMsg , result.ErrorMessage );
        }

    }
}


namespace TestCase1
{
    public class GoodExample
    {
        public static int counter = 0;
        private GoodExample() { }
        public void Calculate()
        {
            Console.WriteLine( "Static Method" );
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

namespace TestCase2
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
