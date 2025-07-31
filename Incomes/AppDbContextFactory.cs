namespace Persistence.Incomes;

using Application.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Factory for creating instances of <see cref="IncomesDbContext"/> at design time.
/// IMPORTANT: The configuration file "appsettings.json" must have the connection string
/// under the key specified in <see cref="WebApiSettings.DBConnection"/>.
/// </summary>
internal class AppDbContextFactory: IDesignTimeDbContextFactory<IncomesDbContext>
{
    public IncomesDbContext CreateDbContext( string[] args )
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;

        var builder = new ConfigurationBuilder()
            .SetBasePath( path )
            .AddJsonFile( Constants.JsonFileName );

        var config = builder.Build();
        var optionsBuilder = new DbContextOptionsBuilder<IncomesDbContext>();

        optionsBuilder.UseSqlServer( config.GetValue<string>( WebApiSettings.DBConnUrl )! );
        return new IncomesDbContext( optionsBuilder.Options );
    }
}
