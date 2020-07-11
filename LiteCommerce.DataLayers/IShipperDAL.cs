using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IShipperDAL
    {
        List<Shipper> List(int page, int pageSize, string searchValue);
        /// <summary>
        /// hien thi so luong tim kiem duoc
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Hiện thị danh sách Shipper
        /// </summary>
        /// <param name="ShipperID"></param>
        /// <returns></returns>
        Shipper Get(int shipperID);
        /// <summary>
        /// Bo sung shipper. ham tra ve ID cua shipper
        /// neu loi tra ve 0 or gia tri nho hon
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Shipper data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Shipper data);
        /// <summary>
        /// Xóa 1 hoặc nhiều Shipper. hàm trả về kiểu số lượng đã xóa 
        /// </summary>
        /// <param name="ShipperIDs"></param>
        /// <returns></returns>
        int Delete(int[] shipperIDs);
    }
}
