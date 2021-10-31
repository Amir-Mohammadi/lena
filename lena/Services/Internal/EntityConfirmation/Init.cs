using lena.Services.Core.Exceptions;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Models.EntityConfirmation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Domains.Enums;

using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.EntityConfirmation
{
  public partial class Confirmation : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public Confirmation(IRepository repository)
    {
      this.repository = repository;
    }

  }
}
