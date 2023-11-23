using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionState
{
    /// <summary>
    /// Interface for SessionState operations
    /// </summary>
    public interface ISessionState
    {
        /// <summary>
        /// Method to retrieve the current list of students
        /// </summary>
        /// <returns><typeparamref name="List"></typeparamref> : containing all students</returns>
        List<Student> GetAllStudents();

        /// <summary>
        /// Method to add a student to the list of students
        /// </summary>
        /// <param name="id">ID of student who is to be added</param>
        /// <param name="name">Name of student who is to be added</param>
        /// <param name="ip">IP of student who is to be added</param>
        /// <param name="port">Port of student who is to be added</param>
        void AddStudent( string id , string name , string ip , int port );
        
        /// <summary>
        /// Method to remove a student from the list of students
        /// </summary>
        /// <param name="id">ID of student who is to be removed</param>
        void RemoveStudent( string id );

        /// <summary>
        /// Method to get the number of students in the list
        /// </summary>
        /// <returns><typeparamref name="int"></typeparamref> : Current number of students</returns>
        public int GetStudentsCount();

        /// <summary>
        /// Method to remove all students from the list
        /// </summary>
        public void RemoveAllStudents();
    }
}
