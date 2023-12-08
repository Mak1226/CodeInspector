/******************************************************************************
 * Filename    = StudentViewModelUnitTest.cs
 *
 * Author      = Prayag Krishna
 *
 * Product     = Analyzer
 * 
 * Project     = DashboardViewModelUnitTests
 *
 * Description = Unit tests for the Student viewmodel.
 *****************************************************************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using ViewModel;
using Networking.Communicator;
namespace DashboardUnitTests
{
    [TestClass]
    public class StudentViewModelUnitTest
    {
        /// <summary>
        /// Tests the disconnection from the instructor when valid.
        /// </summary>
        /// <remarks>Verifies if the communicator sends a message to the server for disconnection.</remarks>
        [TestMethod]
        public void DisconnectInstructorWhenValid()
        {
            // Arrange
            var mockCommunicator = new Mock<ICommunicator>();
            var viewModel = new StudentViewModel("John Doe", "123", "/someImage", mockCommunicator.Object);
            viewModel.SetInstructorAddress("192.168.1.1", "8080");

            // Act
            viewModel.DisconnectFromInstructor();

            // Assert
            mockCommunicator.Verify(x => x.Send(It.IsAny<string>(), "server"), Times.Once);
        }
        /// <summary>
        /// Tests the connection to the instructor when valid.
        /// </summary>
        /// <remarks>Checks if the connection to the instructor is established successfully.</remarks>
        [TestMethod]
        public void ConnectInstructorWhenValid()
        {
            // Arrange
            var mockCommunicator = new Mock<ICommunicator>();
            mockCommunicator.Setup(x => x.Start(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns("192.168.1.1:8080");
            var viewModel = new StudentViewModel("John Doe", "123", "/someImage" , mockCommunicator.Object);
            viewModel.SetInstructorAddress("192.168.1.1", "8080");

            // Act
            bool result = viewModel.ConnectToInstructor();

            // Assert
            Assert.IsTrue(result);
        }
        /// <summary>
        /// Tests the connection to the instructor when invalid.
        /// </summary>
        /// <remarks>Ensures the inability to connect to the instructor with invalid parameters.</remarks>
        [TestMethod]
        public void ConnectInstructorWhenInvalid()
        {
            // Arrange
            var mockCommunicator = new Mock<ICommunicator>();
            var viewModel = new StudentViewModel("John Doe", "123", "/someImage" , mockCommunicator.Object);

            // Act
            bool result = viewModel.ConnectToInstructor();

            // Assert
            Assert.IsFalse(result);
        }
        /// <summary>
        /// Tests setting the instructor's IP address and port.
        /// </summary>
        /// <remarks>Checks if the instructor's IP and port are correctly set in the view model.</remarks>
        [TestMethod]
        public void SetInstructorAddress()
        {
            // Arrange
            var viewModel = new StudentViewModel("John Doe", "123", "/someImage" );

            // Act
            viewModel.SetInstructorAddress("192.168.1.1", "8080");

            // Assert
            Assert.AreEqual("192.168.1.1", viewModel.InstructorIp);
            Assert.AreEqual("8080", viewModel.InstructorPort);
        }
        /// <summary>
        /// Tests handling a received message to set the connection status.
        /// </summary>
        /// <remarks>Checks if the view model updates the connection status based on the received message.</remarks>
        [TestMethod]
        public void HandleMessageRecvSetIsConnectedToTrue()
        {
            // Arrange
            var mockCommunicator = new Mock<ICommunicator>();
            var viewModel = new StudentViewModel("John Doe", "123", "/someImage" , mockCommunicator.Object);

            // Act
            viewModel.HandleMessageRecv(new Networking.Models.Message { Data = "1" });

            // Assert
            Assert.IsTrue(viewModel.IsConnected);
        }
        /// <summary>
        /// Tests handling a received message to set the disconnection status.
        /// </summary>
        /// <remarks>Checks if the view model updates the disconnection status based on the received message.</remarks>
        [TestMethod]
        public void HandleMessageRecvSetIsConnectedToFalse()
        {
            // Arrange
            var mockCommunicator = new Mock<ICommunicator>();
            var viewModel = new StudentViewModel("John Doe", "123", "/someImage" , mockCommunicator.Object);

            // Act
            viewModel.HandleMessageRecv(new Networking.Models.Message { Data = "0" });

            // Assert
            Assert.IsFalse(viewModel.IsConnected);
            mockCommunicator.Verify(x => x.Stop(), Times.Once);
        }
        /// <summary>
        /// Tests disconnecting from the instructor when the instructor's IP and port are null.
        /// </summary>
        /// <remarks>Verifies that no communication is attempted when the instructor's IP and port are null.</remarks>
        [TestMethod]
        public void DisconnectInstructorInstructorIpAndPortAreNull()
        {
            // Arrange
            var mockCommunicator = new Mock<ICommunicator>();
            var viewModel = new StudentViewModel("John Doe", "123", "/someImage" , mockCommunicator.Object);

            // Act
            viewModel.DisconnectFromInstructor();

            // Assert
            mockCommunicator.Verify(x => x.Send(It.IsAny<string>(), "server"), Times.Never);
        }
        /// <summary>
        /// Tests setting student information.
        /// </summary>
        /// <remarks>Checks if the student's name and roll number are correctly set in the view model.</remarks>
        [TestMethod]
        public void SetStudentInfo()
        {
            // Arrange
            var viewModel = new StudentViewModel("Initial Name", "Initial Roll", "/someImage");

            // Act
            viewModel.SetStudentInfo("John Doe", "123");

            // Assert
            Assert.AreEqual("John Doe", viewModel.StudentName);
            Assert.AreEqual("123", viewModel.StudentRoll);
        }
        /// <summary>
        /// Tests the presence of a non-null client communicator.
        /// </summary>
        /// <remarks>Verifies that the communicator in the student view model is not null upon instantiation.</remarks>
        [TestMethod]
        public void TestingNonNullClientCommunicator()
        {
            StudentViewModel viewModel = new ("John Doe Jr", "123", "/someImage");
            Assert.IsNotNull(viewModel.Communicator);
        }
        //[TestMethod]
        //public void ConnectInstructor_WhenInvalidIp_ShouldReturnFalse()
        //{
        //    // Arrange
        //    var mockCommunicator = new Mock<ICommunicator>();
        //    var viewModel = new StudentViewModel( "John Doe" , "123" , mockCommunicator.Object );
        //    viewModel.SetInstructorAddress( "168.1.1" , "808000" );
        //    viewModel.SetStudentInfo( "John Doe" , "123" );

        //    // Act & Assert
        //    bool result = viewModel.ConnectInstructor();
        //    Assert.IsFalse( result );
        //}
    }
}
