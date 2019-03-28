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
    public partial class AttendanceView : Form
    {
        SqlConnection connection = new SqlConnection(Utility.conStr);
        public AttendanceView()
        {
            InitializeComponent();
        }

        private void AttendanceView_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'projectBDataSet.ClassAttendance' table. You can move, or remove it, as needed.
            this.classAttendanceTableAdapter.Fill(this.projectBDataSet.ClassAttendance);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Delete")
            {
                //Get Id of Attendance from Grid
                int att_Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                //Display confirmation msg for delete
                if (MessageBox.Show("Do you want to delete " + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM ClassAttendance WHERE Id = '" + att_Id + "'", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    this.classAttendanceTableAdapter.Fill(this.projectBDataSet.ClassAttendance);
                }
            }
            else if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Edit")
            {
                
                Utility.status = "Update";
                Utility.AttId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                
            }
        }

        private void btnAssessment_Click(object sender, EventArgs e)
        {
            this.Hide();
            Attendance attendance = new Attendance();
            attendance.Show();
        }
    }
}
