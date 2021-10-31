//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains;
using lena.Models.Supplies.RiskParameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {

    #region Gets
    public IQueryable<TResult> GetRiskParameters<TResult>(
        Expression<Func<RiskParameter, TResult>> selector)
    {


      var riskParameter = repository.GetQuery<RiskParameter>();
      return riskParameter.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<RiskParameter, RiskParameterResult>> ToRiskParameterResult =
        riskParameter =>
        new RiskParameterResult
        {
          OccurrenceProbabilityStatus = riskParameter.OccurrenceProbabilityStatus,
          OccurrenceSeverityStatus = riskParameter.OccurrenceSeverityStatus,
          RiskLevelStatus = riskParameter.RiskLevelStatus,
          RowVersion = riskParameter.RowVersion
        };
    #endregion
  }
}
