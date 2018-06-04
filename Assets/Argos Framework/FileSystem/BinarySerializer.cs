using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Argos.Framework.FileSystem
{
    /// <summary>
    /// Binary Serializer.
    /// </summary>
    public static class BinarySerializer
    {
        public static byte[] Serialize(object data)
        {
            byte[] buffer;
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, data);
                buffer = stream.GetBuffer();
            }
            return buffer;
        }

        public static T Deserialize<T>(byte[] buffer)
        {
            T ret;
            using (var stream = new MemoryStream(buffer))
            {
                ret = (T)(new BinaryFormatter()).Deserialize(stream);
            }
            return ret;
        }
    }
}