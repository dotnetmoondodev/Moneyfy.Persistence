using Application.Notifications;
using Domain.Notifications;
using Domain.Payments;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Notifications;

public class NotificationsDbContext( DbContextOptions options ): DbContext( options ), IAppDbContext
{
    public DbSet<Notification> Notifications { get; set; }

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

        modelBuilder.Entity<Notification>( entity =>
        {
            entity.HasKey( n => n.Id );
            entity.Property( p => p.Description ).IsRequired().HasMaxLength( 128 );
            entity.Property( p => p.DateToSend ).IsRequired().HasColumnType( "datetime" );
            entity.Property( p => p.HourToSend ).IsRequired();
            entity.Property( p => p.Frequency ).IsRequired();
            entity.Property( p => p.Method ).IsRequired();
            entity.Property( p => p.PaymentId );
            entity.Property( p => p.Repeatable ).IsRequired();
            entity.Property( p => p.Enable ).IsRequired();
            entity.Property( p => p.Email ).HasMaxLength( 128 );
            entity.Property( p => p.PhoneNumber ).HasMaxLength( 32 );
        } );

        modelBuilder.Entity<Notification>()
            .HasOne( n => n.Payment )
            .WithMany()
            .HasForeignKey( n => n.PaymentId )
            .OnDelete( DeleteBehavior.Cascade );
    }

    public override async Task<int> SaveChangesAsync( CancellationToken cancellationToken = default )
    {
        return await base.SaveChangesAsync( cancellationToken );
    }
}
