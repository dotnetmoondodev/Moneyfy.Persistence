using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NotificationTables: Migration
    {
        /// <inheritdoc />
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>( type: "uniqueidentifier", nullable: false ),
                    DateToSend = table.Column<DateTimeOffset>( type: "datetimeoffset", nullable: false ),
                    HourToSend = table.Column<int>( type: "int", nullable: false ),
                    Frequency = table.Column<int>( type: "int", nullable: false ),
                    Method = table.Column<int>( type: "int", nullable: false ),
                    PaymentId = table.Column<Guid>( type: "uniqueidentifier", nullable: true ),
                    Repeatable = table.Column<bool>( type: "bit", nullable: false ),
                    Enable = table.Column<bool>( type: "bit", nullable: false ),
                    Email = table.Column<string>( type: "nvarchar(128)", maxLength: 128, nullable: true ),
                    PhoneNumber = table.Column<string>( type: "nvarchar(32)", maxLength: 32, nullable: true ),
                    Description = table.Column<string>( type: "nvarchar(128)", maxLength: 128, nullable: false ),
                    CreationDate = table.Column<DateTimeOffset>( type: "datetimeoffset", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Notifications", x => x.Id );
                } );
        }

        /// <inheritdoc />
        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable(
                name: "Notifications" );
        }
    }
}
