using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Models.QualityGuarantee.ProductionCapacity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityGuarantee
{
  public partial class QualityGuarantee : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public QualityGuarantee(IRepository repository)
    {
      this.repository = repository;
    }

  }
}
