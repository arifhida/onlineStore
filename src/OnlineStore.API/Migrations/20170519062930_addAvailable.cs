using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineStore.API.Migrations
{
    public partial class addAvailable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isAvailable",
                table: "Product",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAvailable",
                table: "Product");
        }
    }
}
