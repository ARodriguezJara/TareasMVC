using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TareasMVC.Migrations
{
    /// <inheritdoc />
    public partial class AdminRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                            IF NOT EXISTS(Select Id FROM AspNetRoles WHERE Id = '8f0dd3a9-f877-4a6f-a07c-c23b94798fda')
                            BEGIN
	                            INSERT AspNetRoles(Id,[Name],[NormalizedName])
	                            VALUES ('8f0dd3a9-f877-4a6f-a07c-c23b94798fda','admin','ADMIN')
                            END
                            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE AspNetRoles WHERE Id = '8f0dd3a9-f877-4a6f-a07c-c23b94798fda'");
        }
    }
}
