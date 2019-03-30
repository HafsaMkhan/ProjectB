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
//Pdf generator libraries. . . download from Tools>NuGet Package Manager> Manage NuGet Package Manager
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;



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

        private void btnPdf_Click(object sender, EventArgs e)
        {
            try
            {
                Document document = new Document(iTextSharp.text.PageSize.LETTER, 10, 20, 42, 35);
                PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileStream("Result.pdf", FileMode.Create));
                document.Open();
                connection.Open();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    int studentId = Convert.ToInt32(row.Cells[0].Value);
                    int assessmentComponentId = Convert.ToInt32(row.Cells[1].Value);
                    int RubeicMeasurementId = Convert.ToInt32(row.Cells[2].Value);
                    string query = "Select Student.RegistrationNumber as regNo, CONCAT(Student.FirstName,' ', Student.LastName) as Name," +
                        " AssessmentComponent.Name as Component, AssessmentComponent.TotalMarks as Total," +
                        " RubricLevel.Details as Rubric,RubricLevel.MeasurementLevel as stdLevel,RubricLevel.RubricId as RubricId" +
                        " from Student JOIN StudentResult ON StudentResult.StudentId=Student.Id JOIN AssessmentComponent ON " +
                        "StudentResult.AssessmentComponentId=AssessmentComponent.Id Join RubricLevel ON StudentResult.RubricMeasurementId=RubricLevel.Id WHERE " +
                        "StudentResult.StudentId='" + Convert.ToInt32(studentId) + "' AND StudentResult.AssessmentComponentId='" + Convert.ToInt32(assessmentComponentId) + "' AND " +
                        "StudentResult.RubricMeasurementId='" + Convert.ToInt32(RubeicMeasurementId) + "'";
                    SqlCommand resultCommand = new SqlCommand(query, connection);
                    SqlDataReader reader = resultCommand.ExecuteReader();
                    string Name = "", Reg = "", Component = "", Rubric = "", MeasurementLevel = "";
                    int Total = 0;
                    int rb_Id = 0;
                    while (reader.Read())
                    {
                        Name = reader["Name"].ToString();
                        Reg = reader["regNo"].ToString();
                        Total = Convert.ToInt32(reader["Total"]);
                        Component = reader["Component"].ToString();
                        Rubric = reader["Rubric"].ToString();
                        MeasurementLevel = reader["stdLevel"].ToString();
                        rb_Id = Convert.ToInt32(reader["RubricId"]);
                    }
                    reader.Close();
                    int ab = -1;
                    SqlCommand q = new SqlCommand("Select MAX(MeasurementLevel) as ml From RubricLevel Where RubricId='" + Convert.ToInt32(rb_Id) + "'", connection);
                    SqlDataReader dataReader = q.ExecuteReader();
                    while (dataReader.Read())
                    {
                        ab = Convert.ToInt32(dataReader["ml"]);
                    }
                    dataReader.Close();
                    Paragraph para = new Paragraph("Name: " + Name.ToString() + "\nRegistration Number: " + Reg + "\n\n");
                    PdfPTable table = new PdfPTable(5);
                    string[] list = { "Component", "Rubric", "Component Marks", "Student Rubric Level", "Obtained Marks" };
                    for (int i = 0; i < 5; i++)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(list[i]));
                        cell.Colspan = 1;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);
                    }
                    float obtain = (Convert.ToInt32(MeasurementLevel) / ab) * Total;
                    string[] data = { Component, Rubric, Total.ToString(), MeasurementLevel, obtain.ToString() };
                    for (int i = 0; i < 5; i++)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(data[i]));
                        cell.Colspan = 1;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);
                    }
                    document.Add(para);
                    document.Add(table);
                }
                document.Close();
                MessageBox.Show("Student has been report card downloaded!");
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show("No Data Found");
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
