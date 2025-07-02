using System.Linq.Expressions;
using Application.Incomes;
using Domain.Incomes;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Persistence.Incomes;

public sealed class IncomesRepository(
    IMongoDatabase database )
    : IIncomesRepository
{
    private readonly IMongoCollection<Income> _dbCollection =
        database.GetCollection<Income>( nameof( ApiEndpoints.Incomes ) ) ??
        throw new ArgumentNullException( nameof( database ) );

    private readonly FilterDefinitionBuilder<Income> _filterBuilder = Builders<Income>.Filter;

    public async Task<Income?> GetByIdAsync( Guid id, CancellationToken cancellationToken )
    {
        var filter = _filterBuilder.Eq( entity => entity.Id, id );
        return await _dbCollection.Find( filter ).FirstOrDefaultAsync( cancellationToken: cancellationToken );
    }

    public async Task<Income?> GetByIdAsync( Expression<Func<Income, bool>> filter, CancellationToken cancellationToken )
    {
        ArgumentNullException.ThrowIfNull( filter );
        return await _dbCollection.Find( filter ).FirstOrDefaultAsync( cancellationToken );
    }

    public async Task<IReadOnlyCollection<Income>> GetAllAsync( Expression<Func<Income, bool>>? filter, CancellationToken cancellationToken )
    {
        return await _dbCollection.Find( filter ?? _filterBuilder.Empty ).ToListAsync( cancellationToken );
    }

    public async Task AddAsync( Income income, CancellationToken cancellationToken )
    {
        await _dbCollection.InsertOneAsync( income, cancellationToken: cancellationToken );
    }

    public async Task DeleteAsync( Income income, CancellationToken cancellationToken )
    {
        var filter = _filterBuilder.Eq( entity => entity.Id, income.Id );
        await _dbCollection.DeleteOneAsync( filter, cancellationToken );
    }

    public async Task UpdateAsync( Income income, CancellationToken cancellationToken )
    {
        var filter = _filterBuilder.Eq( entity => entity.Id, income.Id );
        await _dbCollection.ReplaceOneAsync( filter, income, new ReplaceOptions { IsUpsert = false }, cancellationToken );
    }
}
