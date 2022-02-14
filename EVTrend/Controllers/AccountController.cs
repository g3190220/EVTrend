using EVTrend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EVTrend.Controllers
{
    public class AccountController : _BaseController
    {
        /// <summary>
        /// 註冊會員View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 使用者登入View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 檢查是否有重複的帳號
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        [HttpPost]
        public bool DuplicateAccountCheck(string Account)
        {
            //是否為空
            if (string.IsNullOrEmpty(Account)) return false;

            //SQL Select Member
            var sqlStr = string.Format("SELECT Account FROM member WHERE Account = {0}", SqlVal2(Account));

            //SQL Check
            var data = _DB_GetData(sqlStr);

            //資料庫內是否有此帳號
            if (data.Rows.Count > 0)
            {
                //已經有該帳號
                return true;
            }

            //資料庫內目前沒此帳號
            return false;
        }

        /// <summary>
        /// 註冊會員
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(Member Model)
        {
            var loginStatus = getUserStatusNo();
            var sqlStr = "";
            Model.ok = true;

            if (loginStatus == "0") //由管理員新增的使用者
            {
                //SQL Insert Member
                sqlStr = string.Format(
                    @"INSERT INTO member (" +
                        "Account," +
                        "Password," +
                        "Username," +
                        "Gender," +
                        "Birthday," +
                        "CreateTime," +
                        "AccountStart," +
                        "StatusNo" +
                    ")VALUES(" +
                        "{0}," +
                        "{1}," +
                        "{2}," +
                        "{3}," +
                        "{4}," +
                        "{5}," +
                        "{6}," +
                        "{7}",
                        SqlVal2(Model.Account),
                        SqlVal2(SHA256_Encryption(Model.Password)),
                        SqlVal2(Model.Username),
                        SqlVal2(Model.Gender),
                        SqlVal2(Model.Birthday),
                        DBC.ChangeTimeZone(),
                        DBC.ChangeTimeZone(),
                        SqlVal2(Model.StatusNo) + ")"
                    );
            }
            else
            {
                Model.StatusNo = "1";

                //SQL Insert Member
                sqlStr = string.Format(
                    @"INSERT INTO member (" +
                        "Account," +
                        "Password," +
                        "Username," +
                        "Gender," +
                        "Birthday," +
                        "CreateTime," +
                        "AccountStart," +
                        "StatusNo" +
                    ")VALUES(" +
                        "{0}," +
                        "{1}," +
                        "{2}," +
                        "{3}," +
                        "{4}," +
                        "{5}," +
                        "{6}," +
                        "{7}",
                        SqlVal2(Model.Account),
                        SqlVal2(SHA256_Encryption(Model.Password)),
                        SqlVal2(Model.Username),
                        SqlVal2(Model.Gender),
                        SqlVal2(Model.Birthday),
                        DBC.ChangeTimeZone(),
                        DBC.ChangeTimeZone(),
                        SqlVal2(Model.StatusNo) + ")"
                    );
            }

            //SQL Check
            var check = _DB_Execute(sqlStr);

            //新增是否成功
            if (check == 1)
            {
                Model.ResultMessage = "註冊成功";
            }
            else
            {
                Model.ok = false;
                Model.ResultMessage = "註冊失敗";
            }
            
            return View(Model);
        }

        /// <summary>
        /// 登入驗證
        /// 可以在這邊驗證完如果判斷是admin，則把ViewData["admin"] = visible;
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(AccountModels Model)
        {
            //SQL Select Member
            var sqlStr = string.Format("SELECT Account, Password, Username, StatusNo FROM member WHERE Account = {0}", SqlVal2(Model.Account));

            //SQL Check
            var data = _DB_GetData(sqlStr);

            //資料庫內是否有此帳號
            if (data.Rows.Count > 0)
            {
                //帳號與密碼是否相符
                if (Model.Account == data.Rows[0].ItemArray.GetValue(0).ToString() && SHA256_Compare(data.Rows[0].ItemArray.GetValue(1).ToString(), Model.Password))
                {
                    if (data.Rows[0].ItemArray.GetValue(3).ToString() == "2")
                    {
                        //登入成功，但遭到停權
                        Model.ok = false;
                        Model.ResultMessage = "登入失敗，您的帳號已遭到『停權』";
                        return View(Model);
                    }
                    else
                    {
                        // 加入cookie，預設使用者關閉瀏覽器時清除
                        Response.Cookies.Append("userName", data.Rows[0].ItemArray.GetValue(2).ToString());
                        Response.Cookies.Append("account", data.Rows[0].ItemArray.GetValue(0).ToString());

                        //登入成功後返回首頁
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    //登入失敗 帳號或密碼錯誤
                    Model.ok = false;
                    Model.ResultMessage = "登入失敗，帳號或密碼錯誤";
                    return View(Model);
                }
            }
            else
            {
                //登入失敗 找不到此帳號
                Model.ok = false;
                Model.ResultMessage = "登入失敗，找不到此帳號";
                return View(Model);
            }
        }

        /// <summary>
        /// 確認登入狀態
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string CheckLoginStatus()
        {
            var loginStatus = getUserStatusNo();
            if (string.IsNullOrEmpty(loginStatus))
            {
                return null;
            }
            else 
            {
                return loginStatus;
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            // 刪除cookie，預設使用者關閉瀏覽器時清除
            Response.Cookies.Delete("userName");
            Response.Cookies.Delete("account");

            //登出後返回首頁
            return RedirectToAction("Index", "Home");
        }
    }
}
