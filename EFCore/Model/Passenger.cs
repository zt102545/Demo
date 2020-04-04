using MySql.Data.EntityFrameworkCore.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Model
{
    [MySqlCharset("utf8mb4")] //字符集,需要引用MySql.Data.EntityFrameworkCore
    [MySqlCollation("utf8mb4_general_ci")] //排序规则
    public class Passenger
    {

        [Column("id", TypeName = "int(10)")]
        public int Id { get; set; }

        [Column("passengerid", TypeName = "int(11)")]
        public int PassengerId { get; set; }

        public string PassengerName { get; set; }

        [InverseProperty("OrderForPassenger")]
        public List<OrderInfo> OrderInfos { get; set; }
    }
}
