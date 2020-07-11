using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = WebUserRoles.MANAGEMENT)]
    public class SupplierController : Controller
    {
        // GET: Supplier
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Supplier> listOfSupplier = CatalogBLL.ListOfSuppliers(page, pageSize, searchValue, out rowCount);

            var model = new Models.SupplierPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = searchValue,
                RowCount = rowCount,
                Data = listOfSupplier
            };
            return View(model);

            //int pageSize = 3;
            //int rowCount = 0;
            //List<Supplier> model = CatalogBLL.ListOfSuppliers(page, pageSize, searchValue, out rowCount);
            //ViewBag.RowCount = rowCount;
            //return View(model);
        }
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.Title = "Create new supplier";
                    Supplier newSupplier = new Supplier()
                    {
                        SupplierID = 0
                    };
                    return View(newSupplier);
                }
                else
                {
                    ViewBag.Title = "Edit a supplier";
                    Supplier editSupplier = CatalogBLL.GetSupplier(Convert.ToInt32(id));
                    if (editSupplier == null)
                        return RedirectToAction("Index");
                    return View(editSupplier);
                }

            }
            catch (Exception ex)
            {
                //hiện thị lỗi ra ngoài
                return Content(ex.Message + ":" + ex.StackTrace);
            }

        }
        [HttpPost]
        public ActionResult Input(Supplier model)
        {
            //return Json(model, JsonRequestBehavior.AllowGet);

            try
            {
                //ToDO: kiểm tra tính hợp lệ của dữ liệu đc nhập
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
                    model.Phone= "";
                if (string.IsNullOrEmpty(model.Fax))
                    model.Fax = "";
                if (string.IsNullOrEmpty(model.HomePage))
                    model.HomePage = "";

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = model.SupplierID == 0 ? "Add new Supplier" : "Edit Supplier";
                   // return View(model);
                }
                    
                //toDo: Lưu dữ liệu
                if (model.SupplierID == 0)
                {
                    CatalogBLL.AddSupplier(model);
                }
                else
                {
                    CatalogBLL.UpdateSupplier(model);
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
        public ActionResult Delete(int[] supplierIDs=null)
        {
            if(supplierIDs != null)
            {
                CatalogBLL.DeleteSupplier(supplierIDs);
            }
            return RedirectToAction("Index");
        }
    }
}