using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// Hien thi danh sach supplier, phan trang va tim kiem
    /// </summary>
    /// <param name="page">Số trang</param>
    /// <param name="pageSize">Số dòng hiện thị lên 1 trang</param>
    /// <param name="searchValue">từ khóa tìm kiếm</param>
    public interface ISupplierDAL
    {

        List<Supplier> List(int page, int pageSize, string searchValue);
        /// <summary>
        /// hien thi so luong tim kiem duoc
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Hiện thị danh sách supplier
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        Supplier Get(int supplierID);
        /// <summary>
        /// Bo sung supplier. ham tra ve ID cua supplier
        /// neu loi tra ve 0 or gia tri nho hon
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Supplier data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Supplier data);
        /// <summary>
        /// Xóa 1 hoặc nhiều supplier. hàm trả về kiểu số lượng đã xóa 
        /// </summary>
        /// <param name="supplierIDs"></param>
        /// <returns></returns>
        int Delete(int[] supplierIDs);
    }
}
