using Analyzer.Parsing;
using PlantUml.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.UMLDiagram
{
    /// <summary>
    /// 
    /// </summary>
    public class ClassDiagram : DiagramBase
    {
        private StringBuilder _plantUMLCode;
        private Byte[] _plantUMLImage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dllFiles"></param>
        public ClassDiagram(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            _plantUMLCode = new StringBuilder();
            _plantUMLImage = new Byte[1];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Byte[]> Run()
        {
            CodeStr();
            var factory = new RendererFactory();
            var renderer = factory.CreateRenderer(new PlantUmlSettings());

            // Create the PlantUML diagram code
            // string plantUmlCode = "@startuml\r\nclass Car {}\r\n\r\nclass Engine\r\n\r\nCar *-- Engine : contains\r\nEngine <-- Car2\r\n@enduml";
            //string plantUmlCode = "@startuml\r\n\r\n!define hasAttributes \r\n!define hasMethods \r\n\r\nclass MyClass {\r\n    !if (hasAttributes)\r\n    - field1: int\r\n    - field2: string\r\n    !endif\r\n\r\n    !if (hasMethods)\r\n    + method1()\r\n    + method2()\r\n    !endif\r\n}\r\n\r\n@enduml\r\n";

            try
            {
                System.Diagnostics.Debug.WriteLine(_plantUMLCode.ToString());
                // Render the PlantUML diagram asynchronously
                _plantUMLImage = await renderer.RenderAsync(_plantUMLCode.ToString(), OutputFormat.Png);
                System.Diagnostics.Debug.WriteLine(_plantUMLImage);
                System.Diagnostics.Debug.Assert(_plantUMLImage != null);
                // Save the rendered diagram to a file
                File.WriteAllBytes("C:\\Users\\sneha\\OneDrive\\Desktop\\Sem_7\\out.png", _plantUMLImage.ToArray());
                // File.WriteAllBytes("C:\\Users\\sneha\\OneDrive\\Desktop\\Sem_7\\out.png", _plantUMLImage);
                // return bytes;
                Console.WriteLine("PlantUML diagram saved to out.png");
                return _plantUMLImage;
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't render image ", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CodeStr()
        {
            _plantUMLCode.Append("@startuml\r\n");
            //_plantUMLCode.AppendLine($"\r\nclass {parsedDLLFiles.classObjList[0]}");

            foreach (ParsedClassMonoCecil classObj in parsedDLLFiles.classObjListMC)
            {
                _plantUMLCode.Append($"class {classObj.TypeObj.FullName}{{}}\r\n");
            }

            foreach (ParsedInterface interfaceObj in parsedDLLFiles.interfaceObjList)
            {
                _plantUMLCode.Append($"interface {interfaceObj.TypeObj.FullName}\r\n");
            }

            foreach (ParsedClassMonoCecil classObj in parsedDLLFiles.classObjListMC)
            {
                List<string> compositionList = classObj.CompositionList;
                List<string> aggregationList = classObj.AggregationList;
                List<string> inheritanceList = classObj.InheritanceList;
                List<string> usingList = classObj.UsingList;


                AddElement("-->", usingList, classObj.TypeObj.FullName);
                AddElement("o--", aggregationList, classObj.TypeObj.FullName);

                
                foreach (string inheritedFrom in inheritanceList)
                {
                    if (CheckIfInterface(inheritedFrom))
                    {
                        _plantUMLCode.AppendLine($"class {classObj.TypeObj.FullName} implements {RemoveFirstLetter(inheritedFrom)}");
                    }
                    else
                    {
                        _plantUMLCode.AppendLine($"class {classObj.TypeObj.FullName} extends {RemoveFirstLetter(inheritedFrom)}");
                    }
                    
                }

                AddElement("*--", compositionList, classObj.TypeObj.FullName);
            }

            foreach (ParsedInterface interfaceObj in parsedDLLFiles.interfaceObjList)
            {
                foreach (Type parent in interfaceObj.ParentInterfaces)
                {
                    _plantUMLCode.AppendLine($"interface {interfaceObj.TypeObj.FullName} implements {parent}");
                }
                
            }

            _plantUMLCode.Append("\r\n@enduml");
            // _plantUMLCode = new("@startuml\r\nclass Car{}\r\nCar<--Engine\r\n@enduml");
            // _plantUMLCode = new("@startuml\r\nclass BridgePattern.Circle {}\r\nclass BridgePattern.DetailedView {}\r\nclass BridgePattern.BriefView {}\r\nclass BridgePattern.Shapes {}\r\nclass BridgePattern.Square {}\r\nBridgePattern.Circle --> BridgePattern.IDrawingView\r\nBridgePattern.Circle extends BridgePattern.Shapes\r\nBridgePattern.DetailedView extends BridgePattern.IDrawingView\r\nBridgePattern.BriefView extends BridgePattern.IDrawingView\r\nBridgePattern.Shapes --> BridgePattern.IDrawingView\r\nBridgePattern.Square --> BridgePattern.IDrawingView\r\nBridgePattern.Square extends BridgePattern.Shapes\r\n\r\n@enduml\r\n\r\n");


            //_plantUMLCode = new("@startuml\r\nclass BridgePattern.Circle{}\r\nclass BridgePattern.DetailedView{}\r\nclass BridgePattern.BriefView{}\r\nclass BridgePattern.Shapes{}\r\nclass BridgePattern.Square{} \r\n interface BridgePattern.IDrawingView{}\r\n BridgePattern.Circle --> BridgePattern.IDrawingView\r\nBridgePattern.Circle extends BridgePattern.Shapes\r\nBridgePattern.DetailedView implements BridgePattern.IDrawingView\r\nBridgePattern.BriefView implements BridgePattern.IDrawingView\r\nBridgePattern.Shapes --> BridgePattern.IDrawingView\r\nBridgePattern.Square --> BridgePattern.IDrawingView\r\nBridgePattern.Square extends BridgePattern.Shapes\r\n\r\n@enduml\r\n\r\n");
            // _plantUMLCode = new("@startuml\r\nclass BridgePattern.Circle{}\r\nclass BridgePattern.DetailedView{}\r\ninterface BridgePattern.IDrawingView\r\nclass BridgePattern.BriefView{}\r\nclass BridgePattern.Shapes{}\r\nclass BridgePattern.Square{}\r\nclass BridgePattern.Circle extends BridgePattern.Shapes\r\nBridgePattern.Circle --> BridgePattern.IDrawingView\r\nclass BridgePattern.DetailedView implements BridgePattern.IDrawingView\r\n@enduml");
            System.Diagnostics.Debug.Assert(_plantUMLCode != null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relationSymbol"></param>
        /// <param name="relationshipList"></param>
        /// <param name="typeFullName"></param>
        private void AddElement(string relationSymbol, List<string> relationshipList, string typeFullName)
        {
            string relationStatement = string.Empty;

            foreach (string relationName in relationshipList)
            {
                _plantUMLCode.Append(relationStatement + $" {typeFullName} {relationSymbol} {RemoveFirstLetter(relationName)}\r\n");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private bool CheckIfInterface(string type)
        {
            if (type.StartsWith('I'))
            {
                return true;
            }
            else if (type.StartsWith('C'))
            {
                return false;
            }

            throw new ArgumentException($"{type} is not in the correct format.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string RemoveFirstLetter(string type)
        {
            return type.Remove(0, 1);
        }
    }
}