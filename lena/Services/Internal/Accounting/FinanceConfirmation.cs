using lena.Services.Core;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Accounting.FinanceConfirmation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Add
    public FinanceConfirmation AddFinanceConfirmation(
        int financeId,
        FinanceConfirmationStatus financeConfirmationStatus)
    {

      var financeConfirmation = repository.Create<FinanceConfirmation>();
      financeConfirmation.DateTime = DateTime.UtcNow;
      financeConfirmation.UserId = App.Providers.Security.CurrentLoginData.UserId;
      financeConfirmation.Status = financeConfirmationStatus;
      financeConfirmation.FinanceId = financeId;
      repository.Add(financeConfirmation);

      return financeConfirmation;
    }
    #endregion

    #region Get
    public FinanceConfirmation GetFinanceConfirmation(int id) => GetFinanceConfirmation(selector: e => e, id: id);
    public TResult GetFinanceConfirmation<TResult>(
        Expression<Func<FinanceConfirmation, TResult>> selector,
        int id)
    {

      var financeConfirmation = GetFinanceConfirmations(selector: selector,
                id: id).FirstOrDefault();
      if (financeConfirmation == null)
        throw new RecordNotFoundException(id, typeof(FinanceItemConfirmation));
      return financeConfirmation;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetFinanceConfirmations<TResult>(
        Expression<Func<FinanceConfirmation, TResult>> selector,
        TValue<int> id = null,
        TValue<int> userId = null,
        TValue<int> financeId = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<FinanceConfirmationStatus> status = null)
    {


      var financeConfirmation = repository.GetQuery<FinanceConfirmation>();

      if (id != null)
        financeConfirmation = financeConfirmation.Where(i => i.Id == id);

      if (financeId != null)
        financeConfirmation = financeConfirmation.Where(i => i.FinanceId == financeId);

      if (userId != null)
        financeConfirmation = financeConfirmation.Where(i => i.UserId == userId);

      if (fromDateTime != null)
        financeConfirmation = financeConfirmation.Where(i => i.DateTime >= fromDateTime);

      if (toDateTime != null)
        financeConfirmation = financeConfirmation.Where(i => i.DateTime <= toDateTime);

      if (status != null)
        financeConfirmation = financeConfirmation.Where(i => i.Status == status);

      return financeConfirmation.Select(selector);
    }


    #endregion

    #region Delete
    public void DeleteFinanceConfirmation(
      FinanceItemConfirmation financeItemConfirmation)
    {

      repository.Delete(financeItemConfirmation);
    }
    #endregion

    #region ToResult
    public Expression<Func<FinanceConfirmation, FinanceConfirmationResult>> ToFinanceConfirmationResult =
       FinanceConfirmation => new FinanceConfirmationResult
       {
         Id = FinanceConfirmation.Id,
         UserId = FinanceConfirmation.UserId,
         EmployeeName = FinanceConfirmation.User.Employee.FirstName + " " + FinanceConfirmation.User.Employee.LastName,
         DateTime = FinanceConfirmation.DateTime,
         Status = FinanceConfirmation.Status,
       };

    #endregion
  }
}
