using EVTrend.Areas.Member.Models;
using EVTrend.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;

namespace EVTrend.Areas.Member.Controllers
{
    [Area(areaName: "Member")]

    public class MemberController : _BaseController
    {
        /// <summary>
        /// 會員中心View
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            string account;
            Request.Cookies.TryGetValue("account", out account);
            return View("ShowMember", GetMemberModel(account));
        }

        private DataTable GetMember(string account)
        {
            var sqlStr = string.Format(
                "SELECT Account, Username, Gender, Birthday from evtrend.`member` WHERE Account = {0}", SqlVal2(account));
            var data = _DB_GetData(sqlStr);
            return data;
        }

        private MemberModels GetMemberModel(string account)
        {
            var data = GetMember(account);
            MemberModels member = new MemberModels();
            DataRow row = data.Rows[0];

            member.Account = row.ItemArray.GetValue(0).ToString();
            member.Username = row.ItemArray.GetValue(1).ToString();
            string Gender = row.ItemArray.GetValue(2).ToString();

            if (Gender == "1")
            {
                member.Gender = "男";
            }
            else if (Gender == "2")
            {
                member.Gender = "女";
            }
            else
            {
                member.Gender = "不願透漏";
            }

            member.Birthday = DateTime.Parse(row.ItemArray.GetValue(3).ToString());

            return member;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(MemberModels member)
        {
            var Gender = member.Gender;
            if (Gender == "男")
            {
                member.Gender = "1";
            }
            else if (Gender == "女")
            {
                member.Gender = "2";
            }
            else
            {
                member.Gender = "3";
            }

            var sqlStr = string.Format("UPDATE member " +
                "SET Username = {0}, " +
                "Gender  = {1}," +
                "Birthday = {2}, " +
                "ModifyTime = {3} " +
                "WHERE Account = {4}",
                SqlVal2(member.Username),
                SqlVal2(member.Gender),
                SqlVal2(member.Birthday),
                DBC.ChangeTimeZone(),
                SqlVal2(member.Account));

            var check = _DB_Execute(sqlStr);

            if (check == 1)
            {
                return View("ShowMember", member);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}