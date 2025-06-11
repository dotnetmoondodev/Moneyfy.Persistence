using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence.Expenses;

internal class AppDbContextFactory: IDesignTimeDbContextFactory<ExpensesDbContext>
{
    public ExpensesDbContext CreateDbContext( string[] args )
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;

        var builder = new ConfigurationBuilder()
            .SetBasePath( path )
            .AddJsonFile( Constants.AppSettings.JsonFileName );

        var config = builder.Build();
        var optionsBuilder = new DbContextOptionsBuilder<ExpensesDbContext>();

        optionsBuilder.UseSqlServer(
            config.GetConnectionString( Constants.AppSettings.DBConnName ) );

        return new ExpensesDbContext( optionsBuilder.Options );
    }
}
