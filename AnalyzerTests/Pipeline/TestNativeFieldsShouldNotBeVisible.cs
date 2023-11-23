using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer.Parsing;

namespace TestNativeFieldsShouldNotBeVisible
{
    public class PublicClassWithPublicField
    {
        public int _publicField;
    }

    public class HasPublicNativeField
    {
        public IntPtr Native;
    }

    public class HasProtectedNativeField
    {
        protected IntPtr Native;
    }

    public class HasInternalNativeField
    {
        internal IntPtr Native;
    }

    public class HasPublicReadonlyNativeField
    {
        public readonly IntPtr Native;
    }

    public class HasPublicNativeFieldArray
    {
        public IntPtr[]? Native;
    }

    public class HasPublicReadonlyNativeFieldArray
    {
        public IntPtr[]? Native;
    }

    public class HasPublicNativeFieldArrayArray
    {
        public IntPtr[][]? Native;
    }

    public class HasPublicNonNativeField
    {
        public object? Field;
    }

    public class HasPrivateNativeField
    {
        private IntPtr Native;
    }
    
    public class ClassHasProperty
    {
        private int value {  get; }        
    }
        
}

namespace Analyzer.Pipeline.Tests
{
    [TestClass()]
    public class TestNativeFieldsShouldNotBeVisible
    {
        [TestMethod()]
        public void TestHasPublicTypeWithNoFields()
        {
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\NativeFieldsShouldNotBeVisible1.dll";

            ParsedDLLFile dllFile = new( path );

            List<ParsedDLLFile> dllFiles = new() { dllFile };
            NoEmptyInterface noEmptyInterfaces = new( dllFiles );

            Dictionary<string , AnalyzerResult> result = noEmptyInterfaces.AnalyzeAllDLLs();
            Console.WriteLine( result[dllFile.DLLFileName].ErrorMessage );
            Assert.AreEqual( 1 , result[dllFile.DLLFileName].Verdict );
        }
    }
}
