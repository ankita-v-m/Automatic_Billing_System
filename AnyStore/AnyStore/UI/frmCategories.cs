using AnyStore.BLL;
using AnyStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmCategories : Form
    {
        public frmCategories()
        {
            InitializeComponent();
        }

        categoriesBLL c = new categoriesBLL();
        categoriesDAL dal = new categoriesDAL();
        userDAL udal = new userDAL();

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
            c.added_date = DateTime.Now;

            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);

            c.added_by = usr.Id;

            bool success = dal.Insert(c);
            if (success == true)
            {
                MessageBox.Show("New category inserted successfully");
                clear();
            }
            else
            {
                MessageBox.Show("Failed to add new category");
            }
            DataTable dt = dal.Select();
            dgvCategories.DataSource = dt;

        }

        private void clear()
        {
            txtId.Text = "";
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtSearch.Text = "";
        }

        private void frmCategories_Load(object sender, EventArgs e)
        {
            DataTable dt = dal.Select();
            dgvCategories.DataSource = dt;
        }

        private void dgvCategories_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int RowIndex = e.RowIndex;
            txtId.Text = dgvCategories.Rows[RowIndex].Cells[0].Value.ToString();
            txtTitle.Text = dgvCategories.Rows[RowIndex].Cells[1].Value.ToString();
            txtDescription.Text = dgvCategories.Rows[RowIndex].Cells[2].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            c.Id = int.Parse(txtId.Text);
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
            c.added_date = DateTime.Now;

            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);

            c.added_by = usr.Id;

            bool success = dal.Update(c);
            if (success == true)
            {
                MessageBox.Show("Data Updated successfully");
                clear();
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;

            }
            else
            {
                MessageBox.Show("Failed to update category");
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            c.Id = int.Parse(txtId.Text);

            bool success = dal.Delete(c);
            if (success == true)
            {
                MessageBox.Show("Category successfully deleted");
                clear();
            }
            else
            {
                MessageBox.Show("Failed to delete category");
            }
            DataTable dt = dal.Select();
            dgvCategories.DataSource = dt;

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtSearch.Text;

            if (keywords != null)
            {
                DataTable dt = dal.Search(keywords);
                dgvCategories.DataSource = dt;
            }
            else
            {
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }

        }
    }
}
