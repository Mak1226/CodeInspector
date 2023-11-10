using Networking.Serialization;
using System.Xml.Serialization;

namespace Content.Encoder
{
    /// <summary>
    /// Serialises an object by converting it into an XML
    /// For now it's just Tuple<Dictionary<string, string>
    /// </summary>
    internal class AnalyzerResultSerializer : ISerializer
    {
        public string Serialize<T>(T genericObject)
        {
            if (genericObject == null)
            {
                throw new ArgumentNullException(nameof(genericObject));
            }

            var serializer = new XmlSerializer(typeof(T));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, genericObject);
                return writer.ToString();
            }
        }

        public T Deserialize<T>(string serializedString)
        {
            if (string.IsNullOrWhiteSpace(serializedString))
            {
                throw new ArgumentException("Serialized string is null or empty.", nameof(serializedString));
            }

            var serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(serializedString))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

    }
}
