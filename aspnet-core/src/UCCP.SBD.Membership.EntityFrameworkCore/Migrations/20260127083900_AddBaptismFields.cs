using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UCCP.SBD.Membership.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddBaptismFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlaceOfBirth",
                table: "AppMembers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "AppMembers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherName",
                table: "AppMembers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sponsors",
                table: "AppMembers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlaceOfBirth",
                table: "AppMembers");

            migrationBuilder.DropColumn(
                name: "FatherName",
                table: "AppMembers");

            migrationBuilder.DropColumn(
                name: "MotherName",
                table: "AppMembers");

            migrationBuilder.DropColumn(
                name: "Sponsors",
                table: "AppMembers");
        }

    }
}
