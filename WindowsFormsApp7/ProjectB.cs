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
    public partial class ProjectB : Form
    {
        public ProjectB()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentView studentView = new StudentView();
            studentView.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Rubrics clo_Rubric = new Rubrics();
            clo_Rubric.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Clo_Rubric clo_Rubric = new Clo_Rubric();
            clo_Rubric.Show();

        }

        private void ProjectB_Load(object sender, EventArgs e)
        {
            if (Utility.alter == false)
            {
                SqlConnection sqlConnection = new SqlConnection(Utility.conStr);
                sqlConnection.Open();
                string query = "ALTER TABLE [dbo].[Rubric] DROP CONSTRAINT [FK_Rubric_Clo]";
                string q = "ALTER TABLE [dbo].[Rubric]  WITH CHECK ADD CONSTRAINT [FK_Rubric_Clo] FOREIGN KEY([CloId]) REFERENCES[dbo].[Clo]([Id]) ON DELETE CASCADE";
                SqlCommand sql = new SqlCommand(query, sqlConnection);
                SqlCommand sqlCmd = new SqlCommand(q, sqlConnection);
                SqlCommand cmd = new SqlCommand("ALTER TABLE [dbo].[RubricLevel] DROP CONSTRAINT [FK_RubricLevel_Rubric]", sqlConnection);
                SqlCommand cmd1 = new SqlCommand("ALTER TABLE [dbo].[RubricLevel]  WITH CHECK ADD  CONSTRAINT [FK_RubricLevel_Rubric] FOREIGN KEY([RubricId]) REFERENCES [dbo].[Rubric] ([Id]) ON DELETE CASCADE", sqlConnection);

                SqlCommand cmd2 = new SqlCommand("ALTER TABLE [dbo].[AssessmentComponent] DROP CONSTRAINT [FK_AssessmentComponent_Assessment]", sqlConnection);
                SqlCommand cmd3 = new SqlCommand("ALTER TABLE [dbo].[AssessmentComponent]  WITH CHECK ADD  CONSTRAINT [FK_AssessmentComponent_Assessment] FOREIGN KEY([AssessmentId]) REFERENCES [dbo].[Assessment] ([Id]) ON DELETE CASCADE", sqlConnection);

                SqlCommand cmd6 = new SqlCommand("ALTER TABLE [dbo].[StudentAttendance] DROP CONSTRAINT [FK_StudentAttendance_ClassAttendance]", sqlConnection);
                SqlCommand cmd7 = new SqlCommand("ALTER TABLE [dbo].[StudentAttendance]  WITH CHECK ADD  CONSTRAINT [FK_StudentAttendance_ClassAttendance] FOREIGN KEY([AttendanceId]) REFERENCES [dbo].[ClassAttendance] ([Id]) ON DELETE CASCADE", sqlConnection);

                SqlCommand cmd10 = new SqlCommand("ALTER TABLE [dbo].[StudentResult] DROP CONSTRAINT [FK_StudentResult_AssessmentComponent]", sqlConnection);
                SqlCommand cmd11 = new SqlCommand("ALTER TABLE [dbo].[StudentResult]  WITH CHECK ADD  CONSTRAINT [FK_StudentResult_AssessmentComponent] FOREIGN KEY([AssessmentComponentId]) REFERENCES[dbo].[AssessmentComponent]([Id]) ON DELETE CASCADE", sqlConnection);
                SqlCommand cmd12 = new SqlCommand("ALTER TABLE [dbo].[StudentResult] DROP CONSTRAINT [FK_StudentResult_RubricLevel]", sqlConnection);
                SqlCommand cmd13 = new SqlCommand("ALTER TABLE [dbo].[StudentResult]  WITH CHECK ADD  CONSTRAINT [FK_StudentResult_RubricLevel] FOREIGN KEY([RubricMeasurementId]) REFERENCES [dbo].[RubricLevel] ([Id]) ON DELETE CASCADE", sqlConnection);
                SqlCommand cmd14 = new SqlCommand("ALTER TABLE [dbo].[StudentResult] DROP CONSTRAINT [FK_StudentResult_Student]", sqlConnection);
                SqlCommand cmd15 = new SqlCommand("ALTER TABLE [dbo].[StudentResult]  WITH CHECK ADD  CONSTRAINT [FK_StudentResult_Student] FOREIGN KEY([StudentId]) REFERENCES[dbo].[Student]([Id]) ON DELETE CASCADE", sqlConnection);
                //sql.ExecuteNonQuery();
                //sqlCmd.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                cmd3.ExecuteNonQuery(); cmd6.ExecuteNonQuery(); cmd7.ExecuteNonQuery();
                cmd10.ExecuteNonQuery(); cmd11.ExecuteNonQuery(); cmd12.ExecuteNonQuery(); cmd13.ExecuteNonQuery();
                cmd14.ExecuteNonQuery(); cmd15.ExecuteNonQuery();
                sqlConnection.Close();
                
                Utility.alter = true;
            }
            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
