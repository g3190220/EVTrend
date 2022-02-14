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

    public class TotalElecCarbonController : _BaseController
    {


        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            ViewData["TotalElec"] = GetTotalElecModel();
            ViewData["TotalCarbon"] = GetTotalCarbonModel();

            return View("TotalElecCarbon");
            
           

        }


        private DataTable GetTotalElec()
        {
            var sqlStr = string.Format(
                "SELECT YearName, CountryNo, CountryName,T_RegisterNumber,TotalRegisterNumber FROM evtrend.`total_registercar` as a " +
                "inner join evtrend.`countries` as b " +
                "on a.TotalRegisterCountryNo = b.CountryNo " +
                "inner join evtrend.`years` as c " +
                "on a.TotalRegisterYear = c.YearNo " +
                "ORDER BY CountryName,YearName ASC");
            var data = _DB_GetData(sqlStr);
            return data;
        }

        private DataTable GetTotalCarbon()
        {
            var sqlStr = string.Format(
                "SELECT YearName, CountryNo, CountryName,T_CarbonNumber,TotalCarbonNumber FROM evtrend.`total_carbon` as a " +
                "inner join evtrend.`countries` as b " +
                "on a.TotalCarbonCountryNo = b.CountryNo " +
                "inner join evtrend.`years` as c " +
                "on a.TotalCarbonYear = c.YearNo " +
                "ORDER BY CountryName,YearName ASC");
            var data = _DB_GetData(sqlStr);
            return data;
        }
        

        [HttpGet]
        public List<TotalElecModel> GetTotalElecModel()
        {
            var data = GetTotalElec();
            List<TotalElecModel> list = new List<TotalElecModel>();
            foreach (DataRow row in data.Rows)
            {
                TotalElecModel model = new TotalElecModel(); 
                model.Year = row.ItemArray.GetValue(0).ToString();
                model.CountryNo = (int) row.ItemArray.GetValue(1);
                model.Country = row.ItemArray.GetValue(2).ToString();
                model.ElecRegisterNumber = (float)row.ItemArray.GetValue(3);
                model.TotalRegisterNumber = (float)row.ItemArray.GetValue(4);

                //計算電動車數量/總車種數量 => 得到百分比
                var CarPercentage = (float)row.ItemArray.GetValue(3) / (float)row.ItemArray.GetValue(4);
                model.Percentage = (float)Math.Round((Decimal)CarPercentage, 5) * 1000;

                list.Add(model);
            }
            return list;
        }

        [HttpGet]
        public List<TotalCarbonModel> GetTotalCarbonModel()
        {
            var data = GetTotalCarbon();
            List<TotalCarbonModel> list = new List<TotalCarbonModel>();
            foreach (DataRow row in data.Rows)
            {
                TotalCarbonModel model = new TotalCarbonModel();
                model.Year = row.ItemArray.GetValue(0).ToString();
                model.CountryNo = (int)row.ItemArray.GetValue(1);
                model.Country = row.ItemArray.GetValue(2).ToString();
                model.T_CarbonNumber = (float)row.ItemArray.GetValue(3);
                model.TotalCarbonNumber = (float)row.ItemArray.GetValue(4);

                //計算運輸業碳排數量/總碳排數量 => 得到百分比
                var CarbonPercentage = (float)row.ItemArray.GetValue(3) / (float)row.ItemArray.GetValue(4);
                model.Percentage = (float)Math.Round((Decimal)CarbonPercentage, 5) * 100;

                list.Add(model);
            }
            return list;
        }


        /// <summary>
        /// 取得數據用來畫圖
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDraw()
        {
            //探排量
            List<double?> T_carbon_percentage = new List<double?>(); //台灣 carbon_percentage
            List<double?> A_carbon_percentage = new List<double?>(); //美國 carbon_percentage
            List<int> carbon_year = new List<int>(); //存放Carbon年分

            


            //電動車
            List<double?> T_elec_percentage = new List<double?>(); //elec_percentage
            List<double?> A_elec_percentage = new List<double?>(); //elec_percentage
            List<int> elec_year = new List<int>(); //存放Elec年分

            //取得carbon和elec的資料
            List<TotalCarbonModel> list1 = GetTotalCarbonModel();
            List<TotalElecModel> list2 = GetTotalElecModel();
            IEnumerable<TotalCarbonModel> DCs = list1;
            IEnumerable<TotalElecModel> DEs = list2;

            List<TotalCarbonModel> list_TC = new List<TotalCarbonModel>();
            List<TotalCarbonModel> list_AC = new List<TotalCarbonModel>();
            List<TotalElecModel> list_TE = new List<TotalElecModel>();
            List<TotalElecModel> list_AE = new List<TotalElecModel>();

            //總探排資料，將台灣與美國資料分開
            foreach (var current in DCs)
            {
                if(current.CountryNo == 1)
                {
                    list_AC.Add(current);
                }
                else if (current.CountryNo == 2)
                {
                    list_TC.Add(current);
                }
                carbon_year.Add(Int32.Parse(current.Year));    
            }
            //取得正確年份區間
            carbon_year = carbon_year.Distinct().ToList();
            carbon_year.Sort();



            foreach (var this_year in carbon_year)
            {
                bool isExists = list_AC.Exists(x => Int32.Parse(x.Year) == this_year);

                if (isExists)
                {
                    TotalCarbonModel result = list_AC.Find(x => Int32.Parse(x.Year) == this_year);
                    A_carbon_percentage.Add(result.Percentage);
                }
                else //代表他沒有那個年份
                {
                    A_carbon_percentage.Add(null);
                }

                bool isExists2 = list_TC.Exists(x => Int32.Parse(x.Year) == this_year);
                if (isExists2)
                {
                    TotalCarbonModel result = list_TC.Find(x => Int32.Parse(x.Year) == this_year);
                    T_carbon_percentage.Add(result.Percentage);
                }
                else //代表他沒有那個年份
                {
                    T_carbon_percentage.Add(null);
                }

            }


            //總電動車資料
            foreach (var current in DEs)
            {
                if (current.CountryNo == 1)
                {
                    list_AE.Add(current);
                }
                else if (current.CountryNo == 2)
                {
                    list_TE.Add(current);
                }
                elec_year.Add(Int32.Parse(current.Year));
            }
            //取得正確年份區間
            elec_year = elec_year.Distinct().ToList();
            elec_year.Sort();


            foreach (var this_year in elec_year)
            {
                bool isExists = list_AE.Exists(x => Int32.Parse(x.Year) == this_year);
                if (isExists)
                {
                    TotalElecModel result = list_AE.Find(x => Int32.Parse(x.Year) == this_year);
                    A_elec_percentage.Add(result.Percentage);
                }
                else //代表他沒有那個年份
                {
                    A_elec_percentage.Add(null);
                }

                bool isExists2 = list_TE.Exists(x => Int32.Parse(x.Year) == this_year);
                if (isExists2)
                {
                    TotalElecModel result = list_TE.Find(x => Int32.Parse(x.Year) == this_year);
                    T_elec_percentage.Add(result.Percentage);
                }
                else //代表他沒有那個年份
                {
                    T_elec_percentage.Add(null);
                }

            }
  
            return Json(new {
                a_carbon_percentage = A_carbon_percentage, 
                t_carbon_percentage = T_carbon_percentage, 
                carbon_year = carbon_year, 
                a_elec_percentage = A_elec_percentage, 
                t_elec_percentage = T_elec_percentage, 
                elec_year = elec_year}); ;
        }
    }
}