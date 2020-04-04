using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.Migrations
{
    public partial class AddData4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Name = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    LegalPerson = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Mayors",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    name = table.Column<string>(nullable: true),
                    sex = table.Column<int>(nullable: false),
                    CityID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mayors", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Mayors_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityCompanies",
                columns: table => new
                {
                    CityID = table.Column<int>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityCompanies", x => new { x.CityID, x.CompanyID });
                    table.ForeignKey(
                        name: "FK_CityCompanies_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityCompanies_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityCompanies_CompanyID",
                table: "CityCompanies",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Mayors_CityID",
                table: "Mayors",
                column: "CityID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CityCompanies");

            migrationBuilder.DropTable(
                name: "Mayors");

            migrationBuilder.DropTable(
                name: "Companies");

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
    }
}
