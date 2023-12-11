using PagingWPFDataGrid.DAO;
using PagingWPFDataGrid.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PagingWPFDataGrid
{
    /// <summary>
    /// Interaction logic for frmUpdateProduct.xaml
    /// </summary>
    public partial class frmUpdateProduct : Window
    {
        private int idProduct;
        private int idUnit;
        private int idProductType;
        private string _isActive;
        //private decimal priceSingle;
        public frmUpdateProduct()
        {
            InitializeComponent();
        }
        public frmUpdateProduct(DataRowView Item)
        {
            InitializeComponent();
            this.idProduct = Convert.ToInt32(Item["Id"]);
            this.idUnit = Convert.ToInt32(Item["IdUnit"]);
            this.idProductType = Convert.ToInt32(Item["IdProductType"]);
            txtProductName.Text = Convert.ToString(Item["ProductName"]);
            //txtPriceSingle.Text = PriceSingle.ToString();
            this._isActive = Convert.ToString(Item["IsActive"]) == "True"? "1" : "0";
            txtQuantityInStock.Text = Convert.ToString(Item["QuantityInStock"]);
            txtDescripTions.Text = Convert.ToString(Item["DescripTions"]);
        }
        public int IdProduct { get => idProduct; set => idProduct = value; }
        public int IdUnit { get => idUnit; set => idUnit = value; }
        public int IdProductType { get => idProductType; set => idProductType = value; }
        public string IsActives { get => _isActive; set => _isActive = value; }
        //public decimal PriceSingle { get => priceSingle; set => priceSingle = value; }

        private void btnCancelProduct(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnUpdateProduct(object sender, RoutedEventArgs e)
        {
            // Kiểm tra rỗng
            if (cbUnit.Text == "" || cbProductType.Text == "" || txtPriceSingle.Text == "" ||
                txtPriceSingle.Text == "" || cbIsActive.Text == "" || txtQuantityInStock.Text == "")
            {
                MessageBox.Show("Các trường dấu sao không được bỏ trống!");
            }
            else
            {
                if (dtpCreateDate.Text == "")
                    dtpCreateDate.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                #region Get Unit Id
                // Đọc đơn vị được chọn ở combobox
                string cbSelected = cbUnit.SelectedValue.ToString();
                // Tìm
                DataTable idDonVi = DataProvider.Instance.ExecuteQuery("Select Id from Unit Where Name = N'" + cbSelected + "'");
                DataRow rowidDonVi = idDonVi.Rows[0];
                int updateIdDonVi = (int)rowidDonVi["Id"];
                #endregion
                #region Get ProductType Id
                // Đọc đơn vị được chọn ở combobox
                string cbSelectedProductType = cbProductType.SelectedValue.ToString();
                // Tìm
                DataTable idProductype = DataProvider.Instance.ExecuteQuery("Select Id from ProductType Where Name = N'" + cbSelectedProductType + "'");
                DataRow rowidProducttype = idProductype.Rows[0];
                int updateIdProductType = (int)rowidProducttype["Id"];
                #endregion
                #region Update Table Product And Prices
                DataProvider.Instance.ExecuteQuery(@"UPDATE Product SET 
                                                    IdUnit = " + IdUnit +
                                                ",IdProductType =" + updateIdProductType +
                                                ",Name = N'" + txtProductName.Text +
                                                "',QuantityInStock =" + txtQuantityInStock.Text +
                                                ",DescripTions =N'" + txtDescripTions.Text +
                                                "',Modify_Date ='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") +
                                                "' where Id = " + IdProduct
                                                );
                DataProvider.Instance.ExecuteQuery(@"Update Prices SET 
                                                    PriceSingle = 
                                                    " + txtPriceSingle.Text +
                                                    " where IdProduct = " + IdProduct
                                                   );
                #endregion
                this.Close();
            }
        }
        private void LoadDonViTinh(object sender, RoutedEventArgs e)
        {
            DataTable dataComboboxUnit = DataProvider.Instance.ExecuteQuery("Select Id,Name From Unit");
            foreach (DataRow row in dataComboboxUnit.Rows)
            {
                string newAdd = (string)row["Name"];
                cbUnit.Items.Add(newAdd);
            }
            DataTable dataComboboxProductype = DataProvider.Instance.ExecuteQuery("Select Id,Name From ProductType");
            foreach (DataRow row in dataComboboxProductype.Rows)
            {
                string newAdd = (string)row["Name"];
                cbProductType.Items.Add(newAdd);
            }
            cbIsActive.Items.Add(0);
            cbIsActive.Items.Add(1);
            cbIsActive.Text = IsActives;
            DataTable dataPriceSingle = DataProvider.Instance.ExecuteQuery(@"Select Id,PriceSingle 
                                                                        From Prices where IdProduct = "+ IdProduct +
                                                                        " And IdUnit = "+ IdUnit+
                                                                        " And IdProductType = "+ IdProductType);
            foreach (DataRow row in dataPriceSingle.Rows)
            {
                decimal newAdd = (decimal)row["PriceSingle"];
                txtPriceSingle.Text = Convert.ToString(newAdd);
            }
        }

        
    }
}
