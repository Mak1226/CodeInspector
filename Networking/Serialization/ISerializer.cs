using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Serialization
{
    public interface ISerializer
    {
        public string Serialize<T>(T genericObject);

        public T Deserialize<T>(string serializedString);
    }
}
