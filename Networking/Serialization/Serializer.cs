using System.Text.Json;

namespace Networking.Serialization
{
	public class Serializer
	{
        public static T Deserialize<T>(string serializedString)
        {
            T message = JsonSerializer.Deserialize<T>(serializedString);
            return message;
        }

        public static string Serialize<T>(T genericObject)
        {
            string message=JsonSerializer.Serialize(genericObject);
            return message;
        }
    }
}

