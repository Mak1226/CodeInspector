/******************************************************************************
* Filename    = ClassDiagram.cs
* 
* Author      = Sneha Bhattacharjee, Nikhitha Atyam
*
* Product     = Analyzer
* 
* Project     = Analyzer
*
* Description = UML Class relationship diagram rendered using PlantUML.
*****************************************************************************/

using Analyzer.Parsing;
using PlantUml.Net;
using System.Text;


namespace Analyzer.UMLDiagram
{
    /// <summary>
    /// This class creates classDiagram for the List of parsedDLL files passed as input
    /// And also accepts the namespaces which can be removed from class diagram as per the need
    /// </summary>
    public class ClassDiagram : DiagramBase
    {
        private readonly StringBuilder _plantUMLCode;
        private byte[] _plantUMLImage;
        private readonly List<ParsedClassMonoCecil> _parsedClassList;
        private readonly List<ParsedInterface> _parsedInterfaceList;


        // Collecting all the classes and interfaces from the collection of parsedDLLs
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
        /// Provides image bytes of class diagram
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> Run(List<string> removableNamespaces)
        {
            CodeStr(removableNamespaces);
            var factory = new RendererFactory();
            IPlantUmlRenderer renderer = factory.CreateRenderer(new PlantUmlSettings());

            try
            {
                System.Diagnostics.Debug.WriteLine(_plantUMLCode.ToString());

                string emptyDiagramUMLString = "@startuml\r\nhide empty members\r\nskinparam groupInheritance 2\r\nskinparam groupAggregation 2\r\nskinparam groupComposition 2\r\n\r\n@enduml";

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
        /// Generates the plantUML code(string) in the required format from the class relationships
        /// </summary>
        /// <param name="removableNamespaces">Namespaces which should not be included in the classDiagram</param>
        private void CodeStr(List<string> removableNamespaces)
        {
            // Class objects list and interface objects list as they will be different in the graph due to removable namespaces
            List<ParsedClassMonoCecil> graphParsedClassObj = new();
            List<ParsedInterface> graphParsedInterfaceObj = new();

            // Start of plantUMLcode
            _plantUMLCode.Append( "@startuml\r\nhide empty members\r\nskinparam groupInheritance 2\r\nskinparam groupAggregation 2\r\nskinparam groupComposition 2\r\n" );

            // Adding all the classes which are not part of removable Namespaces to the the diagram
            foreach (ParsedClassMonoCecil classObj in _parsedClassList)
            {
                if (!isPartOfRemovableNamespace(classObj.TypeObj.FullName.Insert(0,"C"), removableNamespaces))
                {
                    graphParsedClassObj.Add(classObj);
                    
                    _plantUMLCode.Append($"class {classObj.TypeObj.FullName}{{}}\r\n");
                }
            }

            // Adding all the interfaces which are not part of removable Namespaces to the the diagram
            foreach (ParsedInterface interfaceObj in _parsedInterfaceList)
            {
                if (!isPartOfRemovableNamespace(interfaceObj.TypeObj.FullName.Insert(0,"I"), removableNamespaces))
                {
                    graphParsedInterfaceObj.Add(interfaceObj);
                    _plantUMLCode.Append($"interface {interfaceObj.TypeObj.FullName}\r\n");
                }
            }

            // Adding the relationships between classes/interfaces according to class relationship lists 
            foreach (ParsedClassMonoCecil classObj in graphParsedClassObj)
            {
                HashSet<string> compositionList = classObj.CompositionList;
                HashSet<string> aggregationList = classObj.AggregationList;
                HashSet<string> inheritanceList = classObj.InheritanceList;
                HashSet<string> usingList = classObj.UsingList;

                // adding using relationships to diagram code
                AddElement("-->", usingList, classObj.TypeObj.FullName , removableNamespaces);

                // adding aggregation relationships to diagram code
                AddElement("o--", aggregationList, classObj.TypeObj.FullName, removableNamespaces);

                // For ParentClasses => Inheritance symbol and For Parent Interfaces => Implements symbol for a class object
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

                // adding composition relationships to diagram code
                AddElement( "*--", compositionList, classObj.TypeObj.FullName, removableNamespaces);
            }

            // Similar to class object, interface object also has parent interfaces being implemented
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

            // End of plantUMLCode
            _plantUMLCode.Append("\r\n@enduml");
            System.Diagnostics.Debug.Assert(_plantUMLCode != null);
        }


        /// <summary>
        /// Adding the relationship between relationSymbol and elements of relationshipList with a symbol of "typeFullName"
        /// </summary>
        /// <param name="relationSymbol">Left side of plantuml relation</param>
        /// <param name="relationshipList">Elements of this list will form right side of plantuml relation</param>
        /// <param name="typeFullName">relationShipSymbol</param>
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


        // Checking if given name is Interface or not as names in class relationship lists starts with either "I" or "C"
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


        // Removing the first letter of a string
        private string RemoveFirstLetter(string type)
        {
            return type.Remove(0, 1);
        }


        // checks if the given object is part of removable namespaces
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
