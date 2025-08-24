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

        private bool isUpdateMode = false;
        private int selectedAlumniId = -1;

        private bool isFormLoading = true;


        public AlumniForm()
        {
            InitializeComponent();        
      
            // Initially hide  buttons
            btnDelete.Visible = false;
            btnCancel.Visible = false;
            btnClear.Visible = false;

            // Hook TextChanged events
            txtName.TextChanged += TextBoxes_TextChanged;
            txtYear.TextChanged += TextBoxes_TextChanged;
            txtEmail.TextChanged += TextBoxes_TextChanged;
            txtPhone.TextChanged += TextBoxes_TextChanged;


            // Prevent SelectionChanged from firing during load
            isFormLoading = true;

            LoadAlumni();

            // Form is fully loaded
            isFormLoading = false;

        }

        private void LoadAlumni()
        {
            DataTable dt = DatabaseHelper.GetData("SELECT * FROM Alumni");

            if (!dt.Columns.Contains("PhotoImage"))
                dt.Columns.Add("PhotoImage", typeof(Bitmap));

            foreach (DataRow row in dt.Rows)
            {
                string path = row["PhotoPath"].ToString();
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    row["PhotoImage"] = new Bitmap(path);
            }

            // Temporarily unsubscribe to prevent SelectionChanged firing
            dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;

            dataGridView1.DataSource = dt;

            if (dataGridView1.Columns["PhotoPath"] != null)
                dataGridView1.Columns["PhotoPath"].Visible = false;

            if (dataGridView1.Columns["PhotoImage"] != null)
            {
                dataGridView1.Columns["PhotoImage"].HeaderText = "Photo";
                dataGridView1.Columns["PhotoImage"].Width = 100;
                DataGridViewImageColumn imgCol = dataGridView1.Columns["PhotoImage"] as DataGridViewImageColumn;
                if (imgCol != null)
                    imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            }

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 120;

            // Clear any selection so no row is selected on load
            dataGridView1.ClearSelection();

            // Re-subscribe after clearing selection
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

            // Clear input fields and hide buttons
            ClearFields();
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

            if (!isUpdateMode)
            {
                // Add new
                string query = "INSERT INTO Alumni (FullName, GraduationYear, Email, Phone, PhotoPath) VALUES (@Name,@Year,@Email,@Phone,@PhotoPath)";
                DatabaseHelper.ExecuteCommand(query,
                    new SqlParameter("@Name", txtName.Text),
                    new SqlParameter("@Year", int.Parse(txtYear.Text)),
                    new SqlParameter("@Email", txtEmail.Text),
                    new SqlParameter("@Phone", txtPhone.Text),
                    new SqlParameter("@PhotoPath", destPath)
                );
                ShowSnackbar($"{txtName.Text} has been added.");
            }
            else
            {
                // Update
                string query = "UPDATE Alumni SET FullName=@Name, GraduationYear=@Year, Email=@Email, Phone=@Phone, PhotoPath=@PhotoPath WHERE AlumniID=@ID";
                DatabaseHelper.ExecuteCommand(query,
                    new SqlParameter("@Name", txtName.Text),
                    new SqlParameter("@Year", int.Parse(txtYear.Text)),
                    new SqlParameter("@Email", txtEmail.Text),
                    new SqlParameter("@Phone", txtPhone.Text),
                    new SqlParameter("@PhotoPath", string.IsNullOrEmpty(destPath) ? dataGridView1.CurrentRow.Cells["PhotoPath"].Value.ToString() : destPath),
                    new SqlParameter("@ID", selectedAlumniId)
                );
                ShowSnackbar($"{txtName.Text} has been updated.");
            }

            LoadAlumni();
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

            string name = dataGridView1.CurrentRow.Cells["FullName"].Value.ToString();
            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["AlumniID"].Value);

            var result = MessageBox.Show($"Do you want to delete {name}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM Alumni WHERE AlumniID=@ID";
                DatabaseHelper.ExecuteCommand(query, new SqlParameter("@ID", id));
                ShowSnackbar($"{name} has been deleted.");
                LoadAlumni();
                ClearFields();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (isFormLoading) return;
            if (dataGridView1.CurrentRow == null)
            {
                btnDelete.Visible = false;
                return;
            }

            btnDelete.Visible = true;
            btnCancel.Visible = true;
            isUpdateMode = true;
            selectedAlumniId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["AlumniID"].Value);
            btnAdd.Text = "Update";

            // Fill fields
            txtName.Text = dataGridView1.CurrentRow.Cells["FullName"].Value.ToString();
            txtYear.Text = dataGridView1.CurrentRow.Cells["GraduationYear"].Value.ToString();
            txtEmail.Text = dataGridView1.CurrentRow.Cells["Email"].Value.ToString();
            txtPhone.Text = dataGridView1.CurrentRow.Cells["Phone"].Value.ToString();

            string path = dataGridView1.CurrentRow.Cells["PhotoPath"].Value.ToString();
            picPhoto.Image = (!string.IsNullOrEmpty(path) && File.Exists(path)) ? new Bitmap(path) : null;

            // Update Clear button visibility
            TextBoxes_TextChanged(null, null);

            // Photo fit
            DataGridViewImageColumn imgCol = dataGridView1.Columns["PhotoImage"] as DataGridViewImageColumn;
            if (imgCol != null)
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;

        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.webp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedPhotoPath = ofd.FileName;
                    picPhoto.ImageLocation = selectedPhotoPath;
                    TextBoxes_TextChanged(null, null); // update Clear button visibility
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
            isUpdateMode = false;
            selectedAlumniId = -1;
            btnAdd.Text = "Add";
            btnCancel.Visible = false;
            btnDelete.Visible = false;

            // Temporarily unsubscribe to prevent firing SelectionChanged
            dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;
            dataGridView1.ClearSelection();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

            // Update clear button visibility
            TextBoxes_TextChanged(null, null);
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;
            ClearFields();
            dataGridView1.ClearSelection();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

        }

        private async void ShowSnackbar(string message)
        {
            lblSnackbar.Text = message;
            lblSnackbar.Visible = true;
            await Task.Delay(2000); // show 2 seconds
            lblSnackbar.Visible = false;
        }

        private void TextBoxes_TextChanged(object sender, EventArgs e)
        {
            // Show Clear button if any field is filled or photo is set
            btnClear.Visible = !string.IsNullOrWhiteSpace(txtName.Text) ||
                               !string.IsNullOrWhiteSpace(txtYear.Text) ||
                               !string.IsNullOrWhiteSpace(txtEmail.Text) ||
                               !string.IsNullOrWhiteSpace(txtPhone.Text) ||
                               picPhoto.Image != null;
        }



    }
}
