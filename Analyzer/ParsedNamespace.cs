using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    internal class ParsedNamespace
    {

        public string? _name;
        public List<ParsedClass>? ParsedClassList;

        ParsedNamespace() // Path of single dll file
        {

            // it should contain the root of the tree which is below the this namespace
            // read the DLL file
            

        }

    }
}
