/******************************************************************************
 * Filename    = Student.cs
 *
 * Author      = Aravind Somaraj
 *
 * Product     = Analyzer
 * 
 * Project     = SessionState
 *
 * Description = Defines the Student data type.
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionState
{
    /// <summary>
    /// Class to represent a student
    /// </summary>
    public class Student
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? IP { get; set; }
        public int Port { get; set; }
    }
}
