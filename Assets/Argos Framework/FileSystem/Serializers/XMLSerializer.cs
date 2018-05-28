using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Argos.Framework.FileSystem.Serializers
{
    /// <summary>
    /// XML Serializer.
    /// </summary>
    public static class XMLSerializer
    {
        public static void Serialize(object data, string filename)
        {
            XmlSerializer mySerializer = new XmlSerializer(data.GetType());
            StreamWriter myWriter = new StreamWriter(filename);
            mySerializer.Serialize(myWriter, data);
            myWriter.Close();
        }

        public static T Deserialize<T>(string filename)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            FileStream myFileStream = new FileStream(filename, FileMode.Open);
            T ret = (T)mySerializer.Deserialize(myFileStream);
            myFileStream.Close();
            return ret;
        }
    }
}