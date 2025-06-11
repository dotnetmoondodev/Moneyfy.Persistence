using Application.Notifications;
using Domain.Notifications;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Notifications;

public class NotificationsDbContext( DbContextOptions options ): DbContext( options ), IAppDbContext
{
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
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
