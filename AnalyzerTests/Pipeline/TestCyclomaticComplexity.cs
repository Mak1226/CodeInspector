/******************************************************************************
* Filename    = TestCyclomaticComplexity.cs
* 
* Author      = Nikhitha Atyam
* 
* Product     = Analyzer
* 
* Project     = AnalyzerTests
*
* Description = UnitTests for Analyzer.Pipeline.CyclomaticComplexity.cs 
*               (checks the complexity of different methods in the DLL)
*****************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Analyzer.Parsing;
using Analyzer.Pipeline;
using Analyzer;
using Mono.Cecil;


namespace AnalyzerTests.Pipeline
{
    /// <summary>
    /// Checks the cyclomatic complexity of different test methods
    /// </summary>
    [TestClass]
    public class TestCyclomaticComplexity
    {
        // current DLL details and its parsedDLL object
        public static string currentDLLPath = Assembly.GetExecutingAssembly().Location;
        public static ParsedDLLFile currentParsedDLL = new( currentDLLPath );
        public static CyclomaticComplexity _cyclomaticComplexityAnalyzer;

        // current DLL module to access different test methods
        public static ModuleDefinition currentModule = ModuleDefinition.ReadModule( currentDLLPath );
        public static TypeReference currentTypeReference = currentModule.ImportReference( typeof( SampleComplexityTestCases.SampleComplexityTestClass ) );
        public static TypeDefinition currentTypeDefintion = currentTypeReference.Resolve();


        /// <summary>
        /// Current DLL will be AnalyzerTests.DLL => So filtering the namespaces where test cases were present
        /// And creating an cyclomatic analyzer with current parsedLL as input
        /// </summary>
        [ClassInitialize]
        public static void TestCyclomaticComplexityInitialize( TestContext context )
        {
            currentParsedDLL.classObjList.RemoveAll( cls => cls.TypeObj.Namespace != "SampleComplexityTestCases" );
            currentParsedDLL.interfaceObjList.RemoveAll( iface => iface.TypeObj.Namespace != "SampleComplexityTestCases" );
            currentParsedDLL.classObjListMC.RemoveAll( cls => cls.TypeObj.Namespace != "SampleComplexityTestCases" );

            _cyclomaticComplexityAnalyzer = new CyclomaticComplexity( new() { currentParsedDLL } );
        }


        /// <summary>
        /// This methods checks complexity of methods having conditional statements like IfElse, NestedIf, Ternary etc..
        /// </summary>
        [TestMethod]
        public void CheckIfElseComplexity()
        {
            MethodDefinition sampleIfElseMethod = currentTypeDefintion.Methods.First( method => method.Name == "IfElseMethod" );
            Assert.AreEqual( 2 , _cyclomaticComplexityAnalyzer.GetMethodCyclomaticComplexity( sampleIfElseMethod ) );


            MethodDefinition sampleNestedIfElseMethod = currentTypeDefintion.Methods.First( method => method.Name == "NestedIfElseMethod" );
            Assert.AreEqual( 3 , _cyclomaticComplexityAnalyzer.GetMethodCyclomaticComplexity( sampleNestedIfElseMethod ) );

            MethodDefinition sampleTernaryMethod = currentTypeDefintion.Methods.First( method => method.Name == "TernaryOperatorMethod" );
            Assert.AreEqual( 2 , _cyclomaticComplexityAnalyzer.GetMethodCyclomaticComplexity( sampleTernaryMethod ) );
        }


        /// <summary>
        /// This methods checks complexity of methods having loops
        /// </summary>
        [TestMethod]
        public void CheckLoopComplexity()
        {
            MethodDefinition sampleForLoopMethod = currentTypeDefintion.Methods.First( method => method.Name == "ForLoopMethod" );
            Assert.AreEqual( 2 , _cyclomaticComplexityAnalyzer.GetMethodCyclomaticComplexity( sampleForLoopMethod ) );

            MethodDefinition sampleWhileLoopMethod = currentTypeDefintion.Methods.First( method => method.Name == "WhileLoopMethod" );
            Assert.AreEqual( 2 , _cyclomaticComplexityAnalyzer.GetMethodCyclomaticComplexity( sampleWhileLoopMethod ) );
        }


        /// <summary>
        /// This methods checks complexity of methods having combination of loops and conditional statements
        /// </summary>
        [TestMethod]
        public void CheckCombinedCasesComplexity()
        {
            MethodDefinition sampleIfElseAndLoopMethod = currentTypeDefintion.Methods.First( method => method.Name == "LoopAndIfElseMethod1" );
            Assert.AreEqual( 4 , _cyclomaticComplexityAnalyzer.GetMethodCyclomaticComplexity( sampleIfElseAndLoopMethod ) );

            MethodDefinition sampleCombinedMethod = currentTypeDefintion.Methods.First( method => method.Name == "CombinedIFLoopTernaryMethod" );
            Assert.AreEqual( 8 , _cyclomaticComplexityAnalyzer.GetMethodCyclomaticComplexity( sampleCombinedMethod ) );
        }


        /// <summary>
        /// This methods checks complexity of methods having switch case statement
        /// But as switch case complexity is not guaranteed. It just checks whether complexity is above/below a certain value
        /// </summary>
        [TestMethod]
        public void CheckSwitchCaseComplexity()
        {
            MethodDefinition sampleSwitchMethod1 = currentTypeDefintion.Methods.First( method => method.Name == "SampleSwitchMethod1" );
            Assert.IsTrue( _cyclomaticComplexityAnalyzer.GetMethodCyclomaticComplexity( sampleSwitchMethod1 ) < 10 );
            Console.WriteLine( "Actual - 9" );
            Console.WriteLine( _cyclomaticComplexityAnalyzer.GetMethodCyclomaticComplexity( sampleSwitchMethod1 ) );

            MethodDefinition sampleSwitchMethod2 = currentTypeDefintion.Methods.First( method => method.Name == "SampleSwitchMethod2" );
            Assert.IsTrue( _cyclomaticComplexityAnalyzer.GetMethodCyclomaticComplexity( sampleSwitchMethod2 ) > 10 );
            Console.WriteLine( "Actual - 11" );
            Console.WriteLine( _cyclomaticComplexityAnalyzer.GetMethodCyclomaticComplexity( sampleSwitchMethod2 ) );
        }


        /// <summary>
        /// Rather than checking each method, cyclomatic complexity analyzer tests on a whole DLL and also multiple DLLs
        /// </summary>
        [TestMethod]
        public void CheckCompleteDLL()
        {
            string dllPath = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\BridgePattern.dll";

            ParsedDLLFile parsedDLL = new( dllPath );

            AnalyzerBase cyclomaticComplexityAnalyzer = new CyclomaticComplexity( new() { parsedDLL , currentParsedDLL } );

            Dictionary<string , AnalyzerResult> analyzerResultDict = cyclomaticComplexityAnalyzer.AnalyzeAllDLLs();
            AnalyzerResult bridgeAnalyzerResult = analyzerResultDict[parsedDLL.DLLFileName];
            AnalyzerResult currentAnalyzerResult = analyzerResultDict[currentParsedDLL.DLLFileName];

            Assert.AreEqual( 1 , bridgeAnalyzerResult.Verdict );   // no violation found
            Assert.AreEqual( 0 , currentAnalyzerResult.Verdict );  // violations found i.e. complexity > 10 for some method

            Console.WriteLine( bridgeAnalyzerResult.ErrorMessage );
            Console.WriteLine( currentAnalyzerResult.ErrorMessage );
        }


        /// <summary>
        /// This method is just to check the complexity of all methods in the Analyzer module
        /// Currently there exists methods crossing max allowed value of cyclomatic complexity
        /// </summary>
        [TestMethod]
        public void CheckAnalyzerDLL()
        {
            string dllPath = "..\\..\\..\\..\\Analyzer\\bin\\Debug\\net6.0\\Analyzer.dll";
            ParsedDLLFile parsedDLL = new( dllPath );
            AnalyzerBase cyclomaticComplexityAnalyzer = new CyclomaticComplexity( new() { parsedDLL } );

            Dictionary<string , AnalyzerResult> analyzerResultDict = cyclomaticComplexityAnalyzer.AnalyzeAllDLLs();
            AnalyzerResult parsedDLLAnalyzerResult = analyzerResultDict[parsedDLL.DLLFileName];
            Console.WriteLine( parsedDLLAnalyzerResult.ErrorMessage );

            Assert.AreEqual( 0 , parsedDLLAnalyzerResult.Verdict );
        }
    }
}


// TEST CASES FOR CYCLOMATIC COMPLEXITY
namespace SampleComplexityTestCases
{
    public class SampleComplexityTestClass
    {
        public static void IfElseMethod()
        {
            int x = 0;

            if (x == 0)
            {
                Console.WriteLine( x );
            }
            else
            {
                Console.WriteLine( x + 1 );
            }
        }


        public void NestedIfElseMethod()
        {
            int x = 0;

            if (x != 1)
            {
                Console.WriteLine( x + 1 );

                if (x != 2)
                {
                    Console.WriteLine( x + 2 );
                }
                else
                {
                    Console.WriteLine( x );
                }
            }
        }


        public void ForLoopMethod()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine( i );
            }
        }

        public void WhileLoopMethod()
        {
            int x = 2;
            while (x < 10)
            {
                Console.WriteLine( "Hello" );
                ForLoopMethod();
                x++;
            }
            Console.WriteLine( "3" );
        }

        public void LoopAndIfElseMethod1()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine( i );

                if (i == 2)
                {
                    int y;

                    if (i > 1)
                    {
                        y = i + 1;
                    }
                    else
                    {
                        y = i - 1;
                    }

                    Console.WriteLine( y );
                }
            }
        }

        public void TernaryOperatorMethod()
        {
            int x = 9;
            int y = (x > 1) ? x - 1 : x - 2;

            Console.WriteLine( y );
        }

        public void CombinedIFLoopTernaryMethod()
        {
            int x = 4;

            if (x == 1)
            {
                int y = ((x + 1) > 3) ? 4 : 5;
                Console.WriteLine( y );
            }
            else if (x == 2)
            {

            }
            else if (x == 3)
            {

            }
            else if (x == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    int z = i;
                    while (z < 3)
                    {
                        Console.WriteLine( z );
                        z++;
                    }
                }
            }
        }

        public void SampleSwitchMethod1()
        {
            int option = 9;
            switch (option)
            {
                case 100:
                    Console.WriteLine( "Option 1 selected" );
                    break;

                case 200:
                    Console.WriteLine( "Option 2 selected" );
                    break;

                case 3:
                    Console.WriteLine( "Option 3 selected" );
                    break;

                case 400:
                    Console.WriteLine( "Option 4 selected" );
                    if (option + 2 == 5)
                    {
                        Console.WriteLine( "Random" );
                    }
                    else
                    {
                        Console.WriteLine( "Random2" );
                    }
                    break;

                case 5:
                    Console.WriteLine( "Option 5 selected" );
                    break;

                default:
                    int x = 1;
                    int y = x < 2 ? 2 : 3;
                    Console.WriteLine( $"Invalid option selected - {y}" );
                    break;
            }
        }

        public void SampleSwitchMethod2()
        {
            int x = 0;
            switch (x)
            {
                case 0:
                case 3:
                case 5:
                case 6:
                    break;
                case 2:
                    Console.WriteLine();
                    if (x + 1 == 20)
                    {
                        Console.WriteLine( "Hello" );

                        if (x + 2 == 30)
                        {
                            Console.WriteLine( "Again" );
                        }
                    }
                    else
                    {
                        Console.WriteLine( "4" );
                    }
                    break;
                case 1:
                    Console.WriteLine( "1" );
                    int y = x > 1 ? 2 : 3;
                    Console.WriteLine( y );
                    break;
                default:
                    SampleSwitchMethod2();
                    break;
            }
        }
    }
}

