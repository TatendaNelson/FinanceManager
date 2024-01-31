using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance_Manager
{
    class FMNotifications
    {
        //Checks if the user over spent
        public bool checkOveruse()
        {
            bool overuse = false;

            FMIncome income = new FMIncome();
            FMTransactions expenses = new FMTransactions();

            overuse = (income.getTotal() < expenses.getTotal());
            return overuse;
        }

        //Checks if the user has spent more than the budget
        public bool checkOverBudget()
        {
            bool overuse = false;
            FMBudget budget = new FMBudget();
            FMTransactions expenses = new FMTransactions();

            overuse = (budget.getTotal() < expenses.getTotal());
            return overuse;
        }

        //Check if a bill is overdue
        public int checkBillOverdue()
        {
            FMBills bills = new FMBills();
            return bills.getOverDue();
        }

        //Check the amount spent on bills and whats left to be paid
        public double[] checkBillsPaid()
        {
            FMBills bills = new FMBills();
            double[] billsPaid = { bills.getPaidTotal(), bills.getTotal() - bills.getPaidTotal()};

            return billsPaid;
        }

        //Creates the list of notifications
        public String[] notificationsArray()
        {
            FMLogin login = new FMLogin();

            String currency = login.getCurrency();
            String[] notifications = new string[4];

            if (this.checkOveruse())
            {
                notifications[0] = "You have over used your money.";
            }
            else
            {
                notifications[0] = "You have not yet used all your money.";
            }

            if (this.checkOverBudget())
            {
                notifications[1] = "You have over used more than your budget.";
            }
            else
            {
                notifications[1] = "You are still under your budget, keep it up!";
            }


            if (this.checkBillOverdue() > 0 )
            {
                notifications[2] = "You have ."  + this.checkBillOverdue() + " bills overdue please pay them.";
            }
            else
            {
                notifications[2] = "You have no bills over due!";
            }

            if ( 0 < this.checkBillsPaid()[1] )
            {
                notifications[3] = "You have " + currency + " " + this.checkBillsPaid()[1] + " not yet paid.";
            }
            else
            {
                notifications[3] = "You have paid " + currency + " " + this.checkBillsPaid()[0] + " and cleared your bills. Well done!";
            }

            return notifications;
        }

        //Gets the texts for the notification
        public String notificationsString()
        {
            String str = "";
            String[] arr = this.notificationsArray();

            foreach( String notice in arr ){
                str += "-" + notice + "\n\n";
            }

            return str;
        }
    }
}
