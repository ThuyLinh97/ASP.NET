using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// Hien thi danh sach Order, phan trang va tim kiem
    /// </summary>
    /// <param name="page">Số trang</param>
    /// <param name="pageSize">Số dòng hiện thị lên 1 trang</param>
    /// <param name="searchValue">từ khóa tìm kiếm</param>
    public interface IOrderDAL
    {

        List<Order> List(int page, int pageSize, string searchValue);
        /// <summary>
        /// hien thi so luong tim kiem duoc
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Hiện thị danh sách Order
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        Order Get(int orderID);
        /// <summary>
        /// Bo sung Order. ham tra ve ID cua Order
        /// neu loi tra ve 0 or gia tri nho hon
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Order data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Order data);
        /// <summary>
        /// Xóa 1 hoặc nhiều Order. hàm trả về kiểu số lượng đã xóa 
        /// </summary>
        /// <param name="OrderIDs"></param>
        /// <returns></returns>
        int Delete(int[] orderIDs);
    }
}
