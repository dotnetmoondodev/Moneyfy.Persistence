namespace Persistence.Expenses;

using Application.Expenses;
using Domain.Expenses;
using Microsoft.EntityFrameworkCore;

public class ExpensesDbContext( DbContextOptions options ): DbContext( options ), IAppDbContext
{
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        modelBuilder.Entity<Expense>( entity =>
        {
            entity.HasKey( e => e.Id );
            entity.Property( e => e.Description ).IsRequired().HasMaxLength( 128 );
            entity.Property( e => e.Value ).IsRequired().HasColumnType( "decimal(18,2)" );
        } );
    }

    public override async Task<int> SaveChangesAsync( CancellationToken cancellationToken = default )
    {
        return await base.SaveChangesAsync( cancellationToken );
    }
}
