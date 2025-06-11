using Application.Incomes;
using Domain.Incomes;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Incomes;

public sealed class IncomesRepository(
    IAppDbContext appDbContext )
    : IIncomesRepository
{
    private readonly IAppDbContext _appDbContext = appDbContext ??
        throw new ArgumentNullException( nameof( appDbContext ) );

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

    public async Task<Income?> GetByIdAsync( Guid id, CancellationToken cancellationToken )
    {
        return await _appDbContext.Incomes.SingleOrDefaultAsync( e => e.Id == id, cancellationToken );
    }

    public async Task UpdateAsync( Income income, CancellationToken cancellationToken )
    {
        _appDbContext.Incomes.Update( income );
        await _appDbContext.SaveChangesAsync( cancellationToken );
    }
}
