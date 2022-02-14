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

    public class MgtDetailedElecController : _BaseController
    {
        /// <summary>
        /// 細部電動車數據管理View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(MgtTotalElecModel Model)
        {
            if (getUserStatusNo() != "0")
            {
                return Redirect("~/Home/Error");
            }
            ViewData["GetMgtDetailedElec"] = GetMgtDetailedElec(Model.YearNo);
            ViewData["GetYear"] = GetYear();
            ViewData["GetCountry"] = GetCountry();
            ViewData["SelectYearNo"] = Model.YearNo;

            

            return View("MgtDetailedElec");
        }
        /// <summary>
        /// 取得細部碳排量數據
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<MgtDetailedElecModel> GetMgtDetailedElec(int YearNo = 0)
        {
            var sqlStr = "";
            if (YearNo == 0) //選擇全部
            {
                sqlStr = string.Format(
                "SELECT RegisterCarNo, RegisterCarCountryCarsTypeNo, CountryName, c.CountryNo, CarsTypeName, c.CarsTypeNo, YearNo, YearName ,ElecNumber,TotalNumber,RegisterCarCreateTime,RegisterCarModifyTime FROM evtrend.`register_car` as a " +
                "inner join evtrend.`years` as b " +
                "on a.RegisterCarYear = b.YearNo " +
                "inner join evtrend.`country_carstype` as c " +
                "on a.RegisterCarCountryCarsTypeNo = c.CountryCarsTypeNo " +
                "inner join evtrend.`countries` as d " +
                "on c.CountryNo = d.CountryNo " +
                "inner join evtrend.`cars_type` as e " +
                "on c.CarsTypeNo = e.CarsTypeNo " +
                "ORDER BY RegisterCarYear DESC");
            }
            else //選擇特定年份
            {
                sqlStr = string.Format(
                "SELECT RegisterCarNo, RegisterCarCountryCarsTypeNo, CountryName, c.CountryNo, CarsTypeName, c.CarsTypeNo, YearNo, YearName ,ElecNumber,TotalNumber,RegisterCarCreateTime,RegisterCarModifyTime FROM evtrend.`register_car` as a " +
                "inner join evtrend.`years` as b " +
                "on a.RegisterCarYear = b.YearNo " +
                "inner join evtrend.`country_carstype` as c " +
                "on a.RegisterCarCountryCarsTypeNo = c.CountryCarsTypeNo " +
                "inner join evtrend.`countries` as d " +
                "on c.CountryNo = d.CountryNo " +
                "inner join evtrend.`cars_type` as e " +
                "on c.CarsTypeNo = e.CarsTypeNo " +
                "WHERE YearNo = {0} " +
                "ORDER BY RegisterCarYear DESC", SqlVal2(YearNo));
            }
            var data = _DB_GetData(sqlStr);
            List<MgtDetailedElecModel> list = new List<MgtDetailedElecModel>();
            foreach (DataRow row in data.Rows)
            {
                MgtDetailedElecModel model = new MgtDetailedElecModel();

                model.RegisterCarNo = (int)row.ItemArray.GetValue(0);
                model.RegisterCarCountryCarsTypeNo = (int)row.ItemArray.GetValue(1);
                model.CountryName = row.ItemArray.GetValue(2).ToString();
                model.CountryNo = (int)row.ItemArray.GetValue(3);
                model.CarsTypeName = row.ItemArray.GetValue(4).ToString();
                model.CarsTypeNo = (int)row.ItemArray.GetValue(5);
                model.RegisterCarYear = (int)row.ItemArray.GetValue(6);
                model.YearName = row.ItemArray.GetValue(7).ToString();
                model.ElecNumber = (float)row.ItemArray.GetValue(8);
                model.TotalNumber = (float)row.ItemArray.GetValue(9);
                model.RegisterCarCreateTime = row.ItemArray.GetValue(10).ToString();
                if (row.ItemArray.GetValue(11).ToString() == "")
                {
                    model.RegisterCarModifyTime = "N/A";
                }
                else
                {
                    model.RegisterCarModifyTime = row.ItemArray.GetValue(11).ToString();
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
        public List<MgtDetailedElecModel> GetCountryCarsType(MgtDetailedElecModel Model)
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
            List<MgtDetailedElecModel> list = new List<MgtDetailedElecModel>();
            foreach (DataRow row in data.Rows)
            {
                MgtDetailedElecModel model = new MgtDetailedElecModel();

                model.CarsTypeNo = (int)row.ItemArray.GetValue(0);
                model.CarsTypeName = row.ItemArray.GetValue(1).ToString();
                model.CarsTypeNo = (int)row.ItemArray.GetValue(2);
                list.Add(model);
            }
            return list;
        }


        /// <summary>
        /// 新增細部電動車數據
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool InsertDetailedElec(MgtDetailedElecModel Model)
        {
            //admin check
            if (getUserStatusNo() != "0")
            {
                return false;
            }
            //取得 RegisterCarCountryCarsTypeNo
            var sqlstr1 = string.Format("SELECT CountryCarsTypeNo FROM evtrend.country_carstype WHERE CountryNo = {0} AND CarsTypeNo = {1}", SqlVal2(Model.CountryNo), SqlVal2(Model.CarsTypeNo));
            var return_dt = _DB_GetData(sqlstr1);
            var RegisterCarCountryCarsTypeNo = return_dt.Rows[0][0].ToString();

            // 檢查是否重複新增
            var sqlstr2 = string.Format("SELECT 1 from evtrend.`register_car` " +
                "WHERE RegisterCarCountryCarsTypeNo={0} and RegisterCarYear={1}", RegisterCarCountryCarsTypeNo, SqlVal2(Model.RegisterCarYear));

            var dataSelect = _DB_GetData(sqlstr2);
            if (dataSelect.Rows.Count > 0)
            {
                return false;
            }


            //SQL Insert TotalElec
            var sqlStr3 = string.Format(
                @"INSERT INTO evtrend.`register_car` (" +
                    "RegisterCarCountryCarsTypeNo," +
                    "RegisterCarYear," +
                    "ElecNumber," +
                    "TotalNumber," +
                    "RegisterCarCreateTime" +
                ")VALUES(" +
                    "{0}," +
                    "{1}," +
                    "{2}," +
                    "{3}," +
                    "{4}",
                    RegisterCarCountryCarsTypeNo,
                    SqlVal2(Model.RegisterCarYear),
                    SqlVal2(Model.ElecNumber),
                    SqlVal2(Model.TotalNumber),
                    DBC.ChangeTimeZone() + ")"
                );

            //SQL Check
            var check = _DB_Execute(sqlStr3);

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
        /// 更新細部電動車數據
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool UpdateDetailedElec(MgtDetailedElecModel Model)
        {
            // admin check
            //if (getUserStatusNo() != "0")
            //{
            //    return false;
            //}
            var sqlStr = string.Format("UPDATE evtrend.`register_car` " +
                "SET ElecNumber = {0} ,TotalNumber ={1} " +
                ",RegisterCarModifyTime  = {2} " +
                "WHERE " +
                "RegisterCarNo={3}",
                SqlVal2(Model.ElecNumber),
                SqlVal2(Model.TotalNumber),
                DBC.ChangeTimeZone(),
                SqlVal2(Model.RegisterCarNo));

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
        /// 刪除細部電動車數據
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool DeleteDetailedElec(MgtDetailedElecModel Model)
        {

            // admin check
            if (getUserStatusNo() != "0")
            {
                return false;
            }

            var sqlStr = string.Format("DELETE FROM evtrend.`register_car`" +
                "WHERE RegisterCarNo={0} ",
                SqlVal2(Model.RegisterCarNo));

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
        public bool SelectCountryNo(MgtTotalElecModel Model)
        {
            // admin check
            if (getUserStatusNo() != "0")
            {
                return false;
            }

            if (Model.YearNo.ToString() != null && Model.YearNo.ToString() != "-1")
            {
                if (GetMgtDetailedElec(Model.YearNo).Count > 0)
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
                if (GetMgtDetailedElec().Count > 0)
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