using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EVTrend.Areas.News.Models
{
    public class NewsModel
    {
        [Display(Name = "�����s��")]
        public int NewsNo { get; set; }

        [Display(Name = "��������")]
        public int NewsTypeNo { get; set; }

        [Display(Name = "���D")]
        public string NewsTitle { get; set; }

        [Display(Name = "���e")]
        public string NewsContent { get; set; }

        [Display(Name = "Ĳ�Φ���")]
        public int NewsHits { get; set; }

        [Display(Name = "�Х߮ɶ�")]
        public DateTime CreateTime { get; set; }

        [Display(Name = "�ק�ɶ�")]
        public DateTime ModifyTime { get; set; }

        [Display(Name = "��������")]
        public DateTime NewsEnd { get; set; }

        // [Display(Name = "�����Хߪ�")]
        // public string NewsCreateUser { get; set; }

        // [Display(Name = "�����ק��")]
        //  public string NewsModifyUser { get; set; }

        [Display(Name = "�����s��")]
        public string NewsLink { get; set; }
    }
    
    public class NewsTypeModel
    {
        [Display(Name = "���������s��")]
        public int NewsTypeNo { get; set; }

        [Display(Name = "���������W��")]
        public string TypeName { get; set; }

        [Display(Name = "���������y�z")]
        public string TypeDescription { get; set; }

        [Display(Name = "�Х߮ɶ�")]
        public DateTime CreateTime { get; set; }

        [Display(Name = "�ק�ɶ�")]
        public DateTime ModifyTime { get; set; }
    }

    public class NewsPageModel
    {
        public List<NewsModel> News { get; } = new List<NewsModel>();
        public List<NewsTypeModel> NewsTypes { get; } = new List<NewsTypeModel>();

        public Dictionary<int, NewsTypeModel> NewsTypesDictionary { get; } = new Dictionary<int, NewsTypeModel>();
    }
}
