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
        public InstructorViewModel( string name, string userId, ICommunicator? communicator = null )
        {
            Name = name;
            UserId = userId;
            _studentSessionState = new StudentSessionState();
            Communicator = communicator ?? CommunicationFactory.GetServer();

            string ipPort = Communicator.Start(null, null, "server", "Dashboard");
            Communicator.Subscribe(this, "Dashboard");
            string[] parts = ipPort.Split(':');
            if(parts.Length==2)
            {
                IpAddress = parts[0];
                ReceivePort = parts[1];
                OnPropertyChanged(nameof(IpAddress));
                OnPropertyChanged(nameof(ReceivePort));
            }
            else
            {
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


        private static Dispatcher Dispatcher => Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;

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
            Trace.WriteLine($"One message received {serializedStudnet}");
            if (serializedStudnet != null)
            {
                (string?, string?, string?, int, int) result = DeserializeStudnetInfo(serializedStudnet);
                string? rollNo = result.Item1;
                string? name = result.Item2;
                string? ip = result.Item3;
                int port = result.Item4;
                int isConnect = result.Item5;
                if (rollNo != null && name != null && ip != null)
                {
                    if (isConnect == 1)
                    {
                        _studentSessionState.AddStudent(rollNo, name, ip, port);
                        Communicator.Send("1", $"{rollNo}");
                        Trace.WriteLine($"[Instructor View Model] Added student: Roll No - {rollNo}, Name - {name}, IP - {ip}, Port - {port}");
                    }
                    else if (isConnect == 0)
                    {
                        _studentSessionState.RemoveStudent(rollNo);
                        Communicator.Send("0", $"{rollNo}");
                        Trace.WriteLine($"[Instructor View Model] Removed student: Roll No - {rollNo}");
                    }
                    OnPropertyChanged(nameof(StudentList));
                   
                    OnPropertyChanged(nameof(StudentCount));
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Handles the reception of a network message.
        /// </summary>
        /// <param name="data">The received network message.</param>
        /// <returns>An empty string.</returns>
        public string HandleMessageRecv(Networking.Models.Message data)
        {
            Trace.WriteLine( $"[Instructor View Model] Received message {data.Data}" );
            AddStudnet(data.Data);
            return "";
        }
    }
}
