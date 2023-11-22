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
    [TestClass()]
    public class TestControl
    {
        public class TestClassForControl
        {
            public void MethodWithUselessControlFlow()
            {
                int x = 10;
                int y = 20;

                if (x > y)
                {
                    Console.WriteLine( "x is greater than y" );
                }
                else
                {
                    Console.WriteLine( "x is not greater than y" );
                }

                // This is a useless jump followed by a no-op
                if (x < y)
                {
                    Console.WriteLine( "x is less than y" );
                }
                else
                {
                    Console.WriteLine( "x is not less than y" );
                    // No-op
                    Nop();
                }

                Console.WriteLine( "Method completed" );
            }

            private static void Nop()
            {
                // No-op
            }
        }

        [TestMethod()]
        public void Test1()
        {
            // Specify the path to the DLL file
            //string path = "..\\..\\..\\..\\Analyzer\\TestDLLs\\controlflow.dll";
            string path = Assembly.GetExecutingAssembly().Location;
            // Create a list of DLL paths
            ParsedDLLFile dllFile = new(path);

            //DllFilePaths.Add(path);

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            // Create an instance of RemoveUnusedLocalVariablesRule
            ReviewUselessControlFlowRule analyzer = new(dllFiles);

            // Run the analyzer
            Dictionary<string , AnalyzerResult> result = analyzer.AnalyzeAllDLLs();

            foreach (KeyValuePair<string , AnalyzerResult> dll in result)
            {
                Console.WriteLine(dll.Key);

                AnalyzerResult res = dll.Value;

                Console.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
            }

        }
    }
}
