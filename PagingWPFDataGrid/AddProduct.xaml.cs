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
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        public AddProduct()
        {
            InitializeComponent();
        }
        private void btnCancelProduct(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddProduct(object sender, RoutedEventArgs e)
        {
            
            // Kiểm tra rỗng
            if (cbUnit.Text == "" || cbProductType.Text == "" || txtPriceSingle.Text == ""||
                txtPriceSingle.Text == ""|| cbIsActive.Text == "" || txtQuantityInStock.Text == "")
            {
                MessageBox.Show("Các trường dấu sao không được bỏ trống!");
            }
            else
            {
                if (dtpCreateDate.Text == "")
                    dtpCreateDate.Text = DateTime.Now.ToString();
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
                DataProvider.Instance.ExecuteNonQuery(@"INSERT INTO Product(
                                                IdUnit,
                                                IdProductType,
                                                Name,
                                                QuantityInStock,
                                                DescripTions,
                                                IsActive,
                                                Created_Date)
                                    VALUES (N'" + updateIdDonVi + 
                                    "', N'" + updateIdProductType + 
                                    "', N'" + txtProductName.Text + 
                                    "'," + txtQuantityInStock.Text + 
                                    ",N'" + txtDescripTions.Text + 
                                    "'," + cbIsActive.Text + 
                                    ", '" + Convert.ToDateTime(dtpCreateDate.Text).ToString("yyyy-MM-dd HH:mm:ss") + 
                                    "')");
                // Tìm
                DataTable idProductMax = DataProvider.Instance.ExecuteQuery("Select Max(Id) as Id from Product");
                DataRow rowidProductMax = idProductMax.Rows[0];
                int updateidProductMax = (int)rowidProductMax["Id"];
                // Tìm
                DataTable idIdUnit_IdProductType = DataProvider.Instance.ExecuteQuery("Select IdUnit,IdProductType from Product where Id = "+ updateidProductMax);
                DataRow rowidIdUnit_IdProductType = idIdUnit_IdProductType.Rows[0];
                int _IdUnit= (int)rowidIdUnit_IdProductType["IdUnit"];
                int _IdProductType = (int)rowidIdUnit_IdProductType["IdProductType"];
                DataProvider.Instance.ExecuteNonQuery(@"Insert into 
                                                   Prices(PriceSingle,IdUnit,IdProductType,IdProduct,Created_Date)
                                          VALUES (
                                          " + txtPriceSingle.Text +
                                          ", "+ _IdUnit +
                                          ", "+ _IdProductType +
                                          ", " + updateidProductMax +
                                          ", '" + Convert.ToDateTime(dtpCreateDate.Text).ToString("yyyy/MM/dd") +
                                          "')");
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
        }
    }
}
