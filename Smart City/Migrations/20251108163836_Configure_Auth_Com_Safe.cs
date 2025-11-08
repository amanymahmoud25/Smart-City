using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_City.Migrations
{
    public partial class Configure_Auth_Com_Safe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ====== Complaints: أعمدة جديدة آمنة ======
            migrationBuilder.AddColumn<string>(
                name: "AdminNote",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Complaints",
                type: "datetime2",
                nullable: true);

            // ====== Complaints: تنظيف البيانات قبل تضييق القيود ======
            migrationBuilder.Sql(@"
                UPDATE C
                SET 
                    Title = COALESCE(LEFT(Title, 120), N''),
                    Description = COALESCE(LEFT(Description, 2000), N'')
                FROM Complaints AS C;
            ");

            // ====== Complaints: تحويل Status نصي -> رقم enum بطريقة آمنة ======
            migrationBuilder.AddColumn<int>(
                name: "StatusInt",
                table: "Complaints",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE C SET StatusInt =
                    CASE 
                        WHEN TRY_CONVERT(int, Status) IS NOT NULL THEN TRY_CONVERT(int, Status)
                        WHEN Status IN (N'Pending', N'pending') THEN 0
                        WHEN Status IN (N'InProgress', N'inprogress', N'In Progress', N'in progress') THEN 1
                        WHEN Status IN (N'Resolved', N'resolved') THEN 2
                        WHEN Status IN (N'Rejected', N'rejected') THEN 3
                        ELSE 0
                    END
                FROM Complaints AS C;
            ");

            migrationBuilder.Sql(@"
                UPDATE Complaints
                SET StatusInt = 0
                WHERE StatusInt IS NULL;
            ");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Complaints");

            migrationBuilder.RenameColumn(
                name: "StatusInt",
                table: "Complaints",
                newName: "Status");

            // ====== Complaints: تضييق القيود بعد التنظيف ======
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Complaints",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Complaints",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // ====== Bill.Amount: ضبط الدقة لو الجدول موجود ======
            // لو سبق وضبطتها بــ Attribute/OnModelCreating ده هيبقى مطابق للموديل
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[Bills]', N'U') IS NOT NULL
                BEGIN
                    -- لو العمود موجود بدّله للدقة المناسبة
                    IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'Amount' AND Object_ID = Object_ID(N'Bills'))
                    BEGIN
                        ALTER TABLE [Bills] ALTER COLUMN [Amount] decimal(18,2) NOT NULL;
                    END
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // رجوع قيود العنوان/الوصف
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(120)",
                oldMaxLength: 120);

            // رجوع Status كنصي
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "0");

            migrationBuilder.Sql(@"
                UPDATE C SET Status = CAST(C.Status AS nvarchar(10))
                FROM Complaints AS C;
            ");

            // حذف الأعمدة المضافة
            migrationBuilder.DropColumn(name: "AdminNote", table: "Complaints");
            migrationBuilder.DropColumn(name: "ImageUrl", table: "Complaints");
            migrationBuilder.DropColumn(name: "Location", table: "Complaints");
            migrationBuilder.DropColumn(name: "UpdatedAt", table: "Complaints");
        }
    }
}
