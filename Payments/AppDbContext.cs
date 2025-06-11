using Application.Payments;
using Domain.Payments;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Payments;

public class PaymentsDbContext( DbContextOptions options ): DbContext( options ), IAppDbContext
{
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        modelBuilder.Entity<Payment>( entity =>
        {
            entity.HasKey( p => p.Id );
            entity.Property( p => p.Description ).IsRequired().HasMaxLength( 128 );
            entity.Property( p => p.Value ).IsRequired().HasColumnType( "decimal(18,2)" );
            entity.Property( p => p.Currency ).IsRequired();
            entity.Property( p => p.IsAutoDebit ).IsRequired();
            entity.Property( p => p.PaymentMediaReference ).IsRequired().HasMaxLength( 128 );
        } );
    }

    public override async Task<int> SaveChangesAsync( CancellationToken cancellationToken = default )
    {
        return await base.SaveChangesAsync( cancellationToken );
    }
}
