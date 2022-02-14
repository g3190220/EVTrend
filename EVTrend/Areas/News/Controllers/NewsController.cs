using EVTrend.Areas.News.Models;
using EVTrend.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Globalization;

namespace EVTrend.Areas.News.Controllers
{
    [Area(areaName: "News")]

    public class NewsController : _BaseController
    {
        public IActionResult Index()
        {
            return View("ShowNews", GetNewsPageModel());
        }

        private DataTable GetAllNews()
        {
            var sqlStr = string.Format("SELECT NewsNo, NewsTypeNo, NewsTitle, NewsContent, NewsHits, CreateTime, ModifyTime, NewsEnd, NewsLink from evtrend.`news`");
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
            news.NewsLink = row.ItemArray.GetValue(8).ToString();

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

        private DataTable GetAllNewsTypes()
        {
            var sqlStr = string.Format("SELECT NewsTypeNo, TypeName, TypeDescription, CreateTime, ModifyTime from evtrend.`news_type`");
            var data = _DB_GetData(sqlStr);
            return data;
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

        public bool Hit(NewsModel Model)
        {
            var hits = Model.NewsHits + 1;
            var sqlStr = string.Format("UPDATE news " +
                "SET NewsHits = {0} " +
                "WHERE NewsNo = {1}",
                SqlVal2(hits),
                SqlVal2(Model.NewsNo));

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
    }
}