using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.ExitReceiptRequest;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {

    #region Get
    public ExitReceiptRequestSummary GetExitReceiptRequestSummary(int id) => GetExitReceiptRequestSummary(selector: e => e, id: id);

    public TResult GetExitReceiptRequestSummary<TResult>(
        Expression<Func<ExitReceiptRequestSummary, TResult>> selector,
        int id)
    {

      var exitReceiptRequestSummary = GetExitReceiptRequestSummarys(
                   selector: selector,
                   id: id)


               .FirstOrDefault();

      if (exitReceiptRequestSummary == null)
        throw new ExitReceiptRequestSummaryNotFoundException(id);

      return exitReceiptRequestSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetExitReceiptRequestSummarys<TResult>(
            Expression<Func<ExitReceiptRequestSummary, TResult>> selector,
            TValue<int> id = null
            //TValue<double> permissionQty = null,
            //TValue<double> preparingSendingQty = null,
            //TValue<double> sentQty = null
            )
    {

      var query = repository.GetQuery<ExitReceiptRequestSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      //if (permissionQty != null)
      //    query = query.Where(x => x.PermissionQty == permissionQty);

      return query.Select(selector);
    }
    #endregion
    #region Add
    public ExitReceiptRequestSummary AddExitReceiptRequestSummary(
                int exitReceiptRequestId,
                double permissionQty,
                double preparingSendingQty,
                double sentQty
                )
    {

      var exitReceiptRequest = App.Internals.SaleManagement.GetExitReceiptRequest(exitReceiptRequestId);

      var exitReceiptRequestSummary = repository.Create<ExitReceiptRequestSummary>();
      exitReceiptRequestSummary.ExitReceiptRequest = exitReceiptRequest;
      exitReceiptRequestSummary.PermissionQty = permissionQty;
      exitReceiptRequestSummary.PreparingSendingQty = preparingSendingQty;
      exitReceiptRequestSummary.SendedQty = sentQty;

      repository.Add(exitReceiptRequestSummary);
      return exitReceiptRequestSummary;
    }
    #endregion
    #region Edit
    public ExitReceiptRequestSummary EditExitReceiptRequestSummary(
        int id,
        byte[] rowVersion,
        TValue<double> permissionQty = null,
        TValue<double> preparingSendingQty = null,
        TValue<double> sentQty = null
        )
    {

      var exitReceiptRequestSummary = GetExitReceiptRequestSummary(id: id);

      return EditExitReceiptRequestSummary(
                exitReceiptRequestSummary: exitReceiptRequestSummary,
                rowVersion: rowVersion,
                 permissionQty: permissionQty,
                 preparingSendingQty: preparingSendingQty,
                 sentQty: sentQty
                );
    }

    public ExitReceiptRequestSummary EditExitReceiptRequestSummary(
                ExitReceiptRequestSummary exitReceiptRequestSummary,
                byte[] rowVersion,
               TValue<double> permissionQty = null,
        TValue<double> preparingSendingQty = null,
        TValue<double> sentQty = null

                )
    {


      if (permissionQty != null)
        exitReceiptRequestSummary.PermissionQty = permissionQty;
      if (preparingSendingQty != null)
        exitReceiptRequestSummary.PreparingSendingQty = preparingSendingQty;
      if (sentQty != null)
        exitReceiptRequestSummary.SendedQty = sentQty;

      repository.Update(rowVersion: rowVersion, entity: exitReceiptRequestSummary);
      return exitReceiptRequestSummary;
    }

    #endregion
    #region Delete
    public void DeleteExitReceiptRequestSummary(int id)
    {

      var exitReceiptRequestSummary = GetExitReceiptRequestSummary(id: id);
      repository.Delete(exitReceiptRequestSummary);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ExitReceiptRequestSummaryResult> SortExitReceiptRequestSummaryResult(
        IQueryable<ExitReceiptRequestSummaryResult> query,
        SortInput<ExitReceiptRequestSummarySortType> sort)
    {
      switch (sort.SortType)
      {
        case ExitReceiptRequestSummarySortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ExitReceiptRequestSummarySortType.PermissionQty:
          return query.OrderBy(a => a.PermissionQty, sort.SortOrder);
        case ExitReceiptRequestSummarySortType.PreparingSendingQty:
          return query.OrderBy(a => a.PreparingSendingQty, sort.SortOrder);
        case ExitReceiptRequestSummarySortType.SendedQty:
          return query.OrderBy(a => a.SendedQty, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<ExitReceiptRequestSummaryResult> SearchExitReceiptRequestSummaryResult(
        IQueryable<ExitReceiptRequestSummaryResult> query,
        string searchText)
    {

      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<ExitReceiptRequestSummary, ExitReceiptRequestSummaryResult>> ToExitReceiptRequestSummaryResult =
                exitReceiptRequestSummary => new ExitReceiptRequestSummaryResult
                {
                  Id = exitReceiptRequestSummary.Id,
                  PermissionQty = exitReceiptRequestSummary.PermissionQty,
                  PreparingSendingQty = exitReceiptRequestSummary.PreparingSendingQty,
                  SendedQty = exitReceiptRequestSummary.SendedQty,
                  RowVersion = exitReceiptRequestSummary.RowVersion
                };
    #endregion

  }
}
