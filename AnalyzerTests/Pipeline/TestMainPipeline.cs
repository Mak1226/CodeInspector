using Analyzer.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnalyzerTests.Pipeline
{
    [TestClass]
    public class TestMainPipeline
    {
        [TestMethod]
        public void StartShouldRunAnalyzersAndPopulateResults()
        {
            // Arrange
            var pipeline = new MainPipeline();
            var teacherOptions = new Dictionary<int, bool>
            {
                {101, true},
                {102, false},
                // Add more options as needed
            };
            pipeline.AddTeacherOptions(teacherOptions);

            var dllFiles = new List<string>
            {
                "..\\..\\..\\TestDLLs\\Abstract.dll",
                "..\\..\\..\\TestDLLs\\BridgePattern.dll",
                "..\\..\\..\\TestDLLs\\Proxy.dll"
            };
            pipeline.AddDLLFiles(dllFiles);

            var results = pipeline.Start();

            Assert.IsNotNull(results);
            Assert.AreEqual(dllFiles.Count, results.Count);
        }
    }
}
