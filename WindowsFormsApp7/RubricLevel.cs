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
    public partial class RubricLevel : Form
    {
        public RubricLevel()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(Utility.conStr);

        private void RubricLevel_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'projectBDataSet.Rubric' table. You can move, or remove it, as needed.
            this.rubricTableAdapter.Fill(this.projectBDataSet.Rubric);
            // TODO: This line of code loads data into the 'projectBDataSet.RubricLevel' table. You can move, or remove it, as needed.
            this.rubricLevelTableAdapter.Fill(this.projectBDataSet.RubricLevel);

        }
        public void nullify()
        {
            lblDetails.Text = "";
            lblMeasureLevel.Text = "";
        }
        private void btnAddRubricLevel_Click(object sender, EventArgs e)
        {
            nullify();
            if (txtMeasurementLevel.Text == "")
            {
                lblMeasureLevel.Text = "Measurement level cannot be empty";
            }
            else if (txtDetail.Text == "")
            {
                lblDetails.Text = "Details cannot be empty";
            }
            else if (btnAddRubricLevel.Text== "Add Level")
            {
                connection.Open();
                string query = "Insert into RubricLevel(RubricId,Details,MeasurementLevel) Values('"+Convert.ToInt32(comboRubric.SelectedValue)+"','" + txtDetail.Text.ToString() + "','" + Convert.ToInt32(txtMeasurementLevel.Text) + "')";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Rubric level added successfully!");
                this.rubricLevelTableAdapter.Fill(this.projectBDataSet.RubricLevel);
                txtMeasurementLevel.Text = "";
                txtDetail.Text = "";
                nullify();
            }
            else if(btnAddRubricLevel.Text == "Edit Level")
            {
                connection.Open();
                string query = "UPDATE RubricLevel set RubricId='"+Convert.ToInt32(comboRubric.SelectedValue)+"',Details='"+txtDetail.Text.ToString()+"',MeasurementLevel='"+Convert.ToInt32(txtMeasurementLevel.Text)+"' Where Id='"+ RubricLevelId + "' ";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Rubric level added successfully!");
                this.rubricLevelTableAdapter.Fill(this.projectBDataSet.RubricLevel);
                txtMeasurementLevel.Text = "";
                txtDetail.Text = "";
                nullify();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Rubrics rubric = new Rubrics();
            rubric.Show();
        }

        private void txtMeasurementLevel_TextChanged(object sender, EventArgs e)
        {
            int Number;
            if (txtMeasurementLevel.Text==""){}
            else if(!(int.TryParse(txtMeasurementLevel.Text,out Number)))
            {
                MessageBox.Show("Invalid input");
                txtMeasurementLevel.Text = "";
            }
        }
        int RubricLevelId;
        private void dataRubricLevel_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataRubricLevel.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Delete")
            {
                int rblvl_Id = Convert.ToInt32(dataRubricLevel.Rows[e.RowIndex].Cells[0].Value);
                if (MessageBox.Show("Do you want to delete " + dataRubricLevel.Rows[e.RowIndex].Cells[1].Value.ToString() + "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM RubricLevel WHERE Id  = '" + rblvl_Id + "'", connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    this.rubricLevelTableAdapter.Fill(this.projectBDataSet.RubricLevel);
                }
            }
            else if (dataRubricLevel.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Edit")
            {
                btnAddRubricLevel.Text = "Edit Level";
                RubricLevelId = Convert.ToInt32(dataRubricLevel.Rows[e.RowIndex].Cells[0].Value);
                connection.Open();
                string query = "Select * from RubricLevel WHERE Id=" + RubricLevelId;
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader read = command.ExecuteReader();
                while (read.Read())
                {
                    txtMeasurementLevel.Text = read["MeasurementLevel"].ToString();
                    txtDetail.Text = read["Details"].ToString();
                }
                connection.Close();
                connection.Open();
                int RubricId = Convert.ToInt32(dataRubricLevel.Rows[e.RowIndex].Cells[0].Value);
                string query1 = "Select * from Rubric WHERE Id=" + RubricId;
                SqlCommand command1 = new SqlCommand(query1, connection);
                SqlDataReader read1 = command.ExecuteReader();
                while (read1.Read())
                {
                    comboRubric.DisplayMember = read1["Details"].ToString();
                    comboRubric.SelectedValue = read1["Id"].ToString();

                }
                connection.Close();
            }
        }
    }
}
