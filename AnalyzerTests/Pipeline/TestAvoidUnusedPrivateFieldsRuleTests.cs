using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class TestAvoidUnusedPrivateFieldsRuleTests
    {
        [TestMethod()]
        public void Test1()
        {

            List<string> DllFilePaths = new List<string>();

            DllFilePaths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\ClassLibrary1.dll");

            ParsedDLLFiles dllFiles = new(DllFilePaths);

            AvoidUnusedPrivateFieldsRule avoidUnusedPrivateFieldsRule = new(dllFiles);

            var result = avoidUnusedPrivateFieldsRule.Run();
            
            Console.WriteLine(result.ErrorMessage);

            Assert.AreEqual(1, result.Verdict);

        }

        [TestMethod()]
        public void Test2()
        {

            List<string> DllFilePaths = new List<string>();

            DllFilePaths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\UnusedPrivateFields.dll");

            ParsedDLLFiles dllFiles = new(DllFilePaths);

            AvoidUnusedPrivateFieldsRule avoidUnusedPrivateFieldsRule = new(dllFiles);

            var result = avoidUnusedPrivateFieldsRule.Run();

            Console.WriteLine(result.ErrorMessage);

            Assert.AreEqual(0, result.Verdict);

        }

    }
}