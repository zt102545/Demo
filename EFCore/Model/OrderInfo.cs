using MySql.Data.EntityFrameworkCore.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Model
{
    [MySqlCharset("utf8mb4")] //字符集,需要引用MySql.Data.EntityFrameworkCore
    [MySqlCollation("utf8mb4_general_ci")] //排序规则
    public class OrderInfo
    {

        [Required]
        [Column("id", TypeName = "int(11)")]
        public int Id { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        [Required]
        [Column("order_id", TypeName = "int(11)")]
        public int OrderId { get; set; }

        [Column("passenger_id", TypeName = "int(11)")]
        public int PassengerId { get; set; }



        [Column("address_id", TypeName = "int(11)")]
        public int AddressId { get; set; }

        /// <summary>
        /// 订单价格
        /// </summary>
        [StringLength(maximumLength: 100)]
        [Column("price")]
        public string Price { get; set; }

        /// <summary>
        /// 订单客人信息
        /// </summary>
        [ForeignKey("PassengerId")]
        public Passenger OrderForPassenger { get; set; }

        ///// <summary>
        ///// 订单地址信息
        ///// </summary>
        //[InverseProperty("OrderInfos")]
        //public Address OrderForAddress { get; set; }
    }
}
