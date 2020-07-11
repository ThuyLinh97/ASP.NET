using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// Các chức năng cần thiết của Account
    /// </summary>
     public interface IUserAccountDAL
    {
        /// <summary>
        /// Kiểm tra User va pass hợp lệ không?
        /// Nếu có trả về thông tin User
        /// ngược lại trả về Null
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        UserAccount Authorize(string userName, string passWord);
        bool Update(string id,string oldPass, string newPass);
    }
}
