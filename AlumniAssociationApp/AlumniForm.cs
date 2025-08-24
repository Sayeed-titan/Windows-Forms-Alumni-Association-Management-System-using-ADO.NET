using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlumniAssociationApp
{
    public partial class AlumniForm : Form
    {
        public AlumniForm()
        {
            InitializeComponent();
            LoadAlumni();
        }

        private void LoadAlumni()
        {
            dataGridView1.DataSource = DatabaseHelper.GetData("SELECT * FROM Alumni");

            // Auto-fit columns
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Alumni (FullName, GraduationYear, Email, Phone) VALUES (@Name, @Year, @Email, @Phone)";
            DatabaseHelper.ExecuteCommand(query,
                new SqlParameter("@Name", txtName.Text),
                new SqlParameter("@Year", int.Parse(txtYear.Text)),
                new SqlParameter("@Email", txtEmail.Text),
                new SqlParameter("@Phone", txtPhone.Text)
            );
            LoadAlumni();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["AlumniID"].Value);
                string query = "UPDATE Alumni SET FullName=@Name, GraduationYear=@Year, Email=@Email, Phone=@Phone WHERE AlumniID=@ID";
                DatabaseHelper.ExecuteCommand(query,
                    new SqlParameter("@Name", txtName.Text),
                    new SqlParameter("@Year", int.Parse(txtYear.Text)),
                    new SqlParameter("@Email", txtEmail.Text),
                    new SqlParameter("@Phone", txtPhone.Text),
                    new SqlParameter("@ID", id)
                );
                LoadAlumni();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["AlumniID"].Value);
                string query = "DELETE FROM Alumni WHERE AlumniID=@ID";
                DatabaseHelper.ExecuteCommand(query, new SqlParameter("@ID", id));
                LoadAlumni();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtYear.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";

            // Optional: deselect any selected row in DataGridView
            dataGridView1.ClearSelection();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                txtName.Text = dataGridView1.CurrentRow.Cells["FullName"].Value.ToString();
                txtYear.Text = dataGridView1.CurrentRow.Cells["GraduationYear"].Value.ToString();
                txtEmail.Text = dataGridView1.CurrentRow.Cells["Email"].Value.ToString();
                txtPhone.Text = dataGridView1.CurrentRow.Cells["Phone"].Value.ToString();
            }
        }

        private string selectedPhotoPath = "";

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedPhotoPath = ofd.FileName;
                    picPhoto.ImageLocation = selectedPhotoPath;
                }
            }
        }
    }
}
