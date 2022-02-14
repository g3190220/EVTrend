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

    public class MgtDetailedCarbonController : _BaseController
    {
        /// <summary>
        /// 細部碳排量管理View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(MgtTotalCarbonModel Model)
        {
            if (getUserStatusNo() != "0")
            {
                return Redirect("~/Home/Error");
            }
            ViewData["GetMgtDetailedCarbon"] = GetMgtDetailedCarbon(Model.YearNo);
            ViewData["GetYear"] = GetYear();
            ViewData["GetCountry"] = GetCountry();
            ViewData["SelectYearNo"] = Model.YearNo;


            return View("MgtDetailedCarbon");
        }

        /// <summary>
        /// 取得細部碳排量數據
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<MgtDetailedCarbonModel> GetMgtDetailedCarbon(int YearNo = 0)
        {
            var sqlStr = "";
            if (YearNo == 0) //選擇全部
            {
                sqlStr = string.Format("SELECT CarbonNo, CarbonCountryCarsTypeNo,CountryName, c.CountryNo, CarsTypeName, c.CarsTypeNo, YearNo,YearName,CarbonNumber,CarbonCreateTime,CarbonModifyTime FROM evtrend.`carbon` as a " +
                "inner join evtrend.`years` as b " +
                "on a.CarbonYear = b.YearNo " +
                "inner join evtrend.`country_carstype` as c " +
                "on a.CarbonCountryCarsTypeNo = c.CountryCarsTypeNo " +
                "inner join evtrend.`countries` as d " +
                "on c.CountryNo = d.CountryNo " +
                "inner join evtrend.`cars_type` as e " +
                "on c.CarsTypeNo = e.CarsTypeNo " +
                "ORDER BY CarbonYear DESC");
            }
            else //選擇特定年份
            {
                sqlStr = string.Format("SELECT CarbonNo, CarbonCountryCarsTypeNo,CountryName, c.CountryNo, CarsTypeName, c.CarsTypeNo, YearNo,YearName,CarbonNumber,CarbonCreateTime,CarbonModifyTime FROM evtrend.`carbon` as a " +
                "inner join evtrend.`years` as b " +
                "on a.CarbonYear = b.YearNo " +
                "inner join evtrend.`country_carstype` as c " +
                "on a.CarbonCountryCarsTypeNo = c.CountryCarsTypeNo " +
                "inner join evtrend.`countries` as d " +
                "on c.CountryNo = d.CountryNo " +
                "inner join evtrend.`cars_type` as e " +
                "on c.CarsTypeNo = e.CarsTypeNo " +
                "WHERE YearNo = {0} " +
                "ORDER BY CarbonYear DESC", SqlVal2(YearNo));
            }
            var data = _DB_GetData(sqlStr);
            List<MgtDetailedCarbonModel> list = new List<MgtDetailedCarbonModel>();
            foreach (DataRow row in data.Rows)
            {
                MgtDetailedCarbonModel model = new MgtDetailedCarbonModel();

                model.CarbonNo = (int)row.ItemArray.GetValue(0);
                model.CarbonCountryCarsTypeNo = (int)row.ItemArray.GetValue(1);
                model.CountryName = row.ItemArray.GetValue(2).ToString();
                model.CountryNo = (int)row.ItemArray.GetValue(3);
                model.CarsTypeName = row.ItemArray.GetValue(4).ToString();
                model.CarsTypeNo = (int)row.ItemArray.GetValue(5);
                model.CarbonYear = (int)row.ItemArray.GetValue(6);
                model.YearName = row.ItemArray.GetValue(7).ToString();
                model.CarbonNumber = (float)row.ItemArray.GetValue(8);
                //model.CarbonCreateUser = (int)row.ItemArray.GetValue(7);
                model.CarbonCreateTime = row.ItemArray.GetValue(9).ToString();
                if (row.ItemArray.GetValue(10).ToString() == "")
                {
                    model.CarbonModifyTime = "N/A";
                }
                else
                {
                    model.CarbonModifyTime = row.ItemArray.GetValue(10).ToString();
                }
                list.Add(model);
            }
            return list;
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
        /// 取得國家有的車種數據 - 用在新增
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<MgtDetailedCarbonModel> GetCountryCarsType(MgtDetailedCarbonModel Model)
        {
            //SQL GetCarsType
            var sqlStr = string.Format(
                "SELECT CountryCarsTypeNo, CarsTypeName, a.CarsTypeNo FROM evtrend.`country_carstype` as a " +
                "inner join evtrend.`countries` as b " +
                "on a.CountryNo = b.CountryNo " +
                "inner join evtrend.`cars_type` as c " +
                "on a.CarsTypeNo = c.CarsTypeNo " +
                "WHERE a.CountryNo = {0} " +
                "ORDER BY a.CarsTypeNo ASC", SqlVal2(Model.CountryNo));
            var data = _DB_GetData(sqlStr);
            List<MgtDetailedCarbonModel> list = new List<MgtDetailedCarbonModel>();
            foreach (DataRow row in data.Rows)
            {
                MgtDetailedCarbonModel model = new MgtDetailedCarbonModel();

                model.CarbonCountryCarsTypeNo = (int)row.ItemArray.GetValue(0);
                model.CarsTypeName = row.ItemArray.GetValue(1).ToString();
                model.CarsTypeNo = (int)row.ItemArray.GetValue(2);
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 新增細部碳排量數據
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool InsertDetailedCarbon(MgtDetailedCarbonModel Model)
        {
            //admin check
            if (getUserStatusNo() != "0")
            {
                return false;
            }

            // 檢查是否重複新增
            var sqlSelect = string.Format("SELECT 1 from evtrend.`carbon` " +
                "WHERE CarbonCountryCarsTypeNo={0} and CarbonYear={1}", SqlVal2(Model.CarbonCountryCarsTypeNo), SqlVal2(Model.CarbonYear));

            var dataSelect = _DB_GetData(sqlSelect);
            if (dataSelect.Rows.Count > 0)
            {
                return false;
            }


            //SQL Insert DetailCarbon
            var sqlStr = string.Format(
                @"INSERT INTO evtrend.`carbon` (" +
                    "CarbonCountryCarsTypeNo," +
                    "CarbonYear," +
                    "CarbonNumber," +
                    "CarbonCreateTime" +
                ")VALUES(" +
                    "{0}," +
                    "{1}," +
                    "{2}," +
                    "{3}",
                    SqlVal2(Model.CarbonCountryCarsTypeNo),
                    SqlVal2(Model.CarbonYear),
                    SqlVal2(Model.CarbonNumber),
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
        /// 修改細部碳排量數據
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool UpdateDetailedCarbon(MgtDetailedCarbonModel Model)
        {
            // admin check
            if (getUserStatusNo() != "0")
            {
                return false;
            }
            var sqlStr = string.Format("UPDATE evtrend.`carbon` " +
                "SET CarbonNumber = {0} " +
                ",CarbonModifyTime  = {1} " +
                "WHERE " +
                "CarbonNo={2}",
                SqlVal2(Model.CarbonNumber),
                DBC.ChangeTimeZone(),
                SqlVal2(Model.CarbonNo));

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
        /// 刪除細部碳排量數據
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool DeleteTotalCarbon(MgtDetailedCarbonModel Model)
        {

            // admin check
            if (getUserStatusNo() != "0")
            {
                return false;
            }

            var sqlStr = string.Format("DELETE FROM evtrend.`carbon`" +
                "WHERE CarbonNo={0} ",
                SqlVal2(Model.CarbonNo));

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

            if (Model.YearNo.ToString() != null && Model.YearNo.ToString() != "-1")
            {
                if (GetMgtDetailedCarbon(Model.YearNo).Count > 0)
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
                if (GetMgtDetailedCarbon().Count > 0)
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