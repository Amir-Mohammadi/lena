using lena.Services.Core;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Supplies.RiskResolve;
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
    public RiskResolve AddRiskResolve(
        int riskId,
        string correctiveAction)
    {

      var riskResolve = repository.Create<RiskResolve>();
      riskResolve.DateTime = DateTime.UtcNow;
      riskResolve.CreatorUserId = App.Providers.Security.CurrentLoginData.UserId;
      riskResolve.CorrectiveAction = correctiveAction;
      riskResolve.Status = RiskResolveStatus.NotAction;
      riskResolve.RiskId = riskId;
      repository.Add(riskResolve);

      return riskResolve;
    }
    #endregion

    #region Edit
    public RiskResolve EditRiskResolve(
        int id,
        byte[] rowVersion,
        TValue<string> correctiveAction = null,
        TValue<int> reviewerUserId = null,
        TValue<string> reviewDescription = null,
        TValue<DateTime> revieweDateTime = null,
        TValue<RiskResolveStatus> riskResolveStatus = null)
    {

      var riskResolve = GetRiskResolve(id: id);

      return EditRiskResolve(
                    riskResolve: riskResolve,
                    rowVersion: rowVersion,
                    correctiveAction: correctiveAction,
                    reviewerUserId: reviewerUserId,
                    revieweDateTime: revieweDateTime,
                    reviewDescription: reviewDescription,
                    riskResolveStatus: riskResolveStatus);
    }
    public RiskResolve EditRiskResolve(
        RiskResolve riskResolve,
        byte[] rowVersion,
        TValue<string> correctiveAction = null,
        TValue<int> reviewerUserId = null,
        TValue<string> reviewDescription = null,
        TValue<DateTime> revieweDateTime = null,
        TValue<RiskResolveStatus> riskResolveStatus = null)
    {

      if (correctiveAction != null)
        riskResolve.CorrectiveAction = correctiveAction;
      if (riskResolveStatus != null)
        riskResolve.Status = riskResolveStatus;
      if (reviewerUserId != null)
        riskResolve.ReviewerUserId = reviewerUserId;
      if (reviewDescription != null)
        riskResolve.ReviewDescription = reviewDescription;
      if (revieweDateTime != null)
        riskResolve.RevieweDateTime = revieweDateTime;
      repository.Update(entity: riskResolve, rowVersion: rowVersion);
      return riskResolve;
    }
    #endregion

    #region AddProcess
    public void AddRiskResolveProcess(
        int riskId,
        string correctiveAction)
    {

      var riskResolves = GetRiskResolves(e => e, riskId: riskId);


      if (riskResolves.Any(m => m.Status == RiskResolveStatus.NotAction))
      {
        throw new RiskHaveNotActionResolveException(riskId: riskId);
      }

      if (riskResolves.Any(m => m.Status == RiskResolveStatus.Effective && m.RiskStatus.RiskParameter.RiskLevelStatus == RiskLevelStatus.Low))
      {
        throw new RiskHaveEffectiveRiskResolveException(riskId: riskId);
      }

      var risk = GetRisk(id: riskId);

      var riskResolve = AddRiskResolve(
                riskId: risk.Id,
                correctiveAction: correctiveAction);

      EditRisk(
                risk: risk,
                rowVersion: risk.RowVersion,
                riskResolve: riskResolve);

    }
    #endregion

    #region Delete
    public void DeleteRiskResolve(RiskResolve riskResolve)
    {

      repository.Delete(riskResolve);
    }
    #endregion

    #region DeleteProcess
    public void DeleteRiskResolveProcess(int id)
    {


      var riskResolve = GetRiskResolve(id: id);

      if (riskResolve.Status != RiskResolveStatus.NotAction)
      {
        throw new CannotDeleteRiskResolveInThisStatusException(id: riskResolve.Id);
      }

      var risk = GetRisk(id: riskResolve.RiskId);

      var riskResolves = risk.RiskResolves.ToList();

      if (riskResolves.Any(m => m.Id != id))
      {
        var riskResolveId = riskResolves.Where(m => m.Id != id).Max(m => m.Id);

        var resolve = riskResolves.FirstOrDefault(m => m.Id == riskResolveId);

        EditRisk(
              risk: risk,
              rowVersion: risk.RowVersion,
              riskResolve: resolve);
      }
      else
      {

        EditRisk(
              risk: risk,
              rowVersion: risk.RowVersion,
              riskResolve: new TValue<RiskResolve>(null));
      }


      DeleteRiskResolve(riskResolve: riskResolve);
    }
    #endregion

    #region RiskResolveDetermineProcess
    public void RiskResolveDetermineProcess(
        int id,
        byte[] rowVersion,
        string reviewDescription,
        RiskResolveStatus riskResolveStatus,
        OccurrenceSeverityStatus? occurrenceSeverityStatus,
        OccurrenceProbabilityStatus? occurrenceProbabilityStatus)
    {

      var riskResolve = GetRiskResolve(id: id);

      if (riskResolve.Status != RiskResolveStatus.NotAction)
      {
        throw new RiskResolveHasDeterminedException(riskResolveId: riskResolve.Id);
      }

      EditRiskResolve(
                riskResolve: riskResolve,
                rowVersion: rowVersion,
                reviewDescription: reviewDescription,
                revieweDateTime: DateTime.UtcNow,
                riskResolveStatus: riskResolveStatus,
                reviewerUserId: App.Providers.Security.CurrentLoginData.UserId);

      if (riskResolveStatus == RiskResolveStatus.Effective)
      {
        var risk = GetRisk(id: riskResolve.RiskId);

        var riskStatus = AddRiskStatus(
                  riskId: riskResolve.RiskId,
                  occurrenceSeverityStatus: occurrenceSeverityStatus.Value,
                  occurrenceProbabilityStatus: occurrenceProbabilityStatus.Value,
                  riskResolve: riskResolve);


        EditRisk(
                  risk: risk,
                  rowVersion: risk.RowVersion,
                  riskStatus: riskStatus);
      }


    }
    #endregion

    #region Get
    public RiskResolve GetRiskResolve(int id) => GetRiskResolve(selector: e => e, id: id);
    public TResult GetRiskResolve<TResult>(
        Expression<Func<RiskResolve, TResult>> selector,
        int id)
    {

      var riskResolve = GetRiskResolves(selector: selector,
                id: id).FirstOrDefault();
      if (riskResolve == null)
        throw new RiskResolveNotFoundException(id: id);
      return riskResolve;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetRiskResolves<TResult>(
        Expression<Func<RiskResolve, TResult>> selector,
        TValue<int> id = null,
        TValue<int> riskId = null)
    {


      var risk = repository.GetQuery<RiskResolve>();

      if (id != null)
        risk = risk.Where(i => i.Id == id);
      if (riskId != null)
        risk = risk.Where(i => i.RiskId == riskId);

      return risk.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<RiskResolve, RiskResolveResult>> ToRiskResolveResult =
        riskResolve =>
        new RiskResolveResult
        {
          Id = riskResolve.Id,
          RiskId = riskResolve.RiskId,
          CorrectiveAction = riskResolve.CorrectiveAction,
          CreatorUserId = riskResolve.CreatorUserId,
          DateTime = riskResolve.DateTime,
          Status = riskResolve.Status,
          ReviewerUserId = riskResolve.ReviewerUserId,
          RevieweDateTime = riskResolve.RevieweDateTime,
          ReviewDescription = riskResolve.ReviewDescription,
          CreatorEmployeeName = riskResolve.CreatorUser.Employee.FirstName + " " + riskResolve.CreatorUser.Employee.LastName,
          ReviewerEmployeeName = riskResolve.ReviewerUser.Employee.FirstName + " " + riskResolve.ReviewerUser.Employee.LastName,
          RiskLevelStatus = riskResolve.RiskStatus.RiskParameter.RiskLevelStatus,
          RowVersion = riskResolve.RowVersion,

        };
    #endregion

  }
}
