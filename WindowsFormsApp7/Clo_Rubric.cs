using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp7
{
    public partial class Clo_Rubric : Form
    {
        public Clo_Rubric()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(Utility.conStr);

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ProjectB projectB = new ProjectB();
            projectB.Show();
        }

        private void Clo_Rubric_Load(object sender, EventArgs e)
        {
            button1.Text = "Add CLO";
            // TODO: This line of code loads data into the 'projectBDataSet.Clo' table. You can move, or remove it, as needed.
            this.cloTableAdapter.Fill(this.projectBDataSet.Clo);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtCLO.Text == "")
            {
                lblClo.Text = "Clo cannot be empty!";
            }
            else if (Utility.status=="Edit")
            {
                //Edit Rubric
                connection.Open();
                string query = "UPDATE Clo SET Name='"+txtCLO.Text.ToString()+"',DateUpdated='"+DateTime.Now+"' WHERE Id="+CloId;
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                txtCLO.Text = "";
                //updating grid
                this.cloTableAdapter.Fill(this.projectBDataSet.Clo);
                Utility.status = "";
                button1.Text = "Add CLO";
            }
            else
            {
                //Add Rubric
                connection.Open();
                string query = "INSERT INTO Clo(Name,DateCreated,DateUpdated) VALUES('" + txtCLO.Text.ToString() + "','" + DateTime.Now + "','" + DateTime.Now + "')";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                this.cloTableAdapter.Fill(this.projectBDataSet.Clo);
                txtCLO.Text = "";
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        int CloId;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Delete")
            {
                //Get Id of CLO from Grid
                int clo_Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                //Display confirmation msg for delete
                if (MessageBox.Show("Do you want to delete " + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM Clo WHERE Clo.Id = '" + clo_Id + "'", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    this.cloTableAdapter.Fill(this.projectBDataSet.Clo);
                }
            }
            else if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Edit")
            {
                //Mapping CLO data to be edited in text box
                button1.Text = "Edit CLO";
                Utility.status = "Edit";
                CloId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                connection.Open();
                string query = "Select * from Clo WHERE Id="+CloId;
                SqlCommand command = new SqlCommand(query,connection);
                SqlDataReader read = command.ExecuteReader();
                while (read.Read())
                {
                    txtCLO.Text = read["Name"].ToString();
                }
                connection.Close();


            }
        }
    }
}
