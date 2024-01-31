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
    public partial class Income : Form
    {
        //Currency chosen by user
        private String currency;

        public Income()
        {
            InitializeComponent();

            FMCategories categories = new FMCategories();
            FMLogin login = new FMLogin();

            this.currency = login.getCurrency();
            this.menuUsername.Text = login.checkLogin();
            this.printIncome();
        }

        //Adding the income
        private void button2_Click(object sender, EventArgs e)
        {
            String name = this.textBox1.Text;
            String amount = this.textBox2.Text;
            String notes = this.textBox3.Text;
            String category = this.comboBox1.SelectedItem.ToString();

            FMIncome income = new FMIncome();
            income.addIncome( name, Convert.ToDouble( amount), notes, category  );

            this.printIncome();
        }

        //Displaying income information
        private void printIncome()
        {
            FMCategories categories = new FMCategories();
            FMIncome income = new FMIncome();

            this.dataGridView1.Rows.Clear();

            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
         
            this.comboBox1.Items.Clear();
            this.comboBox1.Items.AddRange( categories.readAsArray() );
            this.comboBox1.SelectedIndex = 0;

            double total = 0.00;

            XmlNodeList incomeList = income.readIncome().ChildNodes;

            foreach( XmlNode incomeNode in incomeList){

                XmlElement incomeElement = (XmlElement)incomeNode;

                String name = incomeElement.GetElementsByTagName("Source")[0].InnerText;
                String category = incomeElement.GetElementsByTagName("Category")[0].InnerText;
                String notes = incomeElement.GetElementsByTagName("Notes")[0].InnerText;
                String amount = incomeElement.GetElementsByTagName("Amount")[0].InnerText;
                String id = incomeElement.GetAttribute("id");

                total += Convert.ToDouble( amount );

                int rowID = this.dataGridView1.Rows.Add();
                DataGridViewRow row = this.dataGridView1.Rows[ rowID];
                Button deleteButton = new Button();
                
                deleteButton.Location = new System.Drawing.Point(0, 0);
                deleteButton.Name = id;
                deleteButton.Size = new System.Drawing.Size(75, 29);
                deleteButton.TabIndex = 1;
                deleteButton.Text = "Delete";
                deleteButton.UseVisualStyleBackColor = true;
                

                this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.deleteIncome);

                row.Cells[0].Value = id;
                row.Cells[1].Value = name;
                row.Cells[2].Value = category;
                row.Cells[3].Value = notes;
                row.Cells[4].Value = this.currency + " " + amount;
                row.Cells[5].Value = "Delete";
            }

            this.label10.Text = this.currency + " " +  total.ToString();

        }

        //Delete income
        private void deleteIncome(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewCell id = this.dataGridView1.Rows[e.RowIndex].Cells[0];
            if (e.ColumnIndex == 5)
            {
                FMIncome income = new FMIncome();

                try
                {
                    income.removeIncome(id.Value.ToString());
                    this.printIncome();
                }
                catch (Exception err)
                {
                }
            }
        }

        //Menu items click
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

        private void menuCategories_Click(object sender, EventArgs e)
        {
            CategoriesUi categories = new CategoriesUi();
            this.Hide();
            categories.Show();
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

    }
}
