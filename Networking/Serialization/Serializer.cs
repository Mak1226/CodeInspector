using System;
using System.Diagnostics;

namespace Networking.Serialization
{
	public class Serializer : ISerializer
	{
        public T Deserialize<T>(string serializedString)
        {
            Trace.WriteLine("Not yet implemented");
            throw new NotImplementedException();
        }

        public string Serialize<T>(T genericObject)
        {
            Trace.WriteLine("Not yet implemented");
            throw new NotImplementedException();
        }
    }
}

