using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Two_Step_File_Protector
{
    public partial class PasswordBox : Form
    {
        public bool pwordok = false;
        public string password;

        public PasswordBox()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Length < 6)
            {
                label3.Text = "password must be greater than 5 characters";
                return;
            }


            if (textBox1.Text.Trim() == textBox2.Text.Trim())
            {
                pwordok = true;
                password = textBox1.Text.Trim();
                this.Hide();
            }

            else
            {
                pwordok = false;
                label3.Text = "passwords don't match";
            }   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
