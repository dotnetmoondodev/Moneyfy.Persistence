using Application.Payments;
using Domain.Payments;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Payments;

public sealed class PaymentsRepository(
    IAppDbContext appDbContext )
    : IPaymentsRepository
{
    private readonly IAppDbContext _appDbContext = appDbContext ??
        throw new ArgumentNullException( nameof( appDbContext ) );

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

    public async Task<Payment?> GetByIdAsync( Guid id, CancellationToken cancellationToken )
    {
        return await _appDbContext.Payments.SingleOrDefaultAsync( e => e.Id == id, cancellationToken );
    }

    public async Task UpdateAsync( Payment payment, CancellationToken cancellationToken )
    {
        _appDbContext.Payments.Update( payment );
        await _appDbContext.SaveChangesAsync( cancellationToken );
    }
}
