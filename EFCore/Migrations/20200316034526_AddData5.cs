using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.Migrations
{
    public partial class AddData5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "ID", "name", "population" },
                values: new object[] { 1, "广东", 9000000 });

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "ID", "name", "population" },
                values: new object[] { 2, "福建", 8000000 });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "ID", "ProvinceID", "areaCode", "name" },
                values: new object[] { 1, 1, null, "汕头" });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "ID", "ProvinceID", "areaCode", "name" },
                values: new object[] { 2, 1, null, "广州" });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "ID", "ProvinceID", "areaCode", "name" },
                values: new object[] { 3, 1, null, "深圳" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "ID",
                keyValue: 1);
        }
    }
}
