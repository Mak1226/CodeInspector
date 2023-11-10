/*using Analyzer.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerTests.Pipeline
{
    [TestClass()]
    public class TestDepthOfInheritance
    {
        [TestMethod]
        public void CalculateDepthOfInheritance_WithSampleData_ReturnsCorrectDepths()
        {
            // Arrange
            var typeA = typeof(ClassA);
            var typeB = typeof(ClassB);
            var typeC = typeof(ClassC);

            List<string> DllFilePaths = new List<string>();

            DllFilePaths.Add("..\\..\\..\\..\\Analyzer\\TestDLLs\\DemoDLL.dll");

            ParsedDLLFiles dllFiles = new(DllFilePaths);
            {
                classObjList = new List<ParsedClass>
                {
                    new ParsedClass { TypeObj = typeA },
                    new ParsedClass { TypeObj = typeB },
                    new ParsedClass { TypeObj = typeC }
                }
            };

            DepthOfInheritance depthOfInheritance = new DepthOfInheritance(dllFiles);

            // Act
            var result = depthOfInheritance.CalculateDepthOfInheritance();

            // Assert
            Assert.AreEqual(0, result[typeA]);
            Assert.AreEqual(1, result[typeB]);
            Assert.AreEqual(0, result[typeC]);
        }

        [TestMethod]
        public void CalculateDepthOfInheritance_WithEmptyData_ReturnsEmptyDictionary()
        {
            // Arrange
            ParsedDLLFiles dllFiles = new ParsedDLLFiles
            {
                classObjList = new List<ParsedClass>()
            };

            DepthOfInheritance depthOfInheritance = new DepthOfInheritance(dllFiles);

            // Act
            var result = depthOfInheritance.CalculateDepthOfInheritance();

            // Assert
            CollectionAssert.IsEmpty(result);
        }
    }

    // Sample classes for testing
    public class ClassA { }
    public class ClassB : ClassA { }
    public class ClassC { }

}
*/