using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Data.SqlClient;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class CustomerUserAccountDAL : IUserAccountDAL
    {
        private string connectionString;

        public CustomerUserAccountDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public UserAccount Authorize(string userName, string passWord)
        {
            //TODO: lấy từ csdl , lấy thông tin kiểm tra từ bảng Customer

            //UserAccount data = null;
            ////TODO: lấy từ csdl, kiểm tra từ bảng Employee
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();
            //    //tao lenh thuc thi truy van du lieu
            //    SqlCommand cmd = new SqlCommand();
            //    cmd.CommandText = @"select EmployeeID,LastName,FirstName,Title,PhotoPath,Roles
            //                     from  Employees                                                                  
            //                    where (Email like @userName) and (Password = @passWord) ";
            //    cmd.CommandType = CommandType.Text;
            //    cmd.Connection = connection;
            //    cmd.Parameters.AddWithValue("@userName", userName);
            //    cmd.Parameters.AddWithValue("@passWord", passWord);


            //    // thuc thi truy van tra ve bien dbReader
            //    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            //    {
            //        if (dbReader.Read())
            //        {
            //            //Tach mang Roles trong csdl 
            //            // string[] arrListRole = dbReader["Roles"].ToString().Split(new char[] { ',' });
            //            data = new UserAccount()
            //            {
            //                UserID = dbReader["EmployeeID"].ToString(),
            //                FullName = $"{dbReader["FirstName"]} {dbReader["LastName"]}",
            //                Title = dbReader["Title"].ToString(),
            //                Photo = dbReader["PhotoPath"].ToString(),
            //                Roles = dbReader["Roles"].ToString()
            //            };
            //        }
            //        connection.Close();
            //    }
            //    return data;
            return new UserAccount()
            {
                UserID = userName,
                FullName = "NTLinh",
                Photo = "a1.jpg"
            };
        }

        public bool Update(string id, string oldPass, string newPass)
        {
            throw new NotImplementedException();
        }
    }
}
