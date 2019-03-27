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
    public partial class AssessmentComponent : Form
    {
        SqlConnection connection = new SqlConnection(Utility.conStr);
        public AssessmentComponent()
        {
            InitializeComponent();
        }
        int Assessment_Id;
        private void modalClear()
        {
            txtTitle.Text = "";
            txtTotalMarks.Text = "";
        }
        private void AssessmentComponent_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'projectBDataSet.AssessmentComponent' table. You can move, or remove it, as needed.
            this.assessmentComponentTableAdapter.Fill(this.projectBDataSet.AssessmentComponent);
            // TODO: This line of code loads data into the 'projectBDataSet.Rubric' table. You can move, or remove it, as needed.
            this.rubricTableAdapter.Fill(this.projectBDataSet.Rubric);
            // TODO: This line of code loads data into the 'projectBDataSet.Assessment' table. You can move, or remove it, as needed.
            this.assessmentTableAdapter.Fill(this.projectBDataSet.Assessment);

        }
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Delete")
            {
                int rb_Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                if (MessageBox.Show("Do you want to delete " + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM AssessmentComponent WHERE Id  = '" + rb_Id + "'", connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    this.assessmentComponentTableAdapter.Fill(this.projectBDataSet.AssessmentComponent);
                }
            }
            else if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Edit")
            {
                btnAdd.Text = "Update";
                Assessment_Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                connection.Open();
                string query = "Select * from AssessmentComponent WHERE Id=" + Assessment_Id;
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader read = command.ExecuteReader();
                while (read.Read())
                {
                    comboAssessment.SelectedValue = read["AssessmentId"].ToString();
                    comboRubric.SelectedValue = read["RubricId"].ToString();
                    txtTitle.Text = read["Name"].ToString();
                    txtTotalMarks.Text = read["TotalMarks"].ToString();
                }
                read.Close();
                connection.Close();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Update")
            {
                string updateQuery = "Update AssessmentComponent set Name='" + txtTitle.Text.ToString() + "'," +
                    "TotalMarks='" + Convert.ToInt32(txtTotalMarks.Text) + "'," +
                    "AssessmentId='" + Convert.ToInt32(comboAssessment.SelectedValue.ToString()) + "'," +
                    "RubricId='"+Convert.ToInt32(comboRubric.SelectedValue.ToString()) +"'," +
                    "DateUpdated='"+DateTime.Now+ "' Where Id='" + Convert.ToInt32(Assessment_Id) + "'";
                connection.Open();
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Data updated successfully!");
                modalClear();
                btnAdd.Text = "Add";
                this.assessmentComponentTableAdapter.Fill(this.projectBDataSet.AssessmentComponent);
            }
            else
            {

                string insertQuery = "Insert into AssessmentComponent(Name,DateCreated,DateUpdated,TotalMarks,AssessmentId,RubricId) " +
                    "Values('" + txtTitle.Text.ToString() + "','" + Convert.ToDateTime(DateTime.Now) + "','" + Convert.ToDateTime(DateTime.Now) + "'," +
                    "'"+Convert.ToInt32(txtTotalMarks.Text)+"','" + Convert.ToInt32(comboAssessment.SelectedValue) + "','" + Convert.ToInt32(comboRubric.SelectedValue) + "')";
                connection.Open();
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Assessment added successfully!");
                modalClear();
                this.assessmentComponentTableAdapter.Fill(this.projectBDataSet.AssessmentComponent);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Assesment assesment = new Assesment();
            assesment.Show();
        }

        private void txtTotalMarks_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if(!Char.IsDigit(c) && c!=8 && c != 46)
            {
                e.Handled = true;
            }
        }
    }
}
