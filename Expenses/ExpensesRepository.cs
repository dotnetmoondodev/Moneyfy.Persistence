using System.Linq.Expressions;
using Application.Expenses;
using Domain.Expenses;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Persistence.Expenses;

public sealed class ExpensesRepository(
    IMongoDatabase database )
    : IExpensesRepository
{
    private readonly IMongoCollection<Expense> _dbCollection =
        database.GetCollection<Expense>( nameof( ApiEndpoints.Expenses ) ) ??
        throw new ArgumentNullException( nameof( database ) );

    private readonly FilterDefinitionBuilder<Expense> _filterBuilder = Builders<Expense>.Filter;

    public async Task<Expense?> GetByIdAsync( Guid id, CancellationToken cancellationToken )
    {
        var filter = _filterBuilder.Eq( entity => entity.Id, id );
        return await _dbCollection.Find( filter ).FirstOrDefaultAsync( cancellationToken: cancellationToken );
    }

    public async Task<Expense?> GetByIdAsync( Expression<Func<Expense, bool>> filter, CancellationToken cancellationToken )
    {
        ArgumentNullException.ThrowIfNull( filter );
        return await _dbCollection.Find( filter ).FirstOrDefaultAsync( cancellationToken );
    }

    public async Task<IReadOnlyCollection<Expense>> GetAllAsync( Expression<Func<Expense, bool>>? filter, CancellationToken cancellationToken )
    {
        return await _dbCollection.Find( filter ?? _filterBuilder.Empty ).ToListAsync( cancellationToken );
    }

    public async Task AddAsync( Expense expense, CancellationToken cancellationToken )
    {
        await _dbCollection.InsertOneAsync( expense, cancellationToken: cancellationToken );
    }

    public async Task DeleteAsync( Expense expense, CancellationToken cancellationToken )
    {
        var filter = _filterBuilder.Eq( entity => entity.Id, expense.Id );
        await _dbCollection.DeleteOneAsync( filter, cancellationToken );
    }

    public async Task UpdateAsync( Expense expense, CancellationToken cancellationToken )
    {
        var filter = _filterBuilder.Eq( entity => entity.Id, expense.Id );
        await _dbCollection.ReplaceOneAsync( filter, expense, new ReplaceOptions { IsUpsert = false }, cancellationToken );
    }
}
