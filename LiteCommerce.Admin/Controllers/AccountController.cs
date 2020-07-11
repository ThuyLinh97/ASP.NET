using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LiteCommerce.Admin.Codes;
using System.IO;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Index(string id="")
        {
            ViewBag.Title = "User Profile";
            Employee getEmployee = CatalogBLL.GetEmployee(Convert.ToInt32(id));
            var userData = User.GetUserData();
            if(userData.UserID==id)
                return View(getEmployee);
            return RedirectToAction("Index", "Account", new { id = userData.UserID });
        }
        [HttpPost]
        public ActionResult Index(Employee model,HttpPostedFileBase uploadfile)
        {
            //return Json(model, JsonRequestBehavior.AllowGet);
            try
            {
                
                //kiểm tra tính hợp lệ của dữ liệu đc nhập
                if (string.IsNullOrEmpty(model.LastName))
                    ModelState.AddModelError("LastName", "Please enter the data");
                if (string.IsNullOrEmpty(model.FirstName))
                    ModelState.AddModelError("FirstName", "Please enter the data");
                if (string.IsNullOrEmpty(model.Email))
                    ModelState.AddModelError("Email", "Please enter the data");
                
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
                if (model.HireDate.Year < (model.BirthDate.Year + 18) && model.HireDate != null && model.BirthDate != null)
                    ModelState.AddModelError("HireDate", "You must be over 18 years old");
                if (uploadfile != null)
                {
                    string folder = Server.MapPath("~/Images/UploadFile");
                    string fileName = $"{DateTime.Now.Ticks}{Path.GetExtension(uploadfile.FileName)}";
                    string filePath = Path.Combine(folder, fileName);
                    uploadfile.SaveAs(filePath);
                    model.PhotoPath = fileName;
                }
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                    //toDo: Lưu dữ liệu
                    if (model.EmployeeID != 0)
                {
                    CatalogBLL.UpdateEmployee(model);
                }
              
                return RedirectToAction("Index","Account",new { id = @model.EmployeeID });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
                return View(model);
            }
           
        }
       [HttpGet]
       public ActionResult ChangePassword()
        {
            return View();
        }
       [HttpPost]
        public ActionResult ChangePassword(string id, string oldPass="",string newPass="",string confirmPass="")
        {
            try {
                
                if (newPass != confirmPass)
                    ModelState.AddModelError("NewPassword", "New Password does not match");
                else
                {
                    if(UserAccountBLL.UpdatePassword(id, Encode.EncodeMD5(oldPass), Encode.EncodeMD5(newPass)))
                    {
                        ViewBag.Success = "Change password success !";
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("OldPassword", "Check Old Password");
                    }
                } 

                if (!ModelState.IsValid)
                {
                    return View();
                }

                return View();
            } catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message + ":" + ex.StackTrace);
                return View();
            }
           
        }
        public ActionResult SignOut()
        {
            Session.Abandon();
            Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("SignIn");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(string email="", string passWord="")
        {
            //TODO: check tài khoản thông qua CSDL
            UserAccount user = UserAccountBLL.Authorize(email,Encode.EncodeMD5(passWord), UserAccountTypes.Employee);
            if(user != null)
            {
                WebUserData userData = new WebUserData()
                {
                    UserID = user.UserID,
                    FullName = user.FullName,
                    GroupName = user.Roles,
                    //TODO: cần thay đổi GroupName cho đúng
                    LoginTime = DateTime.Now,
                    SessionID = Session.SessionID,
                    ClientIP = Request.UserHostAddress,
                    Photo = user.Photo,
                    Title=user.Title

                };
                FormsAuthentication.SetAuthCookie(userData.ToCookieString(), false);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                //tạo thông báo lỗi: modelState("key","value")key có thể rỗng
                ModelState.AddModelError("", "LOGIN thất bại");
                ViewBag.Email = email;
                return View();
            }

            //if(email=="thuylinhem97@gmail.com" && password=="123")
            //{
            //    //ghi nhận phiên đăng nhập của tài khoản
            //    System.Web.Security.FormsAuthentication.SetAuthCookie(email, false);
            //    //chuyển trang
            //    return RedirectToAction("Index", "Dashboard");
            //}
            //else
            //{
            //    //tạo thông báo lỗi: modelState("key","value")key có thể rỗng
            //    ModelState.AddModelError("", "LOGIN thất bại");
            //    ViewBag.Email = email;
            //    return View();
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
    }
}