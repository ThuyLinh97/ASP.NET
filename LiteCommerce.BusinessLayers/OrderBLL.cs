using LiteCommerce.DataLayers;
using LiteCommerce.DataLayers.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;

namespace LiteCommerce.BusinessLayers
{
    public static class OrderBLL
    {
        public static void Initialize(string connectionString)
        {
            OrderDB = new DataLayers.SqlServer.OrderDAL(connectionString);
            EmployeeDB = new DataLayers.SqlServer.EmployeeDAL(connectionString);
            CustomerDB = new DataLayers.SqlServer.CustomerDAL(connectionString);
            ShiperDB= new DataLayers.SqlServer.ShipperDAL(connectionString);
        }
        private static ICustomerDAL CustomerDB { get; set; }
        private static IEmployeeDAL EmployeeDB { get; set; }
        private static IOrderDAL OrderDB { get; set; }
        private static IShipperDAL ShiperDB { get; set; }
        /// <summary>
        /// Hiển thị list Employee theo ID
        /// </summary>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees()
        {
            return EmployeeDB.List(1, -1, "");
        }
        /// <summary>
        /// Hiển thị list Customer theo ID
        /// </summary>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers()
        {
            return CustomerDB.List(1, -1, "", "");
        }
        /// <summary>
        /// Hiển thị list shipper theo ID
        /// </summary>
        /// <returns></returns>
        public static List<Shipper> ListOfShipper()
        {
            return ShiperDB.List(1, -1, "");
        }
        public static List<Order> ListOfOrders(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 20;
            rowCount = OrderDB.Count(searchValue);
            return OrderDB.List(page, pageSize, searchValue);
        }
        /// <summary>
        /// lấy thông tin 1 Order dựa vào id
        /// </summary>
        /// <param name="OrderName"></param>
        /// <returns></returns>
        public static Order GetOrder(int orderID)
        {
            return OrderDB.Get(orderID);
        }
        /// <summary>
        /// Thêm 1 Order
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddOrder(Order data)
        {
            return OrderDB.Add(data);
        }
        /// <summary>
        /// update 1 Order
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateOrder(Order data)
        {
            return OrderDB.Update(data);
        }
        /// <summary>
        /// xóa nhiều orderNames dựa vào id
        /// </summary>
        /// <param name="OrderIDs"></param>
        /// <returns></returns>
        public static int DeleteOrder(int[] orderIDs)
        {
            return OrderDB.Delete(orderIDs);
        }
    }
}
