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
    public class InstructorViewModel : INotifyPropertyChanged, IEventHandler
    {
        private readonly ICommunicator server; // Communicator used to send and receive messages.
        private readonly StudentSessionState _studentSessionState; // To manage the connected studnets

        /// <summary>
        /// Constructor for the DashboardViewModel.
        /// </summary>
        public InstructorViewModel(ICommunicator? communicator = null)
        {
            _studentSessionState = new();
            server = communicator ?? CommunicationFactory.GetServer();

            //IpAddress = GetPrivateIp();

            var ipPort = server.Start(null, null, "server", "Dashboard");
            server.Subscribe(this, "Dashboard");
            string[] parts = ipPort.Split(':');
            try
            {
                IpAddress = parts[0];
                ReceivePort = parts[1];
                OnPropertyChanged(nameof(IpAddress));
                OnPropertyChanged(nameof(ReceivePort));
            }
            catch { }

            _studentSessionState.AddStudent(21323, "arkka", "13151", 123);

        }

        //public List<Student> StudentList 
        //{ 
        //    get
        //    {
        //        return _studentSessionState.GetAllStudents();
        //    }
        //}

        public ObservableCollection<Student> StudentList
        {
            get
            {
                return new ObservableCollection<Student>(_studentSessionState.GetAllStudents());
            }
        }

        public int StudentCount
        {
            get
            {
                return _studentSessionState.GetStudentsCount();
            }
        }

        public ICommunicator Communicator
        {
            get
            {
                return server;
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
        /// Property changed event raised when a property is changed on a component.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the private IP address of the host machine.
        /// </summary>
        /// <param name="property">The name of the property</param>
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private static Dispatcher Dispatcher => Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;
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

        private static string SerializeStudnetInfo(string? name, string? rollNo, string? ip, string? port)
        {
            return $"{rollNo}|{name}|{ip}|{port}";
        }

        private static (int, string?, string?, int, int) DeserializeStudnetInfo(string data)
        {
            string[] parts = data.Split('|');
            if (parts.Length == 5)
            {
                try
                {
                    return
                    (
                        int.Parse(parts[0]),
                        parts[1],
                        parts[2],
                        int.Parse(parts[3]),
                        int.Parse(parts[4])
                    );
                }
                catch { }

            }
            return (0, null, null, 0, 0);
        }

        private bool AddStudnet(string serializedStudnet)
        {
            Debug.WriteLine($"One message received {serializedStudnet}");
            if (serializedStudnet != null)
            {
                var result = DeserializeStudnetInfo(serializedStudnet);
                var rollNo = result.Item1;
                var name = result.Item2;
                var ip = result.Item3;
                var port = result.Item4;
                var isConnect = result.Item5;
                if (name != null && ip != null)
                {
                    if (isConnect == 1)
                    {
                        _studentSessionState.AddStudent(rollNo, name, ip, port);
                        server.Send("1",$"{rollNo}");
                    }
                    else if (isConnect == 0) 
                    {
                        _studentSessionState.RemoveStudent(rollNo);
                        server.Send("0", $"{rollNo}");
                    }
                    //Dispatcher.Invoke((string property) =>
                    //{
                    //    OnPropertyChanged(property);
                    //}, nameof(StudentList));
                    OnPropertyChanged(nameof(StudentList));
                    //Dispatcher.Invoke((string property) =>
                    //{
                    //    OnPropertyChanged(property);
                    //}, nameof(StudentCount));
                    OnPropertyChanged(nameof(StudentCount));
                    return true;
                }
            }
            return false;
        }
        public string HandleMessageRecv(Networking.Models.Message data)
        {
            Dispatcher.Invoke((string data) =>
            {
                AddStudnet(data);
            }, data.Data);
            return "";
        }
    }
}
