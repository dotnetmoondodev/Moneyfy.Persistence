using Application.Notifications;
using Domain.Notifications;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Notifications;

public sealed class NotificationsRepository(
    IAppDbContext appDbContext )
    : INotificationsRepository
{
    private readonly IAppDbContext _appDbContext = appDbContext ??
        throw new ArgumentNullException( nameof( appDbContext ) );

    public async Task AddAsync( Notification notification, CancellationToken cancellationToken )
    {
        _appDbContext.Notifications.Add( notification );
        await _appDbContext.SaveChangesAsync( cancellationToken );
    }

    public async Task DeleteAsync( Notification notification, CancellationToken cancellationToken )
    {
        _appDbContext.Notifications.Remove( notification );
        await _appDbContext.SaveChangesAsync( cancellationToken );
    }

    public async Task<Notification?> GetByIdAsync( Guid id, CancellationToken cancellationToken )
    {
        return await _appDbContext.Notifications.SingleOrDefaultAsync( e => e.Id == id, cancellationToken );
    }

    public async Task UpdateAsync( Notification notification, CancellationToken cancellationToken )
    {
        _appDbContext.Notifications.Update( notification );
        await _appDbContext.SaveChangesAsync( cancellationToken );
    }
}
