using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace Sw_lab1
{
    public partial class serrings : Form
    {
        string constr = "Data source = orcl; User Id = hr; Password = hr;";
        OracleConnection conn;
        OracleCommandBuilder builder;
        OracleDataAdapter adapter;
        DataSet ds = new DataSet();

        public serrings()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            string cmdstr;
            cmdstr = @"SELECT * FROM CUSTOMER WHERE CUSTOMER_NAME=: hh1 AND CUSTOMER_PASSWORD=: hh2";
            adapter = new OracleDataAdapter(cmdstr, constr);
            adapter.SelectCommand.Parameters.Add("hh1", textBox1.Text);
            adapter.SelectCommand.Parameters.Add("hh2", textBox2.Text);
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            builder = new OracleCommandBuilder(adapter);
            adapter.Update(ds.Tables[0]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 f = new Form4();
            f.ShowDialog();
        }
    }
}
