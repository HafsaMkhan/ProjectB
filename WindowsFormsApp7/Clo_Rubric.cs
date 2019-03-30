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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WindowsFormsApp7
{
    public partial class Clo_Rubric : Form
    {
        public Clo_Rubric()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(Utility.conStr);

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ProjectB projectB = new ProjectB();
            projectB.Show();
        }

        private void Clo_Rubric_Load(object sender, EventArgs e)
        {
            button1.Text = "Add CLO";
            // TODO: This line of code loads data into the 'projectBDataSet.Clo' table. You can move, or remove it, as needed.
            this.cloTableAdapter.Fill(this.projectBDataSet.Clo);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtCLO.Text == "")
            {
                lblClo.Text = "Clo cannot be empty!";
            }
            else if (Utility.status=="Edit")
            {
                //Edit Rubric
                connection.Open();
                string query = "UPDATE Clo SET Name='"+txtCLO.Text.ToString()+"',DateUpdated='"+DateTime.Now+"' WHERE Id="+CloId;
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                txtCLO.Text = "";
                //updating grid
                this.cloTableAdapter.Fill(this.projectBDataSet.Clo);
                Utility.status = "";
                button1.Text = "Add CLO";
            }
            else
            {
                //Add Rubric
                connection.Open();
                string query = "INSERT INTO Clo(Name,DateCreated,DateUpdated) VALUES('" + txtCLO.Text.ToString() + "','" + DateTime.Now + "','" + DateTime.Now + "')";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                this.cloTableAdapter.Fill(this.projectBDataSet.Clo);
                txtCLO.Text = "";
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        int CloId;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Delete")
            {
                //Get Id of CLO from Grid
                int clo_Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                //Display confirmation msg for delete
                if (MessageBox.Show("Do you want to delete " + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM Clo WHERE Clo.Id = '" + clo_Id + "'", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    this.cloTableAdapter.Fill(this.projectBDataSet.Clo);
                }
            }
            else if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Edit")
            {
                //Mapping CLO data to be edited in text box
                button1.Text = "Edit CLO";
                Utility.status = "Edit";
                CloId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                connection.Open();
                string query = "Select * from Clo WHERE Id="+CloId;
                SqlCommand command = new SqlCommand(query,connection);
                SqlDataReader read = command.ExecuteReader();
                while (read.Read())
                {
                    txtCLO.Text = read["Name"].ToString();
                }
                connection.Close();


            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnCloResult_Click(object sender, EventArgs e)
        {
            Document document = new Document(iTextSharp.text.PageSize.LETTER, 10, 20, 42, 35);
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileStream("CloReport.pdf", FileMode.Create));
            document.Open();
            PdfPTable table = new PdfPTable(3);
            string[] list = { "Component", "Rubric", "Component Marks" };
            for (int i = 0; i < 3; i++)
            {
                PdfPCell cell = new PdfPCell(new Phrase(list[i]));
                cell.Colspan = 1;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
            }
            connection.Open();
            foreach (DataGridViewRow row in dataGridView1.Rows) 
            {
                int ab = -1;
                int Clo_Id = Convert.ToInt32(row.Cells["idDataGridViewTextBoxColumn"].Value);
                SqlCommand q = new SqlCommand("Select MAX(RubricLevel.MeasurementLevel) as ml /*, Rubric.Details as detail */" +
                    "From RubricLevel INNER JOIN Rubric ON  RubricLevel.RubricId=Rubric.Id INNER JOIN Clo ON " +
                    "Clo.Id=Rubric.CloId WHERE Clo.Id='"+Clo_Id+"'", connection);
                SqlDataReader dataReader = q.ExecuteReader();
                while (dataReader.Read())
                {
                    PdfPCell cell = new PdfPCell(new Phrase(row.Cells[1].Value.ToString()));
                    cell.Colspan = 1;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell); PdfPCell cell1 = new PdfPCell(new Phrase("Rubric"));
                    cell.Colspan = 1;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell1);
                    PdfPCell cell2 = new PdfPCell(new Phrase(dataReader["ml"].ToString()));
                    cell.Colspan = 1;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell2);
                }
                dataReader.Close();
            }
            document.Add(table);
            document.Close();
            connection.Close();
        }
    }
}
