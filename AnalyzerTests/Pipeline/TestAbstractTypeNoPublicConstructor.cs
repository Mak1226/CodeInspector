using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Parsing;
using System.IO;

namespace Analyzer.Pipeline.Tests
{
    public class SampleTestsAbstractClassNoPublicConstructor
    {
        public abstract class AbstractClassWithPublicConstructor
        {
            private readonly int _sampleVar;
            private int _sampleVar2;
            public AbstractClassWithPublicConstructor()
            {
                _sampleVar = 100;
            }

            private void SampleFunction( int sampleVar )
            {
                _sampleVar2 = sampleVar + _sampleVar;
            }
        }

        public abstract class AbstractClassWithProtectedConstructor
        {
            private readonly int _sampleVar;
            private readonly int _sampleVar2;
            protected AbstractClassWithProtectedConstructor()
            {
                _sampleVar = 100;
            }
        }

        public abstract class AbstractClassWithInternalConstructor
        {
            private readonly int _sampleVar;
            private readonly int _sampleVar2;
            internal AbstractClassWithInternalConstructor()
            {
                _sampleVar = 100;
                _sampleVar2 = 2 * _sampleVar;
            }
        }

        private abstract class PrivateAbstractClassWithPublicConstructor
        {
            private readonly int _sampleVar;
            private readonly int _sampleVar2;
            public PrivateAbstractClassWithPublicConstructor()
            {
                _sampleVar = 100;
                _sampleVar2 = 2 * _sampleVar;
            }
        }

        public abstract class AbstractClassWithNoDefaultConstructor
        {
            private readonly int _sampleVar;
            private readonly int _sampleVar2;
        }

        private abstract class PrivateAbstractClassWithNoDefaultConstructor
        {
            private readonly int _sampleVar;
            private readonly int _sampleVar2;
        }

        public class PublicClassWithNoDefaultConstructor
        {
            private readonly int _sampleVar;
            private readonly int _sampleVar2;
        }
    }

    [TestClass()]
    public class TestAbstractTypeNoPublicConstructor
    {
        [TestMethod()]
        public void TestAbstractConstructorPresent()
        {
            string path = "..\\..\\..\\..\\AnalyzerTests\\TestDLLs\\AbstractTypeNoPublicConstructor1.dll";

            ParsedDLLFile dllFile = new (path);

            List<ParsedDLLFile> dllFiles = new() { dllFile };
            AbstractTypeNoPublicConstructor abstractTypeNoPublicConstructor = new(dllFiles);

            Dictionary<string, AnalyzerResult> result = abstractTypeNoPublicConstructor.AnalyzeAllDLLs();
            Console.WriteLine(result[dllFile.DLLFileName].ErrorMessage);
            Assert.AreEqual(0, result[dllFile.DLLFileName].Verdict);
        }
    }
}
