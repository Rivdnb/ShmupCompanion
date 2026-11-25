using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShmupCompanion.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddNoteImageDeletionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "MarkedForDeletionAt",
                table: "NoteImages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ToBeDeleted",
                table: "NoteImages",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkedForDeletionAt",
                table: "NoteImages");

            migrationBuilder.DropColumn(
                name: "ToBeDeleted",
                table: "NoteImages");
        }
    }
}
