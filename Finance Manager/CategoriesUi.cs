using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Finance_Manager
{
    public partial class CategoriesUi : Form
    {
        public CategoriesUi()
        {
            InitializeComponent();
            this.panel1.Dock = DockStyle.Fill;
            this.printCategories();

            FMLogin login = new FMLogin();
            this.menuUsername.Text = login.checkLogin();
        }

        //Displaying categories
        private void printCategories()
        {
            FMCategories categories = new FMCategories();
            XmlNodeList categoryNodes = categories.readCategories().ChildNodes;

            this.flowLayoutPanel1.Controls.Clear();
            int yLocation = 3;

            foreach( XmlNode categoryNode in categoryNodes ){
                XmlElement categoryElement = (XmlElement)categoryNode;

                String name = categoryElement.GetElementsByTagName("Name")[0].InnerText;
                String id = categoryElement.GetAttribute("id");

                Panel parentPanel = new Panel();
                Label nameLabel = new Label();
                Button deleteButton = new Button();

                // Delete Button
                // 
                deleteButton.Location = new System.Drawing.Point(374, 4);
                deleteButton.Name = id;
                deleteButton.Size = new System.Drawing.Size(75, 29);
                deleteButton.TabIndex = 1;
                deleteButton.Text = "Delete";
                deleteButton.UseVisualStyleBackColor = true;
                deleteButton.Cursor = System.Windows.Forms.Cursors.Hand;
                deleteButton.FlatAppearance.BorderSize = 0;
                deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                deleteButton.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                deleteButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
                deleteButton.Click += new System.EventHandler( this.deleteCategory );

                // 
                // Category Name
                // 
                nameLabel.AutoSize = true;
                nameLabel.Location = new System.Drawing.Point(6, 10);
                nameLabel.Name = "label21";
                nameLabel.Size = new System.Drawing.Size(141, 17);
                nameLabel.TabIndex = 0;
                nameLabel.Text = name;

                // 
                // Parent Panel
                // 
                parentPanel.Controls.Add(deleteButton);
                parentPanel.Controls.Add(nameLabel);
                parentPanel.Location = new System.Drawing.Point(3, yLocation);
                parentPanel.Name = "panel9";
                parentPanel.Size = new System.Drawing.Size(477, 39);
                parentPanel.TabIndex = 0;

                this.flowLayoutPanel1.Controls.Add( parentPanel );
                yLocation += 45;
            }
        }

        //Deleting category button
        private void deleteCategory( object sender, EventArgs e)
        {
            Button btn = sender as Button;
            String id = btn.Name;

            FMCategories categories = new FMCategories();

            categories.deleteCategory(id);
            this.printCategories();
        }

        //Buttton click to add category
        private void button2_Click(object sender, EventArgs e)
        {
            String name = this.textBox1.Text;
            FMCategories categories = new FMCategories();

            categories.addCategory( name );
            this.printCategories();

            this.textBox1.Text = "";
        }

        //Menu clicks
        private void menuUsername_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            this.Hide();
            settings.Show();
        }

        private void menuHome_Click(object sender, EventArgs e)
        {
            Dashboard home = new Dashboard();
            this.Hide();
            home.Show();
        }

        private void menuIncome_Click(object sender, EventArgs e)
        {
            Income income = new Income();
            this.Hide();
            income.Show();
        }

        private void menuBills_Click(object sender, EventArgs e)
        {
            BillsInterface bills = new BillsInterface();
            this.Hide();
            bills.Show();
        }

        private void menuBudget_Click(object sender, EventArgs e)
        {
            BudgetInterface budget = new BudgetInterface();
            this.Hide();
            budget.Show();
        }

        private void menuNotifications_Click(object sender, EventArgs e)
        {
            Notifications notifications = new Notifications();
            this.Hide();
            notifications.Show();
        }

        private void menuSettings_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            this.Hide();
            settings.Show();
        }

        private void menuLogout_Click(object sender, EventArgs e)
        {
            FMLogin login = new FMLogin();
            login.logout();
            UserInterface user = new UserInterface();

            this.Hide();
            user.Show();
        }

    }
}
