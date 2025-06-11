using Application.Incomes;
using Domain.Incomes;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Incomes;

public class IncomesDbContext( DbContextOptions options ): DbContext( options ), IAppDbContext
{
    public DbSet<Income> Incomes { get; set; }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        modelBuilder.Entity<Income>( entity =>
        {
            entity.HasKey( i => i.Id );
            entity.Property( i => i.Description ).IsRequired().HasMaxLength( 128 );
            entity.Property( i => i.Value ).IsRequired().HasColumnType( "decimal(18,2)" );
            entity.Property( i => i.Source ).IsRequired().HasMaxLength( 128 );
            entity.Property( i => i.Withholding ).IsRequired().HasColumnType( "decimal(18,2)" );
        } );
    }

    public override async Task<int> SaveChangesAsync( CancellationToken cancellationToken = default )
    {
        return await base.SaveChangesAsync( cancellationToken );
    }
}
