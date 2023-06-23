using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeTG.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDateTimeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lastupdated",
                table: "cards");

            migrationBuilder.AddColumn<long>(
                name: "timeadded",
                table: "cards",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "timeadded",
                table: "cards");

            migrationBuilder.AddColumn<DateTime>(
                name: "lastupdated",
                table: "cards",
                type: "TEXT",
                nullable: true);
        }
    }
}
