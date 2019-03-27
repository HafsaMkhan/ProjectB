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
    public partial class Assesment : Form
    {
        SqlConnection connection = new SqlConnection(Utility.conStr);
        public Assesment()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if(!Char.IsDigit(number)&& number!=8 && number != 46)
            {
                e.Handled = true;
            }
        }

        private void txtWeightage_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8 && number != 46)
            {
                e.Handled = true;
            }
        }

        private void Assesment_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'projectBDataSet.Assessment' table. You can move, or remove it, as needed.
            this.assessmentTableAdapter.Fill(this.projectBDataSet.Assessment);
            
        }
        private void modalClear()
        {
            txtTitle.Text = "";
            txtTotalMarks.Text = "";
            txtWeightage.Text = ""; 
        }
        int Assessment_Id;
        private void btnAddAssesment_Click(object sender, EventArgs e)
        {
            if(btnAddAssesment.Text== "Update Assessment")
            {
                string updateQuery = "Update Assessment set Title='" + txtTitle.Text.ToString() + "'," +
                    "TotalMarks='"+Convert.ToInt32(txtTotalMarks.Text)+"'," +
                    "TotalWeightage='"+ Convert.ToInt32(txtWeightage.Text) + "' Where Id='"+Convert.ToInt32(Assessment_Id)+"'";
                connection.Open();
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Data updated successfully!");
                modalClear();
                btnAddAssesment.Text = "Add Assessment";
                this.assessmentTableAdapter.Fill(this.projectBDataSet.Assessment);
            }
            else
            {
                
                string insertQuery = "Insert into Assessment(Title,DateCreated,TotalMarks,TotalWeightage) " +
                    "Values('" + txtTitle.Text.ToString() + "','" + Convert.ToDateTime(DateTime.Now) + "'," +
                    "'"+Convert.ToInt32(txtTotalMarks.Text)+ "','" + Convert.ToInt32(txtWeightage.Text) + "')";
                connection.Open();
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Assessment added successfully!");
                modalClear();
                this.assessmentTableAdapter.Fill(this.projectBDataSet.Assessment);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Delete")
            {
                int rb_Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                if (MessageBox.Show("Do you want to delete " + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Assessment WHERE Id  = '" + rb_Id + "'", connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    this.assessmentTableAdapter.Fill(this.projectBDataSet.Assessment);
                }
            }
            else if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Edit")
            {
                btnAddAssesment.Text = "Update Assessment";
                Assessment_Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                connection.Open();
                string query = "Select * from Assessment WHERE Id=" + Assessment_Id;
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader read = command.ExecuteReader();
                while (read.Read())
                {
                    txtTitle.Text = read["Title"].ToString();
                    txtTotalMarks.Text = read["TotalMarks"].ToString();
                    txtWeightage.Text = read["TotalWeightage"].ToString();
                }
                connection.Close();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentView studentView = new StudentView();
            studentView.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            AssessmentComponent assesment = new AssessmentComponent();
            assesment.Show();
        }
    }
}


