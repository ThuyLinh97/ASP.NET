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
    [Authorize(Roles = WebUserRoles.MANAGEMENT)]
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(string searchValue="",int page=1, string Category = "", string Supplier= "")
        {
            int rowCount = 0;
            int pageSize = 3;
            if (Category == "" || Supplier == "")
            {
                Category = "0";
                Supplier = "0";
            }   
            List<Product> listOfProduct = CatalogBLL.ListOfProducts(page,pageSize,searchValue, out rowCount, Convert.ToInt32(Category), Convert.ToInt32(Supplier));
            Models.ProductPaginationResult model = new Models.ProductPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = searchValue,
                RowCount = rowCount,
                Category = Category,
                Supplier = Supplier,
                Data = listOfProduct
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
                    Product newProduct = new Product()
                    {
                        ProductID = 0
                    };
                    return View(newProduct);
                }
                else
                {
                    ViewBag.Title = "Edit a Product";
                    Product editProduct = CatalogBLL.GetProduct(Convert.ToInt32(id));
                    if (editProduct == null)
                        return RedirectToAction("Index");
                    return View(editProduct);
                }

            }
            catch (Exception ex)
            {
                //hiện thị lỗi ra ngoài
                return Content(ex.Message + ":" + ex.StackTrace);
            }

        }
        [HttpPost]
        public ActionResult Input(Product model, HttpPostedFileBase uploadfile)
        {
            //return Json(model, JsonRequestBehavior.AllowGet);

            try
            {
                //ToDO: kiểm tra tính hợp lệ của dữ liệu đc nhập
                if (string.IsNullOrEmpty(model.ProductName))
                    ModelState.AddModelError("ProductName", "Please enter the data");
                if (model.CategoryID == 0)
                    ModelState.AddModelError("CategoryID", "Please enter the data");
                if (model.SupplierID == 0)
                    ModelState.AddModelError("SupplierID", "Please enter the data");

                if (string.IsNullOrEmpty(model.Descriptions))
                  model.Descriptions="";

                if ((model.UnitPrice) <= 0 && (model.UnitPrice >= 'a') && (model.UnitPrice <= 'z') && (model.UnitPrice >= 'A') && (model.UnitPrice <= 'Z'))
                    ModelState.AddModelError("UnitPrice", "Please enter the data"); 
                          
                if (string.IsNullOrEmpty(model.QuantityPerUnit))
                    model.QuantityPerUnit = "";
               
                if (string.IsNullOrEmpty(model.PhotoPath))
                    model.PhotoPath = "";

                if (uploadfile != null)
                {
                    string folder = Server.MapPath("~/Images/Products");
                    string fileName = $"{DateTime.Now.Ticks}{Path.GetExtension(uploadfile.FileName)}";
                    string filePath = Path.Combine(folder, fileName);
                    uploadfile.SaveAs(filePath);
                    model.PhotoPath = fileName;
                }

                //Lưu vào Db
                //CatalogBLL.AddProduct(model);
                //return Json(model, JsonRequestBehavior.AllowGet);


                if (!ModelState.IsValid)
                {
                    ViewBag.Title = model.ProductID == 0 ? "Add new Product" : "Edit Product";
                    // return View(model);
                }

                //toDo: Lưu dữ liệu
                if (model.ProductID == 0)
                {
                    CatalogBLL.AddProduct(model);
                }
                else
                {
                    CatalogBLL.UpdateProduct(model);
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
        public ActionResult Delete(int[] productIDs = null)
        {
            if (productIDs != null)
            {
                CatalogBLL.DeleteProduct(productIDs);
            }
            return RedirectToAction("Index");
        }
    }
}