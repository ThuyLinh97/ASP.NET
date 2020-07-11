using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface ICustomerDAL
    {

        List<Customer> List(int page, int pageSize, string searchValue,string country);
        int CountID(string id);
        /// <summary>
        /// hien thi so luong tim kiem duoc
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue,string country);
        /// <summary>
        /// Hiện thị danh sách customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Customer Get(string customerID);
        /// <summary>
        /// Bo sung customer. ham tra ve ID cua customer
        /// neu loi tra ve 0 or gia tri nho hon
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
       string Add(Customer data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Customer data);
        /// <summary>
        /// Xóa 1 hoặc nhiều supplier. hàm trả về kiểu số lượng đã xóa 
        /// </summary>
        /// <param name="customerIDs"></param>
        /// <returns></returns>
        int Delete(string[] customerIDs);
    }
}
