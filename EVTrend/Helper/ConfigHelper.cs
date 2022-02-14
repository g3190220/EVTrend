using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVTrend.Helper;

namespace EVTrend.Helper
{
    public class ConfigHelper
    {
        public static class AppSettings
        {
            /// <summary>
            /// 目前用不到此Class
            /// </summary>
            public enum AppSettingsClass {
                /// <summary>
                /// 系統別
                /// </summary>
                System,
                /// <summary>
                /// 通用
                /// </summary>
                Common,
            }

            /// <summary>
            /// 參數對比
            /// </summary>
            public enum AppSettingsKey {
                /// <summary>
                /// 系統時區
                /// </summary>
                SystemTimeZone,
                /// <summary>
                /// DB 連線字串
                /// </summary>
                Core_ConnectionStrings,
            }

            /// <summary>
            /// 系統時區
            /// </summary>
            public static string SystemTimeZone { get { return ConfigHelperMethods.GetConfig(AppSettingsKey.SystemTimeZone); } }

            /// <summary>
            /// DB 連線字串
            /// </summary>
            public static string ConnectionStrings { get { return ConfigHelperMethods.GetConfig(AppSettingsKey.Core_ConnectionStrings); } }
        }
    }
}
