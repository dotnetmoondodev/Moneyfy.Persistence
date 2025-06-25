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
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>( type: "uniqueidentifier", nullable: false ),
                    Currency = table.Column<int>( type: "int", nullable: false ),
                    IsAutoDebit = table.Column<int>( type: "int", nullable: false ),
                    PaymentMediaReference = table.Column<string>( type: "nvarchar(128)", maxLength: 128, nullable: false ),
                    Description = table.Column<string>( type: "nvarchar(128)", maxLength: 128, nullable: false ),
                    CreationDate = table.Column<DateTime>( type: "datetime2", nullable: false ),
                    Value = table.Column<decimal>( type: "decimal(18,2)", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Payments", x => x.Id );
                } );

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>( type: "uniqueidentifier", nullable: false ),
                    DateToSend = table.Column<DateTime>( type: "datetime2", nullable: false ),
                    HourToSend = table.Column<int>( type: "int", nullable: false ),
                    Frequency = table.Column<int>( type: "int", nullable: false ),
                    Method = table.Column<int>( type: "int", nullable: false ),
                    PaymentId = table.Column<Guid>( type: "uniqueidentifier", nullable: true ),
                    Repeatable = table.Column<int>( type: "int", nullable: false ),
                    Enable = table.Column<int>( type: "int", nullable: false ),
                    Email = table.Column<string>( type: "nvarchar(128)", maxLength: 128, nullable: true ),
                    PhoneNumber = table.Column<string>( type: "nvarchar(32)", maxLength: 32, nullable: true ),
                    Description = table.Column<string>( type: "nvarchar(128)", maxLength: 128, nullable: false ),
                    CreationDate = table.Column<DateTime>( type: "datetime2", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Notifications", x => x.Id );
                    table.ForeignKey(
                        name: "FK_Notifications_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade );
                } );

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_PaymentId",
                table: "Notifications",
                column: "PaymentId" );
        }

        /// <inheritdoc />
        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable(
                name: "Notifications" );

            migrationBuilder.DropTable(
                name: "Payments" );
        }
    }
}
