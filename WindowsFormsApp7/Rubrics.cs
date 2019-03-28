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
    public partial class Rubrics : Form
    {
        public Rubrics()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(Utility.conStr);

        private void Rubrics_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'projectBDataSet.Rubric' table. You can move, or remove it, as needed.
            this.rubricTableAdapter.Fill(this.projectBDataSet.Rubric);
            // TODO: This line of code loads data into the 'projectBDataSet.Clo' table. You can move, or remove it, as needed.
            this.cloTableAdapter.Fill(this.projectBDataSet.Clo);
            
        }
        int RubricId;
        private void btnAddRubric_Click(object sender, EventArgs e)
        {
            if (txtRubricDetail.Text == "")
            {
                MessageBox.Show("Rubric details cannot be empty");
                
            }
            else if(btnAddRubric.Text=="Edit Rubric")
            {
                //Edit Rubric
                connection.Open();
                string query = "UPDATE Rubric set Details='" + txtRubricDetail.Text.ToString() + "',CloId='" + Convert.ToInt32(comboClo.SelectedValue) + "' Where Id='"+Convert.ToInt32(RubricId)+"'";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Rubric edited successfully!");
                this.rubricTableAdapter.Fill(this.projectBDataSet.Rubric);
                txtRubricDetail.Text = "";
                btnAddRubric.Text = "ADD RUBRIC";
            }
            else
            {
                //Add Rubric
                connection.Open();
                string query = "Insert into Rubric(Details,CloId) Values('" + txtRubricDetail.Text.ToString() + "','" + Convert.ToInt32(comboClo.SelectedValue) + "')";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Rubric added successfully!");
                this.rubricTableAdapter.Fill(this.projectBDataSet.Rubric);
                txtRubricDetail.Text = "";
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ProjectB projectB = new ProjectB();
            projectB.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Delete")
            {

                int rb_Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                if (MessageBox.Show("Do you want to delete " + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Rubric WHERE Id  = '" + rb_Id + "'", connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    this.rubricTableAdapter.Fill(this.projectBDataSet.Rubric);
                }
            }
            else if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Edit")
            {
                btnAddRubric.Text = "Edit Rubric";
                RubricId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                connection.Open();
                string query = "Select * from Rubric WHERE Id=" + RubricId;
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader read = command.ExecuteReader();
                while (read.Read())
                {
                    txtRubricDetail.Text = read["Details"].ToString();
                }
                connection.Close();
                
                int cloId= Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select * from Clo WHERE Id='" + cloId + "' ", connection);
                SqlDataReader read1 = cmd.ExecuteReader();
                while (read1.Read())
                {

                    comboClo.DisplayMember = read1["Name"].ToString();
                    comboClo.SelectedValue = read1["Id"].ToString();
                }
                connection.Close();
                
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            RubricLevel rubricLevel = new RubricLevel();
            rubricLevel.Show();
        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            pictureBox1.Tag = "Click here to add levels";
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

