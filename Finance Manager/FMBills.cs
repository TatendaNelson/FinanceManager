using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace Finance_Manager
{
    class FMBills
    {
        public string username;

        public FMBills()
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();
            FMLogin login = new FMLogin();

            this.username = login.checkLogin();

            if (doc.GetElementsByTagName("Bills").Count <= 0)
            {
                XmlElement ele = doc.CreateElement("Bills");

                XmlElement parent = (XmlElement)doc.SelectSingleNode("//Database");
                parent.AppendChild(ele);
            }

            XmlNodeList billsList = doc.SelectNodes("//Transactions/UserBills[@id='" + this.username + "']");

            if (billsList.Count <= 0)
            {
                XmlElement billsEle = (XmlElement)doc.SelectSingleNode("//Bills");
                XmlElement userBillsEle = doc.CreateElement("UserBills");
                userBillsEle.SetAttribute("id", username);

                billsEle.AppendChild(userBillsEle);
            }

            doc.Save(db.xml_file);
        }

        //Adds tthe bill database
        public string addBill(String name, string dueDate, double amount)
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            FMBudget budget = new FMBudget();

            Random rand = new Random();
            int id = rand.Next(10000000);

            while (doc.SelectNodes("BillItem[@id='" + id.ToString() + "']").Count > 0)
            {
                id = rand.Next(10000000);
            }


            XmlElement parentElement = doc.CreateElement("BillItem");
            parentElement.SetAttribute("id", id.ToString());

            String xml = "<Name>" + name + "</Name>" +
                         "<Amount>" + amount.ToString() + "</Amount>" +
                         "<Date>" + dueDate + "</Date>";

            parentElement.InnerXml = xml;

            doc.SelectSingleNode("//UserBills[@id='" + this.username + "']").AppendChild(parentElement);
            budget.addBudget(name + "|" + id, amount, 1, "Bills");

            doc.Save(db.xml_file);
            return "true";
        }

        //Updates the bill item
        public string updateBill(String id, double amount = 0.0)
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            XmlElement billItemAmount = (XmlElement)doc.SelectSingleNode("//BillItem[@id='" + id.ToString() + "']/Amount");

            billItemAmount.InnerText = amount.ToString();

            doc.Save(db.xml_file);

            return "true";
        }

        //Deletes the bills from the database
        public string deleteBill(string id)
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            XmlNode ele = doc.SelectSingleNode("//BillItem[@id='" + id.ToString() + "']");

            ele.ParentNode.RemoveChild(ele);

            doc.Save(db.xml_file);
            return "true";
        }

        //Read bills 
        public XmlNode readBills()
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            return doc.SelectSingleNode("//UserBills[@id='" + this.username + "']");
        }

        //Reads the payment of the bill
        public String[] getPaidDetails(String id)
        {
            FMTransactions transactions = new FMTransactions();
            XmlNodeList userTransactions = transactions.readTransactions().ChildNodes;

            double amount = 0;
            String paidDate = "";

            foreach (XmlNode transaction in userTransactions)
            {
                XmlElement transactionEle = (XmlElement)transaction;

                String link = transactionEle.GetElementsByTagName("Link")[0].InnerText;

                if (link == id )
                {
                    amount += Convert.ToDouble(transactionEle.GetElementsByTagName("Amount")[0].InnerText);
                    paidDate = transactionEle.GetElementsByTagName("Date")[0].InnerText;
                }
            }

            String[] data = {  amount.ToString(), paidDate };
            return data;
        }

        //Returns the bills links ids
        public String[] readBillsLink() {
            XmlNodeList list = this.readBills().ChildNodes;
            

            String[] links = new string[ list.Count + 1 ];
            links[0] = "Not a bill";
            int i = 1;

            foreach( XmlNode item in list ){
                XmlElement ele = (XmlElement)item;

                links[i] = ele.GetAttribute("id");
                i++;
            }
            return links;
        }

        //Checks if the bill is overdue
        public bool isOverdue( String id )
        {
            XmlNode bills = this.readBills();
            XmlElement bill = (XmlElement) bills.SelectSingleNode("//BillItem[@id='" + id + "']");

            XmlElement date = (XmlElement)bill.GetElementsByTagName("Date")[0];

            DateTime today = DateTime.Now;
            DateTime dueDate = DateTime.Parse( date.InnerText );

            return ( dueDate.CompareTo( today ) <= 0);
        }

        //Get the number of bills overdue
        public int getOverDue()
        {
            int overdue = 0;
            XmlNodeList billsList = this.readBills().ChildNodes;

            foreach( XmlNode billNode in billsList ){

                XmlElement ele = (XmlElement) billNode;
                overdue += (this.isOverdue( ele.GetAttribute("id") ) ) ? 1 : 0; 
            }

            return overdue;
        }

        //Get the total of the amount spent on the bills
        public double getPaidTotal()
        {
            double total = 0;


            XmlNodeList billsList = this.readBills().ChildNodes;

            foreach( XmlNode billNode in billsList ){

                XmlElement ele = (XmlElement) billNode;
                total += Convert.ToDouble(this.getPaidDetails(ele.GetAttribute("id"))[0]);
            }

            return total;
        }

        //Gets the total amount to be paid for tyhe bills
        public double getTotal()
        {
            double total = 0;

            XmlElement billsEle = (XmlElement)this.readBills();
            XmlNodeList billsList = billsEle.ChildNodes;

            foreach (XmlNode billsNode in billsList)
            {
                XmlElement ele = (XmlElement)billsNode;

                String amount = ele.GetElementsByTagName("Amount")[0].InnerText;
              

                total += Convert.ToDouble(amount);
            }

            return total;
        }
    }
}
