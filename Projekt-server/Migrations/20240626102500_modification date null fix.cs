using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt_server.Migrations
{
    /// <inheritdoc />
    public partial class modificationdatenullfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apps_Servers_ServerId",
                table: "Apps");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Apps_AppId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Servers_ServerId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AppId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ServerId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Apps_ServerId",
                table: "Apps");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModificationDate",
                table: "Servers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModificationDate",
                table: "Servers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AppId",
                table: "Tasks",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ServerId",
                table: "Tasks",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_Apps_ServerId",
                table: "Apps",
                column: "ServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Apps_Servers_ServerId",
                table: "Apps",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Apps_AppId",
                table: "Tasks",
                column: "AppId",
                principalTable: "Apps",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Servers_ServerId",
                table: "Tasks",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
