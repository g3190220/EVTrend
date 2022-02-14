using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EVTrend.Areas.Content.Models
{//YearName, CarsTypeName,CarbonNumber
    public class DetailedCarbonModel
    {
        public string YearName { get; set; }
        public string CarsTypeName { get; set; }
        public float CarbonNumber { get; set; }



    }
    public class MgtDetailedCarbonModel : DetailedCarbonModel
    {
        public int CarbonNo { get; set; }
        public int CarbonCountryCarsTypeNo { get; set; }
        public int CountryNo { get; set; }
        public string CountryName { get; set; }
        public int CarsTypeNo { get; set; }
        public int CarbonYear { get; set; }

        //public int CarbonCreateUser { get; set; }
        //**
        public string CarbonCreateTime { get; set; }
        public string CarbonModifyTime { get; set; }

    }



}
