namespace Persistence.Notifications;

using Application.Notifications;
using Domain;
using Domain.Notifications;
using Microsoft.EntityFrameworkCore;

public sealed class NotificationsRepository(
    IAppDbContext appDbContext )
    : IRepository<Notification>
{
    private readonly IAppDbContext _appDbContext = appDbContext ??
        throw new ArgumentNullException( nameof( appDbContext ) );

    public async Task<IReadOnlyCollection<Notification>> GetAllAsync( CancellationToken cancellationToken )
    {
        return await _appDbContext.Notifications.ToListAsync( cancellationToken );
    }

    public async Task<Notification?> GetByIdAsync( Guid id, CancellationToken cancellationToken )
    {
        return await _appDbContext.Notifications.SingleOrDefaultAsync( e => e.Id == id, cancellationToken );
    }

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

    public async Task UpdateAsync( Notification notification, CancellationToken cancellationToken )
    {
        _appDbContext.Notifications.Update( notification );
        await _appDbContext.SaveChangesAsync( cancellationToken );
    }
}
