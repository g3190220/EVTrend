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
    /// ��������MODEL
    /// </summary>
    public class StatusModels : SetResult
    {
        //���������s��
        public string StatusNo { get; set; }

        //���������y�z
        public string StatusName { get; set; }
    }

    /// <summary>
    /// �b��MODEL
    /// </summary>
    public class AccountModels : StatusModels
    {
        //�ϥΪ̽s��
        public string MemberNo { get; set; }

        [Display(Name = "�b��")]
        public string Account { get; set; }

        //�K�X
        public string Password { get; set; }

        //�A���T�{�K�X
        public string PasswordCheck { get; set; }

        [Display(Name = "�ʺ�")]
        public string Username { get; set; }
    }

    /// <summary>
    /// �|��MODEL
    /// </summary>
    public class Member : AccountModels
    {
        [Display(Name = "�ʧO")]
        public string Gender { get; set; }

        [Display(Name = "�ͤ�")]
        public DateTime Birthday { get; set; }

        //�b���إ߮ɶ�
        public DateTime CreateTime { get; set; }

        //�b���ק�ɶ�
        public DateTime? ModifyTime { get; set; }

        //�b���ҥήɶ�
        public DateTime AccountStart { get; set; }

        //�b�����ήɶ�
        public DateTime? AccountEnd { get; set; }

        public List<SelectListItem> Genders { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "�k", Text = "�k" },
            new SelectListItem { Value = "�k", Text = "�k"  },
            new SelectListItem { Value = "���@�z�|", Text = "���@�z�|"  },
        };
    }

    /// <summary>
    /// ���榨�\�P�_ �˴���Model
    /// </summary>
    public class SetResult
    {
        public SetResult()
        {
            //�w�]
            ok = true;
            ResultMessage = "";
        }

        //�O�_���\
        public bool ok { get; set; }

        //���G�T��
        public string ResultMessage { get; set; }
    }
}