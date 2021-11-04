using AnyStore.BLL;
using AnyStore.DAL;
using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmPurchaseAndSales : Form
    {
        public frmPurchaseAndSales()
        {
            InitializeComponent();
        }

        DeaCustDAL dcDAL = new DeaCustDAL();
        productsDAL pDAL = new productsDAL();
        userDAL uDAL = new userDAL();
        transactionDAL tDAL = new transactionDAL();
        transactionDetailDAL tdDAL = new transactionDetailDAL();

        DataTable transactionDT = new DataTable();
       
        private void frmPurchaseAndSales_Load(object sender, EventArgs e)
        {
            string type = frmUserDashboard.transactionType;
            lblTop.Text = type;

            transactionDT.Columns.Add("Product Name");
            transactionDT.Columns.Add("Rate");
            transactionDT.Columns.Add("Quantity");
            transactionDT.Columns.Add("Total");
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text;

            if (keyword == "")
            {
                txtName.Text = "";
                txtEmail.Text = "";
                txtContact.Text = "";
                txtAddress.Text = "";
                return;
            }

            DeaCustBLL dc = dcDAL.SearchDealerCustomerForTransaction(keyword);

            txtName.Text = dc.name;
            txtEmail.Text = dc.email;
            txtContact.Text = dc.contact;
            txtAddress.Text = dc.address;
        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearchProduct.Text;

            if (keyword == "")
            {
                txtProductName.Text = "";
                txtInventory.Text = "";
                txtRate.Text = "";
                txtQty.Text = "";
                return;
            }

            productsBLL p = pDAL.SearchProductForTransaction(keyword);

            txtProductName.Text = p.name;
            txtInventory.Text = p.qty.ToString();
            txtRate.Text = p.rate.ToString();            
        }

        private void btnProductAdd_Click(object sender, EventArgs e)
        {
            string productName = txtProductName.Text;
            decimal Rate = decimal.Parse(txtRate.Text);
            decimal Qty = decimal.Parse(txtQty.Text);

            decimal Total = Rate * Qty;

            decimal subTotal = decimal.Parse(txtSubTotal.Text);
            subTotal = subTotal + Total;

            if (productName == "")
            {
                MessageBox.Show("Select the product first.");
            }
            else
            {
                transactionDT.Rows.Add(productName, Rate, Qty,Total);

                dgvAddedProducts.DataSource = transactionDT;

                txtSubTotal.Text = subTotal.ToString();

                txtSearchProduct.Text = "";
                txtProductName.Text = "";
                txtInventory.Text = "0.00";
                txtRate.Text = "0.00";
                txtQty.Text = "0.00";

            }

        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            string value = txtDiscount.Text;

            if(value=="")
            {
                MessageBox.Show("Add discount first.");
            }
            else
            {
                decimal subTotal = decimal.Parse(txtSubTotal.Text);
                decimal discount = decimal.Parse(txtDiscount.Text);

                decimal grandTotal = ((100 - discount) / 100) * subTotal;

                txtGrandTotal.Text = grandTotal.ToString();
            }            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            transactionSBLL transaction = new transactionSBLL();

            transaction.type = lblTop.Text;

            string deaCustName = txtName.Text;
            DeaCustBLL dc = dcDAL.GetDeaCustIDFromName(deaCustName);

            transaction.dea_cust_id = dc.Id;
            transaction.grandTotal = Math.Round(decimal.Parse(txtGrandTotal.Text),2);
            transaction.transaction_date = DateTime.Now;
            transaction.discount = decimal.Parse(txtDiscount.Text);

            string username = frmLogin.loggedIn;
            userBLL u = uDAL.GetIDFromUsername(username);

            transaction.added_by = u.Id;
            transaction.transactionDetails = transactionDT;

            bool success = false;

            using (TransactionScope scope = new TransactionScope())
            {
                int transactionID = -1;
                bool w = tDAL.Insert_Transaction(transaction, out transactionID);

                for(int i=0;i<transactionDT.Rows.Count;i++)
                {
                    transactionDetailBLL transactionDetail = new transactionDetailBLL();

                    string ProductName = transactionDT.Rows[i][0].ToString();
                    productsBLL p = pDAL.GetProductIDFromName(ProductName);

                    transactionDetail.product_id = p.Id;
                    transactionDetail.rate = decimal.Parse(transactionDT.Rows[i][1].ToString());
                    transactionDetail.qty = decimal.Parse(transactionDT.Rows[i][2].ToString());
                    transactionDetail.total = Math.Round(decimal.Parse(transactionDT.Rows[i][3].ToString()),2);
                    transactionDetail.dea_cust_id = dc.Id;
                    transactionDetail.added_date = DateTime.Now;
                    transactionDetail.added_by = u.Id;

                    string transactionType = lblTop.Text;

                    bool x=false;
                    if(transactionType=="Purchase")
                    {
                        x = pDAL.IncreaseProduct(transactionDetail.product_id,transactionDetail.qty);
                    }
                    else if(transactionType == "Sales")
                    {
                        x = pDAL.DecreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                    }

                    bool y = tdDAL.InsertTransactionDetail(transactionDetail);
                    success = w && x && y;
                }              
                if (success == true)
                {
                    scope.Complete();

                    DGVPrinter printer = new DGVPrinter();

                    printer.Title = "\r\n\r\n ANYSTORE PVT. LTD \r\n\r\n";
                    printer.SubTitle = "JNEC, BE-CSE-2 \r\n Phone: 56534896368 \r\n\r\n";
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;                    
                    printer.Footer = "Discount: "+ txtDiscount.Text +"% \r\n" + "Grand Total: " +txtGrandTotal.Text +" \r\n" +"Thank You";
                    printer.FooterSpacing = 15;
                    printer.PrintDataGridView(dgvAddedProducts);


                    MessageBox.Show("Transaction completed successfully");

                    dgvAddedProducts.DataSource = null;
                    dgvAddedProducts.Rows.Clear();

                    txtSearch.Text = "";
                    txtName.Text = "";
                    txtEmail.Text = "";
                    txtContact.Text = "";
                    txtAddress.Text = "";
                    txtSearchProduct.Text = "";
                    txtProductName.Text = "";
                    txtInventory.Text = "0";
                    txtRate.Text = "0";
                    txtQty.Text = "0";
                    txtSubTotal.Text = "0";
                    txtDiscount.Text = "0";
                    txtGrandTotal.Text = "0";
                }
                else
                {
                    MessageBox.Show("Transaction failed");
                }
            }
        }
    }
}
