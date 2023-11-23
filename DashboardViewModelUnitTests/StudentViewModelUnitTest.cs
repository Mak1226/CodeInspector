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
namespace DashboardViewModelUnitTests
{
    [TestClass]
    public class StudentViewModelUnitTest
    {
        [TestMethod]
        public void DisconnectInstructor_WhenValidInstructorIpAndPort_ShouldSendMessageToServer()
        {
            // Arrange
            var mockCommunicator = new Mock<ICommunicator>();
            var viewModel = new StudentViewModel("John Doe", "123", mockCommunicator.Object);
            viewModel.SetInstructorAddress("192.168.1.1", "8080");

            // Act
            viewModel.DisconnectInstructor();

            // Assert
            mockCommunicator.Verify(x => x.Send(It.IsAny<string>(), "server"), Times.Once);
        }

        [TestMethod]
        public void ConnectInstructor_WhenValidInstructorInfoAndStudentRoll_ShouldReturnTrue()
        {
            // Arrange
            var mockCommunicator = new Mock<ICommunicator>();
            mockCommunicator.Setup(x => x.Start(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns("192.168.1.1:8080");
            var viewModel = new StudentViewModel("John Doe", "123", mockCommunicator.Object);
            viewModel.SetInstructorAddress("192.168.1.1", "8080");

            // Act
            bool result = viewModel.ConnectInstructor();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ConnectInstructor_WhenInvalidInstructorInfo_ShouldReturnFalse()
        {
            // Arrange
            var mockCommunicator = new Mock<ICommunicator>();
            var viewModel = new StudentViewModel("John Doe", "123", mockCommunicator.Object);

            // Act
            bool result = viewModel.ConnectInstructor();

            // Assert
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void SetInstructorAddress_WhenCalled_ShouldSetInstructorIpAndPort()
        {
            // Arrange
            var viewModel = new StudentViewModel("John Doe", "123");

            // Act
            viewModel.SetInstructorAddress("192.168.1.1", "8080");

            // Assert
            Assert.AreEqual("192.168.1.1", viewModel.InstructorIp);
            Assert.AreEqual("8080", viewModel.InstructorPort);
        }

        [TestMethod]
        public void HandleMessageRecv_WhenMessageIsOne_ShouldSetIsConnectedToTrue()
        {
            // Arrange
            var mockCommunicator = new Mock<ICommunicator>();
            var viewModel = new StudentViewModel("John Doe", "123", mockCommunicator.Object);

            // Act
            viewModel.HandleMessageRecv(new Networking.Models.Message { Data = "1" });

            // Assert
            Assert.IsTrue(viewModel.IsConnected);
        }

        [TestMethod]
        public void HandleMessageRecv_WhenMessageIsZero_ShouldSetIsConnectedToFalseAndStopCommunicator()
        {
            // Arrange
            var mockCommunicator = new Mock<ICommunicator>();
            var viewModel = new StudentViewModel("John Doe", "123", mockCommunicator.Object);

            // Act
            viewModel.HandleMessageRecv(new Networking.Models.Message { Data = "0" });

            // Assert
            Assert.IsFalse(viewModel.IsConnected);
            mockCommunicator.Verify(x => x.Stop(), Times.Once);
        }

        [TestMethod]
        public void DisconnectInstructor_WhenInstructorIpAndPortAreNull_ShouldNotSendMessage()
        {
            // Arrange
            var mockCommunicator = new Mock<ICommunicator>();
            var viewModel = new StudentViewModel("John Doe", "123", mockCommunicator.Object);

            // Act
            viewModel.DisconnectInstructor();

            // Assert
            mockCommunicator.Verify(x => x.Send(It.IsAny<string>(), "server"), Times.Never);
        }
        
        [TestMethod]
        public void SetStudentInfo_WhenCalled_ShouldSetStudentNameAndRoll()
        {
            // Arrange
            var viewModel = new StudentViewModel("Initial Name", "Initial Roll");

            // Act
            viewModel.SetStudentInfo("John Doe", "123");

            // Assert
            Assert.AreEqual("John Doe", viewModel.StudentName);
            Assert.AreEqual("123", viewModel.StudentRoll);
        }
        //[TestMethod]
        //public void InstructorIp_WhenSet_ShouldUpdatePropertyValue()
        //{
        //    // Arrange
        //    var viewModel = new StudentViewModel("John Doe", "123");

        //    //// Act
        //    //viewModel.ReceivePort = "8080";
        //    //viewModel.IpAddress = "192.168.1.1";
        //    viewModel.SetInstructorAddress("192.168.0.1", "8080");

        //    // Assert
        //    Assert.AreEqual("8080", viewModel.ReceivePort);
        //    Assert.AreEqual("192.168.1.1", viewModel.IpAddress);
        //    //Assert.AreEqual("192.168.0.1", viewModel.InstructorIp);
        //}

        [TestMethod]
        public void TestingNonNullClientCommunicator()
        {
            StudentViewModel viewModel = new ("John Doe Jr", "123");
            Assert.IsNotNull(viewModel.Communicator);
        }
        //[TestMethod]
        //public void ConnectInstructor_WhenInvalidIp_ShouldReturnFalse()
        //{
        //    // Arrange
        //    var mockCommunicator = new Mock<ICommunicator>();
        //    var viewModel = new StudentViewModel("John Doe", "123", mockCommunicator.Object);
        //    viewModel.SetInstructorAddress("168.1.1", "808000");
        //    viewModel.SetStudentInfo("John Doe", "123");

        //    // Act & Assert
        //    bool result = viewModel.ConnectInstructor();
        //    Assert.IsFalse(result);
        //}
    }
}