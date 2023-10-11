using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{

    public enum WhoseFile
    {
        Student, Teacher
    }

    public interface IAnalyzer
    {
        // TODO

        public void Configure(IDictionary<int,bool> TeacherOptions, bool TeacherFlag);
        public void LoadDLLFile(List<string> PathOfDLLFilesOfStudent, string? PathOfDLLFileOfTeacher);
        public IDictionary<string,string> GetAnalysis();  
    }
}