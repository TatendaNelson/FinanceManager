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
    public partial class Notifications : Form
    {

        public Notifications()
        {
            InitializeComponent();
            FMLogin login = new FMLogin();
            FMNotifications notifications = new FMNotifications();

            if (login.getNotify())
            {
                this.label10.Text = notifications.notificationsString();
            }
            else
            {
                this.label10.Text = "Notifications disabled, please go to the settings and enable set goals";
            }
            this.menuUsername.Text = login.checkLogin();

        }

        //Menu clicks
        private void label2_Click(object sender, EventArgs e)
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

        private void menuSettings_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            this.Hide();

            settings.Show();
        }

        private void menuLogout_Click(object sender, EventArgs e)
        {
            FMLogin login = new FMLogin();
            UserInterface user = new UserInterface();

            login.logout();
            this.Hide();

            user.Show();
        }
    }
}
