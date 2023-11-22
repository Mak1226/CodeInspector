using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionState
{
    public interface ISessionState
    {
        List<Student> GetAllStudents();
        void AddStudent( string id , string name , string ip , int port );
        void RemoveStudent(string id);

        public int GetStudentsCount();
    }
}
