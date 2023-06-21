using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeTG.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cards",
                columns: table => new
                {
                    uuid = table.Column<string>(type: "TEXT", nullable: false),
                    collection = table.Column<string>(type: "TEXT", nullable: false),
                    quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    foilquantity = table.Column<int>(type: "INTEGER", nullable: false),
                    lastupdated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cards", x => new { x.uuid, x.collection });
                });

            migrationBuilder.CreateTable(
                name: "collection",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collection", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cards");

            migrationBuilder.DropTable(
                name: "collection");
        }
    }
}
