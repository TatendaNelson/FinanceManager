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
    public partial class BudgetInterface : Form
    {
        //Currency selected by user
        private String currency;

        public BudgetInterface()
        {
            InitializeComponent();
           
            FMLogin login = new FMLogin();
            this.menuUsername.Text = login.checkLogin();
            this.currency = login.getCurrency();

            this.printBudget();

        }

        //Button click to add budget
        private void button2_Click(object sender, EventArgs e)
        {
            String name = this.textBox1.Text;
            String price = this.textBox2.Text;
            String notes = this.textBox3.Text;
            String category = this.comboBox1.SelectedItem.ToString();
            String quantity = this.textBox4.Text;

            FMBudget budget = new FMBudget();
            budget.addBudget( name, Convert.ToDouble(price), Convert.ToInt32(quantity), category, notes);

            this.printBudget();
        }

        //Displaying the budget on the interface
        private void printBudget()
        {

            FMBudget budget = new FMBudget();
            FMCategories categories = new FMCategories();

            this.comboBox1.Items.Clear();
            this.comboBox1.Items.AddRange( categories.readAsArray() );
            this.comboBox1.SelectedIndex = 0;

            XmlNodeList budgetList = budget.readBudget().ChildNodes;

            this.dataGridView1.Rows.Clear();
            double total = 0;

            foreach( XmlNode budgetNode in budgetList){
                XmlElement budgetElement = (XmlElement)budgetNode;

                String name = budgetElement.GetElementsByTagName("Name")[0].InnerText;
                String category = budgetElement.GetElementsByTagName("Category")[0].InnerText;
                String notes = budgetElement.GetElementsByTagName("Notes")[0].InnerText;
                String price = budgetElement.GetElementsByTagName("Price")[0].InnerText;
                String quantity = budgetElement.GetElementsByTagName("Quantity")[0].InnerText;

                int rowID = this.dataGridView1.Rows.Add();
                DataGridViewRow row = this.dataGridView1.Rows[rowID];
                this.dataGridView1.CellClick += new DataGridViewCellEventHandler( this.deleteItem);

                row.Cells[0].Value = budgetElement.GetAttribute("id");
                row.Cells[1].Value = name;
                row.Cells[2].Value = category;
                row.Cells[3].Value = notes;
                row.Cells[4].Value = this.currency + " " + price;
                row.Cells[5].Value = quantity;
                row.Cells[6].Value = "Delete";

                total += Convert.ToInt32(quantity) * Convert.ToDouble(price);
            }

            this.label10.Text = this.currency + " " + total.ToString();
        }

        //Button click to delete budget item
        private void deleteItem(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewCell id = this.dataGridView1.Rows[e.RowIndex].Cells[0];
            if (e.ColumnIndex == 6)
            {
                FMBudget budget = new FMBudget();

                try
                {
                    budget.deleteBudget(id.Value.ToString());
                    this.printBudget();
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

        private void menuCategories_Click(object sender, EventArgs e)
        {
            CategoriesUi categories = new CategoriesUi();
            this.Hide();

            categories.Show();
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
