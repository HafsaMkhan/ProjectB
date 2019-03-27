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
        SqlDataAdapter adapter;
        int Att_id = -1;
        private void AttendanceData()
        {
            if (dateTimePicker1.Value.DayOfWeek.ToString() == "Saturday" || dateTimePicker1.Value.DayOfWeek.ToString() == "Sunday")
            {
                dataGridView1.Hide();
                MessageBox.Show("Official Holiday!");
            }
            else
            {
                dataGridView1.Show();
                dataGridView1.AutoGenerateColumns = false;
                DataTable dataTable = new DataTable();
                Utility.attendanceStatus = "Add";
                btnMarkAttendance.Text = "Add Attendance";
                connection.Open();

                //Retrieving old attendance
                SqlCommand sql = new SqlCommand("Select * from ClassAttendance Where DATEDIFF(DAY, AttendanceDate, '" + Convert.ToDateTime(dateTimePicker1.Value) + "')=0", connection);
                SqlDataReader reader = sql.ExecuteReader();
                
                while (reader.Read())
                {
                    Utility.attendanceStatus = "Update";
                    btnMarkAttendance.Text = "Update Attendance";
                    Att_id = Convert.ToInt32(reader["Id"]);
                }
                reader.Close();
                //Populating Combobox 
                string combox_data_query = "SELECT LookupId,Name from Lookup where LookupId<5";
                SqlCommand sqlCommand = new SqlCommand(combox_data_query, connection);
                SqlDataAdapter combobox_data = new SqlDataAdapter(sqlCommand);
                DataTable dtt = new DataTable();
                combobox_data.Fill(dtt);
                this.attend.DisplayMember = "Name";
                this.attend.ValueMember = "LookupId";
                this.attend.DataSource = dtt;
                connection.Close();
                if (Att_id != -1)
                {
                    string abc = "Select Student.Id as Id, Student.RegistrationNumber as RegistrationNumber, " +
                        "CONCAT(Student.FirstName,' ',Student.LastName) as Name, StudentAttendance.AttendanceId as attend " +
                        "from Student INNER JOIN StudentAttendance ON Id=StudentAttendance.StudentId " +
                        "where AttendanceId='" + Att_id + "'";
                    SqlCommand cmx = new SqlCommand(abc, connection);
                    adapter = new SqlDataAdapter(cmx);
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    Att_id = -1;
                }
                else
                {
                    //When new attendance is going to be taken
                    string qu = "Select Id, RegistrationNumber, CONCAT(FirstName,' ',LastName ) as Name from Student WHERE Status=5";
                    SqlCommand command = new SqlCommand(qu, connection);
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }
        }
        
        private void Attendance_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            AttendanceData();
            
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            AttendanceData();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentView studentView = new StudentView();
            studentView.Show();
        }

        private void btnMarkAttendance_Click(object sender, EventArgs e)
        {
            if (Utility.attendanceStatus == "Update")
            {
                connection.Open();
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {   
                    string query = "Update StudentAttendance set AttendanceStatus='"+ Convert.ToInt32(row.Cells["attend"].Value) + "' " +
                        "WHERE AttendanceId='"+ Convert.ToInt32(Att_id) + "' AND StudentId='" + Convert.ToInt32(row.Cells["Id"].Value) + "'";
                    SqlCommand attendanceUpdate = new SqlCommand(query, connection);
                    attendanceUpdate.ExecuteNonQuery();
                }
                Utility.attendanceStatus = "Add";
                MessageBox.Show("Attendance updated successfully!");
                this.Hide();
                StudentView studentView = new StudentView();
                studentView.Show();
            }
            else
            {
                connection.Open();
                string query = "Insert into ClassAttendance(AttendanceDate) VALUES('"+Convert.ToDateTime(dateTimePicker1.Value)+"')";
                SqlCommand ClassAttendance_Query = new SqlCommand(query, connection);
                ClassAttendance_Query.ExecuteNonQuery();
                SqlCommand getAttendance = new SqlCommand("Select * from ClassAttendance where attendanceDate='" + Convert.ToDateTime(dateTimePicker1.Value) + "'",connection);
                SqlDataReader AttendanceId = getAttendance.ExecuteReader();
                int aID=-1;
                while (AttendanceId.Read())
                {
                    aID = Convert.ToInt32(AttendanceId["Id"].ToString());
                }
                AttendanceId.Close();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(Convert.ToInt32(row.Cells["attend"].Value)))
                    {
                        SqlCommand studentAttendanceQuery = new SqlCommand("Insert INTO StudentAttendance(AttendanceId,StudentId,AttendanceStatus) " +
                        "Values('" + Convert.ToInt32(aID) + "','" + Convert.ToInt32(row.Cells["Id"].Value) + "'," +
                        "'" + Convert.ToInt32(row.Cells["attend"].Value) + "')", connection);
                        studentAttendanceQuery.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Attendance marked and saved!");
                this.Hide();
                StudentView studentView = new StudentView();
                studentView.Show();
            }
        }
    }
}
