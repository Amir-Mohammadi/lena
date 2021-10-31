using System;
using System.Threading;
using System.Threading.Tasks;
using core.Autofac;
using core.Models;
namespace core.FileHandler
{
  public interface IFileService : IScopedDependency
  {
    Task<IFile> GetFileResultWithStreamById(Guid id, byte[] rowVersion, CancellationToken cancellationToken);
  }
}