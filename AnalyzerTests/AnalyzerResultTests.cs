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

        [TestMethod]
        public void AnalyzerResultEqualsAndHashCodeSuccess()
        {
            // Arrange
            var result1 = new AnalyzerResult("Analyzer1", 1, "No errors");
            var result2 = new AnalyzerResult("Analyzer1", 1, "No errors");
            var result3 = new AnalyzerResult("Analyzer2", 0, "Some errors");

            // Act & Assert
            // Check Equals method
            Assert.IsTrue(result1.Equals(result2));
            Assert.IsFalse(result1.Equals(result3));

            // Check operator==
            Assert.IsTrue(result1 == result2);
            Assert.IsFalse(result1 == result3);

            // Check operator!=
            Assert.IsFalse(result1 != result2);
            Assert.IsTrue(result1 != result3);

            // Check GetHashCode method
            Assert.AreEqual(result1.GetHashCode(), result2.GetHashCode());
            Assert.AreNotEqual(result1.GetHashCode(), result3.GetHashCode());
        }

        [TestMethod]
        public void AnalyzerResultToStringSuccess()
        {
            var result = new AnalyzerResult("Analyzer1", 1, "No errors");
            var resultString = result.ToString();
            Assert.AreEqual("AnalyzerID: Analyzer1, Verdict: 1, ErrorMessage: No errors", resultString);
        }

        [TestMethod]
        public void AnalyzerResultCheckIsEqual()
        {
            // Arrange
            var result1 = new AnalyzerResult("Analyzer1", 1, "No errors");
            var result2 = new AnalyzerResult("Analyzer1", 1, "No errors");
            var result3 = new AnalyzerResult("Analyzer2", 0, "Some errors");

            // Act & Assert
            // Check Equals method
            Assert.IsTrue(result1.Equals(result2));
            Assert.IsFalse(result1.Equals(result3));
            Assert.IsFalse(result1.Equals(null));

            // Check operator==
            Assert.IsTrue(result1 == result2);
            Assert.IsFalse(result1 == result3);

            // Check operator!=
            Assert.IsFalse(result1 != result2);
            Assert.IsTrue(result1 != result3);

            // Check GetHashCode method
            Assert.AreEqual(result1.GetHashCode(), result2.GetHashCode());
            Assert.AreNotEqual(result1.GetHashCode(), result3.GetHashCode());
        }
    }
}