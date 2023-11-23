/******************************************************************************
 * Filename    = UnitTest1.cs
 *
 * Author      = Aravind Somaraj
 *
 * Product     = Analyzer
 * 
 * Project     = SessionState
 *
 * Description = Defines the Session state unit tests.
 *****************************************************************************/
using SessionState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace SessionStateUnitTesting
{
    /// <summary>
    /// Unit Tests for SessionState
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Validates that a student is added to list
        /// </summary>
        [TestMethod]
        public void AddStudent_AddsStudentToList()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();

            // Act
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);

            // Assert
            List<Student> students = sessionState.GetAllStudents();
            Assert.AreEqual(1, students.Count);
            Assert.AreEqual("1", students.First().Id);
            Assert.AreEqual("John", students.First().Name);
            Assert.AreEqual("192.168.0.1", students.First().IP);
            Assert.AreEqual(8080, students.First().Port);
        }

        /// <summary>
        /// Validates that a student is not added to list if already present
        /// </summary>
        [TestMethod]
        public void AddStudent_DoesNotAddDuplicateStudent()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();

            // Act
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);
            sessionState.AddStudent("1", "Jane", "192.168.0.2", 8081); // Duplicate student ID

            // Assert
            List<Student> students = sessionState.GetAllStudents();
            Assert.AreEqual(1, students.Count);
        }

        /// <summary>
        /// Validates that a student is removed from list
        /// </summary>
        [TestMethod]
        public void RemoveStudent_RemovesStudentFromList()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);

            // Act
            sessionState.RemoveStudent("1");

            // Assert
            List<Student> students = sessionState.GetAllStudents();
            Assert.AreEqual(0, students.Count);
        }

        /// <summary>
        /// Validates that no change to list occurs if student to be removed is not present in the list
        /// </summary>
        [TestMethod]
        public void RemoveStudent_DoesNothingIfStudentNotFound()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);

            // Act
            sessionState.RemoveStudent("2"); // Student with ID "2" not added

            // Assert
            List<Student> students = sessionState.GetAllStudents();
            Assert.AreEqual(1, students.Count);
        }

        /// <summary>
        /// Validates that the correct number of students is returned
        /// </summary>
        [TestMethod]
        public void GetStudentsCount_ReturnsCorrectCount()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);
            sessionState.AddStudent("2", "Jane", "192.168.0.2", 8081);

            // Act
            int count = sessionState.GetStudentsCount();

            // Assert
            Assert.AreEqual(2, count);
        }

        /// <summary>
        /// Validates that the list is cleared when <typeparamref name="RemoveAllStudents"></typeparamref> is called
        /// </summary>
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
            List<Student> students = sessionState.GetAllStudents();
            Assert.AreEqual(0, students.Count);
        }
        
        /// <summary>
        /// Validates that no change to list occurs when <typeparamref name="RemoveAllStudents"></typeparamref> is called on an empty list
        /// </summary>
        [TestMethod]
        public void RemoveAllStudents_DoesNothingIfListIsEmpty()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();

            // Act
            sessionState.RemoveAllStudents();

            // Assert
            List<Student> students = sessionState.GetAllStudents();
            Assert.AreEqual(0, students.Count);
        }

        /// <summary>
        /// Validates that a student is not added to list if no id is passed
        /// </summary>
        [TestMethod]
        public void AddStudent_WithInvalidData_DoesNotAddStudent()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();

            // Act
            sessionState.AddStudent(null, "John", "192.168.0.1", 8080); // Invalid ID

            // Assert
            List<Student> students = sessionState.GetAllStudents();
            Assert.AreEqual(0, students.Count);
        }

        /// <summary>
        /// Validates that a student is not removed from list if invalid data is passed
        /// </summary>
        [TestMethod]
        public void RemoveStudent_WithInvalidId_DoesNothing()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);

            // Act
            sessionState.RemoveStudent(null); // Invalid ID

            // Assert
            List<Student> students = sessionState.GetAllStudents();
            Assert.AreEqual(1, students.Count);
        }

        /// <summary>
        /// Validates that a student is not added to list if invalid id is passed
        /// </summary>
        [TestMethod]
        public void AddStudent_WithEmptyId_DoesNotAddStudent()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();

            // Act
            sessionState.AddStudent("", "John", "192.168.0.1", 8080); // Empty ID

            // Assert
            List<Student> students = sessionState.GetAllStudents();
            Assert.AreEqual(0, students.Count);
        }

        /// <summary>
        /// Validates that a student is not added to the list if invalid port is passed
        /// </summary>
        [TestMethod]
        public void AddStudent_WithNegativePort_DoesNotAddStudent()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();

            // Act
            sessionState.AddStudent("1", "John", "192.168.0.1", -8080); // Negative port

            // Assert
            List<Student> students = sessionState.GetAllStudents();
            Assert.AreEqual(0, students.Count);
        }

        /// <summary>
        /// Validates that a student is not added to the list if invalid id is passed
        /// </summary>
        [TestMethod]
        public void RemoveStudent_WithNonexistentId_DoesNothing()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);

            // Act
            sessionState.RemoveStudent("2"); // Nonexistent ID

            // Assert
            List<Student> students = sessionState.GetAllStudents();
            Assert.AreEqual(1, students.Count);
        }

        /// <summary>
        /// Validates that a student is not removed from the list if invalid id is passed
        /// </summary>
        [TestMethod]
        public void RemoveStudent_WithNullId_DoesNothing()
        {
            // Arrange
            ISessionState sessionState = new StudentSessionState();
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);

            // Act
            sessionState.RemoveStudent(null); // Null ID

            // Assert
            List<Student> students = sessionState.GetAllStudents();
            Assert.AreEqual(1, students.Count);
        }

        /// <summary>
        /// Validates that no change to list if empty list is cleared
        /// </summary>
        [TestMethod]
        public void RemoveAllStudents_WithNullList_DoesNothing()
        {
            // Arrange
            StudentSessionState sessionState = new();
            sessionState.AddStudent("1", "John", "192.168.0.1", 8080);

            // Act
            sessionState.RemoveAllStudents();
            sessionState.RemoveAllStudents(); // Call again with null list

            // Assert
            List<Student> students = sessionState.GetAllStudents();
            Assert.AreEqual(0, students.Count);
        }
    }

}
