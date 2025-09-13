using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StargateAPI.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_DUTYTITLE_ENUM_TABLE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DutyTitle",
                table: "AstronautDuty");

            migrationBuilder.DropColumn(
                name: "CurrentDutyTitle",
                table: "AstronautDetail");

            migrationBuilder.AddColumn<int>(
                name: "DutyTitleId",
                table: "AstronautDuty",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentDutyTitleId",
                table: "AstronautDetail",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DutyTitle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DutyTitle", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AstronautDuty_DutyTitleId",
                table: "AstronautDuty",
                column: "DutyTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_AstronautDetail_CurrentDutyTitleId",
                table: "AstronautDetail",
                column: "CurrentDutyTitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AstronautDetail_DutyTitle_CurrentDutyTitleId",
                table: "AstronautDetail",
                column: "CurrentDutyTitleId",
                principalTable: "DutyTitle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AstronautDuty_DutyTitle_DutyTitleId",
                table: "AstronautDuty",
                column: "DutyTitleId",
                principalTable: "DutyTitle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // Seed the DutyTitle table with enum values
            foreach (var dt in Enum.GetValues(typeof(Business.Data.DutyTitleEnum)))
            {
                migrationBuilder.InsertData(
                    table: "DutyTitle",
                    columns: new[] { "Id", "Name" },
                    values: new object[] { (int)dt, dt.ToString() });
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AstronautDetail_DutyTitle_CurrentDutyTitleId",
                table: "AstronautDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_AstronautDuty_DutyTitle_DutyTitleId",
                table: "AstronautDuty");

            migrationBuilder.DropTable(
                name: "DutyTitle");

            migrationBuilder.DropIndex(
                name: "IX_AstronautDuty_DutyTitleId",
                table: "AstronautDuty");

            migrationBuilder.DropIndex(
                name: "IX_AstronautDetail_CurrentDutyTitleId",
                table: "AstronautDetail");

            migrationBuilder.DropColumn(
                name: "DutyTitleId",
                table: "AstronautDuty");

            migrationBuilder.DropColumn(
                name: "CurrentDutyTitleId",
                table: "AstronautDetail");

            migrationBuilder.AddColumn<string>(
                name: "DutyTitle",
                table: "AstronautDuty",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrentDutyTitle",
                table: "AstronautDetail",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
