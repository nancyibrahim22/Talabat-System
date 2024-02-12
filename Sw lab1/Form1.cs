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
using System.Globalization;


namespace Sw_lab1
{

    public partial class Form1 : Form
    {
        string ordb = "Data source = orcl; User Id = hr; Password = hr;";
        OracleConnection conn;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //res id
            conn = new OracleConnection(ordb);
            conn.Open();

            OracleCommand cmd3 = new OracleCommand();
            cmd3.Connection = conn;
            cmd3.CommandText = "select res_id from restaurant";
            cmd3.CommandType = CommandType.Text;

            OracleDataReader dr3 = cmd3.ExecuteReader();
            while (dr3.Read())
            {
                comboBox1.Items.Add(dr3[0]);
            }
            dr3.Close();

            //promocodes ID
            cmd3.CommandText = "select pc_id from promo_codes";
            OracleDataReader dr4 = cmd3.ExecuteReader();
            while (dr4.Read())
            {
                comboBox2.Items.Add(dr4[0]);
            }
            dr4.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Dispose();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            OracleCommand cmd2 = new OracleCommand();
            cmd2.Connection = conn;
            cmd2.CommandText = "select res_name, res_phone from restaurant where res_id = :id ";
            cmd2.CommandType = CommandType.Text;
            cmd2.Parameters.Add("id", comboBox1.SelectedItem.ToString());
            OracleDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                textBox2.Text = dr2[0].ToString();
                textBox3.Text = dr2[1].ToString();
            }
            dr2.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select res_name from restaurant where res_id = :res_id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("res_id", textBox2.Text);

        }

        private void insert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Please enter values for all required fields");
                return;
            }

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "insert into restaurant values (:id, :name, :phoneNo)";
            cmd.Parameters.Add("id", comboBox1.Text);
            cmd.Parameters.Add("name", textBox2.Text);
            cmd.Parameters.Add("phoneNo", textBox3.Text);

            try
            {
                int r = cmd.ExecuteNonQuery();
                if (r != -1)
                {
                    comboBox1.Items.Add(comboBox1.Text);
                    MessageBox.Show("New restaurant has been added successfuly!");
                }
            }
            //catch (OracleException excep)
            catch
            {
                //if (excep.ErrorCode == 1)
                //{
                //MessageBox.Show("A restaurant with this ID already exists!");
                //   MessageBox.Show("An error occurred while adding the restaurant: " + excep.Message);
                //}
                //else
                //{
                //MessageBox.Show("An error occurred while adding the restaurant: " + excep.Message);
                MessageBox.Show("A restaurant with this ID already exists!");
                //}
            }
        }

        private void edit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Please enter values for all required fields.");
                return;
            }

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "update restaurant set res_name=:name, res_phone=:phone where  res_id=:id";

            cmd.Parameters.Add("name", textBox2.Text);
            cmd.Parameters.Add("phoneNo", textBox3.Text);
            cmd.Parameters.Add("id", comboBox1.Text);



            int r = cmd.ExecuteNonQuery();
            if (r != -1)
            {

                comboBox1.Items.Add(comboBox1.Text);
                //string newItem = comboBox1.Text;
                //if (!comboBox1.Items.Contains(newItem))
                //{
                //    comboBox1.Items.Add(newItem);
                //}
                // Remove the old item from the combo box items collection
                string oldItem = comboBox1.Text;
                comboBox1.Items.Remove(oldItem);
                // comboBox1.Text = newItem;
                MessageBox.Show("Restaurant has been modified!");
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Please select a restaurant ID to delete.");
                return;
            }

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "delete from restaurant where  res_id=:id";
            cmd.Parameters.Add("id", comboBox1.SelectedItem.ToString());

            try
            {
                int r = cmd.ExecuteNonQuery();
                if (r != -1)
                {
                    comboBox1.Items.Remove(comboBox1.Text);
                    MessageBox.Show("Restauarnt has been deleted!");
                    textBox2.Clear();
                    textBox3.Clear();
                }
            }
            //catch (OracleException excep)
            catch
            {
                //if(excep.ErrorCode == -2292)
                //{
                MessageBox.Show("This restaurant cannot be deleted because it has associated meals");
                //}
                //else
                //{
                //MessageBox.Show("An error occurred while deleting the restaurant: " + excep.Message);
                //}

            }
        }

        private void showPC_Click(object sender, EventArgs e)
        {

            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Please select a Promocode ID!");
                return;
            }

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "getNames";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("pcID", comboBox2.SelectedItem.ToString());
            cmd.Parameters.Add("Cnames", OracleDbType.RefCursor, ParameterDirection.Output);

            OracleDataReader dr = cmd.ExecuteReader();

            // Add columns to the DataGridView control
            dataGridView1.Columns.Clear();

            for (int i = 0; i < dr.FieldCount; i++)
            {
                dataGridView1.Columns.Add(dr.GetName(i), dr.GetName(i));
            }


            while (dr.Read())
            {
                //Add each row to a string array
                string[] rowValues = new string[dr.FieldCount];
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    rowValues[i] = dr[i].ToString();
                }

                // Add the string array to a DataGridView control
                dataGridView1.Rows.Add(rowValues);


            }
            dr.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(comboBox2.Text) || string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox7.Text))
            {
                MessageBox.Show("Please enter values for all required fields");
                return;
            }

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "insert into promo_codes values (:id, :name, :discVal, :activeDate, :days)";
            cmd.Parameters.Add("id", comboBox2.Text);
            cmd.Parameters.Add("name", textBox1.Text);
            cmd.Parameters.Add("discVal", textBox4.Text);
            cmd.Parameters.Add("activeDate", textBox5.Text);
            cmd.Parameters.Add("days", textBox7.Text);

            try
            {
                int r = cmd.ExecuteNonQuery();
                if (r != -1)
                {
                    comboBox2.Items.Add(comboBox2.Text);
                    MessageBox.Show("New restaurant has been added successfuly!");
                }
            }
            catch
            {
                //if (excep.ErrorCode == 1)
                //{
                MessageBox.Show("A promocode with this ID already exists!");
                //}
                //else
                //{
                //    MessageBox.Show("An error occurred while adding the restaurant: " + excep.Message);
                //}
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(comboBox2.Text) || string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox7.Text) || string.IsNullOrEmpty(textBox6.Text))
            //{
            //    MessageBox.Show("Please enter values for all required fields.");
            //    return;
            //}

            //OracleCommand cmd = new OracleCommand();
            //cmd.Connection = conn;
            //cmd.CommandText = "update promo_codes set pc_name=:name, discount_value=:discVal, pc_activedate=:activDate, exp_date=:expDate  where  pc_id=:id";

            //cmd.Parameters.Add("name", textBox1.Text);
            //cmd.Parameters.Add("discVal", textBox4.Text);
            //cmd.Parameters.Add("activDate", textBox5.Text);
            //cmd.Parameters.Add("expDate", textBox7.Text);
            ////cmd.Parameters.Add("phoneNo", textBox6.Text);
            //cmd.Parameters.Add("id", comboBox2.Text);



            //int r = cmd.ExecuteNonQuery();
            //if (r != -1)
            //{

            //    comboBox2.Items.Add(comboBox2.Text);
            //    //string newItem = comboBox1.Text;
            //    //if (!comboBox1.Items.Contains(newItem))
            //    //{
            //    //    comboBox1.Items.Add(newItem);
            //    //}
            //    // Remove the old item from the combo box items collection
            //    string oldItem = comboBox2.Text;
            //    comboBox2.Items.Remove(oldItem);
            //    // comboBox1.Text = newItem;
            //    MessageBox.Show("Promocode has been modified!");
            //}



            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "update promo_codes set pc_name=:name, discount_value=:discVal, pc_activedate=:activDate, exp_date=:expDate  where  pc_id=:id";

            cmd.Parameters.Add("name", textBox1.Text);
            cmd.Parameters.Add("discVal", textBox4.Text);

            DateTime activDate;
            if (!DateTime.TryParseExact(textBox5.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out activDate))
            {
                MessageBox.Show("Please enter the date in the format dd-MM-yyyy.");
                return;
            }
            OracleParameter activDateParam = new OracleParameter("activDate", OracleDbType.Date);
            activDateParam.Value = activDate;
            cmd.Parameters.Add(activDateParam);

            DateTime expDate;
            if (!DateTime.TryParseExact(textBox7.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out expDate))
            {
                MessageBox.Show("Please enter the date in the format dd-MM-yyyy.");
                return;
            }
            OracleParameter expDateParam = new OracleParameter("expDate", OracleDbType.Date);
            expDateParam.Value = expDate;
            cmd.Parameters.Add(expDateParam);

            cmd.Parameters.Add("id", comboBox2.Text);

            try
            {
                int r = cmd.ExecuteNonQuery();
                if (r != -1)
                {
                    comboBox2.Items.Add(comboBox2.Text);
                    string oldItem = comboBox2.Text;
                    comboBox2.Items.Remove(oldItem);
                    MessageBox.Show("Promo code has been modified!");
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("An error occurred while updating the promo code: " + ex.Message);
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox2.Text))
            {
                MessageBox.Show("Please select a promocode ID to delete.");
                return;
            }

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "delete from promo_codes where  pc_id=:id";
            cmd.Parameters.Add("id", comboBox2.SelectedItem.ToString());

            try
            {
                int r = cmd.ExecuteNonQuery();
                if (r != -1)
                {
                    comboBox1.Items.Remove(comboBox1.Text);
                    MessageBox.Show("promocode has been deleted!");
                    textBox2.Clear();
                    textBox3.Clear();
                }
            }
            //catch (OracleException excep)
            catch
            {
                //if(excep.ErrorCode == -2292)
                //{
                MessageBox.Show("This promocode cannot be deleted because it has associated customers");
                //}
                //else
                //{
                //MessageBox.Show("An error occurred while deleting the restaurant: " + excep.Message);
                //}
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            OracleCommand cmd2 = new OracleCommand();
            cmd2.Connection = conn;

            cmd2.CommandText = "select pc_name, discount_value, pc_activedate, exp_date from promo_codes where pc_id = :id ";
            cmd2.CommandType = CommandType.Text;
            cmd2.Parameters.Add("id", comboBox2.SelectedItem.ToString());


            OracleDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                textBox1.Text = dr2[0].ToString();
                textBox4.Text = dr2[1].ToString();
                textBox5.Text = dr2[2].ToString();
                textBox7.Text = dr2[3].ToString();

            }
            dr2.Close();

            OracleCommand cmd3 = new OracleCommand();
            cmd3.Connection = conn;
            cmd3.CommandText = "GetDays";
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.Parameters.Add("pcID", comboBox2.SelectedItem.ToString());
            cmd3.Parameters.Add("days", OracleDbType.Int32, ParameterDirection.Output);
            cmd3.ExecuteNonQuery();
            int validDays;
            validDays = Convert.ToInt32(cmd3.Parameters["days"].Value.ToString());
            textBox6.Text = validDays.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select pc_name from promo_codes where pc_id = :pc_id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("pc_id", textBox1.Text);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select discount_value from promo_codes where pc_id = :pc_id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("pc_id", textBox4.Text);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select pc_activedate from promo_codes where pc_id = :pc_id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("pc_id", textBox5.Text);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select exp_date from promo_codes where pc_id = :pc_id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("pc_id", textBox7.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //this.Hide();
            Hide();
            Form4 f = new Form4();
            f.ShowDialog();
        }
    }
}

