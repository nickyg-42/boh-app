using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boh_api.Migrations
{
    /// <inheritdoc />
    public partial class RemVerbsFromTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VerbTags_Verbs_VerbsId",
                table: "VerbTags");

            migrationBuilder.RenameColumn(
                name: "VerbsId",
                table: "VerbTags",
                newName: "VerbId");

            migrationBuilder.RenameIndex(
                name: "IX_VerbTags_VerbsId",
                table: "VerbTags",
                newName: "IX_VerbTags_VerbId");

            migrationBuilder.AddForeignKey(
                name: "FK_VerbTags_Verbs_VerbId",
                table: "VerbTags",
                column: "VerbId",
                principalTable: "Verbs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VerbTags_Verbs_VerbId",
                table: "VerbTags");

            migrationBuilder.RenameColumn(
                name: "VerbId",
                table: "VerbTags",
                newName: "VerbsId");

            migrationBuilder.RenameIndex(
                name: "IX_VerbTags_VerbId",
                table: "VerbTags",
                newName: "IX_VerbTags_VerbsId");

            migrationBuilder.AddForeignKey(
                name: "FK_VerbTags_Verbs_VerbsId",
                table: "VerbTags",
                column: "VerbsId",
                principalTable: "Verbs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
