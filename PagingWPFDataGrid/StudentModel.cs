using PagingWPFDataGrid.DAO;
using PagingWPFDataGrid.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagingWPFDataGrid
{
    /// <summary>
    /// Generic Class Model Adopted from https://www.codeproject.com/Articles/1092189/WPF-Pagination-for-DataGrid
    /// </summary>
    class ProductModel
    {
        public class Product
        {
            public int Id { get; set; }
            public int IdUnit { get; set; }
            public int IdProductType { get; set; }
            public string ProductName { get; set; }
            public string ProductType { get; set; }
            public string Unit { get; set; }
            public int QuantityInStock { get; set; }
            public string DescripTions { get; set; }
            public bool IsActive { get; set; }
            public DateTime Created_Date { get; set; }
            public DateTime Modify_Date { get; set ; }
            public string Created_By { get; set; }
            public string Modify_By { get; set; }
        }


        public IList<Product> GetData()
        {
            List<Product> genericList = new List<Product>();
            Product studentObj;
            List<Products> products = new List<Products>();
            products.AddRange(NapListViewProduct());
            var _ArrProduct = products.ToArray();
            var _lenght = _ArrProduct.Length;
            for (int i = 0; i < _lenght; i++) //You can make this number anything you can think of (and your processor can handle).
            {
                studentObj = new Product
                {
                    Id = _ArrProduct[i].Id,
                    IdUnit = _ArrProduct[i].IdUnit,
                    IdProductType = _ArrProduct[i].IdProductType,
                    ProductName = _ArrProduct[i].ProductName,
                    ProductType = _ArrProduct[i].ProductType,
                    Unit = _ArrProduct[i].Unit,
                    QuantityInStock = _ArrProduct[i].QuantityInStock,
                    DescripTions = _ArrProduct[i].DescripTions,
                    IsActive = _ArrProduct[i].IsActive,
                    Created_Date = _ArrProduct[i].Created_Date,
                    Modify_Date = _ArrProduct[i].Modify_Date,
                    Created_By = _ArrProduct[i].Created_By,
                    Modify_By = _ArrProduct[i].Modify_By
                };

                genericList.Add(studentObj);

            }
            return genericList;
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
    }
}
