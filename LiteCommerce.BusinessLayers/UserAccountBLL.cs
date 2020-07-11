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
     public class UserAccountBLL
    {
        private static string _connectionString;
        public static void Initialize(string connectionString)
        {
            _connectionString = connectionString;
            //lam lai
            userAccountEDB = new EmployeeUserAccountDAL(_connectionString);
        }
        private static IUserAccountDAL userAccountEDB { get; set; }
        public static UserAccount Authorize(string userName, string passWord,UserAccountTypes userType)
        {
            IUserAccountDAL userAccountDB;
            switch (userType)
            {
                case UserAccountTypes.Employee:
                    userAccountDB = new EmployeeUserAccountDAL(_connectionString);
                    break;
                case UserAccountTypes.Customer:
                    userAccountDB = new CustomerUserAccountDAL(_connectionString);
                    break;
                default:
                    return null;
            }
            return userAccountDB.Authorize(userName, passWord);
        }
        
     
        public static bool UpdatePassword(string id, string oldPass, string newPass)
        {

            return userAccountEDB.Update(id, oldPass, newPass);
        }
    }
}
