using AnyStore.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore
{
    public partial class frmAdminDashboard : Form
    {
        public frmAdminDashboard()
        {
           // this.BackgroundImage = Properties.Resources.trolly;            
            InitializeComponent();
        }

        private void frmAdminDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Hide();
        }

        private void frmAdminDashboard_Load(object sender, EventArgs e)
        {
            lblLoggedInUser.Text = frmLogin.loggedIn;
        }
     
        private void usersToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            frmUsers user = new frmUsers();
            user.Show();
        }

        private void categoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmCategories categories = new frmCategories();
            categories.Show();
        }

        private void productsToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmProducts product = new frmProducts();
            product.Show();
        }

        private void dealerAndCustomerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmDeaCust deacust = new frmDeaCust();
            deacust.Show();
        }

        private void inventoryToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmInventory inventory = new frmInventory();
            inventory.Show();
        }

        private void transactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTransactions transaction = new frmTransactions();
            transaction.Show();
        }

        private void salesFormsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dealerAndCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
