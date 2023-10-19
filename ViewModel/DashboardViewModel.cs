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
using System.Net;
using System.Net.Sockets;



namespace ViewModel
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        { 
            IpAddress = GetPrivateIp();
            ReceivePort = "12365";
        }
        public string? ReceivePort { get; private set; }

        public string? IpAddress { get; private set; }

        private string? GetPrivateIp()
        {
            string hostName = Dns.GetHostName();
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    //Console.WriteLine("IPv4 Address: " + address.ToString());
                    return address.ToString();
                }
            }
            return null;
        }
    }
}