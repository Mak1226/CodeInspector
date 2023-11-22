using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analyzer.Tests
{
    [TestClass()]
    public class AnalyzerResultTests
    {
        [TestMethod]
        public void TestEquality()
        {
            // Arrange
            var result1 = new AnalyzerResult("ID1", 1, "Error 1");
            var result2 = new AnalyzerResult("ID1", 1, "Error 1");
            var result3 = new AnalyzerResult("ID2", 2, "Error 2");

            // Act & Assert
            Assert.AreEqual(result1, result2);
            Assert.AreNotEqual(result1, result3);
            Assert.AreNotEqual(result2, result3);
        }

        [TestMethod]
        public void TestInequality()
        {
            // Arrange
            var result1 = new AnalyzerResult("ID1", 1, "Error 1");
            var result2 = new AnalyzerResult("ID1", 1, "Error 1");
            var result3 = new AnalyzerResult("ID2", 2, "Error 2");

            // Act & Assert
            Assert.IsFalse(result1 != result2);
            Assert.IsTrue(result1 != result3);
            Assert.IsTrue(result2 != result3);
        }
    }
}