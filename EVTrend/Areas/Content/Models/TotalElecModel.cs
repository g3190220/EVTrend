using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EVTrend.Areas.Content.Models
{
    public class TotalElecModel
    {
        public string Year { get; set; }
        public string Country { get; set; }
        public int CountryNo { get; set; }
        public float ElecRegisterNumber { get; set; }
        public float TotalRegisterNumber { get; set; }
        public float Percentage { get; set; }


    }
    public class MgtTotalElecModel:TotalElecModel
    {
        
        public int TotalRegisterNo { get; set; }
        public int YearNo { get; set; }
        //public int CountryNo { get; set; }
        public string CreateTime { get; set; }
        public string ModifyTime { get; set; }

    }


    //用在年份清單
    public class TimeModel
    {
        public int YearNo { get; set; }
        public string YearName { get; set; }
    }

    //用在國家清單
    public class CountryModel
    {
        public int CountryNo { get; set; }
        public string CountryName { get; set; }
    }


}