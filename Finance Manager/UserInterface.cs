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
    public partial class UserInterface : Form
    {
        public UserInterface()
        {
            InitializeComponent();

            FMLogin login = new FMLogin();

            String status = login.checkLogin();

            if( status != "false" ){
                Dashboard home = new Dashboard();

                this.Hide();
                home.Show();
            }
        }

        //Signup button click
        private void button2_Click(object sender, EventArgs e)
        {
            FMLogin login = new FMLogin();

            String firstName = this.textBox4.Text;
            String surname =  this.textBox5.Text;
            String password = this.textBox6.Text;
            String username = this.textBox3.Text;

            this.label7.Text = "";

            String result = login.createAccount(firstName,surname,username, password);

            if (result == "true")
            {
                this.textBox6.Text = "";
                this.label7.Text = "Registration succesful, login.";
            }
            else
            {
                this.label7.Text = result;
            }

        }

        //Login button click
        private void button1_Click(object sender, EventArgs e)
        {
            FMLogin login = new FMLogin();
             this.label6.Text = "";

            String username = this.textBox1.Text;
            String password = this.textBox2.Text;

            String result = login.signIn( username, password );

            if( result == "true" ){
                Dashboard home = new Dashboard();
                this.label6.Text = "Login succesful";

                this.Hide();
                home.Show();
            } else{
                this.label6.Text = result;
            }
        }
    }
}
