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

            //AppDomain domain = AppDomain.CurrentDomain;

            Analyzer analyzer = new();

            IDictionary<int, bool> teacherOptions = new Dictionary<int, bool>();
            teacherOptions[101] = true;
            teacherOptions[102] = true;
            teacherOptions[103] = true;
            teacherOptions[104] = true;
            teacherOptions[105] = true;
            teacherOptions[106] = true;
            teacherOptions[107] = true;
            teacherOptions[108] = true;
            teacherOptions[109] = true;
            teacherOptions[110] = true;
            teacherOptions[111] = true;
            teacherOptions[112] = true;
            teacherOptions[113] = true;
            teacherOptions[114] = true;
            teacherOptions[115] = true;
            teacherOptions[116] = true;

            analyzer.Configure(teacherOptions);

            List<string> paths = new();

            paths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\ClassLibrary1.dll");

            paths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\BridgePattern.dll");

            //string path = "C:\\Users\\HP\\source\\repos\\Demo1\\ClassLibrary1\\bin\\Debug\\net6.0\\ClassLibrary1.dll";

            //paths.Add(path);

            analyzer.LoadDLLFileOfStudent(paths);

            Dictionary<string, List<AnalyzerResult>> result = analyzer.Run();

           foreach(var dll in result)
           {
                Console.WriteLine(dll.Key);

                foreach(var res in dll.Value)
                {
                    Console.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
                }
           }

           // AppDomain.Unload(domain);

            Assert.Fail();
        }
    }
}