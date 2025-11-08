using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_City.Migrations
{
    /// <inheritdoc />
    public partial class AddUserClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Citizens_CitizenId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Admins_AdminId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Citizens_CitizenId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Citizens_CitizenId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_Citizens_CitizenId",
                table: "Suggestions");

            migrationBuilder.DropForeignKey(
                name: "FK_UtilityIssues_Citizens_CitizenId",
                table: "UtilityIssues");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Citizens",
                table: "Citizens");

            migrationBuilder.RenameTable(
                name: "Citizens",
                newName: "Users");

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Suggestions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Suggestions_AdminId",
                table: "Suggestions",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Users_CitizenId",
                table: "Bills",
                column: "CitizenId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Users_AdminId",
                table: "Complaints",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Users_CitizenId",
                table: "Complaints",
                column: "CitizenId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_CitizenId",
                table: "Notifications",
                column: "CitizenId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_Users_AdminId",
                table: "Suggestions",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_Users_CitizenId",
                table: "Suggestions",
                column: "CitizenId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UtilityIssues_Users_CitizenId",
                table: "UtilityIssues",
                column: "CitizenId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Users_CitizenId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Users_AdminId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Users_CitizenId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_CitizenId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_Users_AdminId",
                table: "Suggestions");

            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_Users_CitizenId",
                table: "Suggestions");

            migrationBuilder.DropForeignKey(
                name: "FK_UtilityIssues_Users_CitizenId",
                table: "UtilityIssues");

            migrationBuilder.DropIndex(
                name: "IX_Suggestions_AdminId",
                table: "Suggestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Suggestions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Citizens");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Citizens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Citizens",
                table: "Citizens",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHAsh = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Citizens_CitizenId",
                table: "Bills",
                column: "CitizenId",
                principalTable: "Citizens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Admins_AdminId",
                table: "Complaints",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Citizens_CitizenId",
                table: "Complaints",
                column: "CitizenId",
                principalTable: "Citizens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Citizens_CitizenId",
                table: "Notifications",
                column: "CitizenId",
                principalTable: "Citizens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_Citizens_CitizenId",
                table: "Suggestions",
                column: "CitizenId",
                principalTable: "Citizens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UtilityIssues_Citizens_CitizenId",
                table: "UtilityIssues",
                column: "CitizenId",
                principalTable: "Citizens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
