using Analyzer.Parsing;
using Analyzer.Pipeline;
using Analyzer.UMLDiagram;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlantUml.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerTests.Pipeline
{
    [TestClass]
    public class TestClassDiagram
    {
        [TestMethod]
        public async Task VerifyImages()
        {
            //var factory = new RendererFactory();
            //var renderer = factory.CreateRenderer(new PlantUmlSettings());

            //string plantUmlCode = "@startuml\r\nclass Car {}\r\n\r\nclass Engine\r\n\r\nCar *-- Engine : contains\r\nEngine <-- Car2\r\n@enduml";

            //ClassDi classDig = new ClassDi(plantUmlCode);


            List<string> DllFilePaths = new List<string>();

            DllFilePaths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\BridgePattern.dll");

            ParsedDLLFiles dllFiles = new(DllFilePaths);

            ClassDiagram classDig = new(dllFiles);
            byte[] imageBytes = await classDig.Run();
            Console.WriteLine("I came");

            Console.WriteLine(imageBytes);
            //File.WriteAllBytes("C:\\Users\\sneha\\OneDrive\\Desktop\\Sem_7\\out.png", classDig._plantUMLImage);
            Console.WriteLine(imageBytes.Length);
            Assert.AreNotEqual(imageBytes.Length, 0);

        }
    }
}
