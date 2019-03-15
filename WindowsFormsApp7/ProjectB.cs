using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }
}
