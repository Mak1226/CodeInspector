/******************************************************************************
* Filename    = TestClassDiagram.cs
* 
* Author      = Sneha Bhattacharjee, Atyam Lakshmi Nikhitha
*
* Product     = Analyzer
* 
* Project     = AnalyzerTests
*
* Description = Unit Tests for ClassDiagram.cs
*****************************************************************************/

using Analyzer.Parsing;
using Analyzer.Pipeline;
using Analyzer.UMLDiagram;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlantUml.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerTests.Pipeline
{
    [TestClass()]
    public class TestClassDiagram
    {
        [TestMethod()]
        public void TestVerifyImages()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\BridgePattern.dll"
            };

            List<ParsedDLLFile> dllFiles = new() { new ParsedDLLFile(DllFilePaths[0]) };

            ClassDiagram classDiag = new(dllFiles);

            List<string> removableNamespaces = new() { };

            byte[] imageBytes = classDiag.Run(removableNamespaces).Result;

            Console.WriteLine(imageBytes);
            Console.WriteLine(imageBytes.Length);
            Assert.IsNotNull(imageBytes);
            Assert.AreNotEqual(imageBytes.Length, 0);

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "C:\\Users\\sneha\\OneDrive\\Desktop\\Sem_7\\out1.png" , imageBytes );
            }
        }

        [TestMethod()]
        public void TestVerifyImagesAfterRemovingNamespaces()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\AnalyzerTests.dll"
            };

            List<ParsedDLLFile> dllFiles = new() { new ParsedDLLFile( DllFilePaths[0] ) };

            ClassDiagram classDiag = new( dllFiles );

            List<string> removableNamespaces = new() { "Analyzer" };

            byte[] imageBytes = classDiag.Run( removableNamespaces ).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreNotEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "C:\\Users\\sneha\\OneDrive\\Desktop\\Sem_7\\out2.png" , imageBytes );
            }
        }

        [TestMethod()]
        public void TestVerifyImageAfterRemovingManyNamespaces()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\AnalyzerTests.dll"
            };

            List<ParsedDLLFile> dllFiles = new() { new ParsedDLLFile( DllFilePaths[0] ) };

            ClassDiagram classDiag = new( dllFiles );

            List<string> removableNamespaces = new() { "Analyzer", "Mono.Cecil" };

            byte[] imageBytes = classDiag.Run( removableNamespaces ).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreNotEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "C:\\Users\\sneha\\OneDrive\\Desktop\\Sem_7\\out3.png" , imageBytes );
            }
        }

        [TestMethod()]
        public void TestVerifyImageAfterRemovingNestedNamespaces()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\AnalyzerTests.dll"
            };

            List<ParsedDLLFile> dllFiles = new() { new ParsedDLLFile( DllFilePaths[0] ) };

            ClassDiagram classDiag = new( dllFiles );

            List<string> removableNamespaces = new() { "Analyzer" , "AnalyzerTests.Pipeline" };

            byte[] imageBytes = classDiag.Run( removableNamespaces ).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreNotEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "C:\\Users\\sneha\\OneDrive\\Desktop\\Sem_7\\out4.png" , imageBytes );
            }
        }

        [TestMethod()]
        public void TestVerifyImageAfterRemovingNestedNamespaces2()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\AnalyzerTests.dll"
            };

            List<ParsedDLLFile> dllFiles = new() { new ParsedDLLFile( DllFilePaths[0] ) };

            ClassDiagram classDiag = new( dllFiles );

            List<string> removableNamespaces = new() { "Analyzer" , "Analyzer.Pipeline" };

            byte[] imageBytes = classDiag.Run( removableNamespaces ).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreNotEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "C:\\Users\\sneha\\OneDrive\\Desktop\\Sem_7\\out5.png" , imageBytes );
            }
        }

        [TestMethod()]
        public void TestVerifyImageAfterRemovingNestedNamespaces3()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\AnalyzerTests.dll"
            };

            List<ParsedDLLFile> dllFiles = new() { new ParsedDLLFile( DllFilePaths[0] ) };

            ClassDiagram classDiag = new( dllFiles );

            List<string> removableNamespaces = new() { "Analyzer" , "Mono.Cecil.Cil" };

            byte[] imageBytes = classDiag.Run( removableNamespaces ).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreNotEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "C:\\Users\\sneha\\OneDrive\\Desktop\\Sem_7\\out6.png" , imageBytes );
            }
        }

        [TestMethod()]
        public void TestVerifyImageAfterRemovingClass1()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\AnalyzerTests.dll"
            };

            List<ParsedDLLFile> dllFiles = new() { new ParsedDLLFile( DllFilePaths[0] ) };

            ClassDiagram classDiag = new( dllFiles );

            List<string> removableNamespaces = new() { "Analyzer" , "Mono.Cecil.Cil.Instruction" };

            byte[] imageBytes = classDiag.Run( removableNamespaces ).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreNotEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "C:\\Users\\sneha\\OneDrive\\Desktop\\Sem_7\\out7.png" , imageBytes );
            }
        }

        [TestMethod()]
        public void TestVerifyImageUsingMultipleDLL()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\AnalyzerTests.dll",
                "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\BridgePattern.dll"
            };

            List<ParsedDLLFile> dllFiles = new() { new ParsedDLLFile( DllFilePaths[0] ), new ParsedDLLFile( DllFilePaths[1] ) };

            ClassDiagram classDiag = new( dllFiles );

            List<string> removableNamespaces = new() { "Analyzer" , "Mono.Cecil.Cil.Instruction" };

            byte[] imageBytes = classDiag.Run( removableNamespaces ).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreNotEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "C:\\Users\\sneha\\OneDrive\\Desktop\\Sem_7\\out8.png" , imageBytes );
            }
        }

        [TestMethod()]
        public void TestSamples()
        {
            List<string> DllFilePaths = new()
            {
                "C:\\Users\\sneha\\OneDrive\\Desktop\\Sem_7\\software\\Analyzer\\Dashboard\\bin\\Debug\\net6.0-windows\\System.Memory.Data.dll"
            //"..\\..\\..\\..\\Analyzer\\bin\\Debug\\net6.0\\Analyzer.dll"
            };

            List<ParsedDLLFile> dllFiles = new() { new ParsedDLLFile( DllFilePaths[0] ) };

            ClassDiagram classDiag = new( dllFiles );

            List<string> removableNamespaces = new() { };

            byte[] imageBytes = classDiag.Run( removableNamespaces ).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreNotEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "C:\\Users\\sneha\\OneDrive\\Desktop\\Sem_7\\out9.png" , imageBytes );
            }
        }
    }
}
