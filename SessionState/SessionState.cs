using System.Diagnostics;

namespace SessionState
{
    public class StudentSessionState : ISessionState
    {
        /// <summary>
        /// <typeparamref name="List"></typeparamref> to store all students
        /// </summary>
        private readonly List<Student> _students;

        /// <summary>
        /// Constructor for StudentSessionState
        /// </summary>
        public StudentSessionState()
        {
            _students = new List<Student>();
            Trace.WriteLine("SessionState.StudentSessionState constructor called");
        }

        /// <inheritdoc />
        public void AddStudent(string id, string name, string ip, int port)
        {
            if( id == null || id == "" || port <= 0)    // Check for invalid parameters
            {
                Trace.WriteLine("SessionState.AddStudent called with invalid data");
                return;
            }
            Student? check = _students.Find(s => s.Id == id);
            if (check == null)      // Check if student already exists, if not add to list
            {
                Student student = new()
                {
                    Id = id,
                    Name = name,
                    IP = ip,
                    Port = port
                };
                _students.Add( student );
            }
            Trace.WriteLine("SessionState.AddStudent called");
        }

        /// <inheritdoc />
        public void RemoveStudent(string id)
        {
            if (id == null || id == "")     // Check for invalid parameters
            {
                return;
            }
            Student? studentToRemove = _students.Find(s => s.Id == id);
            if (studentToRemove != null)    // Check if student exists, if yes remove from list
            {
                _students.Remove( studentToRemove );
            }
            Trace.WriteLine("SessionState.RemoveStudent called");
        }

        /// <inheritdoc />
        public List<Student> GetAllStudents()
        {
            Trace.WriteLine("SessionState.GetAllStudents called");
            return _students;
        }

        /// <inheritdoc />
        public int GetStudentsCount()
        {
            Trace.WriteLine("SessionState.GetStudentsCount called");
            return _students.Count;
        }

        /// <inheritdoc />
        public void RemoveAllStudents()
        {
            _students?.Clear();
            Trace.WriteLine("SessionState.RemoveAllStudents called");
        }
    }
}
