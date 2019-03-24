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
    public partial class Attendance : Form
    {
        SqlConnection connection = new SqlConnection(Utility.conStr);
        public Attendance()
        {
            InitializeComponent();
        }

        private void Attendance_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            connection.Open();
            string query = "Select * from StudentAttendance INNER JOIN ClassAttendance ON StudentAttendance.AttendanceId=ClassAttendance.Id WHERE AttendanceDate='"+Convert.ToDateTime(dateTimePicker1.Value)+"'";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                if (dataReader != null)
                {
                    MessageBox.Show("Data got!");
                }
                else if (dataReader == null)
                {
                    MessageBox.Show("Nothing found!");
                }
            }
            connection.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentView studentView = new StudentView();
            studentView.Show();
        }
    }
}
