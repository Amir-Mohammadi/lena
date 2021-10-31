using System;
using System.Threading.Tasks;
using core.Autofac;
using core.Models;
namespace core.FileHandler
{
  public interface IFileCacheService : ISingletonDependency
  {
    Task<IFile> Get(Guid id, string rowVersion);
  }
}