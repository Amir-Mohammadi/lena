using System;
using lena.Services.Core.Foundation;
using lena.Models.Guard.InboundCargoCooperator;
using lena.Models.Guard.OutboundCargo;

using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.Guard
{
  public partial class Guard : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public Guard(IRepository repository)
    {
      this.repository = repository;
    }

  }
}
