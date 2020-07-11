using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers.SqlServer
{
   public class CountryDAL : ICountryDAL
    {

        private string connectionString;
        public CountryDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<Country> List()
        {
            List<Country> data = new List<Country>();
         
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //tao lenh thuc thi truy van du lieu
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from Countries";
                                    
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
               
                // thuc thi truy van tra ve bien dbReader
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new Country()
                        {                        
                            CountryName = Convert.ToString(dbReader["CountryName"])                       
                        });
                    }
                }
                connection.Close();
            }
            return data;
        }
        public string Add(Country data)
        {
            string countryName = "";
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = @"INSERT INTO Countries
                                          (
                                              CountryName
	                                          
                                          )
                                          VALUES
                                          (
                                              @CountryName
	                                         
                                          );
                                         ";
                //cho biết id vừa sinh ra :Identity-trả về 1 dòng 1 cột
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@countryName", data.CountryName);

                countryName = Convert.ToString(cmd.ExecuteScalar());

                connection.Close();
            }

            return countryName;
        }
        /// <summary>
        /// 
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
                cmd.CommandText = @"select COUNT (*) from Countries where (@searchValue =N'' or CountryName like @searchValue)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            //TODO: dem so luong trang
            return count;
        }
        public int Delete(string[] countryNames)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Countries
                                            WHERE(CountryName = @countryName)
                                              AND(CountryName NOT IN(SELECT Country FROM Customers))
                                              AND(CountryName NOT IN(SELECT Country FROM Suppliers))
                                              AND(CountryName NOT IN(SELECT Country FROM Employees))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@countryName", SqlDbType.Char);
                foreach (var countryName in countryNames)
                {
                    cmd.Parameters["@countryName"].Value = countryName;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }


        public Country Get(string countryName)
        {
            Country data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Countries WHERE CountryName = @countryName";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@countryName", countryName);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Country()
                        {
                            CountryName = Convert.ToString(dbReader["CountryName"])
                           
                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        public List<Country> List(int page, int pageSize, string searchValue)
        {
            List<Country> data = new List<Country>();
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
		                                (select ROW_NUMBER() over(order by CountryName) as RowNumber, Countries.*
			                                from Countries
	                                where (@searchValue =N'') or (CountryName like @searchValue)
                                    ) as t
                                where t.RowNumber between  (@page -1)* @pageSize+1 and @page * @pageSize order by t.RowNumber ";
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
                        data.Add(new Country()
                        {
                            CountryName = Convert.ToString(dbReader["CountryName"]),
                          
                        });
                    }

                }
                connection.Close();
            }
            return data;
        }

        public bool Update(Country data,string id)
        {

            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Countries
                                           SET CountryName = @id
                                              
                                          WHERE CountryName = @CountryName";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
               
                cmd.Parameters.AddWithValue("@id", data.CountryName);
                cmd.Parameters.AddWithValue("@CountryName",id);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}
