using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Parsing;

namespace Analyzer
{
    public interface IParsedDLLFiles
    {
        List<ParsedClass> ClassObjList { get; }
        List<ParsedInterface> InterfaceObjList { get; }
        List<ParsedStructure> StructureObjList { get; }
        List<ParsedClassMonoCecil> ClassObjListMC { get; }
        Dictionary<Type, ParsedClass> MapTypeToParsedClass { get; }
        Dictionary<Type, ParsedClassMonoCecil> MapTypeDefinitionToParsedClass { get; }
    }

    public interface ITeacher
    {
        public void Run(IParsedDLLFiles cls);
    }
}
