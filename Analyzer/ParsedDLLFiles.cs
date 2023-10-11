using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    internal class ParsedDLLFiles
    {

        public List<ParsedNamespace>? _namespaces;

        ParsedDLLFiles(List<string> paths) // path of dll files
        {
            
            // take the input of all the dll files
            // it merge the all the ParsedNamespace

        }

    }
}


// it will call the constructor of the ParsedNamespace for each dll file