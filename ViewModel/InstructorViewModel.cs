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
        private readonly ISessionState _studentSessionState; // To manage the connected students

        /// <summary>
        /// Constructor for the DashboardViewModel.
        /// </summary>
        public InstructorViewModel( string userName, string userId, ICommunicator? communicator = null )
        {
            UserName = userName;
            UserId = userId;
            _studentSessionState = new StudentSessionState();
            Communicator = communicator ?? CommunicationFactory.GetServer();

            string ipPort = Communicator.Start(null, null, "server", "Dashboard");
            Communicator.Subscribe(this, "Dashboard");
            string[] parts = ipPort.Split(':');
            try
            {
                IpAddress = parts[0];
                ReceivePort = parts[1];
                OnPropertyChanged(nameof(IpAddress));
                OnPropertyChanged(nameof(ReceivePort));
            }
            catch { }
        }
        /// <summary>
        /// Converting student list from <typeparamref name="List"/> to <typeparamref name="ObservableCollection"/>
        /// </summary>
        public ObservableCollection<Student> StudentList => new( _studentSessionState.GetAllStudents() );

        public int StudentCount => _studentSessionState.GetStudentsCount();

        public ICommunicator Communicator { get; }

        public string UserName { get; init; }

        public string UserId { get; init; }

        public string? ReceivePort { get; private set; }

        public string? IpAddress { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

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

        private static (string?, string?, string?, int, int) DeserializeStudnetInfo(string data)
        {
            string[] parts = data.Split('|');
            if (parts.Length == 5)
            {
                try
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
                catch { }

            }
            return (null, null, null, 0, 0);
        }

        private bool AddStudnet(string serializedStudnet)
        {
            Debug.WriteLine($"One message received {serializedStudnet}");
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
                        Communicator.Send("1",$"{rollNo}");
                    }
                    else if (isConnect == 0) 
                    {
                        _studentSessionState.RemoveStudent(rollNo);
                        Communicator.Send("0", $"{rollNo}");
                    }
                    
                    OnPropertyChanged(nameof(StudentList));
                   
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
