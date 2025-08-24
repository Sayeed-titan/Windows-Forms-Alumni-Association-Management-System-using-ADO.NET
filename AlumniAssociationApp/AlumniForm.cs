using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlumniAssociationApp
{
    public partial class AlumniForm : Form
    {
        private string selectedPhotoPath = "";

        public AlumniForm()
        {
            InitializeComponent();
            LoadAlumni();
        }

        private void LoadAlumni()
        {
            DataTable dt = DatabaseHelper.GetData("SELECT * FROM Alumni");

            // Add image column
            if (!dt.Columns.Contains("PhotoImage"))
                dt.Columns.Add("PhotoImage", typeof(Bitmap));

            foreach (DataRow row in dt.Rows)
            {
                string path = row["PhotoPath"].ToString();
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    row["PhotoImage"] = new Bitmap(path);
            }

            dataGridView1.DataSource = dt;

            // Hide PhotoPath column
            if (dataGridView1.Columns["PhotoPath"] != null)
                dataGridView1.Columns["PhotoPath"].Visible = false;

            // Show image column
            if (dataGridView1.Columns["PhotoImage"] != null)
            {
                dataGridView1.Columns["PhotoImage"].HeaderText = "Photo";
                dataGridView1.Columns["PhotoImage"].Width = 100;
            }

            // Auto-fit
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;

            // Clear selection on load
            dataGridView1.ClearSelection();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string folderPath = Path.Combine(Application.StartupPath, @"..\..\Images");
            folderPath = Path.GetFullPath(folderPath);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string destPath = "";
            if (!string.IsNullOrEmpty(selectedPhotoPath) && File.Exists(selectedPhotoPath))
            {
                destPath = Path.Combine(folderPath, Path.GetFileName(selectedPhotoPath));
                File.Copy(selectedPhotoPath, destPath, true);
            }

            string query = "INSERT INTO Alumni (FullName, GraduationYear, Email, Phone, PhotoPath) " +
                           "VALUES (@Name, @Year, @Email, @Phone, @PhotoPath)";
            DatabaseHelper.ExecuteCommand(query,
                new SqlParameter("@Name", txtName.Text),
                new SqlParameter("@Year", int.Parse(txtYear.Text)),
                new SqlParameter("@Email", txtEmail.Text),
                new SqlParameter("@Phone", txtPhone.Text),
                new SqlParameter("@PhotoPath", destPath)
            );

            LoadAlumni();
            ClearFields();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["AlumniID"].Value);
            string folderPath = Path.Combine(Application.StartupPath, @"..\..\Images");
            folderPath = Path.GetFullPath(folderPath);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string destPath = dataGridView1.CurrentRow.Cells["PhotoPath"].Value.ToString();
            if (!string.IsNullOrEmpty(selectedPhotoPath) && File.Exists(selectedPhotoPath))
            {
                destPath = Path.Combine(folderPath, Path.GetFileName(selectedPhotoPath));
                File.Copy(selectedPhotoPath, destPath, true);
            }

            string query = "UPDATE Alumni SET FullName=@Name, GraduationYear=@Year, Email=@Email, Phone=@Phone, PhotoPath=@PhotoPath WHERE AlumniID=@ID";
            DatabaseHelper.ExecuteCommand(query,
                new SqlParameter("@Name", txtName.Text),
                new SqlParameter("@Year", int.Parse(txtYear.Text)),
                new SqlParameter("@Email", txtEmail.Text),
                new SqlParameter("@Phone", txtPhone.Text),
                new SqlParameter("@PhotoPath", destPath),
                new SqlParameter("@ID", id)
            );

            LoadAlumni();
            ClearFields();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["AlumniID"].Value);
            string query = "DELETE FROM Alumni WHERE AlumniID=@ID";
            DatabaseHelper.ExecuteCommand(query, new SqlParameter("@ID", id));

            LoadAlumni();
            ClearFields();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            txtName.Text = dataGridView1.CurrentRow.Cells["FullName"].Value.ToString();
            txtYear.Text = dataGridView1.CurrentRow.Cells["GraduationYear"].Value.ToString();
            txtEmail.Text = dataGridView1.CurrentRow.Cells["Email"].Value.ToString();
            txtPhone.Text = dataGridView1.CurrentRow.Cells["Phone"].Value.ToString();

            string path = dataGridView1.CurrentRow.Cells["PhotoPath"].Value.ToString();
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
                picPhoto.Image = new Bitmap(path);
            else
                picPhoto.Image = null;

            selectedPhotoPath = ""; // reset selected photo
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.webp" ;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedPhotoPath = ofd.FileName;
                    picPhoto.ImageLocation = selectedPhotoPath;
                }
            }
        }

        private void ClearFields()
        {
            txtName.Text = "";
            txtYear.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            picPhoto.Image = null;
            selectedPhotoPath = "";
            dataGridView1.ClearSelection();
        }



    }
}
