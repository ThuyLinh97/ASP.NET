using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// Hien thi danh sach Employee, phan trang va tim kiem
    /// </summary>
    /// <param name="page"></param>
    ///  <param name="pageSize"></param>
    ///  <param name="searchValue"></param>
    public interface IEmployeeDAL
    {

        List<Employee> List(int page, int pageSize, string searchValue);
        /// <summary>
        /// hien thi so luong tim kiem duoc
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Hiện thị danh sách Employee
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        Employee Get(int employeeID);
        /// <summary>
        /// Bo sung employee. ham tra ve ID cua employee
        /// neu loi tra ve 0 or gia tri nho hon
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Employee data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Employee data);
        /// <summary>
        /// Xóa 1 hoặc nhiều employee. hàm trả về kiểu số lượng đã xóa 
        /// </summary>
        /// <param name="employeeIDs"></param>
        /// <returns></returns>
        int Delete(int[] employeeIDs);
    }
}
