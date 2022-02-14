using EVTrend.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVTrend.Areas.Content.Models;
using System.Data;

namespace EVTrend.Areas.Content.Controllers
{
    [Area(areaName: "Content")]

    public class MgtTotalCarbonController : _BaseController
    {
        /// <summary>
        /// 總體碳排量數據管理View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(MgtTotalCarbonModel Model)
        {
            // admin check
            if (getUserStatusNo() != "0")
            {
                return Redirect("~/Home/Error");
            }
            ViewData["GetTotalCarbon"] = GetCountryTotalCarbon(Model.CountryNo);
            ViewData["GetYear"] = GetYear();
            ViewData["GetCountry"] = GetCountry();
            ViewData["SelectCountryNo"] = Model.CountryNo;
            //ViewData["GetCountry"] = GetCountry();
            return View("MgtTotalCarbon");
        }


        /// <summary>
        /// 取得年分數據
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<TimeModel> GetYear()
        {

            var sqlStr = string.Format(
                "SELECT YearNo, YearName FROM evtrend.`years`");
            var data = _DB_GetData(sqlStr);
            List<TimeModel> list = new List<TimeModel>();
            foreach (DataRow row in data.Rows)
            {
                TimeModel model = new TimeModel();
                model.YearNo = (int)row.ItemArray.GetValue(0);
                model.YearName = row.ItemArray.GetValue(1).ToString();
                list.Add(model);
            }
            return list;

        }

        /// <summary>
        /// 取得國家數據
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<CountryModel> GetCountry()
        {

            var sqlStr = string.Format(
                "SELECT CountryNo, CountryName FROM evtrend.`countries`");
            var data = _DB_GetData(sqlStr);
            List<CountryModel> list = new List<CountryModel>();
            foreach (DataRow row in data.Rows)
            {
                CountryModel model = new CountryModel();
                model.CountryNo = (int)row.ItemArray.GetValue(0);
                model.CountryName = row.ItemArray.GetValue(1).ToString();
                list.Add(model);
            }
            return list;

        }


        /// <summary>
        /// 取得總體碳排量數據
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<MgtTotalCarbonModel> GetTotalCarbon(string CountryNo = null)
        {

            var sqlStr = string.Format(
                "SELECT TotalCarbonNo, TotalCarbonYear, YearName, TotalCarbonCountryNo, countryName,T_CarbonNumber,TotalCarbonNumber,TotalCarbonCreateTime,TotalCarbonModifyTime FROM evtrend.`total_carbon` as a " +
                "inner join evtrend.`countries` as b " +
                "on a.TotalCarbonCountryNo = b.CountryNo " +
                "inner join evtrend.`years` as c " +
                "on a.TotalCarbonYear = c.YearNo " +
                "ORDER BY YearName DESC");
            var data = _DB_GetData(sqlStr);
            List<MgtTotalCarbonModel> list = new List<MgtTotalCarbonModel>();
            foreach (DataRow row in data.Rows)
            {
                MgtTotalCarbonModel model = new MgtTotalCarbonModel();
                model.TotalCarbonNo = (int)row.ItemArray.GetValue(0);
                model.YearNo = (int)row.ItemArray.GetValue(1);
                model.Year = row.ItemArray.GetValue(2).ToString();
                model.CountryNo = (int)row.ItemArray.GetValue(3);
                model.Country = row.ItemArray.GetValue(4).ToString();
                model.T_CarbonNumber = (float)row.ItemArray.GetValue(5);
                model.TotalCarbonNumber = (float)row.ItemArray.GetValue(6);
                model.CreateTime = row.ItemArray.GetValue(7).ToString();
                if (row.ItemArray.GetValue(8).ToString() == "")
                {
                    model.ModifyTime = "NULL";
                }
                else
                {
                    model.ModifyTime = row.ItemArray.GetValue(8).ToString();
                }
                list.Add(model);
            }
            return list;

        }


