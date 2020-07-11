using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    /// <summary>
    /// lưu thông tin liên quan đến tài khoản đăng nhập
    /// </summary>
    public class UserAccount
    {
        /// <summary>
        /// ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// FirstName+LastName
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Tên file ảnh
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Tên Role của Employee
        /// </summary>
        public string Roles { get; set; }
    }
}
