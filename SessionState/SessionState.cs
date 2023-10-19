namespace SessionState
{
    public class StudentSessionState
    {
        private List<Student> students;

        public StudentSessionState()
        {
            students = new List<Student>();
        }

        public void AddStudent(int id, string name, string ip, int port)
        {
            var student = new Student
            {
                Id = id,
                Name = name,
                IP = ip,
                Port = port
            };
            students.Add(student);
        }

        public void RemoveStudent(int id)
        {
            var studentToRemove = students.Find(s => s.Id == id);
            if (studentToRemove != null)
            {
                students.Remove(studentToRemove);
            }
        }

        public List<Student> GetAllStudents()
        {
            return students;
        }
    }
}