        /// <summary>
        /// 取得特定國家總體碳排量數據
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<MgtTotalCarbonModel> GetCountryTotalCarbon(int CountryNo = 0)
        {
            var sqlStr = "";
            if (CountryNo == 0) //選擇全部
            {
                sqlStr = string.Format(
                                "SELECT TotalCarbonNo, TotalCarbonYear, YearName, TotalCarbonCountryNo, countryName,T_CarbonNumber,TotalCarbonNumber,TotalCarbonCreateTime,TotalCarbonModifyTime FROM evtrend.`total_carbon` as a " +
                                "inner join evtrend.`countries` as b " +
                                "on a.TotalCarbonCountryNo = b.CountryNo " +
                                "inner join evtrend.`years` as c " +
                                "on a.TotalCarbonYear = c.YearNo " +
                                "ORDER BY YearName DESC");

            }
            else //選擇台灣或美國
            {
                sqlStr = string.Format(
                                "SELECT TotalCarbonNo, TotalCarbonYear, YearName, TotalCarbonCountryNo, countryName,T_CarbonNumber,TotalCarbonNumber,TotalCarbonCreateTime,TotalCarbonModifyTime FROM evtrend.`total_carbon` as a " +
                                "inner join evtrend.`countries` as b " +
                                "on a.TotalCarbonCountryNo = b.CountryNo " +
                                "inner join evtrend.`years` as c " +
                                "on a.TotalCarbonYear = c.YearNo " +
                                "WHERE TotalCarbonCountryNo = {0} " +
                                "ORDER BY YearName DESC", SqlVal2(CountryNo));

            }
            var data = _DB_GetData(sqlStr);
            List<MgtTotalCarbonModel> list = new List<MgtTotalCarbonModel>();
            foreach (DataRow row in data.Rows)
            {
                MgtTotalCarbonModel model = new MgtTotalCarbonModel();
                model.TotalCarbonNo = (int)row.ItemArray.GetValue(0);
                model.YearNo = (int)row.ItemArray.GetValue(1);
                model.Year = row.ItemArray.GetValue(2).ToString();
                model.CountryNo = (int)row.ItemArray.GetValue(3);
                model.Country = row.ItemArray.GetValue(4).ToString();
                model.T_CarbonNumber = (float)row.ItemArray.GetValue(5);
                model.TotalCarbonNumber = (float)row.ItemArray.GetValue(6);
                model.CreateTime = row.ItemArray.GetValue(7).ToString();
                if (row.ItemArray.GetValue(8).ToString() == "")
                {
                    model.ModifyTime = "NULL";
                }
                else
                {
                    model.ModifyTime = row.ItemArray.GetValue(8).ToString();
                }
                list.Add(model);
            }
            return list;

        }



        /// <summary>
        /// 新增總體碳排量數據
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool InsertTotalCarbon(MgtTotalCarbonModel Model)
        {
            //admin check
            if (getUserStatusNo() != "0")
            {
                return false;
            }

            // 檢查是否重複新增
            var sqlSelect = string.Format("SELECT 1 from evtrend.`total_carbon` " +
                "WHERE TotalCarbonCountryNo={0} and TotalCarbonYear={1}", SqlVal2(Model.CountryNo), SqlVal2(Model.YearNo));

            var dataSelect = _DB_GetData(sqlSelect);
            if (dataSelect.Rows.Count > 0)
            {
                return false;
            }


            //SQL Insert TotalElec
            var sqlStr = string.Format(
                @"INSERT INTO evtrend.`total_carbon` (" +
                    "TotalCarbonCountryNo," +
                    "TotalCarbonYear," +
                    "T_CarbonNumber," +
                    "TotalCarbonNumber," +
                    "TotalCarbonCreateTime" +
                ")VALUES(" +
                    "{0}," +
                    "{1}," +
                    "{2}," +
                    "{3}," +
                    "{4}",
                    SqlVal2(Model.CountryNo),
                    SqlVal2(Model.YearNo),
                    SqlVal2(Model.T_CarbonNumber),
                    SqlVal2(Model.TotalCarbonNumber),
                    DBC.ChangeTimeZone() + ")"
                );

            //SQL Check
            var check = _DB_Execute(sqlStr);

            //新增是否成功
            if (check == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改總體電動車數據
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool UpdateTotalCarbon(MgtTotalCarbonModel Model)
        {
            // admin check
            if (getUserStatusNo() != "0")
            {
                return false;
            }


            //SQL Insert TotalElec
            var sqlStr = string.Format("UPDATE evtrend.`total_carbon` " +
                "SET T_CarbonNumber = {0} " +
                ",TotalCarbonNumber  = {1} " +
                ",TotalCarbonModifyTime = {2} " +
                "WHERE " +
                "TotalCarbonNo={3}",
                SqlVal2(Model.T_CarbonNumber),
                SqlVal2(Model.TotalCarbonNumber),
                DBC.ChangeTimeZone(),
                SqlVal2(Model.TotalCarbonNo));

            var check = _DB_Execute(sqlStr);

            //是否成功
            if (check == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 刪除總體電動車數據
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool DeleteTotalCarbon(MgtTotalCarbonModel Model)
        {

            // admin check (plus)
            if (getUserStatusNo() != "0")
            {
                return false;
            }


            var sqlStr = string.Format("DELETE FROM evtrend.`total_carbon`" +
                "WHERE TotalCarbonNo={0} ",
                SqlVal2(Model.TotalCarbonNo));

            var check = _DB_Execute(sqlStr);

            //是否成功
            if (check == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool SelectCountryNo(MgtTotalCarbonModel Model)
        {
            // admin check
            if (getUserStatusNo() != "0")
            {
                return false;
            }

            if (Model.CountryNo.ToString() != null && Model.CountryNo.ToString() != "-1")
            {
                if (GetCountryTotalCarbon(Model.CountryNo).Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (GetCountryTotalCarbon().Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}