using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            UserModel userModel = new UserModel();
            HttpCookie reqCookies = Request.Cookies["UserInfo"];
            if (reqCookies != null)
            {
                userModel.Username = reqCookies["Username"].ToString();
                userModel.Password = reqCookies["Password"].ToString();
                userModel.RememberMe = true;
            }
            return View(userModel);
        }

        [HttpPost]
        public ActionResult Login(UserModel userModel)
        {
            var Username = userModel.Username;
            var Password = userModel.Password;
            Boolean Check = Convert.ToBoolean(userModel.RememberMe);

            UserModel data = new UserModel();
            DataTable dataTable = new DataTable();
            dataTable = DBModel.GetDataTable("USP_GetLogin '" + Username + "', '" + Password + "'");
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    data.UserId = Convert.ToInt32(row["UserId"]);
                    data.Username = Convert.ToString(row["Username"]);
                    data.Password = Convert.ToString(row["Password"]);
                }
            }
            if (data.UserId != 0)
            {
                Session["UserId"] = data.UserId;
                Session["Username"] = data.Username;

                HttpCookie UserCookies = new HttpCookie("UserCookies");
                UserCookies["UserId"] = Convert.ToString(data.UserId);
                UserCookies["Username"] = data.Username;
                UserCookies["Password"] = data.Password;
                Response.Cookies.Add(UserCookies);

                RememberMe(Username, Password, Check);
                return RedirectToAction("Dashboard", "Dashboard");
            }
            else
            {
                data.ErrorMessage = "Wrong Username and Password.";
                return View("Index", data);
            }
        }

        private void RememberMe(String Username, String Password, Boolean RememberMe)
        {
            if (RememberMe)
            {
                HttpCookie httpCookie = new HttpCookie("UserInfo");
                httpCookie["Username"] = Username;
                httpCookie["Password"] = Password;
                httpCookie.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Add(httpCookie);
            }
            else
            {
                HttpCookie httpCookie = new HttpCookie("UserInfo");
                httpCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(httpCookie);
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Registration()
        {
            return View();
        }

        public JsonResult SendEmail(string Email, int OTP, string Message)
        {
            bool IsSent = false;
            IsSent = DBModel.SendEmail(Email, OTP, Message);
            ResponseModel responseModel = new ResponseModel();
            if (IsSent == true)
            {
                DataTable dataTable = new DataTable();
                dataTable = DBModel.GetDataTable("USP_SaveOTP '" + Email + "','" + OTP + "'");
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        responseModel.Status = Convert.ToInt32(row["Status"]);
                    }
                }
            }
            var jsonResult = Json(responseModel, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult VerifyOTP(string Email, int OTP)
        {
            ResponseModel responseModel = new ResponseModel();
            DataTable dataTable = new DataTable();
            dataTable = DBModel.GetDataTable("USP_VerifyOTP '" + Email + "','" + OTP + "'");
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    responseModel.Status = Convert.ToInt32(row["Status"]);
                    responseModel.Message = Convert.ToString(row["Message"]);
                }
            }
            var jsonResult = Json(responseModel, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult SaveRegistration(UserModel Element)
        {
            ResponseModel responseModel = new ResponseModel();
            DataTable dataTable = new DataTable();
            dataTable = DBModel.GetDataTable("USP_SaveUser '" + Element.Email + "','" + Element.Username + "','" + Element.Password + "','" + Element.Name + "','" + Element.Designation + "','" + Element.ContactNo + "','" + Element.Address + "','" + Element.State + "','" + Element.City + "','" + Element.PinCode + "'");
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    responseModel.Status = Convert.ToInt32(row["Status"]);
                    responseModel.Message = Convert.ToString(row["Message"]);
                }
            }
            var jsonResult = Json(responseModel, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}