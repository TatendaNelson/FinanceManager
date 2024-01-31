using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace Finance_Manager
{
    class FMCategories
    {
        public string username;

        public FMCategories()
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();
            FMLogin login = new FMLogin();

            this.username = login.checkLogin();

            if (doc.GetElementsByTagName("Categories").Count <= 0)
            {
                XmlElement ele = doc.CreateElement("Categories");

                XmlElement parent = (XmlElement)doc.SelectSingleNode("//Database");
                parent.AppendChild(ele);
            }

            XmlNodeList categoriesList = doc.SelectNodes("//Categories/UserCategories[@id='" + this.username + "']");

            if (categoriesList.Count <= 0)
            {
                XmlElement categoriesEle = (XmlElement)doc.SelectSingleNode("//Categories");
                XmlElement userCategoriesEle = doc.CreateElement("UserCategories");
                userCategoriesEle.SetAttribute("id", username);

                categoriesEle.AppendChild(userCategoriesEle);
            }

            doc.Save(db.xml_file);
        }

        //Add category method
        public string addCategory(String name)
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();


            Random rand = new Random();
            int id = rand.Next(10000000);

            while (doc.SelectNodes("CategoryItem[@id='" + id.ToString() + "']").Count > 0)
            {
                id = rand.Next(10000000);
            }


            XmlElement parentElement = doc.CreateElement("CategoryItem");
            parentElement.SetAttribute("id", id.ToString());

            String xml = "<Name>" + name + "</Name>";

            parentElement.InnerXml = xml;

            doc.SelectSingleNode("//UserCategories[@id='" + this.username + "']").AppendChild(parentElement);

            doc.Save(db.xml_file);
            return "true";
        }

        //Delete the category
        public string deleteCategory(string id)
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            XmlNode ele = doc.SelectSingleNode("//CategoryItem[@id='" + id.ToString() + "']");

            ele.ParentNode.RemoveChild(ele);

            doc.Save( db.xml_file );
            return "true";
        }

        //Reading categories information
        public XmlNode readCategories()
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            return doc.SelectSingleNode("//UserCategories[@id='" + this.username + "']");
        }

        //Returns names of the categories
        public String[] readAsArray()
        {
            XmlNodeList listCategories = this.readCategories().ChildNodes;

            String[] categories = new string[listCategories.Count + 1];
            categories[0] = "Uncategorized";

            int i = 1;
            foreach( XmlNode categoriesEle in listCategories ){
               XmlElement ele = (XmlElement) categoriesEle;
               String name = ele.GetElementsByTagName("Name")[0].InnerText;

               categories[i] = name;
               i++;
            }

            return categories;
        }
    }
}
