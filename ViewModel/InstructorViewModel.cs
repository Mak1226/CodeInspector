/******************************************************************************
 * Filename    = InstructorViewModel.cs
 *
 * Author      = Saarang S
 *
 * Product     = Analyzer
 * 
 * Project     = ViewModel
 *
 * Description = Defines the Instructor viewmodel.
 *****************************************************************************/
using Networking.Communicator;
using Networking.Events;
using SessionState;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Networking.Utils;
using Networking.Models;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Media.Animation;
using Logging;
using System.Runtime.CompilerServices;



namespace ViewModel
{
    /// <summary>
    /// Represents the ViewModel for the instructor, providing data and functionality for the instructor's view.
    /// </summary>
    public class InstructorViewModel : INotifyPropertyChanged, IEventHandler
    {
        private readonly ISessionState _studentSessionState;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstructorViewModel"/> class.
        /// </summary>
        /// <param name="name">The Name of the instructor.</param>
        /// <param name="userId">The user ID of the instructor.</param>
        /// <param name="communicator">An optional communicator, default is set to the server communicator.</param>
        public InstructorViewModel( string name, string userId, string userImage, ICommunicator? communicator = null )
        {
            // Set the username and user ID for the InstructorViewModel
            Name = name;
            UserId = userId;
            UserImage = userImage;

            // Initialize the session state for tracking students
            _studentSessionState = new StudentSessionState();

            // Set up the communication infrastructure, using the provided communicator or the default server communicator
            Communicator = communicator ?? CommunicationFactory.GetServer();

            // Start the communication process and subscribe the ViewModel to the "Dashboard" channel
            string ipPort = Communicator.Start(null, null, "server", "Dashboard");
            Communicator.Subscribe(this, "Dashboard");

            // Split the received IP and port information
            string[] parts = ipPort.Split(':');

            // Check if the IP and port information is correctly formatted
            if (parts.Length==2)
            {
                // Extract and set the IP address and receiving port
                IpAddress = parts[0];
                ReceivePort = parts[1];

                // Notify any subscribers about the change in IP address and port
                OnPropertyChanged(nameof(IpAddress));
                OnPropertyChanged(nameof(ReceivePort));
                Logger.Inform($"[]");
            }
            else
            {
                // Throw an exception if the received IP and port information is not in the expected format
                throw new Exception( "Invalid Port/Ip returned by communicator" );
            }
        }

        /// <summary>
        /// Gets the list of students in the session.
        /// </summary>
        public ObservableCollection<Student> StudentList => new( _studentSessionState.GetAllStudents() );

        /// <summary>
        /// Gets the count of students in the session.
        /// </summary>
        public int StudentCount => _studentSessionState.GetStudentsCount();

        /// <summary>
        /// Gets the communicator used for communication.
        /// </summary>
        public ICommunicator Communicator { get; }

        /// <summary>
        /// Gets the Name of the instructor.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the user ID of the instructor.
        /// </summary>
        public string UserId { get; init; }

        /// <summary>
        /// Gets the user image of the instructor.
        /// </summary>
        public string UserImage { get; init; }

        /// <summary>
        /// Gets the receiving port for the instructor.
        /// </summary>
        public string? ReceivePort { get; private set; }

        /// <summary>
        /// Gets the IP address for the instructor.
        /// </summary>
        public string? IpAddress { get; private set; }

        /// <summary>
        /// Event triggered when a property of the ViewModel changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Handles the PropertyChanged event for the ViewModel.
        /// </summary>
        /// <param name="property">The name of the changed property.</param>
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Deserializes student information from a serialized string.
        /// </summary>
        /// <param name="data">The serialized student information.</param>
        /// <returns>A tuple containing student information.</returns>
        private static (string?, string?, string?, int, int) DeserializeStudnetInfo(string data)
        {
            string[] parts = data.Split('|');
            if (parts.Length == 5)
            {
                return
                (
                    parts[0],
                    parts[1],
                    parts[2],
                    int.Parse(parts[3]),
                    int.Parse(parts[4])
                );
            }
            return (null, null, null, 0, 0);
        }

        /// <summary>
        /// Adds a student based on the received serialized student information.
        /// </summary>
        /// <param name="serializedStudnet">The serialized student information.</param>
        /// <returns>True if the student is successfully added, false otherwise.</returns>
        private bool AddStudnet(string serializedStudnet)
        {
            Logger.Inform( $"[Instructor View Model] One message received. Serialized student information: {serializedStudnet}");
            if (serializedStudnet != null)
            {
                // Trying to decerialize the student info
                (string?, string?, string?, int, int) result = DeserializeStudnetInfo(serializedStudnet);
                string? rollNo = result.Item1;
                string? name = result.Item2;
                string? ip = result.Item3;
                int port = result.Item4;
                int isConnect = result.Item5;

                //Proceding if the required values are not null
                if (rollNo != null && name != null && ip != null)
                {
                    if (isConnect == 1)
                    {
                        //adding student in local data structure
                        _studentSessionState.AddStudent(rollNo, name, ip, port);
                        //acknowledging student about accepting connection
                        Communicator.Send("1", $"{rollNo}");
                        Logger.Inform( $"[InstructorViewModel] Added student: Roll No - {rollNo}, Name - {name}, IP - {ip}, Port - {port}");
                    }
                    else if (isConnect == 0)
                    {
                        //removing student in local data structure
                        _studentSessionState.RemoveStudent(rollNo);
                        //acknowledging student about removing connection
                        //Communicator.Send("0", $"{rollNo}");
                        Logger.Inform($"[InstructorViewModel] Removed student: Roll No - {rollNo}");
                    }
                    OnPropertyChanged(nameof(StudentList));
                   
                    OnPropertyChanged(nameof(StudentCount));
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Logs out the instructor, disconnecting all students.
        /// </summary>
        public void Logout()
        {
            DisconnectAllStudents();
            Logger.Inform( $"[InstructorViewModel] Disconnected all students." );

            // Waiting for some time for messages to be send
            Thread.Sleep( 4000 );
            // Stopping the communicator before logging out
            Communicator.Stop();
        }

        /// <summary>
        /// Disconnects all students currently connected.
        /// </summary>
        public void DisconnectAllStudents()
        {
            Logger.Inform( $"[InstructorViewModel] Disconnecting all students." );
            
            // Retrieving the list of all students from the session state
            List<Student> _studentList = new( _studentSessionState.GetAllStudents() );

            // Iterating through each student in the list and sending a disconnection message
            foreach ( Student student in _studentList )
            {
                try
                {
                    // Sending a disconnection message to the student using the Communicator
                    Communicator.Send( "0" , $"{student.Id}" );
                    Logger.Inform( $"[InstructorViewModel] Disconnection message sent to student {student.Id}" );
                }
                catch
                {
                    // Logging if the disconnection message fails to send to a student
                    Logger.Inform( $"[InstructorViewModel] Disconnection message to student {student.Id} failed." );
                }
            }
            _studentSessionState.RemoveAllStudents();
        }

        /// <summary>
        /// Handles the reception of a network message.
        /// </summary>
        /// <param name="data">The received network message.</param>
        /// <returns>An empty string.</returns>
        public string HandleMessageRecv(Networking.Models.Message data)
        {
            Logger.Inform( $"[InstructorViewModel] Received message {data.Data}" );
            AddStudnet(data.Data);
            return "";
        }
    }
}
