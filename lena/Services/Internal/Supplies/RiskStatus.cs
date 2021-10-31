using lena.Services.Core;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Supplies.RiskStatus;
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
    #region Add
    public RiskStatus AddRiskStatus(
        int riskId,
        OccurrenceSeverityStatus occurrenceSeverityStatus,
        OccurrenceProbabilityStatus occurrenceProbabilityStatus,
        RiskResolve riskResolve = null)
    {

      var riskStatus = repository.Create<RiskStatus>();
      riskStatus.DateTime = DateTime.UtcNow;
      riskStatus.UserId = App.Providers.Security.CurrentLoginData.UserId;
      riskStatus.OccurrenceProbabilityStatus = occurrenceProbabilityStatus;
      riskStatus.OccurrenceSeverityStatus = occurrenceSeverityStatus;
      riskStatus.RiskId = riskId;
      riskStatus.RiskResolve = riskResolve;
      repository.Add(riskStatus);

      return riskStatus;
    }
    #endregion

    #region Get
    public RiskStatus GetRiskStatus(int id) => GetRiskStatus(selector: e => e, id: id);
    public TResult GetRiskStatus<TResult>(
        Expression<Func<RiskStatus, TResult>> selector,
        int id)
    {

      var GetRiskStatus = GetRiskStatuses(selector: selector,
                id: id).FirstOrDefault();
      if (GetRiskStatus == null)
        throw new RiskStatusNotFoundException(id: id);
      return GetRiskStatus;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetRiskStatuses<TResult>(
        Expression<Func<RiskStatus, TResult>> selector,
        TValue<int> id = null,
        TValue<int> userId = null,
        TValue<int> riskId = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<RiskLevelStatus> riskLevel = null)
    {


      var riskStatus = repository.GetQuery<RiskStatus>();

      if (id != null)
        riskStatus = riskStatus.Where(i => i.Id == id);
      if (riskId != null)
        riskStatus = riskStatus.Where(i => i.RiskId == riskId);
      if (userId != null)
        riskStatus = riskStatus.Where(i => i.UserId == userId);
      if (fromDateTime != null)
        riskStatus = riskStatus.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        riskStatus = riskStatus.Where(i => i.DateTime <= toDateTime);
      if (riskLevel != null)
        riskStatus = riskStatus.Where(i => i.RiskParameter.RiskLevelStatus == riskLevel);

      return riskStatus.Select(selector);
    }


    #endregion

    #region Delete
    public void DeleteRiskStatus(
      RiskStatus riskStatus)
    {

      repository.Delete(riskStatus);
    }
    #endregion

    #region ToResult
    public Expression<Func<RiskStatus, RiskStatusResult>> ToPurchaseOrderFinanceConfirmationResult =
       riskStatus => new RiskStatusResult
       {
         Id = riskStatus.Id,
         UserId = riskStatus.UserId,
         EmployeeName = riskStatus.User.Employee.FirstName + " " + riskStatus.User.Employee.LastName,
         DateTime = riskStatus.DateTime,
         RiskLevel = riskStatus.RiskParameter.RiskLevelStatus,
       };
    #endregion
  }
}
