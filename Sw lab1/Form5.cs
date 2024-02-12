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
    public partial class Form5 : Form
    {
        CrystalReport1 cr1;
        public Form5()
        {
            InitializeComponent();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void Form5_Load(object sender, EventArgs e)
        {
            cr1 = new CrystalReport1();
            foreach (ParameterDiscreteValue v in cr1.ParameterFields[0].DefaultValues)
                comboBox1.Items.Add(v.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cr1.SetParameterValue(0, comboBox1.Text);
            crystalReportViewer1.ReportSource = cr1;
        }
    }
}
