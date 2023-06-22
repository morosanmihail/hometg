using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeTG.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_cards_uuid_collection",
                table: "cards",
                columns: new[] { "uuid", "collection" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_cards_uuid_collection",
                table: "cards");
        }
    }
}
