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
    public class ShipperController : Controller
    {
        // GET: Shipper
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Shipper> listOfShipper = CatalogBLL.ListOfShippers(page, pageSize, searchValue, out rowCount);

            var model = new Models.ShipperPagitionResult()
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = searchValue,
                RowCount = rowCount,
                Data = listOfShipper
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
                    ViewBag.Title = "Create new shipper";
                    Shipper newShipper = new Shipper()
                    {
                        ShipperID = 0
                    };
                    return View(newShipper);
                }
                else
                {
                    ViewBag.Title = "Edit a shipper";
                    Shipper editShipper = CatalogBLL.GetShipper(Convert.ToInt32(id));
                    if (editShipper == null)
                        return RedirectToAction("Index");
                    return View(editShipper);
                }

            }
            catch (Exception ex)
            {
                //hiện thị lỗi ra ngoài
                return Content(ex.Message + ":" + ex.StackTrace);
            }

        }
        [HttpPost]
        public ActionResult Input(Shipper model)
        {
            //return Json(model, JsonRequestBehavior.AllowGet);

            try
            {
                //ToDO: kiểm tra tính hợp lệ của dữ liệu đc nhập
                if (string.IsNullOrEmpty(model.CompanyName))
                    ModelState.AddModelError("CompanyName", "Please enter the data");
              
                if (string.IsNullOrEmpty(model.Phone))
                    model.Phone = "";
            

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = model.ShipperID == 0 ? "Add new Shipper" : "Edit Shipper";
                    // return View(model);
                }

                //toDo: Lưu dữ liệu
                if (model.ShipperID == 0)
                {
                    CatalogBLL.AddShipper(model);
                }
                else
                {
                    CatalogBLL.UpdateShipper(model);
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
        public ActionResult Delete(int[] shipperIDs = null)
        {
            if (shipperIDs != null)
            {
                CatalogBLL.DeleteShipper(shipperIDs);
            }
            return RedirectToAction("Index");
        }
    }
}