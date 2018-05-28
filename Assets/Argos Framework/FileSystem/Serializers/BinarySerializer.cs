using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Argos.Framework.FileSystem.Serializers
{
    /// <summary>
    /// Binary Serializer.
    /// </summary>
    public static class BinarySerializer
    {
        public static void Serialize(object data, string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static T Deserialize<T>(string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            T ret = (T)formatter.Deserialize(stream);
            stream.Close();
            return ret;
        }
    }
}