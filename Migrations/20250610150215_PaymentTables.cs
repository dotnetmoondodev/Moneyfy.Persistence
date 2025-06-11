using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PaymentTables: Migration
    {
        /// <inheritdoc />
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>( type: "uniqueidentifier", nullable: false ),
                    Currency = table.Column<int>( type: "int", nullable: false ),
                    IsAutoDebit = table.Column<bool>( type: "bit", nullable: false ),
                    PaymentMediaReference = table.Column<string>( type: "nvarchar(128)", maxLength: 128, nullable: false ),
                    Description = table.Column<string>( type: "nvarchar(128)", maxLength: 128, nullable: false ),
                    CreationDate = table.Column<DateTimeOffset>( type: "datetimeoffset", nullable: false ),
                    Value = table.Column<decimal>( type: "decimal(18,2)", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Payments", x => x.Id );
                } );
        }

        /// <inheritdoc />
        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable(
                name: "Payments" );
        }
    }
}
