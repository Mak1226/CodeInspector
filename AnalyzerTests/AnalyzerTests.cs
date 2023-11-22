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

            IDictionary<int , bool> teacherOptions = new Dictionary<int , bool>
            {
                [101] = true ,
                [102] = true ,
                [103] = true ,
                [104] = true ,
                [105] = true ,
                [106] = true ,
                [107] = true ,
                [108] = true ,
                [109] = true ,
                [110] = true ,
                [111] = true ,
                [112] = true ,
                [113] = true ,
                [114] = true ,
                [115] = true ,
                [116] = true
            };

            analyzer.Configure(teacherOptions);

            List<string> paths = new()
            {
                "..\\..\\..\\..\\Analyzer\\TestDLLs\\ClassLibrary1.dll" ,

                "..\\..\\..\\..\\Analyzer\\TestDLLs\\BridgePattern.dll"
            };

            //string path = "C:\\Users\\HP\\source\\repos\\Demo1\\ClassLibrary1\\bin\\Debug\\net6.0\\ClassLibrary1.dll";

            //paths.Add(path);

            analyzer.LoadDLLFileOfStudent(paths);

            Dictionary<string, List<AnalyzerResult>> result = analyzer.Run();

           foreach(KeyValuePair<string , List<AnalyzerResult>> dll in result)
           {
                Console.WriteLine(dll.Key);

                foreach(AnalyzerResult res in dll.Value)
                {
                    Console.WriteLine(res.AnalyserID + " " + res.Verdict + " " + res.ErrorMessage);
                }
           }

           // AppDomain.Unload(domain);

            Assert.Fail();
        }
    }
}
