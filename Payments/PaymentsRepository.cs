namespace Persistence.Payments;

using Application.Payments;
using Domain;
using Domain.Payments;
using Microsoft.EntityFrameworkCore;

public sealed class PaymentsRepository(
    IAppDbContext appDbContext )
    : IRepository<Payment>
{
    private readonly IAppDbContext _appDbContext = appDbContext ??
        throw new ArgumentNullException( nameof( appDbContext ) );

    public async Task<IReadOnlyCollection<Payment>> GetAllAsync( CancellationToken cancellationToken )
    {
        return await _appDbContext.Payments.ToListAsync( cancellationToken );
    }

    public async Task<Payment?> GetByIdAsync( Guid id, CancellationToken cancellationToken )
    {
        return await _appDbContext.Payments.SingleOrDefaultAsync( e => e.Id == id, cancellationToken );
    }

    public async Task AddAsync( Payment payment, CancellationToken cancellationToken )
    {
        _appDbContext.Payments.Add( payment );
        await _appDbContext.SaveChangesAsync( cancellationToken );
    }

    public async Task DeleteAsync( Payment payment, CancellationToken cancellationToken )
    {
        _appDbContext.Payments.Remove( payment );
        await _appDbContext.SaveChangesAsync( cancellationToken );
    }

    public async Task UpdateAsync( Payment payment, CancellationToken cancellationToken )
    {
        _appDbContext.Payments.Update( payment );
        await _appDbContext.SaveChangesAsync( cancellationToken );
    }
}
