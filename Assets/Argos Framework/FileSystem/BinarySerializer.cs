using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Argos.Framework.FileSystem
{
    /// <summary>
    /// Binary Serializer.
    /// </summary>
    public static class BinarySerializer
    {
        #region Delegates
        public delegate void BinarySerializationHandler(ref byte[] buffer);
        #endregion

        #region Static Methods & Functions
        /// <summary>
        /// Serialize data to binary array.
        /// </summary>
        /// <param name="data">Data to serialize.</param>
        /// <param name="onSerialized">Optional event after the data has been serialized (useful for encrypt the data).</param>
        /// <returns>Byte array with the serialized data.</returns>
        public static byte[] Serialize(object data, BinarySerializationHandler onSerialized = null)
        {
            byte[] buffer;
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, data);
                buffer = stream.GetBuffer();
                onSerialized?.Invoke(ref buffer);
            }
            return buffer;
        }

        /// <summary>
        /// Deserialize data.
        /// </summary>
        /// <typeparam name="T">Original type of the serialized data.</typeparam>
        /// <param name="buffer">Byte array with the serialized data.</param>
        /// <param name="onDeserializing">Optional event before the data has been deserialized (useful for desencrypt the data, if previosuly has been encrypted).</param>
        /// <returns>Return an instance of T with the deserialized data.</returns>
        public static T Deserialize<T>(byte[] buffer, BinarySerializationHandler onDeserializing = null)
        {
            T ret;

            onDeserializing?.Invoke(ref buffer);

            using (var stream = new MemoryStream(buffer))
            {
                ret = (T)(new BinaryFormatter()).Deserialize(stream);
            }
            return ret;
        } 
        #endregion
    }
}