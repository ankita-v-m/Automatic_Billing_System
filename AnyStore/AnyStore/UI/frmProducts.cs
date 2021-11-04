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
    public partial class frmProducts : Form
    {
        public frmProducts()
        {
            InitializeComponent();
        }

        productsBLL p = new productsBLL();
        productsDAL dal = new productsDAL();
        userDAL udal = new userDAL();


        private void frmProducts_Load(object sender, EventArgs e)
        {
            DataTable dt = dal.Select();
            dgvProducts.DataSource = dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            p.name = txtName.Text;
            p.category = cmbCategory.Text;
            p.description = txtDescription.Text;
            p.rate = decimal.Parse(txtRate.Text);
            p.added_date = DateTime.Now;

            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);

            p.added_by = usr.Id;

            bool success = dal.Insert(p);
            if (success == true)
            {
                MessageBox.Show("New product inserted successfully");
                clear();
            }
            else
            {
                MessageBox.Show("Failed to add new product");
            }
            DataTable dt = dal.Select();
            dgvProducts.DataSource = dt;
        }

        private void clear()
        {

            txtId.Text = "";
            txtName.Text = "";
            cmbCategory.Text = "";
            txtDescription.Text = "";
            txtRate.Text = "";
            txtSearch.Text = "";
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtSearch.Text;

            if (keywords != null)
            {
                DataTable dt = dal.Search(keywords);
                dgvProducts.DataSource = dt;
            }
            else
            {
                DataTable dt = dal.Select();
                dgvProducts.DataSource = dt;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void dgvProducts_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int RowIndex = e.RowIndex;
            txtId.Text = dgvProducts.Rows[RowIndex].Cells[0].Value.ToString();
            txtName.Text = dgvProducts.Rows[RowIndex].Cells[1].Value.ToString();
            cmbCategory.Text = dgvProducts.Rows[RowIndex].Cells[2].Value.ToString();
            txtDescription.Text = dgvProducts.Rows[RowIndex].Cells[3].Value.ToString();
            txtRate.Text = dgvProducts.Rows[RowIndex].Cells[4].Value.ToString();
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            p.Id = int.Parse(txtId.Text);
            p.name = txtName.Text;
            p.category = cmbCategory.Text;
            p.description = txtDescription.Text;
            p.rate = decimal.Parse(txtRate.Text);
            p.added_date = DateTime.Now;

            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);

            p.added_by = usr.Id;

            bool success = dal.Update(p);
            if (success == true)
            {
                MessageBox.Show("Data Updated successfully");
                clear();
                DataTable dt = dal.Select();
                dgvProducts.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed to update products");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            p.Id = int.Parse(txtId.Text);

            bool success = dal.Delete(p);
            if (success == true)
            {
                MessageBox.Show("Product successfully deleted");
                clear();
            }
            else
            {
                MessageBox.Show("Failed to delete product");
            }
            DataTable dt = dal.Select();
            dgvProducts.DataSource = dt;
        }
    }
}
