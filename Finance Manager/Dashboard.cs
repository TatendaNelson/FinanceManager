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
    public partial class Dashboard : Form
    {
        //Currency set by user
        private String currency;

        public Dashboard()
        {
            InitializeComponent();

            FMLogin login = new FMLogin();
            this.currency = login.getCurrency();
            this.printHome();
        }

        //Displaying the main info 
        private void printHome()
        {
            FMTransactions trans = new FMTransactions();
            FMIncome incomeObj = new FMIncome();


            this.label4.Text = trans.getTotal().ToString() ;
            this.label7.Text = incomeObj.getTotal().ToString();

            this.label5.Text = this.currency;
            this.label8.Text = this.currency;

            this.printBalance();
            this.printExpenses();
            this.printCategories();
        }

        //Displaying expenses
        private void printExpenses()
        {
            FMTransactions transactions = new FMTransactions();
            XmlNodeList transactionsList = transactions.readTransactions().ChildNodes;

            FMBills bills = new FMBills();

            this.comboBox2.Items.Clear();
            this.comboBox2.Items.AddRange( bills.readBillsLink() );
            this.comboBox2.SelectedIndex = 0;

            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";

            this.dataGridView1.Rows.Clear();

            foreach( XmlNode transactionNode in transactionsList){
                XmlElement transElement = (XmlElement)transactionNode;

                String name = transElement.GetElementsByTagName("Name")[0].InnerText;
                String amount = transElement.GetElementsByTagName("Amount")[0].InnerText;
                String notes = transElement.GetElementsByTagName("Notes")[0].InnerText;
                String category = transElement.GetElementsByTagName("Category")[0].InnerText;
                String date = transElement.GetElementsByTagName("Date")[0].InnerText;
                String link = transElement.GetElementsByTagName("Link")[0].InnerText;

                int rowID = this.dataGridView1.Rows.Add();
                DataGridViewRow row = this.dataGridView1.Rows[rowID];
                this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.deleteExpense );

                row.Cells[0].Value = transElement.GetAttribute("id");
                row.Cells[1].Value = date;
                row.Cells[2].Value = name;
                row.Cells[3].Value = category;
                row.Cells[4].Value = notes;
                row.Cells[5].Value = link;
                row.Cells[6].Value = this.currency + " " + amount;
                row.Cells[7].Value = "Delete";
            }

            this.label22.Text = this.currency;
        }
        
        //Displaying balance
        private void printBalance()
        {
            FMLogin login = new FMLogin();
            FMIncome income = new FMIncome();
            FMTransactions transactions = new FMTransactions();

            double totalIncome = income.getTotal();
            double totalExpenses = transactions.getTotal();
            double balance = totalIncome - totalExpenses;

            this.label12.Text = balance.ToString();
            this.label11.Text = login.getCurrency();

            this.menuUsername.Text = login.checkLogin();

        }

        //Displaying the categories
        private void printCategories()
        {
            FMCategories categories = new FMCategories();
            FMTransactions transactions = new FMTransactions();

            XmlNodeList transList = transactions.readTransactions().ChildNodes;

            String[] categoriesList = categories.readAsArray();
            int yLocation = 3;

            this.flowLayoutPanel1.Controls.Clear();

            this.comboBox1.Items.Clear();
            this.comboBox1.Items.AddRange( categoriesList );
            this.comboBox1.SelectedIndex = 0;

            foreach( String category in categoriesList ){

                double total = 0;
                foreach( XmlNode transactionNode in transList ){

                    XmlElement transEle = (XmlElement) transactionNode;

                    String categoryName = transEle.GetElementsByTagName("Category")[0].InnerText;
                    String amount = transEle.GetElementsByTagName("Amount")[0].InnerText;

                    total += (categoryName == category) ? Convert.ToDouble( amount ) : 0;
                }

                Panel parentPanel = new Panel();
                Label nameLabel = new Label();
                Label totalLabel = new Label();

                // 
                // Parent Panel
                // 
                parentPanel.Controls.Add(nameLabel);
                parentPanel.Controls.Add(totalLabel);
                parentPanel.Location = new System.Drawing.Point(3, yLocation);
                parentPanel.Name = "panel9";
                parentPanel.Size = new System.Drawing.Size(292, 63);
                parentPanel.TabIndex = 0;
                // 
                // Name
                // 
                nameLabel.AutoSize = true;
                nameLabel.Location = new System.Drawing.Point(6, 10);
                nameLabel.Name = "label21";
                nameLabel.Size = new System.Drawing.Size(141, 17);
                nameLabel.TabIndex = 0;
                nameLabel.Text = category;

                // 
                // Price
                // 
                totalLabel.AutoSize = true;
                totalLabel.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                totalLabel.Location = new System.Drawing.Point(6, 37);
                totalLabel.Name = "label3";
                totalLabel.Size = new System.Drawing.Size(80, 20);
                totalLabel.TabIndex = 1;
                totalLabel.Text = this.currency + " " + total.ToString();
                this.flowLayoutPanel1.Controls.Add( parentPanel);

                yLocation += 69;
            }
        }

        //Adding expenses button click
        private void button2_Click(object sender, EventArgs e)
        {
            String name = this.textBox1.Text;
            String amount = this.textBox2.Text;
            String notes = this.textBox3.Text;
            String category = this.comboBox1.SelectedItem.ToString();
            String date = this.dateTimePicker1.Value.ToString();
            String link = this.comboBox2.SelectedItem.ToString();

            FMTransactions transactions = new FMTransactions();
            transactions.addTransaction(name, Convert.ToDouble(amount), category, "bank", link, notes, date);
            this.printHome();
        }

        //Menu clicks
        private void menuCategories_Click(object sender, EventArgs e)
        {
            CategoriesUi categories = new CategoriesUi();
            this.Hide();
        }

        private void menuCategories_Click_1(object sender, EventArgs e)
        {
            CategoriesUi categories = new CategoriesUi();
            this.Hide();
            categories.Show();
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

        private void menuUsername_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            this.Hide();
            settings.Show();
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

        private void deleteExpense( object sender, DataGridViewCellEventArgs e)
        {
           
            DataGridViewCell id = this.dataGridView1.Rows[e.RowIndex].Cells[0];
            if( e.ColumnIndex == 7 ){
                FMTransactions transactions = new FMTransactions();

                try
                {
                    transactions.deleteTransaction(id.Value.ToString());
                    this.printHome();
                } catch( Exception err ){
                }
            }
        }
    }
}
