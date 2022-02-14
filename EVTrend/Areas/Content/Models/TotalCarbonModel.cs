using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EVTrend.Areas.Content.Models
{
    public class TotalCarbonModel
    {
        public string Year { get; set; }
        public string Country { get; set; }
        public int CountryNo { get; set; }
        public float T_CarbonNumber { get; set; }
        public float TotalCarbonNumber { get; set; }
        public float Percentage { get; set; }

    }
    public class MgtTotalCarbonModel : TotalCarbonModel
    {
        public int TotalCarbonNo { get; set; }
        public int YearNo { get; set; }
        //public int CountryNo { get; set; }
        public string CreateTime { get; set; }
        public string ModifyTime { get; set; }

    }
    
}