/******************************************************************************
* Filename    = TestClassDiagram.cs
* 
* Author      = Sneha Bhattacharjee
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
    /// <summary>
    /// Unit Tests for ClassDiagram.cs
    /// </summary>
    [TestClass()]
    public class TestClassDiagram
    {
        /// <summary>
        /// Bridge pattern DLL.
        /// </summary>
        [TestMethod()]
        public void TestVerifyImages()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\TestDLLs\\BridgePattern.dll"
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
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out1.png" , imageBytes );
            }
        }

        /// <summary>
        /// Remove one namespace.
        /// </summary>
        [TestMethod()]
        public void TestVerifyImagesAfterRemovingNamespaces()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\TestDLLs\\AnalyzerTests.dll"
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
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out2.png" , imageBytes );
            }
        }

        /// <summary>
        /// Remove multiple namespaces.
        /// </summary>
        [TestMethod()]
        public void TestVerifyImageAfterRemovingManyNamespaces()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\TestDLLs\\AnalyzerTests.dll"
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
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out3.png" , imageBytes );
            }
        }

        /// <summary>
        /// Removing only nested namespace.
        /// </summary>
        [TestMethod()]
        public void TestVerifyImageAfterRemovingNestedNamespaces()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\TestDLLs\\AnalyzerTests.dll"
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
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out4.png" , imageBytes );
            }
        }

        /// <summary>
        /// Trying to remove namespace that does not exist.
        /// </summary>
        [TestMethod()]
        public void TestVerifyImageAfterRemovingNestedNamespaces2()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\TestDLLs\\AnalyzerTests.dll"
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
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out5.png" , imageBytes );
            }
        }

        /// <summary>
        /// Removing only the nested namespace at depth two. 
        /// </summary>
        [TestMethod()]
        public void TestVerifyImageAfterRemovingNestedNamespaces3()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\TestDLLs\\AnalyzerTests.dll"
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
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out6.png" , imageBytes );
            }
        }

        /// <summary>
        /// Attempting to remove a class.
        /// Cannot be removed, only supports namespace removal.
        /// </summary>
        [TestMethod()]
        public void TestVerifyImageAfterRemovingClass1()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\TestDLLs\\.dll"
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
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out7.png" , imageBytes );
            }
        }

        /// <summary>
        /// Using multiple DLLs to generate the image.
        /// </summary>
        [TestMethod()]
        public void TestVerifyImageUsingMultipleDLL()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\TestDLLs\\AnalyzerTests.dll",
                "..\\..\\..\\TestDLLs\\BridgePattern.dll"
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
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out8.png" , imageBytes );
            }
        }

        /// <summary>
        /// Test with all four type relationships.
        /// </summary>
        [TestMethod()]
        public void TestAllRelationships()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\TestDLLs\\TypeRelationships.dll"
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
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out9.png" , imageBytes );
            }
        }

        /// <summary>
        /// Removing every namespace in the image; empty image.
        /// </summary>
        [TestMethod()]
        public void TestEmptyImage()
        {
            List<string> DllFilePaths = new()
            {
                "..\\..\\..\\TestDLLs\\TypeRelationships.dll"
            };

            List<ParsedDLLFile> dllFiles = new() { new ParsedDLLFile( DllFilePaths[0] ) };

            ClassDiagram classDiag = new( dllFiles );

            List<string> removableNamespaces = new() { "TypeRelationships" };

            byte[] imageBytes = classDiag.Run( removableNamespaces ).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out10.png" , imageBytes );
            }
        }
    }
}
