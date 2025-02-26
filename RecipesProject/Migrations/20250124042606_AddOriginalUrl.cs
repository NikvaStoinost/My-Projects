using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipesProject.Migrations
{
    /// <inheritdoc />
    public partial class AddOriginalUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OriginalUrl",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "aa794aa4-3a9f-45f8-811c-0524f20e9332", "defd7438-2a85-4229-9ff0-9dcca65b74ec" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1,
                column: "OriginalUrl",
                value: "1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalUrl",
                table: "Recipes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "c634b983-dc96-4149-b1ff-39379a34d5be", "bff6c43c-74ea-40d0-84a4-61e8b5c9922b" });
        }
    }
}
