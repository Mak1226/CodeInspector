using Analyzer.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerTests.Pipeline
{
    [TestClass()]
    public class TestClassRelationships
    {
        [TestMethod()]
        public void CheckRelationshipsList()
        {
            List<string> DllFilePaths = new List<string>();

            DllFilePaths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\TypeRelationships.dll");

            ParsedDLLFiles dllFiles = new(DllFilePaths);

            List<ParsedClassMonoCecil> parsedClasses = dllFiles.classObjListMC;

            //check diff Relationship Lists
            Assert.AreEqual(5, parsedClasses.Count);

            Dictionary<string, List<string>> InheritanceRel = new();
            Dictionary<string, List<string>> CompositionRel = new();
            Dictionary<string, List<string>> AggregationRel = new();
            Dictionary<string, List<string>> UsingRel = new();


            foreach (var cls in parsedClasses) {
                Console.WriteLine("Class: " + cls.Name);
                Console.WriteLine("Inheritance: ");
                foreach (var inhCls in cls.InheritanceList)
                {
                    Console.WriteLine(inhCls);
                    if (!InheritanceRel.ContainsKey(cls.Name))
                    {
                        InheritanceRel[cls.Name] = new List<string>();
                    }
                    InheritanceRel[cls.Name].Add(inhCls);
                    Console.WriteLine(InheritanceRel[cls.Name]);

                    //Console.WriteLine();
                }
                Console.WriteLine("------------------------------------");
                Console.WriteLine("Composiition: ");
                foreach (var compCls in cls.CompositionList)
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
                foreach (var aggCls in cls.AggregationList)
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
                foreach (var useCls in cls.UsingList)
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

            Dictionary<string, List<string>> InheritanceExp = new();
            Dictionary<string, List<string>> CompositionExp = new();
            Dictionary<string, List<string>> AggregationExp = new();
            Dictionary<string, List<string>> UsingExp = new();


            InheritanceExp["TypeRelationships.Student"] = new List<string> { "TypeRelationships.Person" };
            CompositionExp["TypeRelationships.Car"] = new List<string> { "TypeRelationships.Engine" };
            AggregationExp["TypeRelationships.StudentCar"] = new List<string> { "TypeRelationships.Car" };
            UsingExp["TypeRelationships.StudentCar"] = new List<string>{"TypeRelationships.Student"};
            //CollectionAssert.AreEquivalent(InheritanceExp,InheritanceRel);
            //CollectionAssert.AreEquivalent(CompositionExp,CompositionRel);
            //CollectionAssert.AreEquivalent(AggregationExp,AggregationRel);
            //CollectionAssert.AreEquivalent(UsingExp,UsingRel);


        }
    }
}
