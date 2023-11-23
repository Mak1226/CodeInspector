/******************************************************************************
* Filename    = AnalyzerFactoryTests.cs
*
* Author      = Mangesh Dalvi
* 
* Roll No     = 112001010
*
* Product     = Code Inspector
* 
* Project     = AnalyzerTests
*
* Description = Unit tests for the AnalyzerFactory class.
******************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analyzer.Tests
{
    /// <summary>
    /// Unit tests for the AnalyzerFactory class.
    /// </summary>
    [TestClass()]
    public class AnalyzerFactoryTests
    {
        [TestMethod]
        public void TestGetAllConfigurationOptions()
        {
            var expectedOptions = new List<Tuple<int, string>>
            {
                Tuple.Create(101, "Abstract type no public constructor"),
                Tuple.Create(102, "Avoid constructor in static types"),
                Tuple.Create(103, "Avoid unused private fields"),
                Tuple.Create(104, "Avoid empty interface"),
                Tuple.Create(105, "Depth of inheritance should be less than 3"),
                Tuple.Create(106, "Array field should not be read only"),
                Tuple.Create(107, "Avoid switch statements"),
                Tuple.Create(108, "Disposable field should be disposed"),
                Tuple.Create(109, "Avoid unused local variables"),
                Tuple.Create(110, "Useless control flow rule"),
                Tuple.Create(111, "Abstract class naming checker"),
                Tuple.Create(112, "Casing Checker"),
                Tuple.Create(113, "Cyclomatic Complexity"),
                Tuple.Create(114, "New Line Literal Rule"),
                Tuple.Create(115, "Prefix checker"),
                Tuple.Create(116, "Switch Statement default case checker"),
                Tuple.Create(117, "Avoid goto statements"),
                Tuple.Create(118, "Native fields should not be visible"),
                Tuple.Create(119, "High parameter count rule")
            };

            List<Tuple<int , string>> actualOptions = AnalyzerFactory.GetAllConfigurationOptions();
            CollectionAssert.AreEqual(expectedOptions, actualOptions);
        }

        [TestMethod]
        public void TestGetAnalyzer()
        {
            IAnalyzer analyzer = AnalyzerFactory.GetAnalyzer();
            Assert.IsNotNull(analyzer);
            Assert.IsInstanceOfType(analyzer, typeof(IAnalyzer));
        }
    }
}
