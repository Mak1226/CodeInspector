using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Parsing;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Analyzer.Pipeline
{
    internal class NoHardCodedPaths : BaseAnalyzer
    {
        // (back)slash counters
        private int slashes;
        private int backslashes;

        // method body being checked
        private MethodBody method_body;

        // point counter
        private int current_score;

        private static Dictionary<string, bool> resultCache = new Dictionary<string, bool>();

        public NoHardCodedPaths(ParsedDLLFiles dllFiles) : base(dllFiles)
        {
            // TODO if required
        }
        void AddPoints(int pts)
        {
            current_score += pts;
        }

        public Dictionary<string, bool> FindHardCodedPaths()
        {
            return resultCache;
        }

        static bool CanBeWindowsAbsolutePath(string s)
        {
            // true for strings like ?:\*
            // e.g. 'C:\some\path' or 'D:\something.else"
            return s[1] == ':' && s[2] == '\\';
        }

        static bool CanBeWindowsUNCPath(string s)
        {
            // true for Windows UNC paths
            // e.g. \\Server\Directory\File
            return s[0] == '\\' && s[1] == '\\';
        }

        static bool CanBeUnixAbsolutePath(string s)
        {
            // true for strings like /*
            return s[0] == '/';
        }

        public override int GetScore()
        {
            return 0;
        }
    }
}
