using MySql.Data.EntityFrameworkCore.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace EFCore.Model
{
    [MySqlCharset("utf8mb4")] //字符集,需要引用MySql.Data.EntityFrameworkCore
    [MySqlCollation("utf8mb4_general_ci")] //排序规则
    public class Address
    {

        [Required]//不能为空
        public int Id { get; set; }

        [StringLength(maximumLength: 256)]
        public string Province { get; set; }

        [StringLength(maximumLength: 256)]
        public string City { get; set; }

        [StringLength(maximumLength: 256)]
        public string Area { get; set; }

        [StringLength(maximumLength: 256)]
        public string Street { get; set; }
    }
}
