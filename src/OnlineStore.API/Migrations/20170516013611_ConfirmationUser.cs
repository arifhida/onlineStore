using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineStore.API.Migrations
{
    public partial class ConfirmationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Confirmation",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isConfirmed",
                table: "User",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmation",
                table: "User");

            migrationBuilder.DropColumn(
                name: "isConfirmed",
                table: "User");
        }
    }
}
