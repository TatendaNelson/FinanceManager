using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace Finance_Manager
{
    class FMTransactions
    {
        public string username;

        public FMTransactions()
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();
            FMLogin login = new FMLogin();

            this.username = login.checkLogin();

            if (doc.GetElementsByTagName("Transactions").Count <= 0)
            {
                XmlElement ele = doc.CreateElement("Transactions");

                XmlElement parent = (XmlElement)doc.SelectSingleNode("//Database");
                parent.AppendChild(ele);
            }

            XmlNodeList transactionsList = doc.SelectNodes("//Transactions/UserTransactions[@id='" + this.username + "']");

            if (transactionsList.Count <= 0)
            {
                XmlElement transactionsEle = (XmlElement)doc.SelectSingleNode("//Transactions");
                XmlElement userTransactionsEle = doc.CreateElement("UserTransactions");
                userTransactionsEle.SetAttribute("id", username);

                transactionsEle.AppendChild(userTransactionsEle);
            }

            doc.Save(db.xml_file);
        }

        //Adds the transaction to the database
        public string addTransaction(String name, double amount, String category, String source, String link = null, String notes = "", string date = "")
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            Random rand = new Random();
            int id = rand.Next(10000000);

            while (doc.SelectNodes("TransactionItem[@id='" + id.ToString() + "']").Count > 0)
            {
                id = rand.Next(10000000);
            }


            XmlElement parentElement = doc.CreateElement("TransactionItem");
            parentElement.SetAttribute("id", id.ToString());

            String xml = "<Name>" + name + "</Name>" +
                         "<Amount>" + amount.ToString() + "</Amount>" +
                         "<Category>" + category + "</Category>" +
                         "<Link>" + link + "</Link>" +
                         "<Source>" + source + "</Source>" +
                         "<Notes>" + notes + "</Notes>" +
                         "<Date>" + date.ToString() + "</Date>";

            parentElement.InnerXml = xml;

            doc.SelectSingleNode("//UserTransactions[@id='" + this.username + "']").AppendChild(parentElement);

            doc.Save(db.xml_file);
            return "true";
        }

        //Updates transactions
        public string updateTransaction(String id, String type, String link, String source, double amount = 0.0)
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            XmlElement transactionItemType = (XmlElement)doc.SelectSingleNode("//TransactionItem[@id='" + id.ToString() + "']/Type");
            XmlElement transactionItemAmount = (XmlElement)doc.SelectSingleNode("//TransactionItem[@id='" + id.ToString() + "']/Amount");
            XmlElement transactionItemLink = (XmlElement)doc.SelectSingleNode("//TransactionItem[@id='" + id.ToString() + "']/Link");
            XmlElement transactionItemSource = (XmlElement)doc.SelectSingleNode("//TransactionItem[@id='" + id.ToString() + "']/Source");

            transactionItemType.InnerText = type;
            transactionItemAmount.InnerText = amount.ToString();
            transactionItemLink.InnerText = link;
            transactionItemSource.InnerText = source;

            doc.Save(db.xml_file);

            return "true";
        }

        //Delete the transaction
        public string deleteTransaction(string id)
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            XmlNode ele = doc.SelectSingleNode("//TransactionItem[@id='" + id.ToString() + "']");

            ele.ParentNode.RemoveChild(ele);

            doc.Save(db.xml_file);
            return "true";
        }

        //Reads the transaction
        public XmlNode readTransactions()
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            return doc.SelectSingleNode("//UserTransactions[@id='" + this.username + "']");
        }

        //Gets the total of the expenses
        public double getTotal()
        {
            double total = 0;

            XmlElement incomeEle = (XmlElement)this.readTransactions();
            XmlNodeList amountsList = incomeEle.GetElementsByTagName("Amount");

            foreach (XmlNode amountNode in amountsList)
            {
                total += Convert.ToDouble(amountNode.InnerText);
            }

            return total;
        }
    }
}
