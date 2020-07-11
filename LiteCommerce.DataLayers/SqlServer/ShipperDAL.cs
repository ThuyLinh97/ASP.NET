using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class ShipperDAL : IShipperDAL
    {
        private string connectionString;

        public ShipperDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Add(Shipper data)
        {

            int shipperId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Shippers
                                          (
	                                          CompanyName,
	                                        
	                                          Phone
	                                         
                                          )
                                          VALUES
                                          (
	                                          @CompanyName,
	                                         
	                                          @Phone
	                                          
                                          );
                                          SELECT @@IDENTITY;";
                //cho biết id vừa sinh ra :Identity-trả về 1 dòng 1 cột
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@CompanyName", data.CompanyName);
             
                cmd.Parameters.AddWithValue("@Phone", data.Phone);
               

                shipperId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }

            return shipperId;
        }

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
                cmd.CommandText = @"select COUNT (*) from Shippers where @searchValue =N'' or CompanyName like @searchValue";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            //TODO: dem so luong trang
            return count;
        }

        public int Delete(int[] shipperIDs)
        {

            {
                int countDeleted = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = @"DELETE FROM Shippers
                                            WHERE(ShipperID = @shipperId)
                                              AND(ShipperID NOT IN(SELECT ShipperID FROM Orders))";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.Add("@shipperId", SqlDbType.Int);
                    foreach (int shipperId in shipperIDs)
                    {
                        cmd.Parameters["@shipperId"].Value = shipperId;
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                            countDeleted += 1;
                    }

                    connection.Close();
                }
                return countDeleted;
            }
        }

        public Shipper Get(int shipperID)
        {

            {
                Shipper data = null;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = @"SELECT * FROM Shippers WHERE ShipperID = @shipperID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@shipperID", shipperID);

                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (dbReader.Read())
                        {
                            data = new Shipper()
                            {
                                ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                                CompanyName = Convert.ToString(dbReader["CompanyName"]),
                                Phone = Convert.ToString(dbReader["Phone"])
                               
                            };
                        }
                    }

                    connection.Close();
                }
                return data;
            }
        }

        public List<Shipper> List(int page, int pageSize, string searchValue)
        {
            List<Shipper> data = new List<Shipper>();
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
		                                (select ROW_NUMBER() over(order by CompanyName) as RowNumber, Shippers.*
			                                from Shippers
	                                where	(@searchValue =N'') or (CompanyName like @searchValue) )as t
                                where (@pageSize =-1) or t.RowNumber between  (@page -1)* @pageSize+1 and @page * @pageSize order by t.RowNumber ";
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
                        data.Add(new Shipper()
                        {
                            ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                            CompanyName = Convert.ToString(dbReader["CompanyName"]),
                            Phone = Convert.ToString(dbReader["Phone"])

                        });
                    }

                }
                connection.Close();
            }
            return data;
        }

        public bool Update(Shipper data)
        {

            {
                int rowsAffected = 0;
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = @"UPDATE Shippers
                                           SET CompanyName = @CompanyName ,
                                              Phone = @Phone
                                              
                                          WHERE ShipperID = @ShipperID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);
                    cmd.Parameters.AddWithValue("@CompanyName", data.CompanyName);              
                    cmd.Parameters.AddWithValue("@Phone", data.Phone);
                   

                    rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                    connection.Close();
                }

                return rowsAffected > 0;
            }
        }
    }
}
