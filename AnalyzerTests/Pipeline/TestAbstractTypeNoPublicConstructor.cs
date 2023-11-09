/*using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Parsing;

namespace Analyzer.Pipeline.Tests
{
    [TestClass()]
    public class TestAbstractTypeNoPublicConstructor
    {
        [TestMethod()]
        public void MainPipelineTest()
        {

            List<string> DllFilePaths = new List<string>();

            DllFilePaths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\DemoDLL.dll");

            ParsedDLLFiles dllFiles = new (DllFilePaths);

            AbstractTypeNoPublicConstructor abstractTypeNoPublicConstructor = new(dllFiles);

            var result = abstractTypeNoPublicConstructor.GetScore();

            Assert.AreEqual(1, result);
        }
    }
}*/