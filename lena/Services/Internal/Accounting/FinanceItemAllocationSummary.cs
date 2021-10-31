using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Accounting.CooperatorFinancialAccount;
using lena.Models.Accounting.FinanceItemAllocationSummary;
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
    public FinanceItemAllocationSummary AddFinanceItemAllocationSummary(
        int financeId,
        int cooperatorId,
        double requestedAmount)
    {

      var financeItemAllocationSummary = repository.Create<FinanceItemAllocationSummary>();
      financeItemAllocationSummary.FinanceId = financeId;
      financeItemAllocationSummary.CooperatorId = cooperatorId;
      financeItemAllocationSummary.TotalRequestedAmout = requestedAmount;
      repository.Add(financeItemAllocationSummary);
      return financeItemAllocationSummary;
    }
    #endregion

    #region Edit
    public FinanceItemAllocationSummary EditFinanceItemAllocationSummary(
        FinanceItemAllocationSummary financeItemAllocationSummary,
        byte[] rowVersion,
        TValue<int> financeId = null,
        TValue<int> cooperatorId = null,
        TValue<double> requestedAmount = null,
        TValue<double> allocationAmount = null,
        TValue<double> transferredAmount = null
       )
    {


      if (financeId != null)
        financeItemAllocationSummary.Finance.Id = financeId;
      if (requestedAmount != null)
        financeItemAllocationSummary.TotalRequestedAmout = requestedAmount;
      if (allocationAmount != null)
        financeItemAllocationSummary.TotalAllocatedAmount = allocationAmount;
      if (transferredAmount != null)
        financeItemAllocationSummary.TotalTransferredAmount = transferredAmount;
      if (cooperatorId != null)
        financeItemAllocationSummary.CooperatorId = cooperatorId;

      repository.Update(rowVersion: rowVersion, entity: financeItemAllocationSummary);
      return financeItemAllocationSummary;
    }

    #endregion

    #region Get

    public FinanceItemAllocationSummary GetFinanceItemAllocationSummary(int financeId, int cooperatorId) => GetFinanceItemAllocationSummary(selector: e => e, financeId: financeId, cooperatorId: cooperatorId);
    public TResult GetFinanceItemAllocationSummary<TResult>(
        Expression<Func<FinanceItemAllocationSummary, TResult>> selector,
        int financeId,
        int cooperatorId)
    {

      var financeItemAllocationSummary = GetFinanceItemAllocationSummaries(
                    selector: selector,
                    financeId: financeId,
                    cooperatorId: cooperatorId)


                .FirstOrDefault();

      if (financeItemAllocationSummary == null)
        throw new RecordNotFoundException(financeId, typeof(FinanceItemAllocationSummary));
      return financeItemAllocationSummary;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetFinanceItemAllocationSummaries<TResult>(
            Expression<Func<FinanceItemAllocationSummary, TResult>> selector,
            TValue<int> financeId = null,
            TValue<int> cooperatorId = null
            )
    {

      var query = repository.GetQuery<FinanceItemAllocationSummary>();

      if (financeId != null)
        query = query.Where(x => x.FinanceId == financeId);
      if (cooperatorId != null)
        query = query.Where(x => x.CooperatorId == cooperatorId);

      return query.Select(selector);

    }
    #endregion

    #region Delete 


    public void DeleteFinanceItemAllocationSummary(FinanceItemAllocationSummary financeItemAllocationSummary)
    {

      repository.Delete(financeItemAllocationSummary);
    }
    #endregion


    #region ToResult

    public Expression<Func<FinanceItemAllocationSummary, FinanceItemAllocationSummaryResult>> ToFinanceItemAllocationSummaryResult =
          financeItemAllocationSummary => new FinanceItemAllocationSummaryResult
          {
            FinanceId = financeItemAllocationSummary.FinanceId,
            CooperatorId = financeItemAllocationSummary.CooperatorId,
            TotalRequestedAmount = financeItemAllocationSummary.TotalRequestedAmout,
            TotalAllocatedAmount = financeItemAllocationSummary.TotalAllocatedAmount,
            TotalTransferAmount = financeItemAllocationSummary.TotalTransferredAmount,
            RowVersion = financeItemAllocationSummary.RowVersion
          };

    #endregion
  }
}
