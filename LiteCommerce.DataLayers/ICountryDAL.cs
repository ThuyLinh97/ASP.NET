using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers.SqlServer
{
     public interface ICountryDAL
    {
        /// <summary>
        /// HIển thị danh sách country
        /// </summary>
        /// <returns></returns>
        List<Country> List();
        List<Country> List(int page, int pageSize, string searchValue);
        /// <summary>
        /// hien thi so luong tim kiem duoc
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Hiện thị danh sách country
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns></returns>
        Country Get(string countryName);
        /// <summary>
        /// Bo sung country. ham tra ve ID cua country
        /// neu loi tra ve 0 or gia tri nho hon
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string Add(Country data);
        /// <summary>
        /// Cập nhật lại tên Country
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Country data,string id);
        /// <summary>
        /// Xóa 1 hoặc nhiều country. hàm trả về kiểu số lượng đã xóa 
        /// </summary>
        /// <param name="countryIDs"></param>
        /// <returns></returns>
        int Delete(string[] countryNames);
    }
}
