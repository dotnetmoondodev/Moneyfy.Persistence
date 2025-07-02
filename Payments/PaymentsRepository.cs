using System.Linq.Expressions;
using Application.Payments;
using Domain.Payments;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Persistence.Payments;

public sealed class PaymentsRepository(
    IMongoDatabase database )
    : IPaymentsRepository
{
    private readonly IMongoCollection<Payment> _dbCollection =
        database.GetCollection<Payment>( nameof( ApiEndpoints.Payments ) ) ??
        throw new ArgumentNullException( nameof( database ) );

    private readonly FilterDefinitionBuilder<Payment> _filterBuilder = Builders<Payment>.Filter;

    public async Task<Payment?> GetByIdAsync( Guid id, CancellationToken cancellationToken )
    {
        var filter = _filterBuilder.Eq( entity => entity.Id, id );
        return await _dbCollection.Find( filter ).FirstOrDefaultAsync( cancellationToken: cancellationToken );
    }

    public async Task<Payment?> GetByIdAsync( Expression<Func<Payment, bool>> filter, CancellationToken cancellationToken )
    {
        ArgumentNullException.ThrowIfNull( filter );
        return await _dbCollection.Find( filter ).FirstOrDefaultAsync( cancellationToken );
    }

    public async Task<IReadOnlyCollection<Payment>> GetAllAsync( Expression<Func<Payment, bool>>? filter, CancellationToken cancellationToken )
    {
        return await _dbCollection.Find( filter ?? _filterBuilder.Empty ).ToListAsync( cancellationToken );
    }

    public async Task AddAsync( Payment payment, CancellationToken cancellationToken )
    {
        await _dbCollection.InsertOneAsync( payment, cancellationToken: cancellationToken );
    }

    public async Task DeleteAsync( Payment payment, CancellationToken cancellationToken )
    {
        var filter = _filterBuilder.Eq( entity => entity.Id, payment.Id );
        await _dbCollection.DeleteOneAsync( filter, cancellationToken );
    }

    public async Task UpdateAsync( Payment payment, CancellationToken cancellationToken )
    {
        var filter = _filterBuilder.Eq( entity => entity.Id, payment.Id );
        await _dbCollection.ReplaceOneAsync( filter, payment, new ReplaceOptions { IsUpsert = false }, cancellationToken );
    }
}
