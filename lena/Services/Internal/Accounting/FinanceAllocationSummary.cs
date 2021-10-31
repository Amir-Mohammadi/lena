using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Accounting.FinanceAllocationSummary;
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
    public FinanceAllocationSummary AddFinanceAllocationSummary(
        Finance finance,
        double requestedAmount)
    {

      var financeAllocationSummary = repository.Create<FinanceAllocationSummary>();
      financeAllocationSummary.Finance = finance;
      financeAllocationSummary.RequestedAmount = requestedAmount;
      repository.Add(financeAllocationSummary);
      return financeAllocationSummary;
    }
    #endregion

    #region Edit
    public FinanceAllocationSummary EditFinanceAllocationSummary(
        FinanceAllocationSummary financeAllocationSummary,
        byte[] rowVersion,
        TValue<int> financeId = null,
        TValue<double> requestedAmount = null,
        TValue<double> allocationAmount = null,
        TValue<double> separatedTransferAmount = null,
        TValue<double> transferredAmount = null
       )
    {


      if (financeId != null)
        financeAllocationSummary.Finance.Id = financeId;
      if (requestedAmount != null)
        financeAllocationSummary.RequestedAmount = requestedAmount;
      if (allocationAmount != null)
        financeAllocationSummary.AllocatedAmount = allocationAmount;
      if (separatedTransferAmount != null)
        financeAllocationSummary.SeparatedTransferAmount = separatedTransferAmount;
      if (transferredAmount != null)
        financeAllocationSummary.TransferredAmount = transferredAmount;

      repository.Update(rowVersion: rowVersion, entity: financeAllocationSummary);
      return financeAllocationSummary;
    }

    #endregion

    #region Get

    public FinanceAllocationSummary GetFinanceAllocationSummary(int financeId) => GetFinanceAllocationSummary(selector: e => e, financeId: financeId);
    public TResult GetFinanceAllocationSummary<TResult>(
        Expression<Func<FinanceAllocationSummary, TResult>> selector,
        int financeId)
    {

      var financeAllocationSummary = GetFinanceAllocationSummaries(
                    selector: selector,
                    financeId: financeId)


                .FirstOrDefault();

      if (financeAllocationSummary == null)
        throw new RecordNotFoundException(financeId, typeof(FinanceAllocationSummary));
      return financeAllocationSummary;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetFinanceAllocationSummaries<TResult>(
            Expression<Func<FinanceAllocationSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<int> financeId = null
            )
    {

      var query = repository.GetQuery<FinanceAllocationSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (financeId != null)
        query = query.Where(x => x.Finance.Id == financeId);

      return query.Select(selector);
    }
    #endregion



    #region ToResult
    public Expression<Func<FinanceAllocationSummary, FinanceAllocationSummaryResult>> ToFinanceAllocationSummaryResult =
        financeAllocationSummary => new FinanceAllocationSummaryResult
        {
          Id = financeAllocationSummary.Id,
          FinanceId = financeAllocationSummary.Finance.Id,
          TotalRequestedAmount = financeAllocationSummary.RequestedAmount,
          TotalAllocatedAmount = financeAllocationSummary.AllocatedAmount,
          TotalTransferAmount = financeAllocationSummary.TransferredAmount,
          TotalSeparatedTransferAmount = financeAllocationSummary.SeparatedTransferAmount,
          RowVersion = financeAllocationSummary.RowVersion
        };
    #endregion
  }
}
