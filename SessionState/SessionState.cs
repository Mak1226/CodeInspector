namespace SessionState
{
    public class StudentSessionState : ISessionState
    {
        private readonly List<Student> _students;

        public StudentSessionState()
        {
            _students = new List<Student>();
        }

        public void AddStudent(string id, string name, string ip, int port)
        {
            Student? check = _students.Find(s => s.Id == id);
            if (check == null)
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
        }

        public void RemoveStudent(string id)
        {
            Student? studentToRemove = _students.Find(s => s.Id == id);
            if (studentToRemove != null)
            {
                _students.Remove( studentToRemove );
            }
        }

        public List<Student> GetAllStudents()
        {
            return _students;
        }

        public int GetStudentsCount()
        {
            return _students.Count;
        }
    }
}
