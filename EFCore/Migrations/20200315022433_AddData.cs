using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.Migrations
{
    public partial class AddData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "OrderInfos");

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "ID", "name", "population" },
                values: new object[] { 1, "广东", 9000000 });

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "ID", "name", "population" },
                values: new object[] { 2, "福建", 8000000 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Provinces",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Area = table.Column<string>(maxLength: 256, nullable: true),
                    City = table.Column<string>(maxLength: 256, nullable: true),
                    Province = table.Column<string>(maxLength: 256, nullable: true),
                    Street = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    passengerid = table.Column<int>(type: "int(11)", nullable: false),
                    PassengerName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OrderInfos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    address_id = table.Column<int>(type: "int(11)", nullable: false),
                    order_id = table.Column<int>(type: "int(11)", nullable: false),
                    passenger_id = table.Column<int>(type: "int(11)", nullable: false),
                    price = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderInfos", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrderInfos_Passengers_passenger_id",
                        column: x => x.passenger_id,
                        principalTable: "Passengers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderInfos_passenger_id",
                table: "OrderInfos",
                column: "passenger_id");
        }
    }
}
