using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class CustomerDAL : ICustomerDAL
    {
        private string connectionString;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public CustomerDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Them du lieu, ket qua tra ve la so ban ghi them dc
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        public string Add(Customer data)
        {
            string customerId = "";
            using (SqlConnection connection = new SqlConnection(this.connectionString))
                 {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                
                    cmd.CommandText = @"INSERT INTO Customers
                                          (
                                              CustomerID,
	                                          CompanyName,
	                                          ContactName,
	                                          ContactTitle,
	                                          Address,
	                                          City,
	                                          Country,
	                                          Phone,
	                                          Fax 
                                          )
                                          VALUES
                                          (
                                              @CustomerID,
	                                          @CompanyName,
	                                          @ContactName,
	                                          @ContactTitle,
	                                          @Address,
	                                          @City,
	                                          @Country,
	                                          @Phone,
	                                          @Fax
                                          );
                                        ";
                    //cho biết id vừa sinh ra :Identity-trả về 1 dòng 1 cột
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                    cmd.Parameters.AddWithValue("@CompanyName", data.CompanyName);
                    cmd.Parameters.AddWithValue("@ContactName", data.ContactName);
                    cmd.Parameters.AddWithValue("@ContactTitle", data.ContactTitle);
                    cmd.Parameters.AddWithValue("@Address", data.Address);
                    cmd.Parameters.AddWithValue("@City", data.City);
                    cmd.Parameters.AddWithValue("@Country", data.Country);
                    cmd.Parameters.AddWithValue("@Phone", data.Phone);
                    cmd.Parameters.AddWithValue("@Fax", data.Fax);

                    customerId = Convert.ToString(cmd.ExecuteScalar());

                    connection.Close();
                }
            
            return customerId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>

        public int Count(string searchValue,string country)
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
                cmd.CommandText = @"select COUNT (*) from Customers where (@searchValue =N'' or (CompanyName like @searchValue) )and (@country=N'' or Country like @country)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@country", country);
                count = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            //TODO: dem so luong trang
            return count;
        }

        public int CountID(string id)
        {
            int count = 0;
           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select COUNT (CustomerID) from Customers where ( CustomerID like @id)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
              
                cmd.Parameters.AddWithValue("@id", id);
                count = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            //TODO: dem so luong trang
            return count;
        }

        public int Delete(string[] customerIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Customers
                                            WHERE(CustomerID = @customerId)
                                              AND(CustomerID NOT IN(SELECT CustomerID FROM Orders))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@customerId", SqlDbType.Char);
                foreach (var customerId in customerIDs)
                {
                    cmd.Parameters["@customerId"].Value = customerId;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }


        public Customer Get(string customerID)
        {
            Customer data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Customers WHERE CustomerID = @customerID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@customerID", customerID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Customer()
                        {
                            CustomerID = Convert.ToString(dbReader["CustomerID"]),
                            CompanyName = Convert.ToString(dbReader["CompanyName"]),
                            ContactName = Convert.ToString(dbReader["ContactName"]),
                            ContactTitle = Convert.ToString(dbReader["ContactTitle"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            Phone = Convert.ToString(dbReader["Phone"]),
                            Fax = Convert.ToString(dbReader["Fax"])
                           
                        };
                    }
                }

                connection.Close();
            }
            return data;
        }     

        public List<Customer> List(int page, int pageSize, string searchValue,string country)
        {
            List<Customer> data = new List<Customer>();
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
		                                (select ROW_NUMBER() over(order by CompanyName) as RowNumber, Customers.*
			                                from Customers
	                                where	((@searchValue =N'') or (CompanyName like @searchValue)) and (@country=N'' or Country like @country)
                                    ) as t
                                where (@pagesize=-1)or t.RowNumber between  (@page -1)* @pageSize+1 and @page * @pageSize order by t.RowNumber ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@country", country);
                // thuc thi truy van tra ve bien dbReader
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Customer()
                        {
                            CustomerID = Convert.ToString(dbReader["CustomerID"]),
                            CompanyName = Convert.ToString(dbReader["CompanyName"]),
                            ContactName = Convert.ToString(dbReader["ContactName"]),
                            ContactTitle = Convert.ToString(dbReader["ContactTitle"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),                     
                            Fax = Convert.ToString(dbReader["Fax"]),
                            Phone = Convert.ToString(dbReader["Phone"])

                        });
                    }

                }
                connection.Close();
            }
            return data;
        }

        public bool Update(Customer data)
        {

            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Customers
                                           SET CompanyName = @CompanyName 
                                              ,ContactName = @ContactName
                                              ,ContactTitle = @ContactTitle
                                              ,Address = @Address
                                              ,City = @City
                                              ,Country = @Country
                                              ,Phone = @Phone
                                              ,Fax = @Fax
                                           
                                          WHERE CustomerID = @CustomerID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                cmd.Parameters.AddWithValue("@CompanyName", data.CompanyName);
                cmd.Parameters.AddWithValue("@ContactName", data.ContactName);
                cmd.Parameters.AddWithValue("@ContactTitle", data.ContactTitle);
                cmd.Parameters.AddWithValue("@Address", data.Address);
                cmd.Parameters.AddWithValue("@City", data.City);
                cmd.Parameters.AddWithValue("@Country", data.Country);
                cmd.Parameters.AddWithValue("@Phone", data.Phone);
                cmd.Parameters.AddWithValue("@Fax", data.Fax);
            

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }
            return rowsAffected > 0;
        } 
    }
}
