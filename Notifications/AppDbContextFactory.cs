using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence.Notifications;

internal class AppDbContextFactory: IDesignTimeDbContextFactory<NotificationsDbContext>
{
    public NotificationsDbContext CreateDbContext( string[] args )
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;

        var builder = new ConfigurationBuilder()
            .SetBasePath( path )
            .AddJsonFile( Constants.AppSettings.JsonFileName );

        var config = builder.Build();
        var optionsBuilder = new DbContextOptionsBuilder<NotificationsDbContext>();

        optionsBuilder.UseSqlServer(
            config.GetConnectionString( Constants.AppSettings.DBConnName ) );

        return new NotificationsDbContext( optionsBuilder.Options );
    }
}
