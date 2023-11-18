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
            var factory = new RendererFactory();
            var renderer = factory.CreateRenderer(new PlantUmlSettings());

            //string plantUmlCode = "@startuml\r\nclass Car {}\r\n\r\nclass Engine\r\n\r\nCar *-- Engine : contains\r\nEngine <-- Car2\r\n@enduml";

            //ClassDiagram classDig = new ClassDiagram(plantUmlCode);


            List<string> DllFilePaths = new List<string>();

            DllFilePaths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\BridgePattern.dll");
            //DllFilePaths.Add("C:\\Users\\nikhi\\source\\repos\\BridgePatternDemo\\UnitTests\\bin\\Debug\\UnitTests.dll");
            //DllFilePaths.Add("C:\\Users\\nikhi\\source\\repos\\dotnet-reflection-demo-master\\MonoCecilExecution\\bin\\Debug\\MonoCecilExecution.dll");

            List<ParsedDLLFile> dllFiles = new() { new ParsedDLLFile(DllFilePaths[0]) };

            ClassDiagram classDiag = new(dllFiles);

            List<string> removableNamespaces = new() { };

            byte[] imageBytes = classDiag.Run(removableNamespaces).Result;

            Console.WriteLine(imageBytes);
            Console.WriteLine(imageBytes.Length);
            Assert.AreNotEqual(imageBytes.Length, 0);
            //File.WriteAllBytes("C:\\Users\\nikhi\\source\\repos\\out.png", imageBytes);


            //var factory = new RendererFactory();
            //var renderer = factory.CreateRenderer(new PlantUmlSettings());
            //string plantumlcode = "@startuml\r\n Analyzer.Analyzer o-- Analyzer.Pipeline.MainPipeline\r\n Analyzer.Analyzer o-- Analyzer.DynamicAnalyzer.InvokeCustomAnalyzers\r\nclass Analyzer.Analyzer implements Analyzer.IAnalyzer\r\n Analyzer.AnalyzerFactory o-- Analyzer.Analyzer\r\nclass Analyzer.AnalyzerResult implements Analyzer.IAnalyzerResult\r\n Analyzer.UMLDiagram.ClassDiagram o-- Analyzer.UMLDiagram.ClassDiagram/<Run>d__5\r\nclass Analyzer.UMLDiagram.ClassDiagram extends Analyzer.UMLDiagram.DiagramBase\r\n Analyzer.Pipeline.AbstractTypeNoPublicConstructor --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.AbstractTypeNoPublicConstructor o-- Analyzer.AnalyzerResult\r\nclass Analyzer.Pipeline.AbstractTypeNoPublicConstructor extends Analyzer.Pipeline.AnalyzerBase\r\n Analyzer.Pipeline.AnalyzerBase --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.ArrayFieldsShouldNotBeReadOnlyRule --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.ArrayFieldsShouldNotBeReadOnlyRule o-- Analyzer.AnalyzerResult\r\nclass Analyzer.Pipeline.ArrayFieldsShouldNotBeReadOnlyRule extends Analyzer.Pipeline.AnalyzerBase\r\n Analyzer.Pipeline.AvoidConstructorsInStaticTypes --> Analyzer.Parsing.ParsedClass\r\n Analyzer.Pipeline.AvoidConstructorsInStaticTypes --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.AvoidConstructorsInStaticTypes o-- Analyzer.AnalyzerResult\r\nclass Analyzer.Pipeline.AvoidConstructorsInStaticTypes extends Analyzer.Pipeline.AnalyzerBase\r\n Analyzer.Pipeline.AvoidGotoStatementsAnalyzer --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.AvoidGotoStatementsAnalyzer o-- Analyzer.AnalyzerResult\r\nclass Analyzer.Pipeline.AvoidGotoStatementsAnalyzer extends Analyzer.Pipeline.AnalyzerBase\r\n Analyzer.Pipeline.AvoidSwitchStatementsAnalyzer --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.AvoidSwitchStatementsAnalyzer o-- Analyzer.AnalyzerResult\r\nclass Analyzer.Pipeline.AvoidSwitchStatementsAnalyzer extends Analyzer.Pipeline.AnalyzerBase\r\n Analyzer.Pipeline.AvoidUnusedPrivateFieldsRule --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.AvoidUnusedPrivateFieldsRule o-- Analyzer.AnalyzerResult\r\nclass Analyzer.Pipeline.AvoidUnusedPrivateFieldsRule extends Analyzer.Pipeline.AnalyzerBase\r\n Analyzer.Pipeline.CyclomaticComplexity --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.CyclomaticComplexity --> Mono.Cecil.MethodDefinition\r\n Analyzer.Pipeline.CyclomaticComplexity --> Mono.Cecil.Cil.Instruction\r\n Analyzer.Pipeline.CyclomaticComplexity o-- Analyzer.AnalyzerResult\r\nclass Analyzer.Pipeline.CyclomaticComplexity extends Analyzer.Pipeline.AnalyzerBase\r\n Analyzer.Pipeline.DepthOfInheritance --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.DepthOfInheritance o-- Analyzer.AnalyzerResult\r\nclass Analyzer.Pipeline.DepthOfInheritance extends Analyzer.Pipeline.AnalyzerBase\r\n Analyzer.Pipeline.DisposableFieldsShouldBeDisposedRule --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.DisposableFieldsShouldBeDisposedRule --> Mono.Cecil.TypeDefinition\r\n Analyzer.Pipeline.DisposableFieldsShouldBeDisposedRule --> Mono.Collections.Generic.Collection`1<Mono.Cecil.Cil.Instruction>\r\n Analyzer.Pipeline.DisposableFieldsShouldBeDisposedRule --> Mono.Cecil.FieldDefinition\r\n Analyzer.Pipeline.DisposableFieldsShouldBeDisposedRule o-- Analyzer.AnalyzerResult\r\nclass Analyzer.Pipeline.DisposableFieldsShouldBeDisposedRule extends Analyzer.Pipeline.AnalyzerBase\r\n Analyzer.Pipeline.MainPipeline o-- Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.MainPipeline o-- Analyzer.Pipeline.AbstractTypeNoPublicConstructor\r\n Analyzer.Pipeline.MainPipeline o-- Analyzer.Pipeline.AvoidUnusedPrivateFieldsRule\r\n Analyzer.Pipeline.MainPipeline o-- Analyzer.Pipeline.NoEmptyInterface\r\n Analyzer.Pipeline.MainPipeline o-- Analyzer.Pipeline.DepthOfInheritance\r\n Analyzer.Pipeline.MainPipeline o-- Analyzer.Pipeline.ArrayFieldsShouldNotBeReadOnlyRule\r\n Analyzer.Pipeline.MainPipeline o-- Analyzer.Pipeline.AvoidSwitchStatementsAnalyzer\r\n Analyzer.Pipeline.MainPipeline o-- Analyzer.Pipeline.DisposableFieldsShouldBeDisposedRule\r\n Analyzer.Pipeline.MainPipeline o-- Analyzer.Pipeline.RemoveUnusedLocalVariablesRule\r\n Analyzer.Pipeline.MainPipeline o-- Analyzer.Pipeline.ReviewUselessControlFlowRule\r\n Analyzer.Pipeline.MainPipeline o-- Analyzer.Pipeline.NewLineLiteralRule\r\n Analyzer.Pipeline.MainPipeline o-- Analyzer.Pipeline.PrefixCheckerAnalyzer\r\n Analyzer.Pipeline.MainPipeline o-- Analyzer.Pipeline.SwitchStatementDefaultCaseChecker\r\n Analyzer.Pipeline.MainPipeline o-- Analyzer.AnalyzerResult\r\n Analyzer.Pipeline.MainPipeline o-- Analyzer.Pipeline.MainPipeline/<>c__DisplayClass11_0\r\n Analyzer.Pipeline.MainPipeline o-- Analyzer.UMLDiagram.ClassDiagram\r\n Analyzer.Pipeline.NewLineLiteralRule --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.NewLineLiteralRule --> Mono.Cecil.MethodDefinition\r\n Analyzer.Pipeline.NewLineLiteralRule o-- Analyzer.AnalyzerResult\r\nclass Analyzer.Pipeline.NewLineLiteralRule extends Analyzer.Pipeline.AnalyzerBase\r\n Analyzer.Pipeline.NoEmptyInterface --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.NoEmptyInterface o-- Analyzer.AnalyzerResult\r\nclass Analyzer.Pipeline.NoEmptyInterface extends Analyzer.Pipeline.AnalyzerBase\r\n Analyzer.Pipeline.PrefixCheckerAnalyzer --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.PrefixCheckerAnalyzer o-- Analyzer.AnalyzerResult\r\nclass Analyzer.Pipeline.PrefixCheckerAnalyzer extends Analyzer.Pipeline.AnalyzerBase\r\n Analyzer.Pipeline.RemoveUnusedLocalVariablesRule --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.RemoveUnusedLocalVariablesRule --> Mono.Cecil.MethodDefinition\r\n Analyzer.Pipeline.RemoveUnusedLocalVariablesRule --> Mono.Cecil.Cil.VariableDefinition\r\n Analyzer.Pipeline.RemoveUnusedLocalVariablesRule --> Mono.Collections.Generic.Collection`1<Mono.Cecil.Cil.Instruction>\r\n Analyzer.Pipeline.RemoveUnusedLocalVariablesRule --> Mono.Cecil.Cil.Instruction\r\n Analyzer.Pipeline.RemoveUnusedLocalVariablesRule o-- Analyzer.AnalyzerResult\r\nclass Analyzer.Pipeline.RemoveUnusedLocalVariablesRule extends Analyzer.Pipeline.AnalyzerBase\r\n Analyzer.Pipeline.ReviewUselessControlFlowRule --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.ReviewUselessControlFlowRule --> Mono.Cecil.MethodDefinition\r\n Analyzer.Pipeline.ReviewUselessControlFlowRule --> Mono.Cecil.Cil.Instruction\r\n Analyzer.Pipeline.ReviewUselessControlFlowRule o-- Analyzer.AnalyzerResult\r\nclass Analyzer.Pipeline.ReviewUselessControlFlowRule extends Analyzer.Pipeline.AnalyzerBase\r\n Analyzer.Pipeline.SwitchStatementDefaultCaseChecker --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.Pipeline.SwitchStatementDefaultCaseChecker --> Mono.Cecil.MethodDefinition\r\n Analyzer.Pipeline.SwitchStatementDefaultCaseChecker --> Analyzer.Parsing.ParsedClassMonoCecil\r\n Analyzer.Pipeline.SwitchStatementDefaultCaseChecker o-- Analyzer.AnalyzerResult\r\nclass Analyzer.Pipeline.SwitchStatementDefaultCaseChecker extends Analyzer.Pipeline.AnalyzerBase\r\n Analyzer.Parsing.ParsedClassMonoCecil *-- Mono.Cecil.TypeDefinition\r\n Analyzer.Parsing.ParsedDLLFile o-- Analyzer.Parsing.ParsedStructure\r\n Analyzer.Parsing.ParsedDLLFile o-- Analyzer.Parsing.ParsedClass\r\n Analyzer.Parsing.ParsedDLLFile o-- Analyzer.Parsing.ParsedInterface\r\n Analyzer.Parsing.ParsedDLLFile o-- Analyzer.Parsing.ParsedClassMonoCecil\r\n Analyzer.DynamicAnalyzer.CustomAnalyzer --> Analyzer.Parsing.ParsedDLLFile\r\n Analyzer.DynamicAnalyzer.CustomAnalyzer o-- Analyzer.AnalyzerResult\r\nclass Analyzer.DynamicAnalyzer.CustomAnalyzer extends Analyzer.Pipeline.AnalyzerBase\r\n Analyzer.DynamicAnalyzer.InvokeCustomAnalyzers o-- Analyzer.Parsing.ParsedDLLFile\r\n\r\n@enduml";
            //byte[] imageBytes = renderer.RenderAsync(plantumlcode, OutputFormat.Png).Result;
            //File.WriteAllBytes("C:\\Users\\nikhi\\source\\repos\\out.png", imageBytes);
        }
    }
}
