using PagingWPFDataGrid.DAO;
using PagingWPFDataGrid.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PagingWPFDataGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int numberOfRecPerPage; //Initialize our Variable, Classes and the List

        static Paging PagedTable = new Paging();

        static ProductModel ProductList = new ProductModel();

        IList<ProductModel.Product> myList = ProductList.GetData();

        public MainWindow()
        {
            InitializeComponent();

            PagedTable.PageIndex = 1; //Sets the Initial Index to a default value

            int[] RecordsToShow = { 10, 20, 30, 50, 100 }; //This Array can be any number groups

            foreach (int RecordGroup in RecordsToShow)
            {
                NumberOfRecords.Items.Add(RecordGroup); //Fill the ComboBox with the Array
            }

            NumberOfRecords.SelectedItem = 10; //Initialize the ComboBox

            numberOfRecPerPage = Convert.ToInt32(NumberOfRecords.SelectedItem); //Convert the Combox Output to type int

            DataTable firstTable = PagedTable.SetPaging(myList, numberOfRecPerPage); //Fill a DataTable with the First set based on the numberOfRecPerPage

            dataGrid.ItemsSource = firstTable.DefaultView; //Fill the dataGrid with the DataTable created previously
        }

        /// <summary>
        /// Determines the shown number of records and returns that as a string
        /// </summary>
        /// <returns>string Number of Records Showing</returns>
        public string PageNumberDisplay()
        {
            myList = ProductList.GetData();
            int PagedNumber = numberOfRecPerPage * (PagedTable.PageIndex + 1);
            if (PagedNumber > myList.Count)
            {
                PagedNumber = myList.Count;
            }
            return "Showing " + PagedNumber + " of " + myList.Count; //This dramatically reduced the number of times I had to write this string statement
        }

        private void Forward_Click(object sender, RoutedEventArgs e)    //For each of these you call the direction you want and pass in the List and ComboBox output
        {
            myList = ProductList.GetData();//and use the above function to output the Record number to the Label
            dataGrid.ItemsSource = PagedTable.Next(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void Backwards_Click(object sender, RoutedEventArgs e)
        {
            myList = ProductList.GetData();
            dataGrid.ItemsSource = PagedTable.Previous(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void NumberOfRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)  //I couldn't get this function to update in place (if the grid showed 20 and I selected 100 it would jump to 200)
        {                                                                                          //So instead I had it call the First function and that does an acceptable job.
            myList = ProductList.GetData();
            numberOfRecPerPage = Convert.ToInt32(NumberOfRecords.SelectedItem);
            dataGrid.ItemsSource = PagedTable.First(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void First_Click(object sender, RoutedEventArgs e)
        {
            myList = ProductList.GetData();
            dataGrid.ItemsSource = PagedTable.First(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void Last_Click(object sender, RoutedEventArgs e)
        {
            myList = ProductList.GetData();
            dataGrid.ItemsSource = PagedTable.Last(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }
        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            if (parent == null)
                return null;

            T parentT = parent as T;
            return parentT ?? FindVisualParent<T>(parent);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridRow clickedRow = FindVisualParent<DataGridRow>((DependencyObject)e.OriginalSource);
            if (clickedRow != null)
            {
                int rowIndex = dataGrid.Items.IndexOf(clickedRow.Item);
                #region Remove Row form DataBase 
                if (clickedRow.Item is DataRowView rowView)
                {
                    object _Id = rowView["ID"];
                    object _ProductName = rowView["ProductName"];
                    MessageBoxResult dialogRes = MessageBox.Show("Bạn có muốn xóa Sản Phẩm " + _ProductName + " không?", "Xóa Sản Phẩm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (MessageBoxResult.Yes == dialogRes)
                    {
                        DataProvider.Instance.ExecuteNonQuery("delete from Prices where IdProduct = " + _Id);
                        DataProvider.Instance.ExecuteNonQuery("delete from Product where Id = " + _Id);
                    }
                }
                #endregion
                #region Remove Row form UI 
                myList.RemoveAt(rowIndex);
                dataGrid.ItemsSource = PagedTable.First(myList, numberOfRecPerPage).DefaultView;
                PageInfo.Content = PageNumberDisplay();
                #endregion
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            AddProduct wd = new AddProduct();
            wd.ShowDialog();
            NapListViewProduct();

            myList = ProductList.GetData();
            dataGrid.ItemsSource = PagedTable.First(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridRow clickedRow = FindVisualParent<DataGridRow>((DependencyObject)e.OriginalSource);
            if (clickedRow != null)
            {
                if (clickedRow.Item is DataRowView rowView)
                {
                    frmUpdateProduct wd = new frmUpdateProduct(rowView);
                    wd.ShowDialog();
                    NapListViewProduct();
                }
                myList = ProductList.GetData();
                dataGrid.ItemsSource = PagedTable.First(myList, numberOfRecPerPage).DefaultView;
                PageInfo.Content = PageNumberDisplay();
            }
        }
        private List<Products> NapListViewProduct()
        {
            DataTable data = DataProvider.Instance.ExecuteQuery(@"select 
                                                                pd.Id,
                                                                ut.Id as idUnit,
                                                                prt.Id as idProductType,
                                                                pd.Name as ProductName,
                                                                prt.Name as ProductType,
                                                                ut.Name as Unit,
                                                                pd.QuantityInStock,
                                                                pd.DescripTions,
                                                                pd.IsActive,
                                                                pd.Created_Date,
                                                                pd.Modify_Date,
                                                                pd.Created_By,
                                                                pd.Modify_By 
                                                                from Product pd 
                                                                join Unit ut on pd.IdUnit = ut.Id 
                                                                join ProductType prt on pd.IdProductType = prt.Id 
                                                                order by pd.Created_Date DESC");
            List<Products> items = new List<Products>();
            foreach (DataRow row in data.Rows)
            {
                items.Add(new Products(row));
            }
            return items;
        }

        private void ReloadDataGrid(object sender, System.Windows.Input.KeyEventArgs e)
        {
            myList = ProductList.GetData();
            dataGrid.ItemsSource = PagedTable.First(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void UpdateUnit(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteUnit(object sender, RoutedEventArgs e)
        {

        }

        private void AddUnit(object sender, RoutedEventArgs e)
        {

        }
    }
}
