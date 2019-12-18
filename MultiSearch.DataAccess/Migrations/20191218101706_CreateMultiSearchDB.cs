using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiSearch.DataAccess.Migrations
{
    public partial class CreateMultiSearchDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebPages",
                columns: table => new
                {
                    WebPageEntityId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Queue = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    Snippet = table.Column<string>(nullable: true),
                    Engine = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebPages", x => x.WebPageEntityId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebPages");
        }
    }
}
