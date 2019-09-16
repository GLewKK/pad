using Microsoft.EntityFrameworkCore.Migrations;

namespace MessageChannel.Migrations
{
    public partial class Add_CharOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatOrder",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatOrder",
                table: "Users");
        }
    }
}
