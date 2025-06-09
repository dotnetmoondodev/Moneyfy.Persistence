using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence.Repositories;

internal class AppDbContextFactory: IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext( string[] args )
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;

        var builder = new ConfigurationBuilder()
            .SetBasePath( path )
            .AddJsonFile( Constants.AppSettings.DBConnName );

        var config = builder.Build();
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlServer(
            config.GetConnectionString( Constants.AppSettings.DBConnName ) );
        return new AppDbContext( optionsBuilder.Options );
    }
}
