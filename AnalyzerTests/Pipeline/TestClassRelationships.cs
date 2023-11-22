using Analyzer.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerTests.Pipeline
{
    /// <summary>
    /// Test class to verify the functionality of retrieving class relationships from given dll file.
    /// </summary>
    [TestClass()]
    public class TestClassRelationships
    {
        [TestMethod()]
        public void CheckRelationshipsList()
        {
            List<ParsedDLLFile> DllFileObjs = new();

            string path = "..\\..\\..\\..\\Analyzer\\TestDLLs\\TypeRelationships.dll";
            var parsedDllObj = new ParsedDLLFile(path);

            DllFileObjs.Add(parsedDllObj);

            int classes = 0;
            foreach (ParsedDLLFile dllFileObj in DllFileObjs)
            {
                foreach (ParsedClassMonoCecil cls in dllFileObj.classObjListMC)
                {
                    classes++;
                }
            }

            //check diff Relationship Lists
            Assert.AreEqual(5, classes);

            Dictionary<string, List<string>> InheritanceRel = new();
            Dictionary<string, List<string>> CompositionRel = new();
            Dictionary<string, List<string>> AggregationRel = new();
            Dictionary<string, List<string>> UsingRel = new();

            foreach (ParsedDLLFile dllFileObj in DllFileObjs)
            {
                foreach (ParsedClassMonoCecil cls in dllFileObj.classObjListMC)
                {
                    Debug.WriteLine("\n\n\n\n");
                    Console.WriteLine("Class: " + cls.Name);
                    Console.WriteLine("Inheritance: ");
                    foreach (string inhCls in cls.InheritanceList)
                    {
                        Console.WriteLine(inhCls);
                        if (!InheritanceRel.ContainsKey(cls.Name))
                        {
                            InheritanceRel[cls.Name] = new List<string>();
                        }
                        InheritanceRel[cls.Name].Add(inhCls);
                    }
                    Console.WriteLine("------------------------------------");
                    Console.WriteLine("Composiition: ");
                    foreach (string compCls in cls.CompositionList)
                    {
                        Console.WriteLine(compCls);
                        if (!CompositionRel.ContainsKey(cls.Name))
                        {
                            CompositionRel[cls.Name] = new List<string>();
                        }
                        CompositionRel[cls.Name].Add(compCls);
                    }
                    Console.WriteLine("------------------------------------");

                    Console.WriteLine("Aggregation: ");
                    foreach (string aggCls in cls.AggregationList)
                    {
                        Console.WriteLine(aggCls);
                        if (!AggregationRel.ContainsKey(cls.Name))
                        {
                            AggregationRel[cls.Name] = new List<string>();
                        }
                        AggregationRel[cls.Name].Add(aggCls);
                    }

                    Console.WriteLine("------------------------------------");

                    Console.WriteLine("Using: ");
                    foreach (string useCls in cls.UsingList)
                    {
                        Console.WriteLine(useCls);
                        if (!UsingRel.ContainsKey(cls.Name))
                        {
                            UsingRel[cls.Name] = new List<string>();
                        }
                        UsingRel[cls.Name].Add(useCls);

                    }
                    Console.WriteLine("------------------------------------");

                }
            }

            Dictionary<string, List<string>> InheritanceExp = new();
            Dictionary<string, List<string>> CompositionExp = new();
            Dictionary<string, List<string>> AggregationExp = new();
            Dictionary<string, List<string>> UsingExp = new();

            InheritanceExp["Student"] = new List<string> { "CTypeRelationships.Person" };
            CompositionExp["Car"] = new List<string> { "CTypeRelationships.Engine" };
            AggregationExp["StudentCar"] = new List<string> { "CTypeRelationships.Car" };
            UsingExp["StudentCar"] = new List<string> { "CTypeRelationships.Student" };

            foreach (string key in InheritanceRel.Keys)
            {
                CollectionAssert.AreEqual(InheritanceExp[key], InheritanceRel[key]);
            }
            foreach (string key in CompositionRel.Keys)
            {
                CollectionAssert.AreEqual(CompositionExp[key], CompositionRel[key]);
            }
            foreach (string key in AggregationRel.Keys)
            {
                CollectionAssert.AreEqual(AggregationExp[key], AggregationRel[key]);
            }
            foreach (string key in UsingRel.Keys)
            {
                CollectionAssert.AreEqual(UsingExp[key], UsingRel[key]);
            }
        }
    }
}
