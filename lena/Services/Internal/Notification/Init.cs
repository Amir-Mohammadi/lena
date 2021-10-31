using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.Notification
{
  public partial class Notification : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public Notification(IRepository repository)
    {
      this.repository = repository;
    }

  }
}
