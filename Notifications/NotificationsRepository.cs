using System.Linq.Expressions;
using Application.Notifications;
using Domain.Notifications;
using Domain;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Persistence.Notifications;

public sealed class NotificationsRepository(
    IMongoDatabase database )
    : IRepository<Notification>
{
    private readonly IMongoCollection<Notification> _dbCollection =
        database.GetCollection<Notification>( nameof( ApiEndpoints.Notifications ) ) ??
        throw new ArgumentNullException( nameof( database ) );

    private readonly FilterDefinitionBuilder<Notification> _filterBuilder = Builders<Notification>.Filter;

    public async Task<Notification?> GetByIdAsync( Guid id, CancellationToken cancellationToken )
    {
        var filter = _filterBuilder.Eq( entity => entity.Id, id );
        return await _dbCollection.Find( filter ).FirstOrDefaultAsync( cancellationToken: cancellationToken );
    }

    public async Task<Notification?> GetByIdAsync( Expression<Func<Notification, bool>> filter, CancellationToken cancellationToken )
    {
        ArgumentNullException.ThrowIfNull( filter );
        return await _dbCollection.Find( filter ).FirstOrDefaultAsync( cancellationToken );
    }

    public async Task<IReadOnlyCollection<Notification>> GetAllAsync( Expression<Func<Notification, bool>>? filter, CancellationToken cancellationToken )
    {
        return await _dbCollection.Find( filter ?? _filterBuilder.Empty ).ToListAsync( cancellationToken );
    }

    public async Task AddAsync( Notification notification, CancellationToken cancellationToken )
    {
        await _dbCollection.InsertOneAsync( notification, cancellationToken: cancellationToken );
    }

    public async Task DeleteAsync( Notification notification, CancellationToken cancellationToken )
    {
        var filter = _filterBuilder.Eq( entity => entity.Id, notification.Id );
        await _dbCollection.DeleteOneAsync( filter, cancellationToken );
    }

    public async Task UpdateAsync( Notification notification, CancellationToken cancellationToken )
    {
        var filter = _filterBuilder.Eq( entity => entity.Id, notification.Id );
        await _dbCollection.ReplaceOneAsync( filter, notification, new ReplaceOptions { IsUpsert = false }, cancellationToken );
    }
}
