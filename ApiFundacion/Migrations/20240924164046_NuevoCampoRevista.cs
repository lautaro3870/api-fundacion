using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiFundacion.Migrations
{
    public partial class NuevoCampoRevista : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Revista",
                table: "proyectos",
                type: "text",
                nullable: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
