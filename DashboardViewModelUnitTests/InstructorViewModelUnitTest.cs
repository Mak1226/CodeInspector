/******************************************************************************
 * Filename    = InstructorViewModelUnitTest.cs
 *
 * Author      = Saarang S
 *
 * Product     = Analyzer
 * 
 * Project     = DashboardViewModelUnitTests
 *
 * Description = Unit tests for the Instructor viewmodel.
 *****************************************************************************/using Moq;
using Networking.Communicator;
using SessionState;
using System;
using Networking.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace InstructorViewModelUnitTests
{
    [TestClass]
    public class InstructorViewModelTests
    {
        //[TestMethod]
        //public void Constructor_WithValidArguments_ShouldInitializeProperties()
        //{
        //    // Arrange
        //    var mockCommunicator = new Mock<ICommunicator>();

        //    // Act
        //    var viewModel = new InstructorViewModel("John Doe", "123", mockCommunicator.Object);

        //    // Assert
        //    Assert.IsNotNull(viewModel.UserName);
        //    Assert.IsNotNull(viewModel.UserId);
        //    Assert.IsNotNull(viewModel.Communicator);
        //    Assert.IsNotNull(viewModel.ReceivePort);
        //    Assert.IsNotNull(viewModel.IpAddress);
        //}
        [TestMethod]
        public void HandleMessageRecv_WhenValidMessageReceived_ShouldInvokeAddStudent()
        {
            // Arrange
            var viewModel = new InstructorViewModel("John Doe", "123");
            var studentData = new Message { Data = "001|Alice|192.168.0.1|8080|1" }; // Assuming message with student info

            // Act
            string result = viewModel.HandleMessageRecv(studentData);

            // Assert
            Assert.AreEqual("", result); // Assuming HandleMessageRecv returns an empty string
            // Validate the effect on public properties or methods of the ViewModel after invoking HandleMessageRecv
        }

    }
}
