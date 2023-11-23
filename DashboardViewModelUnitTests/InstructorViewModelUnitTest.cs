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
 *****************************************************************************/
using Moq;
using Networking.Communicator;
using SessionState;
using System;
using Networking.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace InstructorViewModelUnitTests
{
    /// <summary>
    /// Test class for the InstructorViewModel class.
    /// </summary>
    [TestClass]
    public class InstructorViewModelTests
    {
        /// <summary>
        /// Test method for adding a student to the InstructorViewModel.
        /// </summary>
        [TestMethod]
        public void TestAddingStudent()
        {
            InstructorViewModel viewModel = new( "John Doe Jr" , "123@123mail.com" );
            //checking student count before adding student
            Assert.AreEqual( viewModel.StudentCount , 0 );
            // Adding student to instructor
            viewModel.HandleMessageRecv( new Message { Data = "001|Alice|192.168.0.1|8080|1" } );
            //checking student count after adding student
            Assert.AreEqual( viewModel.StudentCount , 1 );
        }

        /// <summary>
        /// Test method for removing a student from the InstructorViewModel.
        /// </summary>
        [TestMethod]
        public void TestRemovingStudnet()
        {
            InstructorViewModel viewModel = new( "John Doe Jr" , "123@123mail.com" );
            // Adding student to instructor
            viewModel.HandleMessageRecv( new Message { Data = "001|Alice|192.168.0.1|8080|1" } );
            //checking student count after adding student
            Assert.AreEqual( viewModel.StudentCount , 1 );
            // Removing student to instructor (last 0 stands for leaving)
            viewModel.HandleMessageRecv( new Message { Data = "001|Alice|192.168.0.1|8080|0" } );
            //checking student count after removing student
            Assert.AreEqual( viewModel.StudentCount , 0 );
        }

        /// <summary>
        /// Test method to check handling of an invalid message received by the InstructorViewModel.
        /// </summary>
        [TestMethod]
        public void TestInvalidMessageReceived()
        {
            InstructorViewModel viewModel = new( "John Doe Jr" , "123@123mail.com" );
            //Trying to add a student in invalid format (port is missing).
            viewModel.HandleMessageRecv( new Message { Data = "001|Alice|192.168.0.1|1" } );
            //Student count remains zero.
            Assert.AreEqual( viewModel.StudentCount, 0 );
        }

        /// <summary>
        /// Test method to check the behavior of ObservableCollection StudentList in InstructorViewModel.
        /// </summary>
        [TestMethod]
        public void TestObservableCollectionStudentList()
        {
            InstructorViewModel viewModel = new( "John Doe Jr" , "123@123mail.com" );
            // Adding student to instructor
            viewModel.HandleMessageRecv( new Message { Data = "001|Alice|192.168.0.1|8080|1" } );
            // Student count increased. ObservableCollection stays consistent
            Assert.AreEqual( viewModel.StudentList.Count , 1 );
            viewModel.HandleMessageRecv( new Message { Data = "001|Alice|192.168.0.1|8080|0" } );
            Assert.AreEqual( viewModel.StudentList.Count , 0 );
        }

        /// <summary>
        /// Test method to check the removal of all students from the InstructorViewModel upon logout.
        /// </summary>
        [TestMethod]
        public void TestRemoveAllStudents()
        {
            
            InstructorViewModel viewModel = new( "John Doe Jr" , "123@123mail.com" );
            // Adding Students
            viewModel.HandleMessageRecv( new Message { Data = "001|Alice|192.168.0.1|8080|1" } );
            Assert.AreEqual( viewModel.StudentList.Count , 1 );
            viewModel.HandleMessageRecv( new Message { Data = "002|Alice Jr|192.168.0.3|8081|1" } );
            Assert.AreEqual( viewModel.StudentList.Count , 2 );

            viewModel.Logout();
            // Logging out disconnects all students
            Assert.AreEqual( viewModel.StudentList.Count , 0 );
        }

        /// <summary>
        /// Test method to check if the InstructorViewModel properly handles communicator start failure.
        /// </summary>
        [TestMethod]
        public void TestCommunicatorStartFailure ()
        {
            // Mock communicator to that fails that return invalid ip,port string
            var mockCommunicator = new Mock<ICommunicator>();
            mockCommunicator.Setup( x => x.Start( It.IsAny<string?>() , It.IsAny<int?>(), It.IsAny<string>() , It.IsAny<string>() ) ).Returns( "SomeStringExceptValidAddress" );

            Assert.ThrowsException<Exception>( () => new InstructorViewModel( "John Doe Jr" , "123@123mail.com" , mockCommunicator.Object ) ,
            "Invalid Port/Ip returned by communicator" );
        }
    }
}
