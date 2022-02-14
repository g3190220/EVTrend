using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EVTrend.Models
{
    /// <summary>
    /// 身分類型MODEL
    /// </summary>
    public class StatusModels : SetResult
    {
        //身分類型編號
        public string StatusNo { get; set; }

        //身分類型描述
        public string StatusName { get; set; }
    }

    /// <summary>
    /// 帳號MODEL
    /// </summary>
    public class AccountModels : StatusModels
    {
        //使用者編號
        public string MemberNo { get; set; }

        [Display(Name = "帳號")]
        public string Account { get; set; }

        //密碼
        public string Password { get; set; }

        //再次確認密碼
        public string PasswordCheck { get; set; }

        [Display(Name = "暱稱")]
        public string Username { get; set; }
    }

    /// <summary>
    /// 會員MODEL
    /// </summary>
    public class Member : AccountModels
    {
        [Display(Name = "性別")]
        public string Gender { get; set; }

        [Display(Name = "生日")]
        public DateTime Birthday { get; set; }

        //帳號建立時間
        public DateTime CreateTime { get; set; }

        //帳號修改時間
        public DateTime? ModifyTime { get; set; }

        //帳號啟用時間
        public DateTime AccountStart { get; set; }

        //帳號停用時間
        public DateTime? AccountEnd { get; set; }

        public List<SelectListItem> Genders { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "男", Text = "男" },
            new SelectListItem { Value = "女", Text = "女"  },
            new SelectListItem { Value = "不願透漏", Text = "不願透漏"  },
        };
    }

    /// <summary>
    /// 執行成功與否 檢測用Model
    /// </summary>
    public class SetResult
    {
        public SetResult()
        {
            //預設
            ok = true;
            ResultMessage = "";
        }

        //是否成功
        public bool ok { get; set; }

        //結果訊息
        public string ResultMessage { get; set; }
    }
}