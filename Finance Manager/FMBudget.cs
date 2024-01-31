using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace Finance_Manager
{
    class FMBudget
    {
        String username;

        public FMBudget()
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();
            FMLogin login = new FMLogin();

            this.username = login.checkLogin();

            if (doc.GetElementsByTagName("Budget").Count <= 0)
            {
                XmlElement ele = doc.CreateElement("Budget");

                XmlElement parent = (XmlElement)doc.SelectSingleNode("//Database");
                parent.AppendChild(ele);
            }

            XmlNodeList budgetList = doc.SelectNodes("//Budget/UserBudget[@id='" + this.username + "']");

            if (budgetList.Count <= 0)
            {
                XmlElement budgetEle = (XmlElement)doc.SelectSingleNode("//Budget");
                XmlElement userBudgetEle = doc.CreateElement("UserBudget");
                userBudgetEle.SetAttribute("id", username);

                budgetEle.AppendChild(userBudgetEle);
            }

            doc.Save(db.xml_file);
        }

        //Adds budgets to the database
        public string addBudget(String name, double price, int quantity, String category = "Uncategorized", String notes = "")
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            XmlElement userBudget = (XmlElement)doc.SelectSingleNode("//UserBudget[@id='" + this.username + "']");

            XmlElement budgetNameEle = doc.CreateElement("Name");
            XmlElement budgetPriceEle = doc.CreateElement("Price");
            XmlElement budgetQuantityEle = doc.CreateElement("Quantity");
            XmlElement budgetCategoryEle = doc.CreateElement("Category");
            XmlElement notesEle = doc.CreateElement("Notes");

            budgetNameEle.InnerText = name;
            budgetPriceEle.InnerText = price.ToString();
            budgetQuantityEle.InnerText = quantity.ToString();
            budgetCategoryEle.InnerText = category;
            notesEle.InnerText = notes;

            Random rand = new Random();
            int id = rand.Next(10000000);

            while (doc.SelectNodes("BudgetItem[@id='" + id.ToString() + "']").Count > 0)
            {
                id = rand.Next(10000000);
            }

            XmlElement budgetItemEle = doc.CreateElement("BudgetItem");

            budgetItemEle.SetAttribute("id", id.ToString());
            budgetItemEle.AppendChild(budgetNameEle);
            budgetItemEle.AppendChild(budgetPriceEle);
            budgetItemEle.AppendChild(budgetQuantityEle);
            budgetItemEle.AppendChild(budgetCategoryEle);
            budgetItemEle.AppendChild(notesEle);

            userBudget.AppendChild(budgetItemEle);

            doc.Save(db.xml_file);

            return "true";
        }

        //Updates the database
        public string updateBudget(String id, String category = "Uncategorized", double price = 0.0, int quantity = 1)
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            XmlElement budgetItemCategory = (XmlElement)doc.SelectSingleNode("//BudgetItem[@id='" + id.ToString() + "']/Category");
            XmlElement budgetItemPrice = (XmlElement)doc.SelectSingleNode("//BudgetItem[@id='" + id.ToString() + "']/Price");
            XmlElement budgetItemQuantity = (XmlElement)doc.SelectSingleNode("//BudgetItem[@id='" + id.ToString() + "']/Quantity");

            budgetItemCategory.InnerText = category;
            budgetItemPrice.InnerText = price.ToString();
            budgetItemQuantity.InnerText = quantity.ToString();


            doc.Save(db.xml_file);

            return "true";
        }

        //Deletes budget
        public string deleteBudget(string id)
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            XmlNode ele = doc.SelectSingleNode("//BudgetItem[@id='" + id.ToString() + "']");

            ele.ParentNode.RemoveChild(ele);

            doc.Save(db.xml_file);
            return "true";
        }

        //Reads budget information for the user account
        public XmlNode readBudget()
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            return doc.SelectSingleNode("//UserBudget[@id='" + this.username + "']");
        }

        //Gets total amount of the budget for the user
        public double getTotal()
        {
            double total = 0;

            XmlElement budgetEle = (XmlElement)this.readBudget();
            XmlNodeList budgetList = budgetEle.ChildNodes;

            foreach (XmlNode budgetNode in budgetList)
            {
                XmlElement ele = (XmlElement)budgetNode;

                String price = ele.GetElementsByTagName("Price")[0].InnerText;
                String quantity = ele.GetElementsByTagName("Quantity")[0].InnerText;

                total += Convert.ToDouble(price) * Convert.ToInt32( quantity );
            }

            return total;
        }
    }
}