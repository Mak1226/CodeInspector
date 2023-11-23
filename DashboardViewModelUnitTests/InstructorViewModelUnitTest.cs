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

namespace InstructorViewModelUnitTests
{
    [TestClass]
    public class InstructorViewModelTests
    {
        [TestMethod]
        public void TestAddingStudent()
        {
            InstructorViewModel viewModel = new( "John Doe Jr" , "123@123mail.com" );
            Assert.AreEqual( viewModel.StudentCount , 0 );
            viewModel.HandleMessageRecv( new Message { Data = "001|Alice|192.168.0.1|8080|1" } );
            Assert.AreEqual( viewModel.StudentCount , 1 );
        }

        [TestMethod]
        public void TestRemovingStudnet()
        {
            InstructorViewModel viewModel = new( "John Doe Jr" , "123@123mail.com" );
            viewModel.HandleMessageRecv( new Message { Data = "001|Alice|192.168.0.1|8080|1" } );
            Assert.AreEqual( viewModel.StudentCount , 1 );
            viewModel.HandleMessageRecv( new Message { Data = "001|Alice|192.168.0.1|8080|0" } );
            Assert.AreEqual( viewModel.StudentCount , 0 );
        }

        [TestMethod]
        public void TestInvalidMessageReceived()
        {
            InstructorViewModel viewModel = new( "John Doe Jr" , "123@123mail.com" );
            viewModel.HandleMessageRecv( new Message { Data = "001|Alice|192.168.0.1|1" } );
            Assert.AreEqual( viewModel.StudentCount, 0 );
        }

        [TestMethod]
        public void TestObservableCollectionStudentList()
        {
            InstructorViewModel viewModel = new( "John Doe Jr" , "123@123mail.com" );
            viewModel.HandleMessageRecv( new Message { Data = "001|Alice|192.168.0.1|8080|1" } );
            Assert.AreEqual( viewModel.StudentList.Count , 1 );
            viewModel.HandleMessageRecv( new Message { Data = "001|Alice|192.168.0.1|8080|0" } );
            Assert.AreEqual( viewModel.StudentList.Count , 0 );
        }
    }
}
