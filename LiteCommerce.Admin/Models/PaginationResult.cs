using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    public abstract class PaginationResult
    {
        /// <summary>
        /// Số trang hiện tại
        /// </summary>
        public int Page { get;set;}
        /// <summary>
        /// sô dòng trong cơ sở dữ liệu
        /// </summary>
        public int RowCount { get;set;}
        /// <summary>
        /// Số dòng hiển thị trong 1 trang
        /// </summary>
        public int PageSize{ get;set;}
        /// <summary>
        /// Từ khóa tìm kiếm
        /// </summary>
        public string SearchValue { get;set;}
        /// <summary>
        /// Số phân trang hiển thị 
        /// </summary>
        public int PageCount {
            get{
                int p = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                    p += 1;
                return p;
              }
        }

    }
    
}