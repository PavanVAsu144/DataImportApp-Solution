//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace DataImportApp.Migrations
//{
//    /// <inheritdoc />
//    public partial class AddImportLogTable : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "ImportedFiles",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    ImportedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_ImportedFiles", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "ImportLogs",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    User = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_ImportLogs", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Users",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Users", x => x.Id);
//                });
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "ImportedFiles");

//            migrationBuilder.DropTable(
//                name: "ImportLogs");

//            migrationBuilder.DropTable(
//                name: "Users");
//        }
//    }
//}
