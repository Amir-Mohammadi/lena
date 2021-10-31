using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using core.Data;
namespace core.Transaction
{
  public class TransactionManager : ITransactionManager
  {
    readonly DbContext applicationDbContext;
    public IDbContextTransaction Transaction { get; private set; }
    public TransactionManager(ApplicationDbContext applicationDbContext)
    {
      this.applicationDbContext = applicationDbContext;
    }
    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
      var task = this.applicationDbContext.Database.BeginTransactionAsync(cancellationToken: cancellationToken);
      this.Transaction = task.Result;
      return task;
    }
    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
      this.applicationDbContext.SaveChangesAsync(cancellationToken: cancellationToken);
      return this.Transaction.CommitAsync(cancellationToken: cancellationToken);
    }
    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
      return this.Transaction.RollbackAsync(cancellationToken: cancellationToken);
    }
  }
}