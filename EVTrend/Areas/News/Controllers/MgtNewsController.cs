using EVTrend.Areas.News.Models;
using EVTrend.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EVTrend.Areas.News.Controllers
{
    [Area(areaName: "News")]

    public class MgtNewsController : _BaseController
    {
        public IActionResult Index()
        {
            if (getUserStatusNo() == "0")
            {
                return View("MgtNews", GetNewsPageModel());
            }
            else
            {
                return Redirect("~/Home/Error");
            }
        }

        private DataTable GetAllNews()
        {
            var sqlStr = string.Format("SELECT NewsNo, NewsTypeNo, NewsTitle, NewsContent, NewsHits, CreateTime, ModifyTime, NewsEnd from evtrend.`news`");
            var data = _DB_GetData(sqlStr);
            return data;
        }
        private DataTable GetAllNewsTypes()
        {
            var sqlStr = string.Format("SELECT NewsTypeNo, TypeName, TypeDescription, CreateTime, ModifyTime from evtrend.`news_type`");
            var data = _DB_GetData(sqlStr);
            return data;
        }

        private NewsModel ConvertRowToNewsModel(DataRow row)
        {
            var news = new NewsModel();

            news.NewsNo = (int)row.ItemArray.GetValue(0);
            news.NewsTypeNo = (int)row.ItemArray.GetValue(1);
            news.NewsTitle = row.ItemArray.GetValue(2).ToString();
            news.NewsContent = row.ItemArray.GetValue(3).ToString();
            news.NewsHits = (int)row.ItemArray.GetValue(4);
            news.CreateTime = DateTime.Parse(row.ItemArray.GetValue(5).ToString());
            if (row.ItemArray.GetValue(6).ToString() != "")
            {
                news.ModifyTime = DateTime.Parse(row.ItemArray.GetValue(6).ToString());
            }
            // news.NewsEnd = DateTime.Parse(row.ItemArray.GetValue(7).ToString());
            // news.NewsCreateUser = row.ItemArray.GetValue(8).ToString();
            // news.NewsModifyUser = row.ItemArray.GetValue(9).ToString();

            return news;
        }
        private NewsTypeModel ConvertRowToNewsTypeModel(DataRow row)
        {
            var newsType = new NewsTypeModel();

            newsType.NewsTypeNo = (int)row.ItemArray.GetValue(0);
            newsType.TypeName = row.ItemArray.GetValue(1).ToString();
            newsType.TypeDescription = row.ItemArray.GetValue(2).ToString();
            newsType.CreateTime = DateTime.Parse(row.ItemArray.GetValue(3).ToString());            
            if (row.ItemArray.GetValue(4).ToString() != "")
            {
                newsType.ModifyTime = DateTime.Parse(row.ItemArray.GetValue(4).ToString());
            }
            return newsType;
        }

        private NewsPageModel GetNewsPageModel()
        {
            var newsPageModel = new NewsPageModel();

            var news = GetAllNews();
            var newsTypes = GetAllNewsTypes();
            foreach (DataRow row in news.Rows)
            {
                newsPageModel.News.Add(ConvertRowToNewsModel(row));
            }
            foreach (DataRow row in newsTypes.Rows)
            {
                var newsTypeModel = ConvertRowToNewsTypeModel(row);
                newsPageModel.NewsTypes.Add(newsTypeModel);
                newsPageModel.NewsTypesDictionary[newsTypeModel.NewsTypeNo] = newsTypeModel;
            }

            return newsPageModel;
        }

        public bool UpdateNews(NewsModel Model)
        {
            if (getUserStatusNo() != "0")
            {
                return false;
            }

            var sqlStr = string.Format("UPDATE news " +
                "SET NewsTitle = {0}, " +
                "NewsContent  = {1}," +
                "ModifyTime = {2}," +
                "NewsTypeNo = {4}" +
                "WHERE NewsNo = {3}",
                SqlVal2(Model.NewsTitle),
                SqlVal2(Model.NewsContent),
                DBC.ChangeTimeZone(),
                SqlVal2(Model.NewsNo),
                SqlVal2(Model.NewsTypeNo));

            var check = _DB_Execute(sqlStr);

            if (check == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteNews(NewsModel Model)
        {
            if (getUserStatusNo() != "0")
            {
                return false;
            }

            var sqlStr = string.Format("DELETE FROM news WHERE NewsNo = {0}", SqlVal2(Model.NewsNo));

            var check = _DB_Execute(sqlStr);

            if (check == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public bool AddNews(NewsModel Model)
        {
            string resMsg = "";
            string checkMsg = "";

            // 長度限制
            if (string.IsNullOrEmpty(Model.NewsTitle) ||
                string.IsNullOrEmpty(Model.NewsContent) ||
                Model.NewsContent.Length > 200 ||
                Model.NewsTitle.Length > 50)
            {

                resMsg = "標題或內容不符合長度限制!! 標題與內容不可為空，且標題要在50字內，內容不可超過200字";

                checkMsg = "false";

            }
            else
            {
                var sqlStr = string.Format(
                    @"INSERT INTO news (" +
                        "NewsContent," +
                        "NewsTitle," +
                        "NewsTypeNo," +
                        //"NewsCreateUser," +
                        "CreateTime," +
                        "NewsLink" +
                    ")VALUES(" +
                        "{0}," +
                        "{1}," +
                        "{4}," +
                        //"2," +
                        "{2}," +
                        "{3}",
                        SqlVal2(Model.NewsContent),
                        SqlVal2(Model.NewsTitle),
                        DBC.ChangeTimeZone(),
                        SqlVal2(Model.NewsLink) + ")",
                        SqlVal2(Model.NewsTypeNo)
                    );
            
                var check = _DB_Execute(sqlStr);

                if (check == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}