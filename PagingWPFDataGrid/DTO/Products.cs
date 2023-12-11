using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagingWPFDataGrid.DTO
{
    internal class Products
    {
        private int id;
        private int idUnit;
        private int idProductType;
        private string productName;
        private string productType;
        private string unit;
        private int quantityInStock;
        private string descripTions;
        private bool isActive;
        private DateTime created_Date;
        private DateTime modify_Date;
        private string created_By;
        private string modify_By;

        public int Id { get => id; set => id = value; }
        public int IdUnit { get => idUnit; set => idUnit = value; }
        public int IdProductType { get => idProductType; set => idProductType = value; }
        public string ProductName { get => productName; set => productName = value; }
        public string ProductType { get => productType; set => productType = value; }
        public string Unit { get => unit; set => unit = value; }
        public int QuantityInStock { get => quantityInStock; set => quantityInStock = value; }
        public string DescripTions { get => descripTions; set => descripTions = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public DateTime Created_Date { get => created_Date; set => created_Date = value; }
        public DateTime Modify_Date { get => modify_Date; set => modify_Date = value; }
        public string Created_By { get => created_By; set => created_By = value; }
        public string Modify_By { get => modify_By; set => modify_By = value; }

        public Products(int id, int idUnit, int idProductType, string productName, string productType, int quantityInStock, string descripTions, bool isActive,
            DateTime created_Date, DateTime modify_Date, string created_By, string modify_By)
        {
            this.Id = id;
            this.IdUnit = idUnit;
            this.IdProductType = idProductType;
            this.ProductName = productName;
            this.ProductType = productType;
            this.Unit = unit;
            this.QuantityInStock = quantityInStock;
            this.DescripTions = descripTions;
            this.IsActive = isActive;
            this.Created_Date = created_Date;
            this.Modify_Date = modify_Date;
            this.Created_By = created_By;
            this.Modify_By = modify_By;
        }
        public Products(DataRow row)
        {
            this.Id = (int)row["id"];
            this.IdUnit = (int)row["idUnit"];
            this.IdProductType = (int)row["idProductType"];
            this.ProductName = (string)row["productName"];
            this.ProductType = (string)row["productType"];
            this.unit = (string)row["Unit"];
            this.QuantityInStock = (int)row["quantityInStock"];
            this.DescripTions = (string)row["descripTions"];
            this.IsActive = (bool)row["isActive"];
            var _dt = row["created_Date"];
            this.Created_Date = _dt != DBNull.Value ? (DateTime)_dt : DateTime.Now;
            var _mddt = row["modify_Date"];
            this.Modify_Date = _mddt != DBNull.Value ? (DateTime)_mddt : DateTime.Now;
            var _creb = row["created_By"];
            this.Created_By = _creb != DBNull.Value ? (string)_creb : "";
            var _modb = row["modify_By"];
            this.Modify_By = _modb != DBNull.Value ? (string)_modb : "";
        }
    }
}
