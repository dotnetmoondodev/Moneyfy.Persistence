namespace Persistence.Incomes;

using Application.Incomes;
using Domain;
using Domain.Incomes;
using Microsoft.EntityFrameworkCore;

public sealed class IncomesRepository(
    IAppDbContext appDbContext )
    : IRepository<Income>
{
    private readonly IAppDbContext _appDbContext = appDbContext ??
        throw new ArgumentNullException( nameof( appDbContext ) );

    public async Task<IReadOnlyCollection<Income>> GetAllAsync( CancellationToken cancellationToken )
    {
        return await _appDbContext.Incomes.ToListAsync( cancellationToken );
    }

    public async Task<Income?> GetByIdAsync( Guid id, CancellationToken cancellationToken )
    {
        return await _appDbContext.Incomes.SingleOrDefaultAsync( e => e.Id == id, cancellationToken );
    }

    public async Task AddAsync( Income income, CancellationToken cancellationToken )
    {
        _appDbContext.Incomes.Add( income );
        await _appDbContext.SaveChangesAsync( cancellationToken );
    }

    public async Task DeleteAsync( Income income, CancellationToken cancellationToken )
    {
        _appDbContext.Incomes.Remove( income );
        await _appDbContext.SaveChangesAsync( cancellationToken );
    }

    public async Task UpdateAsync( Income income, CancellationToken cancellationToken )
    {
        _appDbContext.Incomes.Update( income );
        await _appDbContext.SaveChangesAsync( cancellationToken );
    }
}
