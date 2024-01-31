using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace Finance_Manager
{
    class FMIncome
    {
        private String username;

        public FMIncome()
        {
            FMLogin login = new FMLogin();
            FMDatabase db = new FMDatabase();

            XmlDocument doc = db.read();
            username = login.checkLogin();

            //Check if the parent Income exists
            if (doc.GetElementsByTagName("Income").Count <= 0)
            {
                XmlElement ele = doc.CreateElement("Income");
                XmlElement databaseEle = (XmlElement)doc.GetElementsByTagName("Database")[0];
                databaseEle.AppendChild(ele);
            }

            XmlNodeList usersList = doc.SelectNodes("//UserIncome[@id='" + username + "']");

            //Check if the current user has income
            if (usersList.Count <= 0)
            {
                XmlElement ele = (XmlElement)doc.GetElementsByTagName("Income")[0];

                XmlElement childEle = doc.CreateElement("UserIncome");
                childEle.SetAttribute("id", username);

                ele.AppendChild(childEle);

            }

            doc.Save(db.xml_file);
        }

        //Adds income to the database
        public string addIncome(String source, double amount, String notes, String category)
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();
            Random rand = new Random();

            int id = rand.Next(3000000);
            while (doc.GetElementById(id.ToString()) != null)
            {
                id = rand.Next(3000000);
            }

            XmlElement ele = (XmlElement)doc.SelectNodes("//UserIncome[@id='" + username + "']")[0];

            XmlElement sourceEle = doc.CreateElement("Source");
            XmlElement notesEle = doc.CreateElement("Notes");
            XmlElement categoryEle = doc.CreateElement("Category");
            XmlElement amountEle = doc.CreateElement("Amount");

            sourceEle.InnerText = source;
            notesEle.InnerText = notes;
            categoryEle.InnerText = category;
            amountEle.InnerText = amount.ToString();

            XmlElement incomeItemEle = doc.CreateElement("IncomeItem");
            incomeItemEle.SetAttribute("id", id.ToString());

            incomeItemEle.AppendChild(sourceEle);
            incomeItemEle.AppendChild(amountEle);
            incomeItemEle.AppendChild(notesEle);
            incomeItemEle.AppendChild( categoryEle );

            ele.AppendChild(incomeItemEle);

            doc.Save(db.xml_file);

            return "Done";
        }

        // Updates income
        public string updateIncome(int id, String source = null, double amount = 0.0)
        {

            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            XmlElement ele = (XmlElement)doc.SelectSingleNode("//IncomeItem[@id='" + id.ToString() + "']");

            XmlElement sourceEle = (XmlElement)ele.SelectSingleNode("//Source");
            XmlElement amountEle = (XmlElement)ele.SelectSingleNode("//Amount");

            sourceEle.InnerText = source;
            amountEle.InnerText = amount.ToString();

            doc.Save(db.xml_file);

            return "true";
        }

        //Deletes income from the database
        public string removeIncome( String id)
        {

            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            XmlNode ele = doc.SelectSingleNode("//IncomeItem[@id='" + id.ToString() + "']");

            ele.ParentNode.RemoveChild(ele);

            doc.Save(db.xml_file);

            return "true";
        }

        //Reads income information
        public XmlNode readIncome()
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            XmlNode ele = doc.SelectNodes("//UserIncome[@id='" + username + "']")[0];

            return ele;
        }

        //Gets the total income
        public double getTotal()
        {
            double total = 0;

            XmlElement incomeEle = (XmlElement) this.readIncome();
            XmlNodeList amountsList = incomeEle.GetElementsByTagName("Amount");

            foreach( XmlNode amountNode in amountsList ){
                total += Convert.ToDouble( amountNode.InnerText );
            }

            return total;
        }
    }
}
