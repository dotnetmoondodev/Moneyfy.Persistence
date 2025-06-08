using Application.Abstractions;
using Domain.Expenses;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public sealed class ExpensesRepository(
	IAppDbContext appDbContext )
	: IExpensesRepository
{
	private readonly IAppDbContext _appDbContext = appDbContext ??
		throw new ArgumentNullException( nameof( appDbContext ) );

	public async Task AddAsync( Expense expense, CancellationToken cancellationToken )
	{
		_appDbContext.Expenses.Add( expense );
		await _appDbContext.SaveChangesAsync( cancellationToken );
	}

	public async Task DeleteAsync( Expense expense, CancellationToken cancellationToken )
	{
		_appDbContext.Expenses.Remove( expense );
		await _appDbContext.SaveChangesAsync( cancellationToken );
	}

	public async Task<Expense?> GetByIdAsync( Guid id, CancellationToken cancellationToken )
	{
		return await _appDbContext.Expenses.SingleOrDefaultAsync( e => e.Id == id, cancellationToken );
	}

	public async Task UpdateAsync( Expense expense, CancellationToken cancellationToken )
	{
		_appDbContext.Expenses.Update( expense );
		await _appDbContext.SaveChangesAsync( cancellationToken );
	}
}
