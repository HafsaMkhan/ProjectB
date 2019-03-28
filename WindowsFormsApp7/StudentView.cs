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

    public partial class StudentView : Form
    {
        public StudentView()
        {
            InitializeComponent();
        }
        const string conStr = "Data Source=DESKTOP-2FD4D2N\\SQLEXPRESS;Initial Catalog=ProjectB;Integrated Security=True";
        SqlConnection connect = new SqlConnection(conStr);

        private void StudentView_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'projectBDataSet.Student' table. You can move, or remove it, as needed.
            this.studentTableAdapter.Fill(this.projectBDataSet.Student);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegisterStudent registerStudent = new RegisterStudent();
            registerStudent.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Edit")
            {
                Utility.status = "Edit";
                Utility.StudentId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                this.Hide();
                RegisterStudent registerStudent = new RegisterStudent();
                registerStudent.Show();
            }
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Delete")
            {
                int stdId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                if (MessageBox.Show("Do you want to delete "+ dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connect.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM Student WHERE Id  = '" + stdId + "'", connect);
                    command.ExecuteNonQuery();
                    connect.Close();
                    this.studentTableAdapter.Fill(this.projectBDataSet.Student);
                }
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ProjectB projectB = new ProjectB();
            projectB.Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            AttendanceView attendance = new AttendanceView();
            attendance.Show();
        }

        private void btnAssessment_Click(object sender, EventArgs e)
        {
            this.Hide();
            Assesment assesment = new Assesment();
            assesment.Show();
        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            this.Hide();
            Result result = new Result();
            result.Show();
        }
    }
}
