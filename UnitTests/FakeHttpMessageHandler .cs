/******************************************************************************
* Filename    = FakeHttpMessageHandler.cs
*
* Author      = Nideesh N
*
* Product     = Analyzer
* 
* Project     = Cloud Unit Test
*
* Description = Contains the implementation of HttpMessageHandler
*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudUnitTests
{
    /// <summary>
    /// A fake implementation of HttpMessageHandler for unit testing, allowing simulation of HTTP responses or errors.
    /// </summary>
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly object _fakeResponseOrError;

        /// <summary>
        /// Initializes a new instance of the FakeHttpMessageHandler class.
        /// </summary>
        /// <param name="fakeResponseOrError">The simulated response or error to be used during testing.</param>
        public FakeHttpMessageHandler( object fakeResponseOrError )
        {
            _fakeResponseOrError = fakeResponseOrError;
        }

        /// <summary>
        /// Simulates sending an HTTP request by returning a Task with a simulated response or throwing a simulated error.
        /// </summary>
        /// <param name="request">The HttpRequestMessage representing the request.</param>
        /// <param name="cancellationToken">The CancellationToken for the asynchronous operation.</param>
        /// <returns>A Task containing the simulated HttpResponseMessage or throwing a simulated Exception.</returns>
        protected override Task<HttpResponseMessage> SendAsync( HttpRequestMessage request , CancellationToken cancellationToken )
        {
            if (_fakeResponseOrError is HttpResponseMessage response)
            {
                return Task.FromResult( response );
            }
            else if (_fakeResponseOrError is Exception error)
            {
                return Task.FromException<HttpResponseMessage>( error );
            }
            else
            {
                throw new InvalidOperationException( "Invalid type for fake response or error." );
            }
        }
    }

}
