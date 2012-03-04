using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Globalization;

namespace MediaVF.Common.Communication
{
    public class SerializationUtility
    {
        bool UseEncryption { get; set; }

        string EncryptionKey { get; set; }

        public SerializationUtility(bool useEncryption, string encryptionKey)
        {
            UseEncryption = useEncryption;
            EncryptionKey = encryptionKey;
        }

        public SerializationUtility()
            : this(false, string.Empty) { }

        public List<InvokeParameter> CreateInvokeParameters(List<Tuple<Type, object>> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                List<InvokeParameter> serializedParameters = new List<InvokeParameter>();
                foreach (Tuple<Type, object> parameter in parameters)
                    serializedParameters.Add(new InvokeParameter() { ParameterType = parameter.Item1.FullName, SerializedParameter = Serialize(parameter.Item1, parameter.Item2) });

                return serializedParameters;
            }

            return null;
        }

        public string Serialize(Type t, object obj)
        {
            string xml;

            if (t.GetCustomAttributes(typeof(DataContractAttribute), true).Any())
            {
                StringBuilder objXml = new StringBuilder();
                XmlWriter xmlWriter = XmlWriter.Create(objXml);
                DataContractSerializer serializer = new DataContractSerializer(t);
                serializer.WriteObject(xmlWriter, obj);
                xmlWriter.Flush();
                xml = objXml.ToString();
                xmlWriter.Close();
            }
            else
                xml = obj.ToString();

            if (UseEncryption)
                return EncryptionUtility.Encrypt(EncryptionKey, xml);
            else
                return xml;
        }

        public object Deserialize(Type t, string xmlData)
        {
            if (UseEncryption)
                xmlData = EncryptionUtility.Decrypt(EncryptionKey, xmlData);

            object obj;
            if (t.GetCustomAttributes(typeof(DataContractAttribute), true).Any())
            {
                XmlReader xmlReader = XmlReader.Create(new StringReader(xmlData));
                DataContractSerializer serializer = new DataContractSerializer(t);
                obj = serializer.ReadObject(xmlReader);
                xmlReader.Close();
            }
            else
                obj = Convert.ChangeType(xmlData, t, CultureInfo.CurrentCulture);

            return obj;
        }
    }
}
