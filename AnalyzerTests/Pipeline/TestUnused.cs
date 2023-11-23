using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Parsing;
using System.Reflection;

namespace Analyzer.Pipeline.Tests
{
    public class TestClass
    {
        public static void MethodWithUnusedVariables()
        {
            int x = 42;
            int y = 10;

            //Console.WriteLine(x+y);
            int u;
            int z;
            int thirdunused;
            int n = x + y;
        }

        public static void MethodWithUsedVariables()
        {
            int usedVariable1 = 3;

            Console.WriteLine( usedVariable1 );
        }

        public static void MethodWithNoLocals() => Console.WriteLine( "No local variables in this method." );
    }

    [TestClass()]
    public class TestUnused
    {
        [TestMethod()]
        public void TestRemoveUnusedLocalVariables()
        {
            // Specify the path to the DLL file
            //string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\TestUnused.dll";

            string path = Assembly.GetExecutingAssembly().Location;

            // Create a list of DLL paths
            ParsedDLLFile dllFile = new(path);

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            RemoveUnusedLocalVariablesRule analyzer = new(dllFiles);

            // Run the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                AnalyzerResult res = dll.Value;

                Console.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);

                Assert.AreEqual( 0 , res.Verdict , "There are no unused local variables!" );
            }
        }
    }
}
