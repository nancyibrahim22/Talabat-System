using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.Shared;

namespace Sw_lab1
{
    public partial class Form6 : Form
    {
        CrystalReport2 cr2;
        public Form6()
        {
            InitializeComponent();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void Form6_Load(object sender, EventArgs e)
        {
            cr2 = new CrystalReport2();
            //foreach (ParameterDiscreteValue v in cr2.ParameterFields[0].DefaultValues)
                //comboBox1.Items.Add(v.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cr2.SetParameterValue(0, textBox1.Text);
            //cr2.SetParameterValue(0, comboBox1.Text);
            crystalReportViewer1.ReportSource = cr2;
        }
    }
}
