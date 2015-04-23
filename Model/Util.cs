using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Model
{
    public class Util
    {
        /// <summary>
        /// Converts a String into a byte array
        /// </summary>
        /// <param name="pXmlString">UTF8 Encoded object string</param>
        /// <returns>Array fo bytes</returns>
        private static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }


        /// <summary>
        /// Transform an array of byts to a UTF8 Encoded string
        /// </summary>
        /// <param name="pCharacters">Array of bytes</param>
        /// <returns>UTF8 Encoded string</returns>
        private static String UTF8ByteArrayToString(Byte[] pCharacters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(pCharacters);
            return (constructedString);
        }

        /// <summary>
        /// Transforms XML to an object
        /// </summary>
        /// <param name="pXmlObject">Class formated XML string</param>
        /// <param name="pType">Type of object</param>
        /// <returns>object</returns>
        public static Object Deserialize(string pXmlObject, Type pType)
        {
            XmlSerializer oXs = new XmlSerializer(pType);
            MemoryStream oMemoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlObject));
            XmlTextWriter oXmlTextWriter = new XmlTextWriter(oMemoryStream, Encoding.UTF8);

            return oXs.Deserialize(oMemoryStream);
        }


        /// <summary>
        /// Transform a C# Object to a XML string
        /// </summary>
        /// <param name="pObject">C# Object</param>
        /// <param name="pType">Type of the object to serialize</param>
        /// <returns>XML string</returns>
        public static String SerializeObject(Object pObject, Type pType)
        {

            try
            {
                String XmlizedString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(pType);
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                xs.Serialize(xmlTextWriter, pObject);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
                return XmlizedString;
            }
            catch
            {
                return null;
            }
        }
    }
}
