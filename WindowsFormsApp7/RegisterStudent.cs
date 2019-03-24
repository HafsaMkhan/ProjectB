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
using System.Text.RegularExpressions;

namespace WindowsFormsApp7
{
    public partial class RegisterStudent : Form
    {
        public RegisterStudent()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(Utility.conStr);

        public void modalClear()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtContact.Text = "";
            txtRegisterNo.Text = "";
        }
        public void errorClear()
        {
            lblFirstName.Text = "";
            lblLastName.Text = "";
            lblEmail.Text = "";
            lblRegister.Text = "";
            lblContact.Text = "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            errorClear();
            if (txtFirstName.Text == "" && Regex.IsMatch(txtFirstName.Text, @"^([a-zA-Z ]*?)$"))
            {
                lblFirstName.Text = "First name is missing or invalid";
            }
            else if (txtLastName.Text == "" && Regex.IsMatch(txtFirstName.Text, @"^([a-zA-Z ]*?)$"))
            {
                lblLastName.Text = "Last name is missing or invalid";
            }
            else if (txtEmail.Text == "" && Regex.IsMatch(txtEmail.Text, @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
     + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
     + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$"))
            {
                lblEmail.Text = "Email is missing or invalid";
            }
            else if (txtContact.Text == "" || Regex.IsMatch(txtContact.Text, @"^(([+]{1}[0-9]{2}|0)[0-9]{9})$"))
            {
                lblContact.Text = "Contact is missing or invalid";
            }
            else if (txtRegisterNo.Text == "")
            {
                lblRegister.Text = "Registration Number is missing or invalid";
            }
            else if (Utility.status == "Edit" && txtRegisterNo.Text != "")
            {
                int stat = 5;
                if (comboStatus.SelectedItem.ToString() == "In Active")
                {
                    stat = 6;
                }
                connection.Open();
                string query = "UPDATE Student SET FirstName='" + txtFirstName.Text.ToString() +"',LastName='" + txtLastName.Text.ToString() + "',Contact='" + txtContact.Text.ToString() + "',Email='" + txtEmail.Text.ToString() + "',RegistrationNumber='" + txtRegisterNo.Text.ToString() + "',Status='"+stat+"' WHERE Id=" + Utility.StudentId;
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Data has been updated successfully!");
                modalClear();
                Utility.status = "";
                btnRegisterStudent.Text = "Register";
                Utility.StudentId = 0;
            }
            else
            {
                connection.Open();
                string query = "INSERT INTO Student(FirstName,LastName,Contact,Email,RegistrationNumber,Status) VALUES('" + txtFirstName.Text.ToString() + "','" + txtLastName.Text.ToString() + "','" + txtContact.Text.ToString() + "','" + txtEmail.Text.ToString() + "','" + txtRegisterNo.Text.ToString() + "',5)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Student has been registered successfully!");
                modalClear();
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Utility.status = "";
            Utility.StudentId = 0;
            this.Hide();
            StudentView studentView = new StudentView();
            studentView.Show();
        }

        private void RegisterStudent_Load(object sender, EventArgs e)
        {
            errorClear();
            modalClear();
            btnRegisterStudent.Text = "Register";
            comboStatus.Visible = false;
            label7.Visible = false;
            if (Utility.status == "Edit")
            {
                comboStatus.Visible = true;
                label7.Visible = true;
                btnRegisterStudent.Text = "Update";
                connection.Open();
                string query = "Select * from Student WHERE Id="+Utility.StudentId;
                SqlCommand sqlCommand = new SqlCommand(query,connection);
                SqlDataReader read=  sqlCommand.ExecuteReader();
                while (read.Read())
                {
                    txtFirstName.Text = read["FirstName"].ToString();
                    txtLastName.Text = read["LastName"].ToString();
                    txtEmail.Text = read["Email"].ToString();
                    txtContact.Text = read["Contact"].ToString();
                    txtRegisterNo.Text = read["RegistrationNumber"].ToString();
                    if (Convert.ToInt32(read["Status"]) == 5)
                    {
                        comboStatus.Text = "Active";
                    }
                    else if (Convert.ToInt32(read["Status"]) == 6)
                    {
                        comboStatus.Text = "In Active";
                    }
                     
                }
                connection.Close();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ProjectB projectB = new ProjectB();
            projectB.Show();
        }

        private void comboClo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
