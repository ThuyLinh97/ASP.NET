using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles =WebUserRoles.ACCOUNTTANTS)]
    public class EmployeeController : Controller
    {

        /// <summary>
        /// GET: Employee
        /// </summary>
        /// <param name="page"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Employee> listOfEmployee = CatalogBLL.ListOfEmployee(page, pageSize, searchValue, out rowCount);

            var model = new Models.EmployeePaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = searchValue,
                RowCount = rowCount,
                Data = listOfEmployee
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.Title = "Create new employee";
                    Employee newEmployee = new Employee()
                    {
                        EmployeeID = 0
                    };
                    return View(newEmployee);
                }
                else
                {
                    ViewBag.Title = "Edit a employee";
                    Employee editEmployee = CatalogBLL.GetEmployee(Convert.ToInt32(id));
                    if (editEmployee == null)
                        return RedirectToAction("Index");
                    return View(editEmployee);
                }

            }
            catch (Exception ex)
            {
                //hiện thị lỗi ra ngoài
                return Content(ex.Message + ":" + ex.StackTrace);
            }

        }
        [HttpPost]
        public ActionResult Input(Employee model, HttpPostedFileBase uploadfile)
        {
            //return Json(model, JsonRequestBehavior.AllowGet);

            try
            {
                //ToDO: kiểm tra tính hợp lệ của dữ liệu đc nhập
           
                if (string.IsNullOrEmpty(model.LastName))
                    ModelState.AddModelError("LastName", "Please enter the data");
                if (string.IsNullOrEmpty(model.FirstName))
                    ModelState.AddModelError("FirstName", "Please enter the data");
                if (string.IsNullOrEmpty(model.Email))
                    ModelState.AddModelError("Email", "Please enter the data");
                if(string.IsNullOrEmpty(model.Title))
                    ModelState.AddModelError("Title", "Please enter the data");
              
                if (string.IsNullOrEmpty(model.Address))
                    model.Address = "";
                if (string.IsNullOrEmpty(model.City))
                    model.City = "";
                if (string.IsNullOrEmpty(model.Country))
                    model.Country = "";
                if (string.IsNullOrEmpty(model.HomePhone))
                    model.HomePhone = "";
                if (string.IsNullOrEmpty(model.Notes))
                    model.Notes = "";
                if (string.IsNullOrEmpty(model.PhotoPath))
                    model.PhotoPath = "";              
                if (string.IsNullOrEmpty(model.Password))
                    model.Password = "123456"; 
                           
                if (string.IsNullOrEmpty(model.Password))
                    model.Password = "";

                if (Convert.ToDateTime(model.HireDate).Year < (Convert.ToDateTime(model.BirthDate).Year+18) && model.HireDate !=null&& model.BirthDate!=null)
                    ModelState.AddModelError("HireDate", "You must be over 18 years old");

                if (uploadfile != null)
                {
                    string folder = Server.MapPath("~/Images/UploadFile");
                    string fileName = $"{DateTime.Now.Ticks}{Path.GetExtension(uploadfile.FileName)}";
                    string filePath = Path.Combine(folder, fileName);
                    uploadfile.SaveAs(filePath);
                    model.PhotoPath = fileName;
                }

                //Lưu vào Db
                //CatalogBLL.AddEmployee(model);
                //return Json(model, JsonRequestBehavior.AllowGet);
                 

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = model.EmployeeID == 0 ? "Add new Employee" : "Edit Employee";
                    // return View(model);

                }

                //toDo: Lưu dữ liệu
                if (model.EmployeeID == 0)
                {
                    CatalogBLL.AddEmployee(model);
                }
                else
                {
                    CatalogBLL.UpdateEmployee(model);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Delete(int[] employeeIDs = null)
        {
            if (employeeIDs != null)
            {
                CatalogBLL.DeleteEmployee(employeeIDs);
            }
            return RedirectToAction("Index");
        }                 
    }
}