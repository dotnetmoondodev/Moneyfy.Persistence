using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ExpenseTables: Migration
    {
        /// <inheritdoc />
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>( type: "uniqueidentifier", nullable: false ),
                    Description = table.Column<string>( type: "nvarchar(128)", maxLength: 128, nullable: false ),
                    CreationDate = table.Column<DateTimeOffset>( type: "datetime", nullable: false ),
                    Value = table.Column<decimal>( type: "decimal(18,2)", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Expenses", x => x.Id );
                } );
        }

        /// <inheritdoc />
        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable(
                name: "Expenses" );
        }
    }
}
