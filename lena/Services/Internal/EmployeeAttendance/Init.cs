//using LinqLib.Sort;
using lena.Services.Common;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.EmployeeAttendance;
using lena.Models.QualityGuarantee.ProductionCapacity;
using System;
// using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using core.Data;
using lena.Domains.Enums;
namespace lena.Services.Internals.EmployeeAttendance
{
  public partial class EmployeeAttendance : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public EmployeeAttendance(IRepository repository)
    {
      this.repository = repository;
    }
  }
}