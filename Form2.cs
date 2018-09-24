using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PProyecto2
{

    public partial class Form2 : Form
    {
        public static List<string> passwords = new List<string>();

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textPassword.Text==textCPassword.Text)
            {
                foreach (string item in passwords)
                {
                    if (item==textPassword.Text)
                    {
                        MessageBox.Show("Password has already been used");
                        return;
                    }
                }
                if (passwords.Count>=2)
                {
                    passwords.RemoveAt(0);
                }
                passwords.Add(textPassword.Text);
            }

            this.Close();
        }
    }
}
