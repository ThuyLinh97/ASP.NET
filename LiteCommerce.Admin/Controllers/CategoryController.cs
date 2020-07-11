using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize (Roles=WebUserRoles.MANAGEMENT)]
    public class CategoryController : Controller
    {
        // GET: Category

        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            List<Category> listOfCategory = CatalogBLL.ListOfCategory(page, pageSize, searchValue, out rowCount);

            var model = new Models.CategoryPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = searchValue,
                RowCount = rowCount,
                Data = listOfCategory
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
                    ViewBag.Title = "Create new category";
                    Category newCategory = new Category()
                    {
                        CategoryID = 0
                    };
                    return View(newCategory);
                }
                else
                {
                    ViewBag.Title = "Edit a Category";
                    Category editCategory = CatalogBLL.GetCategory(Convert.ToInt32(id));
                    if (editCategory == null)
                        return RedirectToAction("Index");
                    return View(editCategory);
                }

            }
            catch (Exception ex)
            {
                //hiện thị lỗi ra ngoài
                return Content(ex.Message + ":" + ex.StackTrace);
            }

        }
        [HttpPost]
        public ActionResult Input(Category model)
        {
           //return Json(model, JsonRequestBehavior.AllowGet);

            try
            {
                //ToDO: kiểm tra tính hợp lệ của dữ liệu đc nhập
                if (string.IsNullOrEmpty(model.CategoryName))
                    ModelState.AddModelError("CategoryName", "Please enter the data");
             
               
                if (string.IsNullOrEmpty(model.Description))
                    model.Description = "";

                if (!ModelState.IsValid)
                {
                    //ViewBag.Title = model.CategoryID == 0 ? "Add new Category" : "Edit Category";
                    return View(model);
                }

                //toDo: Lưu dữ liệu
                if (model.CategoryID == 0)
                {
                    CatalogBLL.AddCategory(model);
                }
                else
                {
                    CatalogBLL.UpdateCategory(model);
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
        public ActionResult Delete(int[] categoryIDs = null)
        {
            if (categoryIDs != null)
            {
                CatalogBLL.DeleteCategory(categoryIDs);
            }
            return RedirectToAction("Index");
        }
    } 
 }