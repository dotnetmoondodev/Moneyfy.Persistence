using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence.Incomes;

internal class AppDbContextFactory: IDesignTimeDbContextFactory<IncomesDbContext>
{
    public IncomesDbContext CreateDbContext( string[] args )
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;

        var builder = new ConfigurationBuilder()
            .SetBasePath( path )
            .AddJsonFile( Constants.AppSettings.JsonFileName );

        var config = builder.Build();
        var optionsBuilder = new DbContextOptionsBuilder<IncomesDbContext>();

        optionsBuilder.UseSqlServer(
            config.GetConnectionString( Constants.AppSettings.DBConnName ) );

        return new IncomesDbContext( optionsBuilder.Options );
    }
}
