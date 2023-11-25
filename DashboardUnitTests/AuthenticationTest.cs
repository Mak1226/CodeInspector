//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Dashboard.Authentication;

using Dashboard;


namespace DashboardUnitTests
{
    [TestClass]
    public class AuthenticationTest
    {
        [TestMethod]
        public async Task AuthenticationViewModelUnitTest()
        {
            AuthenticationResult result = await Authenticator.Authenticate();
            Assert.IsNotNull( result );
            if (result.IsAuthenticated == false) { Assert.Fail( "Authentication failed " ); }
            else
            { Console.WriteLine( "Authentication done" ); }


        }
        //[Fact]
        //     Giving a timeout of 50 ms to reduce the wait time
        //  var returnval = await _viewModel.AuthenticateUser( 50 );
        // Assert
        //ssert.Equals( "false" , returnval[0] );

    }
}
