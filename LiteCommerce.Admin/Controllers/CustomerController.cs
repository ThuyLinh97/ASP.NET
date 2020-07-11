using LiteCommerce.BusinessLayers;
using LiteCommerce.DataLayers.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = WebUserRoles.MANAGEMENT)]
    public class CustomerController : Controller
    {
        // GET: Customer
        
        public ActionResult Index(int page = 1, string searchValue = "",string country="")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Customer> listOfCustomer = CatalogBLL.ListOfCustomers(page, pageSize, searchValue, out rowCount,country);

            var model = new Models.CustomerPagitionResult()
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = searchValue,
                RowCount = rowCount,
                Country=country,
                Data = listOfCustomer
            };
            return View(model);
        }
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            //truyen method sang View: Add
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.Title = "Create new customer";
                    ViewBag.Method = "Add";
                    Customer newCustomer = new Customer()
                    {
                        CustomerID = ""
                    };
                    return View(newCustomer);
                }
                else
                {
                    ViewBag.Title = "Edit a customer";
                    ViewBag.Method = "Edit";
                    Customer editCustomer = CatalogBLL.GetCustomer(id);
                    if (editCustomer == null)
                        return RedirectToAction("Index");
                    return View(editCustomer);
                }

            }
            catch (Exception ex)
            {
                //hiện thị lỗi ra ngoài
                return Content(ex.Message + ":" + ex.StackTrace);
            }

        }

        [HttpPost]
        public ActionResult Input(Customer model,string method)
        {
            //return Json(model, JsonRequestBehavior.AllowGet);

            try
            {
                //ToDO: kiểm tra tính hợp lệ của dữ liệu đc nhập
                if (string.IsNullOrEmpty(model.CustomerID))
                    ModelState.AddModelError("CustomerID)", "Please enter the data");
                else if (CatalogBLL.GetCustomer(model.CustomerID) != null)
                    ModelState.AddModelError("CustomerID", "Duplicated");

                if (string.IsNullOrEmpty(model.CompanyName))
                    ModelState.AddModelError("CompanyName", "Please enter the data");
                if (string.IsNullOrEmpty(model.ContactName))
                    ModelState.AddModelError("ContactName", "Please enter the data");
                if (string.IsNullOrEmpty(model.ContactTitle))
                    ModelState.AddModelError("ContactTitle", "Please enter the data");
                if (string.IsNullOrEmpty(model.Address))
                    model.Address = "";
                if (string.IsNullOrEmpty(model.City))
                    model.City = "";
                if (string.IsNullOrEmpty(model.Country))
                    model.Country = "";
                if (string.IsNullOrEmpty(model.Phone))
                    model.Phone = "";
                if (string.IsNullOrEmpty(model.Fax))
                    model.Fax = "";
                

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = model.CustomerID == "" ? "Add new Customer" : "Edit Customer";
                    ViewBag.Method = method == "Add" ? "Add" : "Edit";
                  
                    return View(model);
                }

                //toDo: Lưu dữ liệu
                if (method.Equals("Add"))
                {
                    CatalogBLL.AddCustomer(model);
                }
               else
                {
                    CatalogBLL.UpdateCustomer(model);
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
        public ActionResult Delete(string[] customerIDs)
        {
            if (customerIDs != null)
            {
                CatalogBLL.DeleteCustomer(customerIDs);
            }
            return RedirectToAction("Index");
        }
    }
}