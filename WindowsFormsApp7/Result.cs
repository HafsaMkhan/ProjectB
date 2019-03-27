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
    public partial class Result : Form
    {
        SqlConnection connection = new SqlConnection(Utility.conStr);
        public Result()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentView student = new StudentView();
            student.Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void populateStudent()
        {
            connection.Open();
            string combox_data_query = "SELECT Id,RegistrationNumber from Student where Status=5";
            SqlCommand sqlCommand = new SqlCommand(combox_data_query, connection);
            sqlCommand.ExecuteNonQuery();
            SqlDataAdapter combobox_data = new SqlDataAdapter(sqlCommand);
            DataTable dtt = new DataTable();
            combobox_data.Fill(dtt);
            comboStudent.DisplayMember = "RegistrationNumber";
            comboStudent.SelectedItem = "Id";
            comboStudent.DataSource = dtt;
            connection.Close();
        }
        private void Result_Load(object sender, EventArgs e)
        {
            populateStudent();
            this.studentResultTableAdapter.Fill(this.projectBDataSet.StudentResult);
            // TODO: This line of code loads data into the 'projectBDataSet.RubricLevel' table. You can move, or remove it, as needed.
            this.rubricLevelTableAdapter.Fill(this.projectBDataSet.RubricLevel);
            // TODO: This line of code loads data into the 'projectBDataSet.AssessmentComponent' table. You can move, or remove it, as needed.
            this.assessmentComponentTableAdapter.Fill(this.projectBDataSet.AssessmentComponent);

        }
        int stu_rst;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Update")
            {
                string updateQuery = "Update StudentResult set RubricMeasurementId='" + Convert.ToInt32(comboRubric.SelectedValue) + "'," +
                    "AssessmentComponentId='" + Convert.ToInt32(comboAssessment.SelectedValue.ToString()) + "'" +
                    " Where StudentId='" + Convert.ToInt32(comboStudent.SelectedValue) + "'";
                connection.Open();
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Data updated successfully!");
                btnAdd.Text = "Add";
                dateTimePicker1.Show();
                lblDate.Show();
                this.studentResultTableAdapter.Fill(this.projectBDataSet.StudentResult);
            }
            else
            {
                bool flag = false;
                try
                {
                    string insertQuery = "Insert into StudentResult(StudentId,AssessmentComponentId,RubricMeasurementId,EvaluationDate) " +
                    "Values('" + Convert.ToInt32(comboStudent.SelectedValue) + "','" + Convert.ToInt32(comboAssessment.SelectedValue) + "'," +
                    "'" + Convert.ToInt32(comboRubric.SelectedValue) + "','" + Convert.ToDateTime(dateTimePicker1.Value) + "')";
                    connection.Open();
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Result added successfully!");
                    this.studentResultTableAdapter.Fill(this.projectBDataSet.StudentResult);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Student result record already exists!");
                }
                
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
                    SqlCommand cmd = new SqlCommand("DELETE FROM StudentResult WHERE StudentId  = '" + rb_Id + "'", connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    this.studentResultTableAdapter.Fill(this.projectBDataSet.StudentResult);
                }
            }
            else if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Edit")
            {
                btnAdd.Text = "Update";
                dateTimePicker1.Hide();
                lblDate.Hide();
                stu_rst = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                connection.Open();
                string query = "Select * from StudentResult WHERE StudentId=" + stu_rst;
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader read = command.ExecuteReader();
                while (read.Read())
                {
                    comboAssessment.SelectedValue = read["AssessmentComponentId"].ToString();
                    comboRubric.SelectedValue = read["RubricMeasurementId"].ToString();
                    comboStudent.SelectedValue = read["StudentId"].ToString();
                }
                read.Close();
                connection.Close();
            }
        }
    }
}
