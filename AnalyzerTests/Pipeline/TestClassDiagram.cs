﻿/******************************************************************************
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

using System.Collections;
using Analyzer.Parsing;
using Mono.Cecil;
using Analyzer.UMLDiagram;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace AnalyzerTests.Pipeline
{
    /// <summary>
    /// Unit Tests for ClassDiagram.cs
    /// </summary>
    [TestClass()]
    public class TestClassDiagram
    {
        private readonly string _dllFile;
        private readonly ParsedDLLFile _parsedDLL;
        public TestClassDiagram()
        {
            _dllFile = Assembly.GetExecutingAssembly().Location;
            _parsedDLL = new( _dllFile );
        }
        private static bool IsAutoGenerated( Type type )
        {
            try
            {
                return type.IsDefined( typeof( CompilerGeneratedAttribute ) , false );
            }
            catch (Exception e)
            {
                Trace.WriteLine( e.Message );
                return false;
            }
        }

        private static bool IsAutoGeneratedMC( TypeDefinition type )
        {
            try
            {
                return type.CustomAttributes.Any( attr => attr.AttributeType.FullName ==
                typeof( System.Runtime.CompilerServices.CompilerGeneratedAttribute ).FullName );
            }
            catch (Exception e)
            {
                Trace.WriteLine( e.Message );
                return false;
            }
        }
        /// <summary>
        /// Bridge pattern DLL.
        /// </summary>
        [TestMethod()]
        public void TestVerifyImages()
        {
            List<string> DllFilePaths = new()
            {
                // ExamplarDLLs.BridgePattern
                 "..\\..\\..\\TestDLLs\\BridgePattern.dll"
            };

            List<ParsedDLLFile> dllFiles = new() { new ParsedDLLFile(DllFilePaths[0]) };

            ClassDiagram classDiag = new(dllFiles);

            List<string> removableNamespaces = new() { };

            byte[] imageBytes = classDiag.RenderImageBytes(removableNamespaces, true).Result;

            Console.WriteLine(imageBytes);
            Console.WriteLine(imageBytes.Length);
            Assert.IsNotNull(imageBytes);
            Assert.AreNotEqual(imageBytes.Length, 0);

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out1.svg" , imageBytes );
            }
        }

        /// <summary>
        /// Remove one namespace.
        /// </summary>
        [TestMethod()]
        public void TestVerifyImagesAfterRemovingNamespaces()
        {
            _parsedDLL.classObjList.RemoveAll( cls => !(cls.TypeObj.FullName.StartsWith("AnalyzerTests") || (cls.TypeObj.FullName.StartsWith( "Mono" ))));
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            ClassDiagram classDiag = new( parseddllFiles );
            List<string> removableNamespaces = new() { "Analyzer" };

            byte[] imageBytes = classDiag.RenderImageBytes( removableNamespaces, true).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreNotEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out2.svg" , imageBytes );
            }

            //List<string> DllFilePaths = new()
            //{
            //    "..\\..\\..\\TestDLLs\\AnalyzerTests.dll"
            //};

            //List<ParsedDLLFile> dllFiles = new() { new ParsedDLLFile( DllFilePaths[0] ) };

            //ClassDiagram classDiag = new( dllFiles );

            //List<string> removableNamespaces = new() { "Analyzer" };

            //byte[] imageBytes = classDiag.RenderImageBytes( removableNamespaces ).Result;

            //Console.WriteLine( imageBytes );
            //Console.WriteLine( imageBytes.Length );
            //Assert.IsNotNull( imageBytes );
            //Assert.AreNotEqual( imageBytes.Length , 0 );

            //if (imageBytes != null && imageBytes.Length > 0)
            //{
            //    File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out2.svg" , imageBytes );
            //}
        }

        /// <summary>
        /// Remove multiple namespaces.
        /// </summary>
        [TestMethod()]
        public void TestVerifyImageAfterRemovingManyNamespaces()
        {
            List<ParsedDLLFile> parseddllFiles = new() { _parsedDLL };

            ClassDiagram classDiag = new( parseddllFiles );
            List<string> removableNamespaces = new() { "Analyzer", "Mono.Cecil" };

            byte[] imageBytes = classDiag.RenderImageBytes( removableNamespaces, true ).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreNotEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out3.svg" , imageBytes );
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

            byte[] imageBytes = classDiag.RenderImageBytes( removableNamespaces, true ).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreNotEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out4.svg" , imageBytes );
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

            byte[] imageBytes = classDiag.RenderImageBytes( removableNamespaces, true ).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreNotEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out5.svg" , imageBytes );
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

            byte[] imageBytes = classDiag.RenderImageBytes( removableNamespaces, true ).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreNotEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out6.svg" , imageBytes );
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
                "..\\..\\..\\TestDLLs\\AnalyzerTests.dll"
            };

            List<ParsedDLLFile> dllFiles = new() { new ParsedDLLFile( DllFilePaths[0] ) };

            ClassDiagram classDiag = new( dllFiles );

            List<string> removableNamespaces = new() { "Analyzer" , "Mono.Cecil.Cil.Instruction" };

            byte[] imageBytes = classDiag.RenderImageBytes( removableNamespaces, true ).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreNotEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out7.svg" , imageBytes );
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

            byte[] imageBytes = classDiag.RenderImageBytes( removableNamespaces, true ).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreNotEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out8.svg" , imageBytes );
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

            byte[] imageBytes = classDiag.RenderImageBytes( removableNamespaces, true ).Result;

            Console.WriteLine( imageBytes );
            Console.WriteLine( imageBytes.Length );
            Assert.IsNotNull( imageBytes );
            Assert.AreNotEqual( imageBytes.Length , 0 );

            if (imageBytes != null && imageBytes.Length > 0)
            {
                File.WriteAllBytes( "..\\..\\..\\UMLDiagramResults\\out9.svg" , imageBytes );
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

            byte[] imageBytes = classDiag.RenderImageBytes( removableNamespaces, false ).Result;

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
