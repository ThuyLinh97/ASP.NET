using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin
{
      /// <summary>
      /// Định nghĩa danh sách các Role của user
      /// </summary>
      public class WebUserRoles
      {
            /// <summary>
            /// Không xác định
            /// </summary>
            public const string ANONYMOUS = "anonymous";
            /// <summary>
            /// Nhân viên
            /// </summary>
            public const string STAFF = "staff";
            /// <summary>
            /// Quản trị hệ thống
            /// </summary>
            public const string ADMINISTRATOR = "administrator";

            /// <summary>
            /// nhân viên quản trị tài khoản: đc sd cn liên quan đến nhân viên
            /// </summary>
            public const string ACCOUNTTANTS = "Accounttants";
            /// <summary>
            /// Nhân viên bán hàng: đc phép sử dụng chức năng quản ly đơn hàng(saleman)
            /// </summary>
            public const string SALEMAN = "Saleman";
            /// <summary>
            /// nhân viên quản trị dữ liệu: sd cn liên quan đến catalog(management)
            /// </summary>
            public const string MANAGEMENT = "Management";
    }
}