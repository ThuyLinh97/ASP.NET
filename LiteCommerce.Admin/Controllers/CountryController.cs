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
    public class CountryController : Controller
    {
        
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 3;
            int rowCount = 0;
            searchValue = searchValue.Trim();
            List<Country> listOfCountry = CountryBLL.ListOfCoutries(page, pageSize, searchValue, out rowCount);

            var model = new Models.CountryPaginationResult
            {
                Page = page,
                PageSize = pageSize,
                SearchValue = searchValue,
                RowCount = rowCount,
                Data = listOfCountry
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
                    ViewBag.Method = "Add";
                    ViewBag.Title = "Create new Country";
                    Country newCountry = new Country()
                    {
                        CountryName = ""
                    };
                    return View(newCountry);
                }
                else
                {
                    ViewBag.Method = "Edit";
                    ViewBag.Title = "Edit a Country";
                    Country editCountry = CountryBLL.GetCountry(id);
                    if (editCountry == null)
                        return RedirectToAction("Index");
                    return View(editCountry);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message + "." + ex.StackTrace);
            }

        }
        [HttpPost]
        public ActionResult Input(Country model, string method, string id = "")
        {
            // Kiểm tra tính hợp lệ của dữ liệu được nhập
            if (string.IsNullOrEmpty(model.CountryName))
                ModelState.AddModelError("CountryName", "Please enter the data");
            else if (CountryBLL.GetCountry(model.CountryName) != null)
                ModelState.AddModelError("CountryName", "CountryName duplicated");
           
            if (!ModelState.IsValid)
            {
                ViewBag.Title = method == "Add" ? "Add new Country" : "Edit Country";
                ViewBag.Method = method == "Add" ? "Add" : "Edit";
                if (method.Equals("Edit"))
                {
                    model.CountryName = id;
                }
                return View(model);
            }
            // Lưu trữ dữ liệu vào DB
            if (method.Equals("Add"))
            {

                CountryBLL.AddCountry(model);
            }
            else
            {
                CountryBLL.UpdateCountry(model,id);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Delete(string[] countryNames = null)
        {
            if (countryNames != null)
            {
                CountryBLL.DeleteCountry(countryNames);
            }
            return RedirectToAction("Index");
        }
    }
}