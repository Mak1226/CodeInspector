using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Pipeline.Tests
{
    [TestClass]
    public class MainPipelineTests
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