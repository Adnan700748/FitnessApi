using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FitnessApi.Migrations
{
    /// <inheritdoc />
    public partial class RefineFitnessModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_FitnessClasses_FitnessClassId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Members_MemberId",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Members",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MembershipNumber",
                table: "Members",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "MembershipLevel",
                table: "Members",
                type: "numeric(3,2)",
                precision: 3,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "FitnessClasses",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "FitnessClasses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "Grade",
                table: "Bookings",
                type: "numeric(4,2)",
                precision: 4,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookedAt",
                table: "Bookings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    MaxScore = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    Weight = table.Column<decimal>(type: "numeric(4,2)", precision: 4, scale: 2, nullable: false),
                    FitnessClassId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assessments_FitnessClasses_FitnessClassId",
                        column: x => x.FitnessClassId,
                        principalTable: "FitnessClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SerialNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    MemberId = table.Column<int>(type: "integer", nullable: false),
                    FitnessClassId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Badges_FitnessClasses_FitnessClassId",
                        column: x => x.FitnessClassId,
                        principalTable: "FitnessClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Badges_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_MembershipNumber",
                table: "Members",
                column: "MembershipNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FitnessClasses_Code",
                table: "FitnessClasses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_FitnessClassId",
                table: "Assessments",
                column: "FitnessClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Badges_FitnessClassId",
                table: "Badges",
                column: "FitnessClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Badges_MemberId",
                table: "Badges",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Badges_SerialNumber",
                table: "Badges",
                column: "SerialNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_FitnessClasses_FitnessClassId",
                table: "Bookings",
                column: "FitnessClassId",
                principalTable: "FitnessClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Members_MemberId",
                table: "Bookings",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_FitnessClasses_FitnessClassId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Members_MemberId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropTable(
                name: "Badges");

            migrationBuilder.DropIndex(
                name: "IX_Members_MembershipNumber",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_FitnessClasses_Code",
                table: "FitnessClasses");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Members",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "MembershipNumber",
                table: "Members",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<decimal>(
                name: "MembershipLevel",
                table: "Members",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(3,2)",
                oldPrecision: 3,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "FitnessClasses",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "FitnessClasses",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<decimal>(
                name: "Grade",
                table: "Bookings",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(4,2)",
                oldPrecision: 4,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookedAt",
                table: "Bookings",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_FitnessClasses_FitnessClassId",
                table: "Bookings",
                column: "FitnessClassId",
                principalTable: "FitnessClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Members_MemberId",
                table: "Bookings",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
