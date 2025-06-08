using Application.Abstractions;
using Domain.Expenses;
using Domain.Incomes;
using Domain.Notifications;
using Domain.Payments;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class AppDbContext( DbContextOptions options ) : DbContext( options ), IAppDbContext
{
	public DbSet<Expense> Expenses { get; set; }
    public DbSet<Income> Incomes { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
	{
		modelBuilder.Entity<Expense>( entity =>
		{
			entity.HasKey( e => e.Id );
			entity.Property( e => e.Description ).IsRequired().HasMaxLength( 128 );
			entity.Property( e => e.Value ).IsRequired().HasColumnType( "decimal(18,2)" );
		} );

        modelBuilder.Entity<Income>( entity =>
        {
            entity.HasKey( i => i.Id );
            entity.Property( i => i.Description ).IsRequired().HasMaxLength( 128 );
            entity.Property( i => i.Value ).IsRequired().HasColumnType( "decimal(18,2)" );
            entity.Property( i => i.Source ).IsRequired().HasMaxLength( 128 );
            entity.Property( i => i.Withholding ).IsRequired().HasColumnType( "decimal(18,2)" );
        } );

        modelBuilder.Entity<Payment>( entity =>
        {
            entity.HasKey( p => p.Id );
            entity.Property( p => p.Description ).IsRequired().HasMaxLength( 128 );
            entity.Property( p => p.Value ).IsRequired().HasColumnType( "decimal(18,2)" );
            entity.Property( p => p.Currency ).IsRequired();
            entity.Property( p => p.IsAutoDebit ).IsRequired();
            entity.Property( p => p.PaymentMediaReference ).IsRequired().HasMaxLength( 128 );
        } );

        modelBuilder.Entity<Notification>( entity =>
        {
            entity.HasKey( n => n.Id );
            entity.Property( p => p.Description ).IsRequired().HasMaxLength( 128 );
            entity.Property( p => p.DateToSend ).IsRequired().HasColumnType( "datetimeoffset" );
            entity.Property( p => p.HourToSend ).IsRequired();
            entity.Property( p => p.Frequency ).IsRequired();
            entity.Property( p => p.Method ).IsRequired();
            entity.Property( p => p.PaymentId );
            entity.Property( p => p.Repeatable ).IsRequired();
            entity.Property( p => p.Enable ).IsRequired();
            entity.Property( p => p.Email ).HasMaxLength( 128 );
            entity.Property( p => p.PhoneNumber ).HasMaxLength( 32 );
        } );
    }

	public override async Task<int> SaveChangesAsync( CancellationToken cancellationToken = default )
	{
		return await base.SaveChangesAsync( cancellationToken );
	}
}
