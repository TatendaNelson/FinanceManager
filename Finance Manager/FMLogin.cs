using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading.Tasks;

namespace Finance_Manager
{
    //It manages the creation of user accounts
    class FMLogin
    {
        public FMLogin()
        {
            FMDatabase db = new FMDatabase();
            string databaseFile = db.xml_file;


            string xmlData = File.ReadAllText(databaseFile);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData);

            XmlNodeList users = doc.GetElementsByTagName("Users");

            if (users.Count <= 0)
            {
                XmlElement usersElement = doc.CreateElement("Users");
                doc.ChildNodes[0].AppendChild(usersElement);
            }

            doc.Save(databaseFile);
        }

        //Create Account
        public string createAccount(String firstName, String surname, String username, String password)
        {

            FMDatabase db = new FMDatabase();

            //Check if user submitted first name
            if (firstName.Length <= 0)
            {
                return "First name is required.";
            }

            //Check if user submitted surname
            if (surname.Length <= 0)
            {
                return "Surname is required.";
            }

            //Check if  user submitted username with at least 3 characters
            if (username.Length < 3)
            {
                return "A username with 3 characters is required.";
            }

            //Check if  user submitted password with at least 8 characters
            if (password.Length < 8)
            {
                return "A password with 8 characters is required.";
            }

            //Check if user account already exists
            if (this.accountExists(username))
            {
                return "This acccount exists choose another username.";
            }

            XmlDocument doc = db.read();

            XmlElement firstNameEle = doc.CreateElement("FirstName");
            firstNameEle.InnerText = firstName;

            XmlElement surnameEle = doc.CreateElement("Surname");
            surnameEle.InnerText = surname;

            XmlElement usernameEle = doc.CreateElement("Username");
            usernameEle.InnerText = username;

            XmlElement passwordEle = doc.CreateElement("Password");
            passwordEle.InnerText = password;

            //Main user element
            XmlElement userEle = doc.CreateElement("User");
            userEle.SetAttribute("username", username);
            userEle.SetAttribute("password", password);
            userEle.SetAttribute("notify", "false");
            userEle.SetAttribute("currency", "USD");

            userEle.AppendChild(firstNameEle);
            userEle.AppendChild(surnameEle);
            userEle.AppendChild(usernameEle);
            userEle.AppendChild(passwordEle);


            //Getting document elements
            int counter = 0;

            for (counter = 0; counter < doc.ChildNodes[0].ChildNodes.Count; counter++)
            {
                XmlNode ele = doc.ChildNodes[0].ChildNodes[counter];
                if (ele.Name == "Users")
                {
                    doc.ChildNodes[0].ChildNodes[counter].AppendChild(userEle);
                }
            }

            doc.Save(db.xml_file);

            return "true";
        }

        //Update password
        public string updatePassword(String oldPassword, String newPassword )
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();
            String username = this.checkLogin();

            //Check if  user submitted password with at least 8 characters
            if (oldPassword.Length < 8)
            {
                return "Old password with 8 characters is required.";
            }


            //Check if  user submitted password with at least 8 characters
            if (newPassword.Length < 8)
            {
                return "A new password with 8 characters is required.";
            }



            //Check if user account already exists
            if (!this.accountExists(username))
            {
                return "Account does not exist.";
            }

            //Check password
            XmlNodeList users = doc.GetElementsByTagName("User");


            int counter = 0;
            for (counter = 0; counter < users.Count; counter++)
            {
                XmlElement user = (XmlElement)users[counter];

                if (user.GetAttribute("username") == username)
                {

                    if (user.GetAttribute("password") == oldPassword)
                    {
                        user.SetAttribute("password", newPassword);
                        user.GetElementsByTagName("Password")[0].InnerText = newPassword;

                        doc.Save( db.xml_file );
                        return "true";
                    }
                    else
                    {
                        return "Old password not correct";
                    }
                }
            }

            return "Sorry login failed, try again.";
        }

        //Checking if account exists
        private bool accountExists(String username)
        {
            bool accountExists = false;

            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            XmlNodeList users = doc.GetElementsByTagName("Username");

            int count = 0;
            for (count = 0; users.Count > count; count++)
            {
                if (users[count].InnerText == username)
                {
                    accountExists = true;
                }
            }

            return accountExists;
        }

        //Signing in
        public string signIn(String username, String password)
        {
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            //Check if  user submitted username with at least 3 characters
            if (username.Length < 3)
            {
                return "A username with 3 characters is required.";
            }

            //Check if  user submitted password with at least 8 characters
            if (password.Length < 8)
            {
                return "A password with 8 characters is required.";
            }

            //Check if user account already exists
            if (!this.accountExists(username))
            {
                return "Wrong username.";
            }

            //Check password
            XmlNodeList users = doc.GetElementsByTagName("User");


            int counter = 0;
            for (counter = 0; counter < users.Count; counter++)
            {
                XmlElement user = (XmlElement)users[counter];

                if (user.GetAttribute("username") == username)
                {

                    if (user.GetAttribute("password") == password)
                    {
                        this.createLoginFile(username);
                        return "true";
                    }
                    else
                    {
                        return "Wrong password";
                    }
                }
            }

            return "Sorry login failed, try again.";
        }

        //Creating login file and record
        private void createLoginFile(String username)
        {
            File.WriteAllText("login.txt", username);
        }

        //Updating currency
        public void updateCurrency( String currency )
        {
            FMDatabase database = new FMDatabase();
            XmlDocument doc = database.read();

            XmlElement user = (XmlElement) doc.SelectSingleNode("//User[@username='" + this.checkLogin() + "']");
            user.SetAttribute("currency", currency);
            doc.Save( database.xml_file );
        }

        //Updating Notifications
        public void updateNotifications(String notify)
        {
            FMDatabase database = new FMDatabase();
            XmlDocument doc = database.read();

            XmlElement user = (XmlElement)doc.SelectSingleNode("//User[@username='" + this.checkLogin() + "']");
            user.SetAttribute("notify", notify);
            doc.Save(database.xml_file);
        }

        //Check login
        public string checkLogin()
        {
            if (File.Exists("login.txt"))
            {
                return File.ReadAllText("login.txt");
            }

            return "false";
        }

        //Logout method
        public void logout()
        {
            File.Delete("login.txt");
        }

        //Delete account
        public void deleteAccount(String username)
        {

            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            XmlNode user = doc.SelectSingleNode("//User[@username='" + username + "']");
            user.ParentNode.RemoveChild(user);

            doc.Save(db.xml_file);
            this.logout();

        }

        //Read user account
        public XmlNode readUser()
        {
            String username = this.checkLogin();
            FMDatabase db = new FMDatabase();
            XmlDocument doc = db.read();

            XmlNode user = doc.SelectSingleNode("//User[@username='" + username + "']");

            return user;

        }

        //Read currents
        public string getCurrency() {
            XmlElement user = (XmlElement)this.readUser();
            return user.GetAttribute("currency");
        }

        //Checks if theuser wants to get notifications
        public bool getNotify()
        {
            XmlElement user = (XmlElement)this.readUser();
            return ( user.GetAttribute("notify") == "true");
        }
    }
}
