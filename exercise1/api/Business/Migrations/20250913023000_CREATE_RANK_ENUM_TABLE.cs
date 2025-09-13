using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StargateAPI.Migrations
{
    /// <inheritdoc />
    public partial class CREATE_RANK_ENUM_TABLE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rank",
                table: "AstronautDuty");

            migrationBuilder.DropColumn(
                name: "CurrentRank",
                table: "AstronautDetail");

            migrationBuilder.AddColumn<int>(
                name: "RankId",
                table: "AstronautDuty",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CareerStartDate",
                table: "AstronautDetail",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentRankId",
                table: "AstronautDetail",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Rank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rank", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AstronautDuty_RankId",
                table: "AstronautDuty",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_AstronautDetail_CurrentRankId",
                table: "AstronautDetail",
                column: "CurrentRankId");

            migrationBuilder.AddForeignKey(
                name: "FK_AstronautDetail_Rank_CurrentRankId",
                table: "AstronautDetail",
                column: "CurrentRankId",
                principalTable: "Rank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AstronautDuty_Rank_RankId",
                table: "AstronautDuty",
                column: "RankId",
                principalTable: "Rank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // Seed the Rank table with enum values
            foreach (var rank in Enum.GetValues(typeof(Business.Data.RankEnum)))
            {
                migrationBuilder.InsertData(
                    table: "Rank",
                    columns: new[] { "Id", "Name" },
                    values: new object[] { (int)rank, rank.ToString() });
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AstronautDetail_Rank_CurrentRankId",
                table: "AstronautDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_AstronautDuty_Rank_RankId",
                table: "AstronautDuty");

            migrationBuilder.DropTable(
                name: "Rank");

            migrationBuilder.DropIndex(
                name: "IX_AstronautDuty_RankId",
                table: "AstronautDuty");

            migrationBuilder.DropIndex(
                name: "IX_AstronautDetail_CurrentRankId",
                table: "AstronautDetail");

            migrationBuilder.DropColumn(
                name: "RankId",
                table: "AstronautDuty");

            migrationBuilder.DropColumn(
                name: "CurrentRankId",
                table: "AstronautDetail");

            migrationBuilder.AddColumn<string>(
                name: "Rank",
                table: "AstronautDuty",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CareerStartDate",
                table: "AstronautDetail",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "CurrentRank",
                table: "AstronautDetail",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
