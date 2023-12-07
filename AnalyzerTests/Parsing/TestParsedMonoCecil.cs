/******************************************************************************
* Filename    = TestParsedMonoCecil.cs
* 
* Author      = Yukta Salunkhe
* 
* Project     = Analyzer
*
* Description = Testing the generation of ParsedClassMonoCecil Object
*****************************************************************************/

using System.Reflection;
using Analyzer.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnalyzerTests.Pipeline
{
    /// <summary>
    /// Testing the generation of ParsedClassMonoCecil Object
    /// </summary>
    [TestClass()]
    public class TestParsedMonoCecil
    {
        /// <summary>
        /// Testing all memebers of the generated ParsedClassMonoCecil Object
        /// </summary>
        [TestMethod()]
        public void TestMonoCecilTypes()
        {
            string dllFile = Assembly.GetExecutingAssembly().Location;
            ParsedDLLFile parsedDLL = new(dllFile);
            List<ParsedClassMonoCecil> classObjList = parsedDLL.classObjListMC;
            foreach (ParsedClassMonoCecil? cls in classObjList)
            {
                if (cls.Name == "TestNullNamespaceClassMC" || cls.TypeObj.Namespace == null)
                {
                    Assert.AreEqual("", cls.TypeObj.Namespace);
                }
                //Console.WriteLine(cls.Name);
                //Console.WriteLine(cls.TypeObj.Namespace);
                if (cls.Name == "BMW" && cls.TypeObj.Namespace == "TestParsedMonoCecil")
                {
                    Assert.AreEqual(2, cls.Constructors.Count);
                    Assert.AreEqual("TestParsedMonoCecil.Car", cls.ParentClass?.FullName);
                    Assert.AreEqual("TestParsedMonoCecil.IBMWSpec", cls.Interfaces[0].InterfaceType.FullName);
                    Assert.AreEqual(2, cls.MethodsList.Count);
                    Assert.AreEqual(2, cls.FieldsList.Count);
                    Assert.AreEqual(1, cls.PropertiesList.Count);
                }
            }
        }
    } 
}

class TestNullNamespaceClassMC
{
    TestNullNamespaceClassMC()
    {
        Console.WriteLine("This class has no namespace");
    }
}
namespace TestParsedMonoCecil
{
    public interface IVehicle
    {

    }
    public interface ICar : IVehicle
    {

    }

    public interface IBMWSpec
    {

    }
    public class Car : ICar
    {
        public Car() { }
    }

    public class BMW : Car, IBMWSpec
    {
        private readonly string _name = "BMW";
        public int seatCapacity;
        public BMW()
        {
            Console.WriteLine("BMW Car");
        }
        public BMW(string model) { }

        public string Name
        {
            get 
            {
                Console.WriteLine();
                return _name; 
            }
        }
        public void Drive()
        {
            Console.WriteLine("Driving ");
        }
    }
}

namespace TestParsedMonoCecil2
{
    public class BaseClass
    {
        public string field1;
    }
    public class ChildClass : BaseClass
    {
        public string field2;
        public string prop1 { get; set; }
        public ChildClass()
        {
            Console.WriteLine("--");
        }
    }
}

//namespace TestCompilerGenerated
//{
//    public class CheckCompilerGenerated
//    {
//        public CheckCompilerGenerated()
//        {
//            int val = 30;

//            Action lambda = () => { Console.WriteLine( "value: " + val ); };
//            lambda();
//        }
//}
//}

