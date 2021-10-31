using System;
using System.Threading.Tasks;
using core.Autofac;

namespace core.StateManager
{
  public interface IStateManagerService : IScopedDependency
  {
    Task<bool> SetState<T>(string key, T value);
    Task<bool> SetState<T>(string key, T value, TimeSpan timeSpan);
    Task<T> GetState<T>(string key);
    Task<bool> RemoveState(string value);
    Task<bool> ClearState();
  }
}