using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analyzer.Tests
{
    [TestClass()]
    public class AnalyzerFactoryTests
    {

        [TestMethod]
        public void TestGetAllConfigurationOptions()
        {
            // Arrange
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
                Tuple.Create(114, "New Linelteral Rule"),
                Tuple.Create(115, "Prefix checker"),
                Tuple.Create(116, "Switch Statement default case checker"),
                Tuple.Create(117, "Avoid goto statements"),
                Tuple.Create(118, "Native fields should not be visible")
            };

            // Act
            var actualOptions = AnalyzerFactory.GetAllConfigurationOptions();

            // Assert
            CollectionAssert.AreEqual(expectedOptions, actualOptions);
        }

        [TestMethod]
        public void TestGetAnalyzer()
        {
            // Arrange
            // You may need to use a mocking framework to mock IAnalyzer if it has complex behavior.

            // Act
            var analyzer = AnalyzerFactory.GetAnalyzer();

            // Assert
            Assert.IsNotNull(analyzer);
            Assert.IsInstanceOfType(analyzer, typeof(IAnalyzer));
        }
    }
}