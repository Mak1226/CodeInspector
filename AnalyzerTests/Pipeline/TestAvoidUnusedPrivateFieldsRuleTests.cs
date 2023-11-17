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
    [TestClass()]
    public class TestAvoidUnusedPrivateFieldsRuleTests
    {
        [TestMethod()]
        public void Test1()
        {

            //List<string> DllFilePaths = new List<string>();

            //string path = "C:\\Users\\HP\\source\\repos\\Demo1\\ClassLibrary1\\bin\\Debug\\net6.0\\ClassLibrary1.dll";

            string path = "..\\..\\..\\..\\Analyzer\\TestDLLs\\UnusedPrivateFields.dll";
            ParsedDLLFile dllFile = new ParsedDLLFile(path);

            //DllFilePaths.Add(path);

            List<ParsedDLLFile> dllFiles = new() { dllFile };

            AvoidUnusedPrivateFieldsRule avoidUnusedPrivateFieldsRule = new(dllFiles);

            var result = avoidUnusedPrivateFieldsRule.AnalyzeAllDLLs();

            Console.WriteLine(result[dllFile.DLLFileName].ErrorMessage);

            Assert.AreEqual(1, result[dllFile.DLLFileName].Verdict);

        }

/*        [TestMethod()]
        public void Test2()
        {

            List<string> DllFilePaths = new List<string>();

            DllFilePaths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\UnusedPrivateFields.dll");

            ParsedDLLFiles dllFiles = new(DllFilePaths);

            AvoidUnusedPrivateFieldsRule avoidUnusedPrivateFieldsRule = new(dllFiles);

            var result = avoidUnusedPrivateFieldsRule.Run();

            Console.WriteLine(result.ErrorMessage);

            Assert.AreEqual(0, result.Verdict);

        }*/

    }
}