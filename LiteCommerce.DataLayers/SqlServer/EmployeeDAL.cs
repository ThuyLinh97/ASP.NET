using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class EmployeeDAL : IEmployeeDAL
    {
        private string connectionString;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public EmployeeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Add(Employee data)
        {
            {
                int employeeId = 0;
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = @"INSERT INTO Employees
                                          (
	                                          LastName,
	                                          FirstName,
	                                          Title,
                                              BirthDate,
                                              HireDate,
                                              Email,         
	                                          Address,
	                                          City,
	                                          Country,
	                                          HomePhone,
	                                          Notes,
                                              PhotoPath,
                                              Password
                                          )
                                          VALUES
                                          (
                                              @LastName,
	                                          @FirstName,
	                                          @Title,
                                              @BirthDate,
                                              @HireDate,
                                              @Email,         
	                                          @Address,
	                                          @City,
	                                          @Country,
	                                          @HomePhone,
	                                          @Notes,
                                              @PhotoPath,
                                              @Password
                                          
	                                         
                                          );
                                          SELECT @@IDENTITY;";
                    //cho biết id vừa sinh ra :Identity-trả về 1 dòng 1 cột
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@LastName", data.LastName);
                    cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                    cmd.Parameters.AddWithValue("@Title", data.Title);
                    cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
                    cmd.Parameters.AddWithValue("@HireDate", data.HireDate);
                    cmd.Parameters.AddWithValue("@Email", data.Email);
                    cmd.Parameters.AddWithValue("@Address", data.Address);
                    cmd.Parameters.AddWithValue("@City", data.City);
                    cmd.Parameters.AddWithValue("@Country", data.Country);
                    cmd.Parameters.AddWithValue("@HomePhone", data.HomePhone);
                    cmd.Parameters.AddWithValue("@Notes", data.Notes);
                    cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                    cmd.Parameters.AddWithValue("@Password", data.Password);
                   
                    employeeId = Convert.ToInt32(cmd.ExecuteScalar());

                    connection.Close();
                }

                return employeeId;
            }
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
                cmd.CommandText = @"select COUNT (*) from Employees where @searchValue =N'' or LastName like @searchValue";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            //TODO: dem so luong trang
            return count;
        }

        public int Delete(int[] employeeIDs)
        {
            {
                int countDeleted = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = @"DELETE FROM Employees
                                            WHERE(EmployeeID = @employeeId)
                                              AND(EmployeeID NOT IN(SELECT EmployeeID FROM Orders))";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.Add("@employeeId", SqlDbType.Int);
                    foreach (int employeeId in employeeIDs)
                    {
                        cmd.Parameters["@employeeId"].Value = employeeId;
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                            countDeleted += 1;
                    }

                    connection.Close();
                }
                return countDeleted;
            }
        }

        public Employee Get(int employeeID)
        {

            {
                Employee data = null;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = @"SELECT * FROM Employees WHERE EmployeeID = @employeeID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@employeeID", employeeID);

                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (dbReader.Read())
                        {
                            data = new Employee()
                            {
                                EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                                LastName = Convert.ToString(dbReader["LastName"]),
                                FirstName= Convert.ToString(dbReader["FirstName"]),
                                Title = Convert.ToString(dbReader["Title"]),
                                Email = Convert.ToString(dbReader["Email"]),
                                PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                                Password = Convert.ToString(dbReader["Password"]),                   
                                HireDate = Convert.ToDateTime(dbReader["HireDate"]),                   
                                Address = Convert.ToString(dbReader["Address"]),
                                City = Convert.ToString(dbReader["City"]),
                                Country = Convert.ToString(dbReader["Country"]),
                                HomePhone = Convert.ToString(dbReader["HomePhone"]),
                                Notes = Convert.ToString(dbReader["Notes"]),
                                BirthDate = Convert.ToDateTime(dbReader["BirthDate"])
                            };
                        }
                    }

                    connection.Close();
                }
                return data;
            }
        }

        public List<Employee> List(int page, int pageSize, string searchValue)
        {
            List<Employee> data = new List<Employee>();
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
		                                (select ROW_NUMBER() over(order by FirstName) as RowNumber, Employees.*
			                             from Employees
	                                     where	(@searchValue =N'') or (LastName like @searchValue) )
                                     as t
                                where (@pagesize=-1) or t.RowNumber between  (@page -1)* @pageSize+1 and @page * @pageSize order by t.RowNumber ";
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
                        data.Add(new Employee()
                        {
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            LastName = Convert.ToString(dbReader["LastName"]),
                            FirstName = Convert.ToString(dbReader["FirstName"]),
                            Title = Convert.ToString(dbReader["Title"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            HomePhone = Convert.ToString(dbReader["HomePhone"]),
                            BirthDate= Convert.ToDateTime(dbReader["BirthDate"]),
                            HireDate = Convert.ToDateTime(dbReader["HireDate"]),
                            PhotoPath=Convert.ToString(dbReader["PhotoPath"])
                           
                        });
                    }

                }
                connection.Close();
            }
            return data;
        }

        public bool Update(Employee data)
        {  
                int rowsAffected = 0;
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = @"UPDATE Employees
                                           SET LastName = @LastName
                                              ,FirstName = @FirstName
                                              ,Title = @Title
                                              ,Address = @Address
                                              ,City = @City
                                              ,Country = @Country
                                              ,HomePhone = @HomePhone
                                              ,BirthDate = @BirthDate
                                              ,HireDate = @HireDate
                                              ,Email = @Email
                                              ,Notes = @Notes
                                              ,PhotoPath = @PhotoPath
                                              ,Password = @Password
                                          WHERE EmployeeID = @EmployeeID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                    cmd.Parameters.AddWithValue("@LastName", data.LastName);
                    cmd.Parameters.AddWithValue("@FirstName", data.FirstName);
                    cmd.Parameters.AddWithValue("@Title", data.Title);
                    cmd.Parameters.AddWithValue("@Address", data.Address);
                    cmd.Parameters.AddWithValue("@City", data.City);
                    cmd.Parameters.AddWithValue("@Country", data.Country);
                    cmd.Parameters.AddWithValue("@HomePhone", data.HomePhone);
                    cmd.Parameters.AddWithValue("@Notes", data.Notes);
                    cmd.Parameters.AddWithValue("@Email", data.Email);
                    cmd.Parameters.AddWithValue("@Password", data.Password);
                    cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                    cmd.Parameters.AddWithValue("@BirthDate", data.BirthDate);
                    cmd.Parameters.AddWithValue("@HireDate", data.HireDate);

                    rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                    connection.Close();
                }

                return rowsAffected > 0;
            }
        }
}
