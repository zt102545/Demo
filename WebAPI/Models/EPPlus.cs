using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public enum Gender
    {
        男, 女
    }
    public class EPPlus
    {
        //用户名
        [StringLength(20, ErrorMessage = "用户名超出范围")]
        public string Name { get; set; }
        //年龄
        [Range(0, 120, ErrorMessage = "年龄范围在0到120岁之间")]
        public int Age { get; set; }
        //性别
        public Gender Gender { get; set; }
        [Range(0, 750, ErrorMessage = "总成绩在0-750")]
        public double? Achievement { get; set; }

    }
}
