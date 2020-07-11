using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
namespace LiteCommerce.DataLayers.SqlServer
{
    public class OrderDAL : IOrderDAL
    {
        private string connectionString;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public OrderDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Order data)
        {
            int orderId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Orders
                                          (
	                                          CustomerID,
	                                          EmployeeID,
	                                          ShipperID,
	                                          OrderDate,
	                                          RequiredDate,
	                                          ShippedDate,
	                                          Freight,
	                                          ShipAddress,
	                                          ShipCity,
                                              ShipCountry
                                          )
                                          VALUES
                                          (
	                                          @CustomerID,
	                                          @EmployeeID,
	                                          @ShipperID,
	                                          @OrderDate,
	                                          @RequiredDate,
	                                          @ShippedDate,
	                                          @Freight,
	                                          @ShipAddress,
	                                          @ShipCity,
                                              @ShipCountry
                                          );
                                          SELECT @@IDENTITY;";
                //cho biết id vừa sinh ra :Identity-trả về 1 dòng 1 cột
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);
                cmd.Parameters.AddWithValue("@OrderDate", data.OrderDate);
                cmd.Parameters.AddWithValue("@RequiredDate", data.RequiredDate);
                cmd.Parameters.AddWithValue("@ShippedDate", data.ShippedDate);
                cmd.Parameters.AddWithValue("@Freight", data.Freight);
                cmd.Parameters.AddWithValue("@ShipAddress", data.ShipAddress);
                cmd.Parameters.AddWithValue("@ShipCity", data.ShipCity);
                cmd.Parameters.AddWithValue("@ShipCountry", data.ShipCountry);

                orderId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return orderId;
        }
        /// <summary>
        /// Đếm số dòng tìm kiếm đc
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>

        public int Count(string searchValue)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select COUNT (*) from Orders where (@searchValue = N'') or (CustomerID like @searchValue) or (EmployeeID like @searchValue)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            //TODO: dem so luong trang
            return count;
        }

        public int Delete(int[] orderIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Orders
                                            WHERE(OrderID = @orderId)
                                              AND(OrderID NOT IN(SELECT OrderID FROM [OrderDetails]))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@orderId", SqlDbType.Int);
                foreach (int orderId in orderIDs)
                {
                    cmd.Parameters["@orderId"].Value = orderId;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }

        public Order Get(int orderID)
        {
            Order data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Orders WHERE OrderID = @orderID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@orderID", orderID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Order()
                        {
                            OrderID = Convert.ToInt32(dbReader["OrderID"]),
                            CustomerID = Convert.ToString(dbReader["CustomerID"]),
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            Freight = Convert.ToDecimal(dbReader["Freight"]),
                            ShipAddress = Convert.ToString(dbReader["ShipAddress"]),
                            ShipCountry = Convert.ToString(dbReader["ShipCountry"]),
                            ShipCity = Convert.ToString(dbReader["ShipCity"]),
                            OrderDate = Convert.ToDateTime(dbReader["OrderDate"]),
                            RequiredDate = Convert.ToDateTime(dbReader["RequiredDate"]),
                            ShippedDate = Convert.ToDateTime(dbReader["ShippedDate"]),
                            ShipperID=Convert.ToInt32(dbReader["ShipperID"])
                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        public List<Order> List(int page, int pageSize, string searchValue)
        {
            List<Order> data = new List<Order>();
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //tao lenh thuc thi truy van du lieu
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                          from
                                          (
	                                            select	row_number() over(order by OrderID) as RowNumber,
			                                          Orders.*
	                                            from	Orders
	                                            where	(@searchValue = N'') or (CustomerID like @searchValue) or (EmployeeID like @searchValue)
                                          ) as t
                                          where	 (t.RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                                          order by t.RowNumber";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                // thuc thi truy van tra ve bien dbReader
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Order()
                        {
                            OrderID = Convert.ToInt32(dbReader["OrderID"]),
                            CustomerID = Convert.ToString(dbReader["CustomerID"]),
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            Freight = Convert.ToDecimal(dbReader["Freight"]),
                            ShipAddress = Convert.ToString(dbReader["ShipAddress"]),
                            ShipCountry = Convert.ToString(dbReader["ShipCountry"]),
                            ShipCity = Convert.ToString(dbReader["ShipCity"]),
                            OrderDate = Convert.ToDateTime(dbReader["OrderDate"]),
                            RequiredDate = Convert.ToDateTime(dbReader["RequiredDate"]),
                            ShippedDate = Convert.ToDateTime(dbReader["ShippedDate"]),
                            ShipperID = Convert.ToInt32(dbReader["ShipperID"])
                            
                        });
                    }
                }
                connection.Close();
            }
            return data;
        }

        public bool Update(Order data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Orders
                                           SET CustomerID = @CustomerID 
                                              ,EmployeeID = @EmployeeID
                                              ,ShipperID = @ShipperID
                                              ,OrderDate = @OrderDate
                                              ,RequiredDate = @RequiredDate
                                              ,ShippedDate = @ShippedDate
                                              ,Freight = @Freight
                                              ,ShipAddress = @ShipAddress
                                              ,ShipCity = @ShipCity
                                              ,ShipCountry = @ShipCountry
                                          WHERE OrderID = @OrderID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@OrderID", data.OrderID);
                cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);
                cmd.Parameters.AddWithValue("@OrderDate", data.OrderDate);
                cmd.Parameters.AddWithValue("@RequiredDate", data.RequiredDate);
                cmd.Parameters.AddWithValue("@ShippedDate", data.ShippedDate);
                cmd.Parameters.AddWithValue("@Freight", data.Freight);
                cmd.Parameters.AddWithValue("@ShipAddress", data.ShipAddress);
                cmd.Parameters.AddWithValue("@ShipCity", data.ShipCity);
                cmd.Parameters.AddWithValue("@ShipCountry", data.ShipCountry);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}
