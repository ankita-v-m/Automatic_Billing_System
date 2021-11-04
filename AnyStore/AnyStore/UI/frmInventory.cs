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
    public partial class frmInventory : Form
    {
        public frmInventory()
        {
            InitializeComponent();
        }

        categoriesDAL cDAL = new categoriesDAL();
        productsDAL pdal = new productsDAL();

        private void frmInventory_Load(object sender, EventArgs e)
        {
            DataTable cDt = cDAL.Select();

            cmbCategory.DataSource = cDt;

            cmbCategory.DisplayMember = "title";
            cmbCategory.ValueMember = "title";

            DataTable pdt = pdal.Select();
            dgvProducts.DataSource = pdt;
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string category = cmbCategory.Text;

            DataTable dt = pdal.DisplayProductsByCategories(category);
            dgvProducts.DataSource = dt;
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            DataTable dt = pdal.Select();
            dgvProducts.DataSource = dt;
        }
    }
}
