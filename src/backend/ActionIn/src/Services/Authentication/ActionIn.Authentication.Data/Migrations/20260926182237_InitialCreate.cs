using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActionIn.Authentication.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "varchar(100)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "varchar(100)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "varchar(100)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_email",
                table: "Accounts",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_username",
                table: "Accounts",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
