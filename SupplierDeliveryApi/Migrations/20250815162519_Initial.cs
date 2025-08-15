using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupplierDeliveryAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    IdProduct = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NmProduct = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.IdProduct);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    IdSupplier = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NmSupplier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.IdSupplier);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    IdDelivery = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtDelivery = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    DsStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdSupplier = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.IdDelivery);
                    table.ForeignKey(
                        name: "FK_Deliveries_Suppliers_IdSupplier",
                        column: x => x.IdSupplier,
                        principalTable: "Suppliers",
                        principalColumn: "IdSupplier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryAddresses",
                columns: table => new
                {
                    IdDelivery = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAddresses", x => x.IdDelivery);
                    table.ForeignKey(
                        name: "FK_DeliveryAddresses_Deliveries_IdDelivery",
                        column: x => x.IdDelivery,
                        principalTable: "Deliveries",
                        principalColumn: "IdDelivery",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryProducts",
                columns: table => new
                {
                    IdDelivery = table.Column<int>(type: "int", nullable: false),
                    IdProduct = table.Column<int>(type: "int", nullable: false),
                    QtProduct = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryProducts", x => new { x.IdDelivery, x.IdProduct });
                    table.ForeignKey(
                        name: "FK_DeliveryProducts_Deliveries_IdDelivery",
                        column: x => x.IdDelivery,
                        principalTable: "Deliveries",
                        principalColumn: "IdDelivery",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryProducts_Products_IdProduct",
                        column: x => x.IdProduct,
                        principalTable: "Products",
                        principalColumn: "IdProduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_IdSupplier",
                table: "Deliveries",
                column: "IdSupplier");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryProducts_IdProduct",
                table: "DeliveryProducts",
                column: "IdProduct");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryAddresses");

            migrationBuilder.DropTable(
                name: "DeliveryProducts");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
