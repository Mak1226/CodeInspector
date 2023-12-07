/******************************************************************************
 * Filename    = StudentViewModel.cs
 *
 * Author      = Prayag Krishna
 *
 * Product     = Analyzer
 * 
 * Project     = ViewModel
 *
 * Description = Defines the Student viewmodel.
 *****************************************************************************/
using Networking.Communicator;
using Networking.Events;
using SessionState;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Networking.Utils;
using Networking.Models;

namespace ViewModel
{
    public class StudentViewModel : INotifyPropertyChanged , IEventHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StudentViewModel"/> class.
        /// </summary>
        /// <param name="name">The name of the student.</param>
        /// <param name="id">The ID of the student.</param>
        /// <param name="communicator">The communicator interface parameter.</param>
        public StudentViewModel( string name , string id , string userImage , ICommunicator? communicator = null)
        {
            Client = communicator ?? CommunicationFactory.GetClient();
            StudentName = name;
            StudentRoll = id;
            StudentImage = userImage;
            IpAddress = GetPrivateIp();

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
        /// Gets the instructor's ip
        /// </summary>
        public string? InstructorIp { get; private set; }

        /// <summary>
        /// Gets the instructor's port
        /// </summary> 
        public string? InstructorPort { get; private set; }

        private bool _isConnected = false;

        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }

            private set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    OnPropertyChanged(nameof(IsConnected));
                }
            }
        }

        public ICommunicator Communicator => Client;

        /// <summary>
        /// Handles the property changed event raised on a component.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the name of the student.
        /// </summary>
        public string StudentName { get; private set; }

        /// <summary>
        /// Gets the user ID of the student.
        /// </summary>
        public string StudentRoll { get; private set; }

        /// <summary>
        /// Gets the user image of the student.
        /// </summary>
        public string StudentImage { get; init; }
        public ICommunicator Client { get; }

        /// <summary>
        /// <param name="property">The name of the property</param>
        /// </summary>
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Retrieves the private IP address of the host machine.
        /// </summary>
        /// <returns>The private IP address if found, otherwise null.</returns>
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
                        Trace.WriteLine($"Private IP address found: {address}");
                        return address.ToString();
                    }
                }
            }
            Trace.WriteLine("No suitable private IP address found.");
            return null;
        }

        /// <summary>
        /// Serializes student information into a string format.
        /// </summary>
        /// <param name="name">The name of the student.</param>
        /// <param name="rollNo">The roll number of the student.</param>
        /// <param name="ip">The IP address of the student.</param>
        /// <param name="port">The port of the student.</param>
        /// <param name="connect">The connection status (1 for connected, 0 for disconnected).</param>
        /// <returns>The serialized string containing student information.</returns>
        private static string SerializeStudnetInfo(string? name, string? rollNo, string? ip, string? port, int connect)
        {
            string serializedInfo = $"{rollNo}|{name}|{ip}|{port}|{connect}";
            Trace.WriteLine($"Serialized student information: {serializedInfo}");
            return serializedInfo;
        }

        /// <summary>
        /// Handles the message received from the instructor.
        /// </summary>
        /// <param name="message">The received message.</param>
        private void HandleMessage(string message)
        {
            if (message == "1")
            {
                IsConnected = true;
                Trace.WriteLine("Connected to Instructor");
            }
            else if (message == "0")
            {
                IsConnected = false;
                Client.Stop();
                Trace.WriteLine("Disconnected from Instructor");
            }
        }

        /// <summary>
        /// Disconnects from the instructor.
        /// </summary>
        public void DisconnectInstructor()
        {
            string message = SerializeStudnetInfo(StudentName, StudentRoll, IpAddress, ReceivePort, 0);
            
            if (InstructorIp != null && InstructorPort != null)
            {
                Client.Send(message, "server");
            }
        }
        /// <summary>
        /// Connects to the instructor.
        /// </summary>
        /// <returns>True if connection succeeds, false otherwise.</returns>
        public bool ConnectInstructor()
        {
            if (InstructorIp != null && InstructorPort != null && StudentRoll!=null)
            {
                string ipPort = Client.Start( InstructorIp , int.Parse( InstructorPort ) , StudentRoll , "Dashboard" );
                if(ipPort == "failed")
                {
                    return false;
                }
                Client.Subscribe(this, "Dashboard");
                Trace.WriteLine(ipPort);
                string[] parts = ipPort.Split(':');
                try
                {
                    IpAddress = parts[0];
                    ReceivePort = parts[1];
                    OnPropertyChanged(nameof(IpAddress));
                    OnPropertyChanged(nameof(ReceivePort));

                    string message = SerializeStudnetInfo(StudentName, StudentRoll, IpAddress, ReceivePort, 1);
                    Client.Send(message, "server");
                    return true;
                }
                catch { }
            }
            return false;
        }

        /// <summary>
        /// Sets the instructor's IP address and port.
        /// </summary>
        /// <param name="ip">The instructor's IP address.</param>
        /// <param name="port">The instructor's port.</param>
        public void SetInstructorAddress(string ip, string port)
        {
            InstructorIp = ip;
            InstructorPort = port;
        }

        public void SetStudentInfo(string name, string roll)
        {
            StudentName = name;
            StudentRoll = roll;
        }

        /// <summary>
        /// Receives message from the network and relays it to the HandleMessage function 
        /// </summary>
        /// <param name="data">The received message data.</param>
        /// <returns>An empty string.</returns>
        public string HandleMessageRecv(Networking.Models.Message data)
        {
            HandleMessage(data.Data);
            return "";
        }
    }
}
