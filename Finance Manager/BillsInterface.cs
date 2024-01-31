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
    public partial class BillsInterface : Form
    {
        //Currency chosen by user
        private String currency;

        public BillsInterface()
        {
            InitializeComponent();
           
            FMLogin login = new FMLogin();

            currency = login.getCurrency();
            this.label2.Text = login.checkLogin();
            this.printBills();
        }

        //Button click to add the bill
        private void button2_Click(object sender, EventArgs e)
        {
            String name = this.textBox1.Text;
            String amount = this.textBox2.Text;
            String date = this.dateTimePicker1.Value.ToShortDateString();

            FMBills bills = new FMBills();
            bills.addBill( name, date, Convert.ToDouble( amount ) );

            this.printBills();
        }

        //Displays the bills on the interface
        private void printBills()
        {
            FMBills bills = new FMBills();
            XmlNodeList billList = bills.readBills().ChildNodes;

            double total = 0.00;
            int overdue = 0;

            this.dataGridView1.Rows.Clear();

            foreach( XmlNode billNode in billList ){
                XmlElement billElement = (XmlElement)billNode;

                String name = billElement.GetElementsByTagName("Name")[0].InnerText;
                String date = billElement.GetElementsByTagName("Date")[0].InnerText;
                String amount = billElement.GetElementsByTagName("Amount")[0].InnerText;
                String id = billElement.GetAttribute("id");


                if ( bills.isOverdue( id ) )
                {
                    overdue++;
                }

                int rowID = this.dataGridView1.Rows.Add();
                DataGridViewRow row = this.dataGridView1.Rows[rowID];


                this.dataGridView1.CellClick += new DataGridViewCellEventHandler( this.deleteBill );
                row.Cells[0].Value = id;
                row.Cells[1].Value = name;
                row.Cells[2].Value = date;
                row.Cells[3].Value = this.currency + " " + amount;
                row.Cells[4].Value = "Delete";

                total += Convert.ToDouble( amount );
                
            }

            this.textBox1.Text = "";
            this.textBox2.Text = "";

            this.label18.Text = this.currency + " " + total.ToString();
            this.label20.Text = overdue.ToString();

        }

        //Deletes the bill from the database
        private void deleteBill(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewCell id = this.dataGridView1.Rows[e.RowIndex].Cells[0];
            if (e.ColumnIndex == 4)
            {
                FMBills bills = new FMBills();

                try
                {
                    bills.deleteBill(id.Value.ToString());
                    this.printBills();
                }
                catch (Exception err)
                {
                }
            }
        }

        //Menu buttons click events
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

        private void label2_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            this.Hide();

            settings.Show();
        }

        private void menuBillls_Click(object sender, EventArgs e)
        {
            BillsInterface bills = new BillsInterface();
            this.Hide();

            bills.Show();
        }
    }
}
