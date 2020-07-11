using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Data.SqlClient;
using System.Data;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class EmployeeUserAccountDAL : IUserAccountDAL
    {
        private string connectionString;

        public EmployeeUserAccountDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public UserAccount Authorize(string userName, string passWord)
        {
            UserAccount data = null;
            //TODO: lấy từ csdl, kiểm tra từ bảng Employee
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //tao lenh thuc thi truy van du lieu
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select EmployeeID,LastName,FirstName,Title,PhotoPath,Roles
	                                from  Employees                                                                  
                                where (Email like @userName) and (Password = @passWord) ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@userName", userName);
                cmd.Parameters.AddWithValue("@passWord", passWord);
              
                // thuc thi truy van tra ve bien dbReader
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        //Tach mang Roles trong csdl 
                       // string[] arrListRole = dbReader["Roles"].ToString().Split(new char[] { ',' });
                        data = new UserAccount()
                        {
                            UserID = dbReader["EmployeeID"].ToString(),
                            FullName = $"{dbReader["FirstName"]} {dbReader["LastName"]}",
                            Title = dbReader["Title"].ToString(),
                            Photo = dbReader["PhotoPath"].ToString(),
                            Roles = dbReader["Roles"].ToString()
                        };
                   }
                    connection.Close();
                }
                return data;
                //return new UserAccount()
                //{
                //    UserID = userName,
                //    FullName = "NTL",
                //    Photo = "a1.jpg",
                //    Title = "Sinh viên"
                //};
            }
        }

        public bool Update(string id, string oldPass, string newPass)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Employees
                                    SET Password = @newPass
                                    WHERE EmployeeID = @id and Password=@oldPass";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@id",Convert.ToInt32(id));
                cmd.Parameters.AddWithValue("@newPass",newPass);
                cmd.Parameters.AddWithValue("@oldPass", oldPass);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}
