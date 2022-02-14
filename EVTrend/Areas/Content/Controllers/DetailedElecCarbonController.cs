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

    public class DetailedElecCarbonController : _BaseController
    {
        /// <summary>
        /// 細部碳排量與電動車數據View
        /// </summary>
        /// <returns></returns>
        
        //public IActionResult Index()

        
        public IActionResult Index()
        {
            //ViewData["DetailedCarbon"] = GetDetailCarbonModel();
            //ViewData["DetailedElec"] = GetDetailElecModel();
            var chaeck = getUserStatusNo();
            if (chaeck != "0" && chaeck != "1")
            {
                return Redirect("~/Home/Error");
            }
            return View("DetailedElecCarbon");
        }
        private DataTable GetDetailedCarbon()
        {
            var sqlStr = string.Format(
                "SELECT YearName, CarsTypeName,CarbonNumber FROM evtrend.`carbon` as a " +
                "inner join evtrend.`years` as b " +
                "on a.CarbonYear = b.YearNo " +
                "inner join evtrend.`country_carstype` as c " +
                "on a.CarbonCountryCarsTypeNo = c.CountryCarsTypeNo " +
                "inner join evtrend.`cars_type` as d " +
                "on c.CarsTypeNo = d.CarsTypeNo " +
                "ORDER BY YearName,CarsTypeName ASC");
            var data = _DB_GetData(sqlStr);
            return data;
        }
        private DataTable GetDetailedElec()
        {
            var sqlStr = string.Format(
                "SELECT YearName, CarsTypeName,ElecNumber,TotalNumber FROM evtrend.`register_car` as a " +
                "inner join evtrend.`years` as b " +
                "on a.RegisterCarYear = b.YearNo " +
                "inner join evtrend.`country_carstype` as c " +
                "on a.RegisterCarCountryCarsTypeNo = c.CountryCarsTypeNo " +
                "inner join evtrend.`countries` as d " +
                "on c.CountryNo = d.CountryNo " +
                "inner join evtrend.`cars_type` as e " +
                "on c.CarsTypeNo = e.CarsTypeNo " +
                "ORDER BY YearName,CarsTypeName ASC");
            var data = _DB_GetData(sqlStr);
            return data;
        }
        private List<DetailedCarbonModel> GetDetailCarbonModel()
        {
            var data = GetDetailedCarbon();
            List<DetailedCarbonModel> list = new List<DetailedCarbonModel>();
            foreach (DataRow row in data.Rows)
            {
                DetailedCarbonModel model = new DetailedCarbonModel();

                model.YearName = row.ItemArray.GetValue(0).ToString();
                model.CarsTypeName = row.ItemArray.GetValue(1).ToString();
                model.CarbonNumber = (float)row.ItemArray.GetValue(2);

                list.Add(model);
            }
            return list;
        }
        private List<DetailedElecModel> GetDetailElecModel()
        {
            var data = GetDetailedElec();
            List<DetailedElecModel> list = new List<DetailedElecModel>();
            foreach (DataRow row in data.Rows)
            {
                DetailedElecModel model = new DetailedElecModel();

                model.YearName = row.ItemArray.GetValue(0).ToString();
                model.CarsTypeName = row.ItemArray.GetValue(1).ToString();
                model.ElecNumber = (float)row.ItemArray.GetValue(2);
                model.TotalNumber = (float)row.ItemArray.GetValue(3);

                list.Add(model);
            }
            return list;
        }
        [HttpGet]
        public JsonResult GetDraw(string car_type_id)
        {
            var chaeck = getUserStatusNo();
            if (chaeck != "0" && chaeck != "1")
            {
                return Json(new { message = "error" }); ;
            }
            List<double> data1 = new List<double>();
            List<double> data2 = new List<double>();
            List<double> data3 = new List<double>();
            List<int> data4 = new List<int>();
            List<DetailedCarbonModel> list1 = GetDetailCarbonModel();
            List<DetailedElecModel> list2 = GetDetailElecModel();
            IEnumerable<DetailedCarbonModel> DCs = list1;
            IEnumerable<DetailedElecModel> DEs = list2;
            //current.CarsTypeName == car_type_id || data4.Contains(Int32.Parse(current.YearName))

            foreach (var current in DCs)
            {
                if (current.CarsTypeName == car_type_id)
                {
                    data4.Add(Int32.Parse(current.YearName));
                }
            }
            foreach (var current in DEs)
            {
                if (current.CarsTypeName == car_type_id)
                {
                    if (!data4.Contains(Int32.Parse(current.YearName)))
                    {
                        data4.Add(Int32.Parse(current.YearName));
                    }
                }
            }
            data4.Sort();
            foreach (var current in DCs)
            {
                if (current.CarsTypeName == car_type_id)
                {
                    if (current.CarsTypeName == car_type_id)
                    {
                        data3.Add(current.CarbonNumber);
                    }
                //    else
                //    {
                //        data3.Add(0.0);
                //    }
                }
            }
            foreach (var current in DEs)
            {
                if (current.CarsTypeName == car_type_id)
                {
                        data1.Add(current.TotalNumber);
                        data2.Add(current.ElecNumber);
                }
            }
            //if (car_type_id == "大客車")
            //{
            //    data1 = new List<double> { 1.1, 2.2, 3.3, 4.4 };
            //    data2 = new List<double> { 2.2, 3.3, 4.4, 1.1 };
            //    data3 = new List<double> { 2.2, 3.3, 4.4, 1.1 };
            //}
            //else if (car_type_id == "大貨車")
            //{
            //    data1 = new List<double> { 1.1, 2.2, 3.3, 4.4 };
            //    data2 = new List<double> { 2.2, 3.3, 4.4, 2.2 };
            //    data3 = new List<double> { 2.2, 3.3, 4.4, 2.2 };
            //}
            //else if (car_type_id == "小客車")
            //{
            //    data1 = new List<double> { 1.1, 2.2, 3.3, 4.4 };
            //    data2 = new List<double> { 2.2, 3.3, 4.4, 3.3 };
            //    data3 = new List<double> { 2.2, 3.3, 4.4, 3.3 };
            //}
            //else if (car_type_id == "小貨車")
            //{
            //    data1 = new List<double> { 1.1, 2.2, 3.3, 4.4 };
            //    data2 = new List<double> { 2.2, 3.3, 4.4, 4.4 };
            //    data3 = new List<double> { 2.2, 3.3, 4.4, 4.4 };
            //}
            //else if (car_type_id == "機車")
            //{
            //    data1 = new List<double> { 1.1, 2.2, 3.3, 4.4 };
            //    data2 = new List<double> { 2.2, 3.3, 4.4, 5.5 };
            //    data3 = new List<double> { 2.2, 3.3, 4.4, 5.5 };
            //}
            return Json(new { total_car = data1, elec_car = data2, carbon = data3 ,year=data4 }); ;
        }
    }
}