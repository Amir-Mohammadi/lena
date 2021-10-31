using lena.Services.Core;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Accounting.FinanceItemConfirmation;
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
    public FinanceItemConfirmation AddFinanceItemConfirmation(
        int financeItemId,
        FinanceItemConfirmationStatus financeItemConfirmationStatus)
    {

      var financeItemConfirmation = repository.Create<FinanceItemConfirmation>();
      financeItemConfirmation.DateTime = DateTime.UtcNow;
      financeItemConfirmation.UserId = App.Providers.Security.CurrentLoginData.UserId;
      financeItemConfirmation.Status = financeItemConfirmationStatus;
      financeItemConfirmation.FinanceItemId = financeItemId;
      repository.Add(financeItemConfirmation);

      return financeItemConfirmation;
    }
    #endregion

    #region Get
    public FinanceItemConfirmation GetFinanceItemConfirmation(int id) => GetFinanceItemConfirmation(selector: e => e, id: id);
    public TResult GetFinanceItemConfirmation<TResult>(
        Expression<Func<FinanceItemConfirmation, TResult>> selector,
        int id)
    {

      var financeItemConfirmation = GetFinanceItemConfirmations(selector: selector,
                id: id).FirstOrDefault();
      if (financeItemConfirmation == null)
        throw new RecordNotFoundException(id, typeof(FinanceItemConfirmation));
      return financeItemConfirmation;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetFinanceItemConfirmations<TResult>(
        Expression<Func<FinanceItemConfirmation, TResult>> selector,
        TValue<int> id = null,
        TValue<int> userId = null,
        TValue<int> financeItemId = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<FinanceItemConfirmationStatus> status = null)
    {


      var financeItemConfirmation = repository.GetQuery<FinanceItemConfirmation>();

      if (id != null)
        financeItemConfirmation = financeItemConfirmation.Where(i => i.Id == id);

      if (financeItemId != null)
        financeItemConfirmation = financeItemConfirmation.Where(i => i.FinanceItemId == financeItemId);

      if (userId != null)
        financeItemConfirmation = financeItemConfirmation.Where(i => i.UserId == userId);

      if (fromDateTime != null)
        financeItemConfirmation = financeItemConfirmation.Where(i => i.DateTime >= fromDateTime);

      if (toDateTime != null)
        financeItemConfirmation = financeItemConfirmation.Where(i => i.DateTime <= toDateTime);

      if (status != null)
        financeItemConfirmation = financeItemConfirmation.Where(i => i.Status == status);

      return financeItemConfirmation.Select(selector);
    }


    #endregion

    #region Delete
    public void DeleteFinanceItemConfirmation(
      FinanceItemConfirmation financeItemConfirmation)
    {

      repository.Delete(financeItemConfirmation);
    }
    #endregion

    #region ToResult
    public Expression<Func<FinanceItemConfirmation, FinanceItemConfirmationResult>> ToFinanceItemConfirmationResult =
       financeItemConfirmation => new FinanceItemConfirmationResult
       {
         Id = financeItemConfirmation.Id,
         UserId = financeItemConfirmation.UserId,
         EmployeeName = financeItemConfirmation.User.Employee.FirstName + " " + financeItemConfirmation.User.Employee.LastName,
         DateTime = financeItemConfirmation.DateTime,
         Status = financeItemConfirmation.Status,
       };

    #endregion
  }
}
