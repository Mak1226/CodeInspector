using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Tests
{
    [TestClass()]
    public class AnalyzerTests
    {
        [TestMethod()]
        public void AnalyzerTest()
        {

            Analyzer analyzer = new();

            IDictionary<int, bool> teacherOptions = new Dictionary<int, bool>();
            teacherOptions[101] = true;
            teacherOptions[102] = true;
            teacherOptions[103] = true;
            teacherOptions[104] = true;

            analyzer.Configure(teacherOptions, false);

            List<string> paths = new();
            paths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\ClassLibrary1.dll");
            paths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\BridgePattern.dll");

            analyzer.LoadDLLFile(paths, string.Empty);

            var result = analyzer.Run();

            foreach (var item in result)
            {
                Console.WriteLine(item.AnalyserID);
                Console.WriteLine(item.Verdict);
                Console.WriteLine(item.ErrorMessage);
            }

            Assert.Fail();
        }
    }
}