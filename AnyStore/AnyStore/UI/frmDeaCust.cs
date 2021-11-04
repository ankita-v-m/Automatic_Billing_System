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
    public partial class frmDeaCust : Form
    {
        public frmDeaCust()
        {
            InitializeComponent();
        }

        DeaCustBLL d = new DeaCustBLL();
        DeaCustDAL dal = new DeaCustDAL();
        userDAL udal = new userDAL();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            d.type = cmbType.Text;
            d.name = txtName.Text;
            d.email = txtEmail.Text;
            d.contact = txtContact.Text;
            d.address = txtAddress.Text;
            d.added_date = DateTime.Now;

            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);

            d.added_by = usr.Id;

            bool success = dal.Insert(d);
            if (success == true)
            {
                MessageBox.Show("New DeaCust inserted successfully");
                clear();
            }
            else
            {
                MessageBox.Show("Failed to add new DeaCust");
            }
            DataTable dt = dal.Select();
            dgvDeaCust.DataSource = dt;

        }

        private void clear()
        {
            txtDeaCustId.Text = "";
            cmbType.Text = "";
            txtName.Text = "";
            txtEmail.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
            txtSearch.Text = "";
        }

        private void frmDeaCust_Load(object sender, EventArgs e)
        {
            DataTable dt = dal.Select();
            dgvDeaCust.DataSource = dt;

        }

        private void dgvDeaCust_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int RowIndex = e.RowIndex;
            txtDeaCustId.Text = dgvDeaCust.Rows[RowIndex].Cells[0].Value.ToString();
            cmbType.Text = dgvDeaCust.Rows[RowIndex].Cells[1].Value.ToString();
            txtName.Text = dgvDeaCust.Rows[RowIndex].Cells[2].Value.ToString();
            txtEmail.Text = dgvDeaCust.Rows[RowIndex].Cells[3].Value.ToString();
            txtContact.Text = dgvDeaCust.Rows[RowIndex].Cells[4].Value.ToString();
            txtAddress.Text = dgvDeaCust.Rows[RowIndex].Cells[5].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            d.Id = int.Parse(txtDeaCustId.Text);
            d.type = cmbType.Text;
            d.name = txtName.Text;
            d.email = txtEmail.Text;
            d.contact = txtContact.Text;
            d.address = txtAddress.Text;
            d.added_date = DateTime.Now;

            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);

            d.added_by = usr.Id;

            bool success = dal.Update(d);
            if (success == true)
            {
                MessageBox.Show("Data Updated successfully");
                clear();
                DataTable dt = dal.Select();
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed to update DeaCust");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            d.Id = int.Parse(txtDeaCustId.Text);

            bool success = dal.Delete(d);
            if (success == true)
            {
                MessageBox.Show("DeaCust successfully deleted");
                clear();
            }
            else
            {
                MessageBox.Show("Failed to delete DeaCust");
            }
            DataTable dt = dal.Select();
            dgvDeaCust.DataSource = dt;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtSearch.Text;

            if (keywords != null)
            {
                DataTable dt = dal.Search(keywords);
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                DataTable dt = dal.Select();
                dgvDeaCust.DataSource = dt;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
