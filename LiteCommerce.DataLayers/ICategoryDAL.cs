using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface ICategoryDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>

        List<Category> List(int page, int pageSize, string searchValue);
        /// <summary>
        /// hien thi so luong tim kiem duoc
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Hiện thị danh sách Category
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        Category Get(int categoryID);
        /// <summary>
        /// Bo sung category. ham tra ve ID cua category
        /// neu loi tra ve 0 or gia tri nho hon
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Category data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Category data);
        /// <summary>
        /// Xóa 1 hoặc nhiều Category. hàm trả về kiểu số lượng đã xóa 
        /// </summary>
        /// <param name="categoryIDs"></param>
        /// <returns></returns>
        int Delete(int[] categoryIDs);
    }
}
