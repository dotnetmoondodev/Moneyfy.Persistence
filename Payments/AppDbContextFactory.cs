namespace Persistence.Payments;

using Application.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Factory for creating instances of <see cref="PaymentsDbContext"/> at design time.
/// IMPORTANT: The configuration file "appsettings.json" must have the connection string
/// under the key specified in <see cref="WebApiSettings.DBConnection"/>.
/// </summary>
internal class AppDbContextFactory: IDesignTimeDbContextFactory<PaymentsDbContext>
{
    public PaymentsDbContext CreateDbContext( string[] args )
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;

        var builder = new ConfigurationBuilder()
            .SetBasePath( path )
            .AddJsonFile( Constants.JsonFileName );

        var config = builder.Build();
        var optionsBuilder = new DbContextOptionsBuilder<PaymentsDbContext>();

        optionsBuilder.UseSqlServer( config.GetValue<string>( WebApiSettings.DBConnUrl )! );
        return new PaymentsDbContext( optionsBuilder.Options );
    }
}
