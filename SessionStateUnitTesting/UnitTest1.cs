using SessionState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace SessionStateUnitTesting
{

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddStudent_AddsStudentToList()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();

            // Act
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);

            // Assert
            var students = sessionState.GetAllStudents();
            Assert.AreEqual(1, students.Count);
            Assert.AreEqual("1", students.First().Id);
            Assert.AreEqual("John", students.First().Name);
            Assert.AreEqual("192.168.0.1", students.First().IP);
            Assert.AreEqual(8080, students.First().Port);
        }

        [TestMethod]
        public void AddStudent_DoesNotAddDuplicateStudent()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();

            // Act
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);
            sessionState.AddStudent("1", "Jane", "192.168.0.2", 8081); // Duplicate student ID

            // Assert
            var students = sessionState.GetAllStudents();
            Assert.AreEqual(1, students.Count);
        }

        [TestMethod]
        public void RemoveStudent_RemovesStudentFromList()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);

            // Act
            sessionState.RemoveStudent("1");

            // Assert
            var students = sessionState.GetAllStudents();
            Assert.AreEqual(0, students.Count);
        }

        [TestMethod]
        public void RemoveStudent_DoesNothingIfStudentNotFound()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);

            // Act
            sessionState.RemoveStudent("2"); // Student with ID "2" not added

            // Assert
            var students = sessionState.GetAllStudents();
            Assert.AreEqual(1, students.Count);
        }

        [TestMethod]
        public void GetStudentsCount_ReturnsCorrectCount()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);
            sessionState.AddStudent("2", "Jane", "192.168.0.2", 8081);

            // Act
            var count = sessionState.GetStudentsCount();

            // Assert
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void RemoveAllStudents_ClearsStudentList()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);
            sessionState.AddStudent("2", "Jane", "192.168.0.2", 8081);

            // Act
            sessionState.RemoveAllStudents();

            // Assert
            var students = sessionState.GetAllStudents();
            Assert.AreEqual(0, students.Count);
        }

        [TestMethod]
        public void RemoveAllStudents_DoesNothingIfListIsEmpty()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();

            // Act
            sessionState.RemoveAllStudents();

            // Assert
            var students = sessionState.GetAllStudents();
            Assert.AreEqual(0, students.Count);
        }

        [TestMethod]
        public void AddStudent_WithInvalidData_DoesNotAddStudent()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();

            // Act
            sessionState.AddStudent(null, "John", "192.168.0.1", 8080); // Invalid ID

            // Assert
            var students = sessionState.GetAllStudents();
            Assert.AreEqual(0, students.Count);
        }

        [TestMethod]
        public void RemoveStudent_WithInvalidId_DoesNothing()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);

            // Act
            sessionState.RemoveStudent(null); // Invalid ID

            // Assert
            var students = sessionState.GetAllStudents();
            Assert.AreEqual(1, students.Count);
        }

        [TestMethod]
        public void AddStudent_WithEmptyId_DoesNotAddStudent()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();

            // Act
            sessionState.AddStudent("", "John", "192.168.0.1", 8080); // Empty ID

            // Assert
            var students = sessionState.GetAllStudents();
            Assert.AreEqual(0, students.Count);
        }

        [TestMethod]
        public void AddStudent_WithNegativePort_DoesNotAddStudent()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();

            // Act
            sessionState.AddStudent("1", "John", "192.168.0.1", -8080); // Negative port

            // Assert
            var students = sessionState.GetAllStudents();
            Assert.AreEqual(0, students.Count);
        }

        [TestMethod]
        public void RemoveStudent_WithNonexistentId_DoesNothing()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);

            // Act
            sessionState.RemoveStudent("2"); // Nonexistent ID

            // Assert
            var students = sessionState.GetAllStudents();
            Assert.AreEqual(1, students.Count);
        }

        [TestMethod]
        public void RemoveStudent_WithNullId_DoesNothing()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);

            // Act
            sessionState.RemoveStudent(null); // Null ID

            // Assert
            var students = sessionState.GetAllStudents();
            Assert.AreEqual(1, students.Count);
        }

        [TestMethod]
        public void RemoveAllStudents_WithNullList_DoesNothing()
        {
            // Arrange
            StudentSessionState sessionState = new StudentSessionState();
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);

            // Act
            sessionState.RemoveAllStudents();
            sessionState.RemoveAllStudents(); // Call again with null list

            // Assert
            var students = sessionState.GetAllStudents();
            Assert.AreEqual(0, students.Count);
        }
    }

}