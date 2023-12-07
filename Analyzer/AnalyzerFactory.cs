/******************************************************************************
* Filename    = AnalyzerFactory.cs
*
* Author      = Mangesh Dalvi
* 
* Roll No     = 112001010
*
* Product     = Code Inspector
* 
* Project     = Analyzer
*
* Description = Factory class for creating analyzers and retrieving configuration options.
******************************************************************************/

namespace Analyzer
{
    /// <summary>
    /// Factory class for creating analyzers and retrieving configuration options.
    /// </summary>
    public static class AnalyzerFactory
    {
        /// <summary>
        /// Retrieves a list of all available configuration options for analyzers.
        /// </summary>
        /// <returns>A list of tuples containing analyzer IDs and their corresponding descriptions.</returns>
        public static List<Tuple<int, string>> GetAllConfigurationOptions()
        {
            List<Tuple<int, string>> analyzers = new()
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
                Tuple.Create(110, "Void Async Methods"),
                Tuple.Create(111, "Abstract class naming checker"),
                Tuple.Create(112, "Casing Checker"),
                Tuple.Create(113, "Cyclomatic Complexity"),
                Tuple.Create(114, "New Line Literal Rule"),
                Tuple.Create(115, "Prefix checker"),
                Tuple.Create(116, "Switch Statement default case checker"),
                Tuple.Create(117, "Avoid goto statements"),
                Tuple.Create(118, "Avoid visible instance fields"),
                Tuple.Create(119, "High parameter count rule"),
                Tuple.Create(120, "Not implement checker"),
            };

            return analyzers;
        }

        /// <summary>
        /// Creates an instance of the default analyzer.
        /// </summary>
        /// <returns>An instance of the default analyzer.</returns>
        public static IAnalyzer GetAnalyzer()
        {
            IAnalyzer Analyzer = new Analyzer();
            return Analyzer;
        }

    }
}
