/******************************************************************************
 * Filename    = DashboardViewModel.cs
 *
 * Author      = Saarang S
 *
 * Product     = Analyzer
 * 
 * Project     = ViewModel
 *
 * Description = Defines the Dashboard viewmodel.
 *****************************************************************************/
using System;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;



namespace ViewModel
{
    /// <summary>
    /// ViewModel for the Dashboard.
    /// </summary>
    public class DashboardViewModel
    {
        /// <summary>
        /// Constructor for the DashboardViewModel.
        /// </summary>
        public DashboardViewModel()
        { 
            IpAddress = GetPrivateIp();
            ReceivePort = "12365";
        }

        /// <summary>
        /// Gets the receive port.
        /// </summary>
        public string? ReceivePort { get; private set; }

        /// <summary>
        /// Gets the IP address.
        /// </summary>
        public string? IpAddress { get; private set; }
        
        /// <summary>
        /// Gets the instructor's IP address.
        /// </summary>
        public string? InstructorIp { get; private set; }

        /// <summary>
        /// Gets the instructor's port.
        /// </summary>
        public string? InstructorPort { get; private set; }

        /// <summary>
        /// Gets the private IP address of the host machine.
        /// </summary>
        private string? GetPrivateIp()
        {
            string hostName = Dns.GetHostName();
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            return addresses[2].ToString();
            //foreach (IPAddress address in addresses)
            //{
            //    if (address.AddressFamily == AddressFamily.InterNetwork)
            //    {
            //        return address.ToString();
            //    }
            //}
            //return null;
        }

        /// <summary>
        /// Sets the instructor's IP address and port.
        /// </summary>
        /// <param name="ip">The instructor's IP address.</param>
        /// <param name="port">The instructor's port.</param>
        private void SetInstructorAddress (string ip, string port)
        {
            InstructorIp = ip;
            InstructorPort = port;
        }
    }
}