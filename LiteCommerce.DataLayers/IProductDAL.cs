using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// Hien thi danh sach product, phan trang va tim kiem
    /// </summary>
    /// <param name="page">Số trang</param>
    /// <param name="pageSize">Số dòng hiện thị lên 1 trang</param>
    /// <param name="searchValue">từ khóa tìm kiếm</param>
    public interface IProductDAL
    {
        /// <summary>
        /// HIện thị danh sách product
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Product> List(int page, int pageSize, string searchValue, int categoryId, int supplierId);
        /// <summary>
        /// hien thi so luong tim kiem duoc
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue, int categoryId, int supplierId);
        /// <summary>
        /// Hiện thị danh sách product
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        Product Get(int productID);
        /// <summary>
        /// Bo sung product. ham tra ve ID cua product
        /// neu loi tra ve 0 or gia tri nho hon
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Product data);
        /// <summary>
        /// Cập nhập lại Product thông qua ID, trả về true nếu ok còn false ngược lại
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Product data);
        /// <summary>
        /// Xóa 1 hoặc nhiều product. hàm trả về kiểu số lượng đã xóa 
        /// </summary>
        /// <param name="productIDs"></param>
        /// <returns></returns>
        int Delete(int[] productIDs);
    }
}
