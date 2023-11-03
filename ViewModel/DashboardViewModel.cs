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
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Networking;
using ChatMessaging;
using SessionState;


namespace ViewModel
{
    /// <summary>
    /// ViewModel for the Dashboard.
    /// </summary>
    public class DashboardViewModel
    {
        private readonly ICommunicator _communicator; // Communicator used to send and receive messages.
        private readonly ChatMessenger _newConnection; // To communicate between instructor and student used to send and receive chat messages.
        private readonly StudentSessionState _studentSessionState; // To manage the connected studnets
        /// <summary>
        /// Constructor for the DashboardViewModel.
        /// </summary>
        public DashboardViewModel(bool isInstructor, ICommunicator? communicator = null)
        {
            _studentSessionState = new ();
            _communicator = communicator ?? CommunicatorFactory.CreateCommunicator();

            IpAddress = GetPrivateIp();

            // Update the port that the communicator is listening on.
            ReceivePort = _communicator.ListenPort.ToString();
            OnPropertyChanged(nameof(ReceivePort));

            // Create an instance of the chat messenger and signup for callback.
            _newConnection = new(_communicator);
            if (isInstructor)
            {
                _newConnection.OnChatMessageReceived += delegate (string message)
                {
                    AddStudnet(message);
                };
            }
            else
            {
                _newConnection.OnChatMessageReceived += delegate (string message)
                {
                    HandleStudentMessage(message);
                };
            } 
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
        /// Gets the instructor connection status
        /// </summary>
        public bool IsConnected { get; private set; } = false;

        /// <summary>
        /// Property changed event raised when a property is changed on a component.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Handles the property changed event raised on a component.
        /// </summary>
        public string? StudentName { get; private set; }

        /// <summary>
        /// Gets the instructor's port.
        /// </summary>
        public string? StudentRoll { get; private set; }

        /// <summary>
        /// Gets the private IP address of the host machine.
        /// </summary>
        /// <param name="property">The name of the property</param>
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private string? GetPrivateIp()
        {
            string hostName = Dns.GetHostName();
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    if (address.ToString().Length >= 3 && address.ToString().Substring(0, 3) == "10.")
                    {
                        return address.ToString();
                    }
                }
            }
            return null;
        }

        private static string SerializeStudnetInfo(string name, string rollNo, string ip, string port)
        {  
            return $"{rollNo}|{name}|{ip}|{port}";
        }

        private static (int, string?, string?, int) DeserializeStudnetInfo (string data)
        {
            string[] parts = data.Split('|');
            if (parts.Length == 4)
            {
                try
                {
                    return
                    (
                        int.Parse(parts[0]),
                        parts[1],
                        parts[2],
                        int.Parse(parts[3])
                    );
                }
                catch { }
                
            }
            return (0, null, null, 0);
        }
        private bool AddStudnet (string serializedStudnet)
        {
            if (serializedStudnet != null)
            { 
                var result = DeserializeStudnetInfo(serializedStudnet);
                var rollNo = result.Item1;
                var name = result.Item2;
                var ip = result.Item3;
                var port = result.Item4;
                if (name != null && ip != null)
                {
                    _studentSessionState.AddStudent(rollNo, name, ip, port);
                    _newConnection.SendMessage(ip,port,"1");
                    return true;
                }
            }
            return false;
        }

        private void HandleStudentMessage(string message)
        {
            if(message == "1")
            {
                IsConnected = true;
            }
            else if (message == "0") 
            { 
                IsConnected= false;
            }
        }

        //private void ConnectInstructor ()
        //{
        //    var message = SerializeStudnetInfo()
        //    _newConnection.SendMessage(InstructorIp,InstructorIp,)
        //}

        /// <summary>
        /// Sets the instructor's IP address and port.
        /// </summary>
        /// <param name="ip">The instructor's IP address.</param>
        /// <param name="port">The instructor's port.</param>
        public void SetInstructorAddress (string ip, string port)
        {
            InstructorIp = ip;
            InstructorPort = port;

            Debug.WriteLine(InstructorIp);
            Debug.WriteLine(InstructorPort);
        }
        public void SetStudentInfo(string name, string roll)
        {
            StudentName = name;
            StudentRoll = roll;

            Debug.WriteLine(StudentName);
            Debug.WriteLine(StudentRoll);
        }
    }
}