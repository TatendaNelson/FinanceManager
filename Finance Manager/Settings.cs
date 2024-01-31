using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Finance_Manager
{
    public partial class Settings : Form
    {

        public Settings()
        {
            InitializeComponent();
            FMLogin login = new FMLogin();

            this.textBox3.Text = login.getCurrency();
            this.menuUsername.Text = login.checkLogin();
          
            bool state = login.getNotify();

            if (state)
            {
                this.button4.Text = "Disable";   
            }
            else
            {
                this.button4.Text = "Enable";
            }
        }

        //Update password button
        private void button2_Click(object sender, EventArgs e)
        {
            String oldpassword = this.textBox1.Text;
            String newPassword = this.textBox4.Text;

            FMLogin login = new FMLogin();
            String status = login.updatePassword( oldpassword, newPassword);

            if (status == "true")
            {
                this.label2.Text = "Password updated succesfully";
            }
            else
            {
                this.label2.Text = status;
            }
        }

        //Update currency button
        private void button3_Click(object sender, EventArgs e)
        {
            FMLogin login = new FMLogin();
            String currency = this.textBox3.Text;

            login.updateCurrency(currency);
        }

        //Set notifications button
        private void button4_Click(object sender, EventArgs e)
        {
            FMLogin login = new FMLogin();
            bool state = login.getNotify();


            Button btn = sender as Button;

            if( state ){
               
                login.updateNotifications("false");
            } else{
              
                login.updateNotifications("true");
            }

            btn.Text = ( "Disable" == btn.Text ) ? "Enable" : "Disable";
        }

        //Menu clicks
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

        private void menuLogout_Click(object sender, EventArgs e)
        {
            FMLogin login = new FMLogin();
            UserInterface user = new UserInterface();

            login.logout();
            this.Hide();
            user.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FMLogin login = new FMLogin();
            UserInterface user = new UserInterface();

            login.logout();
            this.Hide();
            user.Show();
        }
    }
}
