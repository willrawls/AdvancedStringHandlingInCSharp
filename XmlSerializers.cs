using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AdvancedStringHandlingInCSharp
{
    public static class XmlSerializers
    {
        /// <summary>The ?xml directive that should be at the top of each file</summary>
        public const string Declaration = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";

        private static SortedList<int, XmlSerializer> _mSerializers;

        /// <summary>
        /// Turns an xml string into a object
        /// </summary>
        /// <typeparam name="T">The type to return a XmlSerializer for</typeparam>
        /// <param name="xmlDoc">An xml string containing the serialized object</param>
        /// <returns>The deserializd object</returns>
        public static T FromXml<T>(string xmlDoc)
        {
            using (var sr = new StringReader(xmlDoc))
                return (T)Serializer(typeof(T)).Deserialize(sr);
        }

        /// <summary>
        /// Turns the xml contents of a file into an object
        /// </summary>
        /// <typeparam name="T">The type to return a XmlSerializer for</typeparam>
        /// <param name="filePath">The file to read the xml from</param>
        /// <returns>The deserializd object</returns>
        public static T LoadFile<T>(string filePath)
        {
            if (File.Exists(filePath))
                using (var xtr = new XmlTextReader(filePath))
                    return (T)Serializer(typeof(T)).Deserialize(xtr);
            //using (StreamReader s = System.IO.File.OpenText(FilePath))
            //return (T) Serializer(typeof(T)).Deserialize(s);
            return default(T);
        }

        /// <summary>
        /// Save a object as xml into a file. If the file is already there it is deleted then recreated with the xml contents of the supplied object.
        /// </summary>
        /// <typeparam name="T">The type to return a XmlSerializer for</typeparam>
        /// <param name="filePath">The file to write the xml to</param>
        /// <param name="toSerialize">The object to serialize</param>
        public static void SaveFile<T>(string filePath, T toSerialize)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                File.Delete(filePath);
            }
            using (var xtw = new XmlTextWriter(filePath, Encoding.UTF8))
                Serializer(typeof(T)).Serialize(xtw, toSerialize);
            //using (StreamWriter sw = File.CreateText(FilePath))
            //Serializer(typeof(T)).Serialize(sw, ToSerialize);
        }

        /// <summary>
        /// Returns a XmlSerializer for the given type. Repeated calls pull the serializer previously used. Serializers are stored internally in a sorted list for quick retrieval.
        /// </summary>
        /// <param name="t">The type to return a XmlSerializer for</param>
        /// <returns>The XmlSerializer for the type</returns>
        public static XmlSerializer Serializer(Type t)
        {
            if (_mSerializers == null)
                _mSerializers = new SortedList<int, XmlSerializer>(10);
            XmlSerializer xs = null;
            var hash = t.FullName.GetHashCode();
            if (_mSerializers.ContainsKey(hash))
                xs = _mSerializers[hash];
            else
            {
                xs = new XmlSerializer(t);
                _mSerializers.Add(hash, xs);
            }
            return xs;
        }

        /// <summary>
        /// Turns an object into an xml string
        /// </summary>
        /// <typeparam name="T">The type to return a XmlSerializer for</typeparam>
        /// <param name="toSerialize">The object to serialize</param>
        /// <param name="removeNamespaces">True if you're serializing a node and don't want any of the standard xml namespaces included</param>
        /// <returns></returns>
        public static string ToXml<T>(T toSerialize, bool removeNamespaces = true) where T : new()
        {
            var sb = new StringBuilder();
            using (var xw = GetXmlWriter(sb))
                Serializer(typeof(T)).Serialize(xw, toSerialize);
            if (!removeNamespaces) return sb.ToString();
            
            sb.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", string.Empty);
            sb.Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
            return sb.ToString();
        }

        /// <summary>
        /// Don't forget to close the XmlWriter or wrap this line in a using statement
        /// </summary>
        /// <param name="output">The stream to wrap</param>
        /// <returns></returns>
        public static XmlWriter GetXmlWriter(this Stream output)
        {
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            };
            return XmlWriter.Create(output, settings);
        }

        /// <summary>
        /// Don't forget to close the XmlWriter or wrap this line in a using statement
        /// </summary>
        /// <param name="output">The TextWriter to wrap</param>
        /// <returns></returns>
        public static XmlWriter GetXmlWriter(this TextWriter output)
        {
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            };
            return XmlWriter.Create(output, settings);
        }

        /// <summary>
        /// Don't forget to close the XmlWriter or wrap this line in a using statement
        /// </summary>
        /// <param name="output">The StringBuilder to wrap</param>
        /// <returns></returns>
        public static XmlWriter GetXmlWriter(this StringBuilder output)
        {
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            };
            return XmlWriter.Create(output, settings);
        }
    }
}