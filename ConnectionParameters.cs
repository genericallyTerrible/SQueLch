using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SQueLch
{
    [Serializable()]
    public class ConnectionParameters
    {
        [XmlIgnore]
        public IPAddress ServerIP;
        [XmlElement("serverIP")]
        public string ServerIPForXml
        {
            get => ServerIP.ToString();
            set => ServerIP = string.IsNullOrEmpty(value) ? null :
                              IPAddress.Parse(value);
        }

        public uint Port;
        public string UserID;
        public string Password;

        public static string DefaultConnectionString()
        {
            return "server=127.0.0.1;"
                 + "port=3306;"
                 + "uid=root;"
                 + "pwd=;";
        }

        public static ConnectionParameters DefaultConnectionParameters()
        {
            return new ConnectionParameters()
            {
                ServerIP = IPAddress.Parse("127.0.0.1"),
                Port     = 3306,
                UserID   = "root",
                Password = ""
            };
        }

        public void Serialize(string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };

            XmlSerializer ser = new XmlSerializer(typeof(ConnectionParameters));
            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                ser.Serialize(writer, this);
            }
        }

        public static ConnectionParameters Deserialize(string path)
        {
            ConnectionParameters conPar;
            if (File.Exists(path))
            {
                XmlSerializer ser = new XmlSerializer(typeof(ConnectionParameters));

                using (XmlReader reader = XmlReader.Create(path))
                {
                    conPar = (ConnectionParameters)ser.Deserialize(reader);
                }
            }
            else
            {
                return DefaultConnectionParameters();
            }
            return conPar;
        }

        public override string ToString()
        {
            return "server=" + ServerIP.ToString()
                 + ";port=" + Port.ToString()
                 + ";uid=" + UserID
                 + ";pwd=" + Password
                 + ";";
        }
    }
}
