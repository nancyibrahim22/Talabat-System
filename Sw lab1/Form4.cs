using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sw_lab1
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 adminForm = new Form1();
            adminForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serrings customerForm = new serrings();
            customerForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form5 report1form = new Form5();
            report1form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form6 report2form = new Form6();
            report2form.ShowDialog();
        }
    }
}
