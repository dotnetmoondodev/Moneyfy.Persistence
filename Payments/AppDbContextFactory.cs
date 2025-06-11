using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence.Payments;

internal class AppDbContextFactory: IDesignTimeDbContextFactory<PaymentsDbContext>
{
    public PaymentsDbContext CreateDbContext( string[] args )
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;

        var builder = new ConfigurationBuilder()
            .SetBasePath( path )
            .AddJsonFile( Constants.AppSettings.JsonFileName );

        var config = builder.Build();
        var optionsBuilder = new DbContextOptionsBuilder<PaymentsDbContext>();

        optionsBuilder.UseSqlServer(
            config.GetConnectionString( Constants.AppSettings.DBConnName ) );

        return new PaymentsDbContext( optionsBuilder.Options );
    }
}
