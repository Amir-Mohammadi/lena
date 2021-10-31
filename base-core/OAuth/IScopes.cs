using System.Collections.Generic;
using core.Autofac;
namespace core.OAuth
{
  public interface IScopes : ISingletonDependency
  {
    Dictionary<string, int[]> Map { get; }
  }
}