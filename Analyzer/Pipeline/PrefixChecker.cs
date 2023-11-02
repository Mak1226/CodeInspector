using System;
using Analyzer.Parsing;

namespace Analyzer.Pipeline.Analyzers
{
    /// <summary>
    /// An analyzer that checks the correctness of type name prefixes in parsed DLL files.
    /// </summary>
    public class PrefixCheckerAnalyzer : BaseAnalyzer
    {
        public PrefixCheckerAnalyzer(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
        }

        public override int GetScore()
        {
            int errorCount = 0;

            foreach (var type in parsedDLLFiles.Types)
            {
                if (type.IsInterface)
                {
                    if (!IsCorrectInterfaceName(type.Name))
                    {
                        Console.WriteLine($"[Error] Incorrect interface prefix: {type.Name}");
                        errorCount++;
                    }
                }
                else
                {
                    if (!IsCorrectTypeName(type.Name))
                    {
                        Console.WriteLine($"[Error] Incorrect type prefix: {type.Name}");
                        errorCount++;
                    }
                }
            }

            return errorCount;
        }

        private bool IsCorrectInterfaceName(string name)
        {
            return name.Length >= 2 && name[0] == 'I' && char.IsUpper(name[1]);
        }

        private bool IsCorrectTypeName(string name)
        {
            return name.Length < 3 || char.IsLower(name[1]) || (char.IsUpper(name[0]) && char.IsUpper(name[2]));
        }
    }
}
