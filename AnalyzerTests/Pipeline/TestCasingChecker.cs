// Created By - Monesh Vanga (112001047)
using Analyzer.Parsing;
using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AnalyzerTests.Pipeline
{
    /// <summary>
    /// Test class for testing the analyzer - PrefixChecker.
    /// </summary>
    [TestClass()]
    public class TestCasingChecker
    {
        /// <summary>
        /// Test method for a case in which classes don't follow the above mentioned rule.
        /// </summary>
        /// 

        //CasingChecker.cs

        //namespace badNamespace
        //{
        //    public interface iBadInterface
        //    {
        //        // ...
        //    }

        //    public class badClass
        //    {
        //        public bool badMethod1(int BadName)
        //        {
        //            if (BadName == 0)
        //            {
        //                return true;
        //            }

        //            return true;
        //        }

        //        public bool badMethod2(int _BadName)
        //        {
        //            return false;
        //        }
        //    }
        //}

        [TestMethod()]
        public void TestBadExample()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\CasingChecker.dll";
            var parsedDllObj = new ParsedDLLFile(path);

            DllFileObjs.Add(parsedDllObj);

            CasingChecker casingChecker = new(DllFileObjs);

            Dictionary<string, Analyzer.AnalyzerResult> resultObj = casingChecker.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["CasingChecker.dll"];

            Assert.AreEqual(0, result.Verdict);
        }

        /// <summary>
        /// Test method for a case in which all classes follow the rule 
        /// </summary>
        
        //CasingChecker1.cs

        //namespace GoodNamespace
        //{
        //    public interface IGoodInterface
        //    {
        //        // ...
        //    }

        //    public class GoodClass
        //    {
        //        public bool GoodMethod1(int goodName)
        //        {
        //            if (goodName == 0)
        //            {
        //                return true;
        //            }

        //            return true;
        //        }

        //        public bool GoodMethod2(int _goodName)
        //        {
        //            return false;
        //        }
        //    }
        //}

        [TestMethod()]
        public void TestGoodExample()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\TestDLLs\\CasingChecker1.dll";
            var parsedDllObj = new ParsedDLLFile(path);

            DllFileObjs.Add(parsedDllObj);

            CasingChecker casingChecker = new(DllFileObjs);

            Dictionary<string, Analyzer.AnalyzerResult> resultObj = casingChecker.AnalyzeAllDLLs();

            Analyzer.AnalyzerResult result = resultObj["CasingChecker1.dll"];
            Assert.AreEqual(1, result.Verdict);
        }

    }
}

