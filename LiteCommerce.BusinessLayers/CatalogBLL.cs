using LiteCommerce.DataLayers;
using LiteCommerce.DataLayers.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    /// <summary>
    /// cac chuc nang xu li nghiep vu chung cua he thong nhu: ncc, khachhang, mathang
    /// </summary>
    public static class CatalogBLL
    {
        public static void Initialize(string connectionString)
        {
            SupplierDB = new DataLayers.SqlServer.SupplierDAL(connectionString);
            CustomerDB = new DataLayers.SqlServer.CustomerDAL(connectionString);
            ShipperDB = new DataLayers.SqlServer.ShipperDAL(connectionString);
            CategoryDB = new DataLayers.SqlServer.CategoryDAL(connectionString);
            EmployeeDB = new DataLayers.SqlServer.EmployeeDAL(connectionString);
            ProductDB = new DataLayers.SqlServer.ProductDAL(connectionString);
          
        }

        private static ISupplierDAL SupplierDB { get; set; }
        private static ICustomerDAL CustomerDB { get; set; }
        private static IShipperDAL ShipperDB { get; set; }
        private static ICategoryDAL CategoryDB { get; set; }
        private static IEmployeeDAL EmployeeDB { get; set; }
        private static IProductDAL ProductDB { get; set; }
      
        /// <summary>
        /// Product
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Product> ListOfProducts(int page, int pageSize, string searchValue, out int rowCount, int categoryId, int supplierId)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = ProductDB.Count(searchValue, categoryId, supplierId);
            return ProductDB.List(page, pageSize, searchValue, categoryId, supplierId);
        }
        
        /// <summary>
        /// lấy thông tin 1 Product dựa vào id
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public static Product GetProduct(int productID)
        {
            return ProductDB.Get(productID);
        }
        /// <summary>
        /// Thêm 1 Product
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddProduct(Product data)
        {
            return ProductDB.Add(data);
        }
        /// <summary>
        /// update 1 Product
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateProduct(Product data)
        {
            return ProductDB.Update(data);
        }
        /// <summary>
        /// xóa nhiều Product dựa vào id
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public static int DeleteProduct(int[] productIDs)
        {
            return ProductDB.Delete(productIDs);
        }

        /// <summary>
        /// Supplier
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = SupplierDB.Count(searchValue);
            return SupplierDB.List(page, pageSize, searchValue);
        }
        public static List<Supplier> ListOfSuppliers()
        {
            return SupplierDB.List(1, -1, "");
        }
       
        /// <summary>
        /// lấy thông tin 1 supplier dựa vào id
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public static Supplier GetSupplier(int supplierID)
        {
            return SupplierDB.Get(supplierID);
        }
        /// <summary>
        /// Thêm 1 supplier
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddSupplier(Supplier data)
        {
            return SupplierDB.Add(data);
        }
        /// <summary>
        /// update 1 supplier
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateSupplier(Supplier data)
        {
            return SupplierDB.Update(data);
        }
        /// <summary>
        /// xóa nhiều suppliers dựa vào id
        /// </summary>
        /// <param name="supplierIDs"></param>
        /// <returns></returns>
        public static int DeleteSupplier(int[] supplierIDs)
        {
            return SupplierDB.Delete(supplierIDs);
        }


        /// <summary>
        /// Customer
        /// </summary>
        public static List<Customer> ListOfCustomers(int page, int pageSize, string searchValue, out int rowCount,string country)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = CustomerDB.Count(searchValue,country);
            return CustomerDB.List(page, pageSize, searchValue,country);
        }
        /// <summary>
        /// lấy thông tin 1 Customer dựa vào id
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public static Customer GetCustomer(string customerID)
        {
            return CustomerDB.Get(customerID);
        }
        /// <summary>
        /// Thêm 1 Customer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string AddCustomer(Customer data)
        {
            return CustomerDB.Add(data);
        }
        /// <summary>
        /// update 1 Customer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer data)
        {
            return CustomerDB.Update(data);
        }
        /// <summary>
        /// xóa nhiều suppliers dựa vào id
        /// </summary>
        /// <param name="CustomerIDs"></param>
        /// <returns></returns>
        public static int DeleteCustomer(string[] customerIDs)
        {
            return CustomerDB.Delete(customerIDs);
        }





        /// <summary>
        /// shipper
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = ShipperDB.Count(searchValue);
            return ShipperDB.List(page, pageSize, searchValue);
        }
        /// <summary>
        /// lấy thông tin 1 Shipper dựa vào id
        /// </summary>
        /// <param name="ShipperID"></param>
        /// <returns></returns>
        public static Shipper GetShipper(int shipperID)
        {
            return ShipperDB.Get(shipperID);
        }
        /// <summary>
        /// Thêm 1 Shipper
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddShipper(Shipper data)
        {
            return ShipperDB.Add(data);
        }
        /// <summary>
        /// update 1 Shipper
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateShipper(Shipper data)
        {
            return ShipperDB.Update(data);
        }
        /// <summary>
        /// xóa nhiều Shipper dựa vào id
        /// </summary>
        /// <param name="shiperIDs"></param>
        /// <returns></returns>
        public static int DeleteShipper(int[] shipperIDs)
        {
            return ShipperDB.Delete(shipperIDs);
        }





        /// <summary>
        /// Categories
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Category> ListOfCategory(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize <= 0)
                pageSize = 20;
            rowCount = CategoryDB.Count(searchValue);
            return CategoryDB.List(page, pageSize, searchValue);
        }
        public static List<Category> ListOfCategory()
        {
            return CategoryDB.List(1, -1, "");
        }
        /// <summary>
        /// lấy thông tin 1 Category dựa vào id
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        public static Category GetCategory(int categoryID)
        {
            return CategoryDB.Get(categoryID);
        }
        /// <summary>
        /// Thêm 1 Category
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCategory(Category data)
        {
            return CategoryDB.Add(data);
        }
        /// <summary>
        /// update 1 Category
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCategory(Category data)
        {
            return CategoryDB.Update(data);
        }
        /// <summary>
        /// xóa nhiều Categorys dựa vào id
        /// </summary>
        /// <param name="categoryIDs"></param>
        /// <returns></returns>
        public static int DeleteCategory(int[] categoryIDs)
        {
            return CategoryDB.Delete(categoryIDs);
        }

        /// <summary>
        /// Employee
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Employee> ListOfEmployee(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = EmployeeDB.Count(searchValue);
            return EmployeeDB.List(page, pageSize, searchValue);
        }
        /// <summary>
        /// lấy thông tin 1 Employee dựa vào id
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public static Employee GetEmployee(int employeeID)
        {
            return EmployeeDB.Get(employeeID);
        }
        /// <summary>
        /// Thêm 1 Employee
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddEmployee(Employee data)
        {
            return EmployeeDB.Add(data);
        }
        /// <summary>
        /// update 1 Employee
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateEmployee(Employee data)
        {
            return EmployeeDB.Update(data);
        }
        /// <summary>
        /// xóa nhiều Shipper dựa vào id
        /// </summary>
        /// <param name="employeeIDs"></param>
        /// <returns></returns>
        public static int DeleteEmployee(int[] employeeIDs)
        {
            return EmployeeDB.Delete(employeeIDs);
        }
       
    }
}
