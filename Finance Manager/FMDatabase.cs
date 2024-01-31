using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading.Tasks;

namespace Finance_Manager
{
    //Helps reading, adding, deleting and updatin the data for the application
    class FMDatabase
    {
        public string xml_file = "data.xml";

        public FMDatabase()
        {
            if (!File.Exists(this.xml_file))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Database></Database>");

                doc.Save(xml_file);

            }
        }

        //Reads the data for the application
        public XmlDocument read()
        {
            string xmlData = File.ReadAllText(this.xml_file);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData);

            return doc;
        }
    }
}
