using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using core.Autofac;
namespace core.Transaction
{
  public interface ITransactionManager : IScopedDependency
  {
    IDbContextTransaction Transaction { get; }
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
  }
}