using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = WebUserRoles.SALEMAN)]
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index(int page = 1, string searchValue = "")
        {

            int pageSize = 3;
            int rowCount = 0;
            List<Order> listOfOrder = OrderBLL.ListOfOrders(page, pageSize, searchValue, out rowCount);

            var model = new Models.OrderPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = searchValue,
                RowCount = rowCount,
                Data = listOfOrder
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
                    ViewBag.Title = "Create new Product";
                    Order newOrder = new Order()
                    {
                        OrderID = 0
                    };
                    return View(newOrder);
                }
                else
                {
                    ViewBag.Title = "Edit a Order";
                    Order editOrder = OrderBLL.GetOrder(Convert.ToInt32(id));
                    if (editOrder == null)
                        return RedirectToAction("Index");
                    return View(editOrder);
                }

            }
            catch (Exception ex)
            {
                //hiện thị lỗi ra ngoài
                return Content(ex.Message + ":" + ex.StackTrace);
            }

        }
        [HttpPost]
        public ActionResult Input(Order model)
        {
            //return Json(model, JsonRequestBehavior.AllowGet);

            try
            {
                //ToDO: kiểm tra tính hợp lệ của dữ liệu đc nhập
                if (string.IsNullOrEmpty(model.CustomerID))
                    ModelState.AddModelError("CustomerID", "Please enter the data");
                if (model.EmployeeID == 0)
                    ModelState.AddModelError("EmployeeID", "Please enter the data");
                if (model.ShipperID == 0)
                    ModelState.AddModelError("SupplierID", "Please enter the data");

                if (string.IsNullOrEmpty(model.ShipCountry))
                    ModelState.AddModelError("ShipCountry", "Please enter the data");

                if (string.IsNullOrEmpty(model.ShipAddress))
                    ModelState.AddModelError("ShipAddress", "Please enter the data");

                if (model.RequiredDate==null)
                    ModelState.AddModelError("RequiredDate", "Please enter the data");
                if (model.ShippedDate == null)
                    ModelState.AddModelError("ShippedDate", "Please enter the data");
                if (model.OrderDate == null)
                    ModelState.AddModelError("OrderDate", "Please enter the data");
                if (string.IsNullOrEmpty(Convert.ToString(model.Freight)))
                    ModelState.AddModelError("Freight", "Please enter the data");
                //Lưu vào Db
                //CatalogBLL.AddProduct(model);
                //return Json(model, JsonRequestBehavior.AllowGet);


                if (!ModelState.IsValid)
                {
                    ViewBag.Title = model.OrderID == 0 ? "Add new Product" : "Edit Product";
                    // return View(model);
                }

                //toDo: Lưu dữ liệu
                if (model.OrderID == 0)
                {
                    OrderBLL.AddOrder(model);
                }
                else
                {
                    OrderBLL.UpdateOrder(model);
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
        public ActionResult Delete(int[] orderIDs = null)
        {
            if (orderIDs != null)
            {
                OrderBLL.DeleteOrder(orderIDs);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            return View();
        }
    }
}