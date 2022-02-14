using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EVTrend.Areas.Content.Models
{//YearName, CarsTypeName,ElecNumber,TotalNumber
    public class DetailedElecModel
    {
        public string YearName { get; set; }
        public string CarsTypeName { get; set; }
        public float ElecNumber { get; set; }
        public float TotalNumber { get; set; }
    }
    public class MgtDetailedElecModel : DetailedElecModel
    {
        public int RegisterCarNo { get; set; }
        public int RegisterCarCountryCarsTypeNo { get; set; }
        public int CountryNo { get; set; }
        public string CountryName { get; set; }
        public int CarsTypeNo { get; set; }
        public int RegisterCarYear { get; set; }
        public string RegisterCarCreateTime { get; set; }
        public string RegisterCarModifyTime { get; set; }

    }
}