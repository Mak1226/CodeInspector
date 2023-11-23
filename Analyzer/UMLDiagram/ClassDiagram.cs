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
        private readonly StringBuilder _plantUMLCode;
        private byte[] _plantUMLImage;
        private readonly List<ParsedClassMonoCecil> _parsedClassList;
        private readonly List<ParsedInterface> _parsedInterfaceList;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dllFiles"></param>
        public ClassDiagram(List<ParsedDLLFile> dllFiles) : base(dllFiles)
        {
            _plantUMLCode = new StringBuilder();
            _plantUMLImage = new byte[1];

            _parsedClassList = new List<ParsedClassMonoCecil>(); 
            _parsedInterfaceList = new List<ParsedInterface>();

            foreach(ParsedDLLFile dllFile in dllFiles)
            {
                _parsedClassList.AddRange(dllFile.classObjListMC);
                _parsedInterfaceList.AddRange(dllFile.interfaceObjList);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> Run(List<string> removableNamespaces)
        {
            CodeStr(removableNamespaces);
            var factory = new RendererFactory();
            IPlantUmlRenderer renderer = factory.CreateRenderer(new PlantUmlSettings());

            // Create the PlantUML diagram code
            // string plantUmlCode = "@startuml\r\nclass Car {}\r\n\r\nclass Engine\r\n\r\nCar *-- Engine : contains\r\nEngine <-- Car2\r\n@enduml";
            //string plantUmlCode = "@startuml\r\n\r\n!define hasAttributes \r\n!define hasMethods \r\n\r\nclass MyClass {\r\n    !if (hasAttributes)\r\n    - field1: int\r\n    - field2: string\r\n    !endif\r\n\r\n    !if (hasMethods)\r\n    + method1()\r\n    + method2()\r\n    !endif\r\n}\r\n\r\n@enduml\r\n";

            try
            {
                System.Diagnostics.Debug.WriteLine(_plantUMLCode.ToString());

                string emptyDiagramUMLString = "@startuml\r\n\r\n@enduml";

                if(_plantUMLCode.ToString() != emptyDiagramUMLString)
                {
                    // Render the PlantUML diagram asynchronously
                    _plantUMLImage = await renderer.RenderAsync( _plantUMLCode.ToString() , OutputFormat.Png );
                    System.Diagnostics.Debug.WriteLine( _plantUMLImage );
                    System.Diagnostics.Debug.Assert( _plantUMLImage != null );

                    return _plantUMLImage;
                }
                else
                {
                    return Array.Empty<byte>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't render image ", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CodeStr(List<string> removableNamespaces)
        {
            List<ParsedClassMonoCecil> graphParsedClassObj = new();
            List<ParsedInterface> graphParsedInterfaceObj = new();
            _plantUMLCode.Append( "@startuml\r\n" );
            foreach (ParsedClassMonoCecil classObj in _parsedClassList)
            {
                if (!isPartOfRemovableNamespace(classObj.TypeObj.FullName.Insert(0,"C"), removableNamespaces))
                {
                    graphParsedClassObj.Add(classObj);
                    
                    _plantUMLCode.Append($"class {classObj.TypeObj.FullName}{{}}\r\n");
                }
            }

            foreach (ParsedInterface interfaceObj in _parsedInterfaceList)
            {
                if (!isPartOfRemovableNamespace(interfaceObj.TypeObj.FullName.Insert(0,"I"), removableNamespaces))
                {
                    graphParsedInterfaceObj.Add(interfaceObj);
                    _plantUMLCode.Append($"interface {interfaceObj.TypeObj.FullName}\r\n");
                }
            }

            foreach (ParsedClassMonoCecil classObj in graphParsedClassObj)
            {
                HashSet<string> compositionList = classObj.CompositionList;
                HashSet<string> aggregationList = classObj.AggregationList;
                HashSet<string> inheritanceList = classObj.InheritanceList;
                HashSet<string> usingList = classObj.UsingList;


                AddElement("-->", usingList, classObj.TypeObj.FullName , removableNamespaces);
                AddElement("o--", aggregationList, classObj.TypeObj.FullName, removableNamespaces);


                foreach (string inheritedFrom in inheritanceList)
                {
                    if (CheckIfInterface(inheritedFrom) && !isPartOfRemovableNamespace(inheritedFrom , removableNamespaces))
                    {
                        _plantUMLCode.AppendLine($"class {classObj.TypeObj.FullName} implements {RemoveFirstLetter(inheritedFrom)}");
                    }
                    else
                    {
                        _plantUMLCode.AppendLine($"class {classObj.TypeObj.FullName} extends {RemoveFirstLetter(inheritedFrom)}");
                    }

                }

                AddElement("*--", compositionList, classObj.TypeObj.FullName, removableNamespaces);
            }

            foreach (ParsedInterface interfaceObj in graphParsedInterfaceObj)
            {
                foreach (Type parent in interfaceObj.ParentInterfaces)
                {
                    if(!isPartOfRemovableNamespace(parent.FullName.Insert(0, "I") , removableNamespaces))
                    {
                        _plantUMLCode.AppendLine($"interface {interfaceObj.TypeObj.FullName} implements {parent}");
                    }
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
        private void AddElement(string relationSymbol, HashSet<string> relationshipList, string typeFullName , List<string> removableNamespaces)
        {
            string relationStatement = string.Empty;

            foreach (string relationName in relationshipList)
            {
                if (!isPartOfRemovableNamespace(relationName, removableNamespaces))
                {
                    _plantUMLCode.Append(relationStatement + $" {typeFullName} {relationSymbol} {RemoveFirstLetter(relationName)}\r\n");
                }
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

        /*
        private bool isPartOfRemovableNamespace(string objName, List<string> removableNamespaces)
        {
            string[] splittedString = objName.Split(".");

            if (removableNamespaces != null && removableNamespaces.Contains( splittedString[0].Remove(0 , 1)))
            {
                Console.WriteLine(objName);
                return true;
            }
            else
            {
                return false;
            }
        }
        */

        // checks if the given namespace name in present in the removable namespace list
        private bool isPartOfRemovableNamespace( string objName , List<string> removableNamespaces )
        {
            // string[] splittedString = objName.Split( "." );
            bool check = false;
            if (removableNamespaces != null)
            {
                foreach (string rem in removableNamespaces)
                {
                    if (!objName.Remove( 0 , 1 ).Contains(rem + "."))
                    {
                        continue;
                    }
                    else
                    {
                        check = true;
                        break;
                    }
                }
            }

            return check;
        }
        
    }
}
