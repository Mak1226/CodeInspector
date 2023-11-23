/******************************************************************************
 * Filename    = Serialization/ISerializer.cs
 *
 * Author      = Shubhang Kedia
 *
 * Product     = Analyzer
 * 
 * Project     = Networking
 *
 * Description = Defines an interface for serialization and deserialization of objects.
 *****************************************************************************/

namespace Networking.Serialization
{
    /// <summary>
    /// Defines an interface for serialization and deserialization of objects.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serializes an object of type <typeparamref name="T"/> into a string.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="genericObject">The object to serialize.</param>
        /// <returns>The serialized string. "failed" if failed</returns>
        string Serialize<T>( T genericObject );

        /// <summary>
        /// Deserializes a string into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="serializedString">The string to deserialize.</param>
        /// <returns>The deserialized object. default if deserialization failed.</returns>
        T Deserialize<T>( string serializedString );
    }
}
