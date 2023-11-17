using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Analyzer.Parsing;
using Analyzer.Pipeline;
using Analyzer;
using Microsoft.VisualBasic.FileIO;
using Mono.Cecil.Cil;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace AnalyzerTests.Pipeline
{
    public class SampleComplexityTestClass
    {
        public static void SampleIfElseMethod()
        {
            int x = 0;

            if (x == 0)
            {
                Console.WriteLine(x);
                int y = 3;
                if(y == 1)
                {
                    x = 4;
                }

                
            }
            else
            {
                Console.WriteLine(x+1);
            }
        }

        public void SampleSwitchMethod1() 
        {
            int option = 9;
            switch (option)
            {
                case 100:
                    Console.WriteLine("Option 1 selected");
                    // Additional statements for case 1
                    break;

                case 200:
                    Console.WriteLine("Option 2 selected");
                    // Additional statements for case 2
                    break;

                case 3:
                    Console.WriteLine("Option 3 selected");
                    // Additional statements for case 3
                    break;

                case 400:
                    Console.WriteLine("Option 4 selected");
                    // Additional statements for case 4
                    if (option + 2 == 5)
                    {
                        Console.WriteLine("Random");
                    }
                    else
                    {
                        Console.WriteLine("Random2");
                    }
                    break;

                case 5:
                    Console.WriteLine("Option 5 selected");
                    // Additional statements for case 5
                    break;

                //default:
                //    Console.WriteLine("Invalid option selected");
                //    // Additional statements for default case
                //    break;
            }
        }

        public void SampleSwitchMethod2()
        {

            int x = 0;
            switch (x)
            {
                case 0:
                    break;
                case 2:
                    Console.WriteLine();
                    if (x + 1 == 20)
                    {
                        Console.WriteLine("Hello");

                        if (x + 2 == 30)
                        {
                            Console.WriteLine("Again");
                        }
                    }
                    else
                    {
                        Console.WriteLine("4");
                    }
                    break;
                case 1:
                    Console.WriteLine("1");
                    int y = x > 1 ? 2 : 3;
                    Console.WriteLine(y);
                    break;
                default:
                    SampleSwitchMethod2();
                    break;
            }
        }


        public static int GetOperandType(Instruction self, MethodDefinition method)
        {
            int i = 0;
            switch (self.OpCode.Code)
            {
                case Code.Ldarg_0:
                case Code.Ldarg_1:
                case Code.Ldarg_2:
                case Code.Ldarg_3:
                case Code.Ldarg:
                case Code.Ldarg_S:
                case Code.Ldarga:
                case Code.Ldarga_S:
                case Code.Starg:
                case Code.Starg_S:
                    i = 1;
                    Console.WriteLine("arguments");
                    break;
                case Code.Conv_R4:
                case Code.Ldc_R4:
                case Code.Ldelem_R4:
                case Code.Ldind_R4:
                case Code.Stelem_R4:
                case Code.Stind_R4:
                    i = 2;
                    Console.WriteLine("singles");
                    break;
                case Code.Conv_R8:
                case Code.Ldc_R8:
                case Code.Ldelem_R8:
                case Code.Ldind_R8:
                case Code.Stelem_R8:
                    i = 3;
                    Console.WriteLine("doubles");
                    break;
                case Code.Ldloc_0:
                case Code.Ldloc_1:
                case Code.Ldloc_2:
                case Code.Ldloc_3:
                case Code.Ldloc:
                case Code.Ldloc_S:
                case Code.Ldloca:
                case Code.Ldloca_S:
                case Code.Stloc_0:
                case Code.Stloc_1:
                case Code.Stloc_2:
                case Code.Stloc_3:
                case Code.Stloc:
                case Code.Stloc_S:
                    i = 4;
                    Console.WriteLine("locals");
                    break;
                case Code.Ldfld:
                case Code.Ldflda:
                case Code.Ldsfld:
                case Code.Ldsflda:
                case Code.Stfld:
                case Code.Stsfld:
                    i = 5;
                    Console.WriteLine("fields");
                    break;
                case Code.Call:
                case Code.Callvirt:
                case Code.Newobj:
                    i = 6;
                    Console.WriteLine("calls");
                    break;
                    //default:
                    //    i = 7;
                    //    Console.WriteLine("default");
                    //    break;
            }
            Console.WriteLine("end");
            return i;
        }
    }

    [TestClass]
    public class TestCyclomaticComplexity
    {
        [TestMethod]
        public void checkIfElseComplexity()
        {
            var dllFile = Assembly.GetExecutingAssembly().Location;
            //var dllFile = "C:\\Users\\nikhi\\source\\repos\\nikhi9603\\Analyzer\\Analyzer\\bin\\Debug\\net6.0\\Analyzer.dll";

            ParsedDLLFile parsedDLL = new ParsedDLLFile(dllFile);

            List<ParsedDLLFile> parseddllFiles = new() { parsedDLL };

            CyclomaticComplexity cyclomaticComplexityRule = new(parseddllFiles);

            var result = cyclomaticComplexityRule.AnalyzeAllDLLs();

            ModuleDefinition module = ModuleDefinition.ReadModule(dllFile);

            TypeReference typeReference = module.ImportReference(typeof(SampleComplexityTestClass));

            // Resolve the TypeReference to get the TypeDefinition
            TypeDefinition typeDefinition = typeReference.Resolve();
            MethodDefinition method = typeDefinition.Methods.FirstOrDefault(m => m.Name == "SampleIfElseMethod");
            Assert.AreEqual(3, cyclomaticComplexityRule.GetMethodCyclomaticComplexity(method));

            //AnalyzerResult r = result[parsedDLL.DLLFileName];

            //var x = r.AnalyserID;
            //var y = r.Verdict;
            //var z = r.ErrorMessage;

            //Console.WriteLine(parsedDLL.DLLFileName);
            //Console.WriteLine($"{r.AnalyserID}");
            //Console.WriteLine($"{r.Verdict}");

            ////Assert.AreEqual(kv.Value.Verdict, 0);
            //Console.WriteLine($"{r.ErrorMessage}");
            //Console.WriteLine("\n\n");

            foreach (KeyValuePair<string, AnalyzerResult> kv in result)
            {
                Console.WriteLine(kv.Key);
                Console.WriteLine($"{kv.Value.AnalyserID}");
                Console.WriteLine($"{kv.Value.Verdict}");

                //Assert.AreEqual(kv.Value.Verdict, 0);
                Console.WriteLine($"{kv.Value.ErrorMessage}");
                Console.WriteLine("\n\n");
            }
        }
    }
}
