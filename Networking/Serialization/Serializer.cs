/******************************************************************************
 * Filename    = Serialization/Serializer.cs
 *
 * Author      = VM Sreeram
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = Provides static methods for serializing and deserializing objects using JSON.
 *****************************************************************************/

using System.Text.Json;

namespace Networking.Serialization
{
    /// <summary>
    /// Provides static methods for serializing and deserializing objects using JSON.
    /// </summary>
    public class Serializer
    {
        /// <summary>
        /// Deserializes a JSON string into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="serializedString">The JSON string to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static T Deserialize<T>( string serializedString )
        {
            T message = JsonSerializer.Deserialize<T>( serializedString );
            return message;
        }

        /// <summary>
        /// Serializes an object of type <typeparamref name="T"/> into a JSON string.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="genericObject">The object to serialize.</param>
        /// <returns>The serialized JSON string.</returns>
        public static string Serialize<T>( T genericObject )
        {
            string message = JsonSerializer.Serialize( genericObject );
            return message;
        }
    }
}
