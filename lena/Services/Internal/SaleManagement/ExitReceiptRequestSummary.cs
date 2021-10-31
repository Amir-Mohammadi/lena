using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {

    #region Get
    public ExitReceiptRequestSummary GetExitReceiptRequestSummaryByExitReceiptRequestId(int exitReceiptRequestId) =>
    GetExitReceiptRequestSummaryByExitReceiptRequestId(selector: e => e, exitReceiptRequestId: exitReceiptRequestId);
    public TResult GetExitReceiptRequestSummaryByExitReceiptRequestId<TResult>(
        Expression<Func<ExitReceiptRequestSummary, TResult>> selector,
        int exitReceiptRequestId)
    {

      var exitReceiptRequestSummary = GetExitReceiptRequestSummarys(
                    selector: selector,
                    exitReceiptRequestId: exitReceiptRequestId)


                .FirstOrDefault();
      if (exitReceiptRequestSummary == null)
        throw new ExitReceiptRequestSummaryForExitReceiptRequestNotFoundException(exitReceiptRequestId: exitReceiptRequestId);
      return exitReceiptRequestSummary;
    }
    #endregion



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
        throw new ExitReceiptRequestSummaryNotFoundException(id: id);
      return exitReceiptRequestSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetExitReceiptRequestSummarys<TResult>(
            Expression<Func<ExitReceiptRequestSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> plannedQty = null,
            TValue<double> producedQty = null,
            TValue<double> blockedQty = null,
            TValue<double> permissionQty = null,
            TValue<double> preparingSendingQty = null,
            TValue<double> sendedQty = null,
            TValue<int> exitReceiptRequestId = null)
    {

      var query = repository.GetQuery<ExitReceiptRequestSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (permissionQty != null)
        query = query.Where(x => x.PermissionQty == permissionQty);
      if (preparingSendingQty != null)
        query = query.Where(x => x.PreparingSendingQty == preparingSendingQty);
      if (sendedQty != null)
        query = query.Where(x => x.SendedQty == sendedQty);
      if (exitReceiptRequestId != null)
        query = query.Where(x => x.ExitReceiptRequest.Id == exitReceiptRequestId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public ExitReceiptRequestSummary AddExitReceiptRequestSummary(
        double permissionQty,
        double preparingSendingQty,
        double sendedQty,
        int exitReceiptRequestId)
    {

      var exitReceiptRequestSummary = repository.Create<ExitReceiptRequestSummary>();
      exitReceiptRequestSummary.PermissionQty = permissionQty;
      exitReceiptRequestSummary.PreparingSendingQty = preparingSendingQty;
      exitReceiptRequestSummary.SendedQty = sendedQty;
      exitReceiptRequestSummary.ExitReceiptRequest = GetExitReceiptRequest(id: exitReceiptRequestId);
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
        TValue<double> sendedQty = null)
    {

      var exitReceiptRequestSummary = GetExitReceiptRequestSummary(id: id);
      return EditExitReceiptRequestSummary(
                    exitReceiptRequestSummary: exitReceiptRequestSummary,
                    rowVersion: rowVersion,
                    permissionQty: permissionQty,
                    preparingSendingQty: preparingSendingQty,
                    sendedQty: sendedQty);

    }

    public ExitReceiptRequestSummary EditExitReceiptRequestSummary(
                ExitReceiptRequestSummary exitReceiptRequestSummary,
                byte[] rowVersion,
                TValue<double> permissionQty = null,
                TValue<double> preparingSendingQty = null,
                TValue<double> sendedQty = null)
    {


      if (permissionQty != null)
        exitReceiptRequestSummary.PermissionQty = permissionQty;
      if (preparingSendingQty != null)
        exitReceiptRequestSummary.PreparingSendingQty = preparingSendingQty;
      if (sendedQty != null)
        exitReceiptRequestSummary.SendedQty = sendedQty;
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
    #region Reset
    public ExitReceiptRequestSummary ResetExitReceiptRequestSummaryByExitReceiptRequestId(int exitReceiptRequestId)
    {

      var exitReceiptRequestSummary = GetExitReceiptRequestSummaryByExitReceiptRequestId(exitReceiptRequestId: exitReceiptRequestId); ; return ResetExitReceiptRequestSummary(exitReceiptRequestSummary: exitReceiptRequestSummary);

    }
    public ExitReceiptRequestSummary ResetExitReceiptRequestSummary(int id)
    {

      var exitReceiptRequestSummary = GetExitReceiptRequestSummary(id: id); ; return ResetExitReceiptRequestSummary(exitReceiptRequestSummary: exitReceiptRequestSummary);

    }

    public ExitReceiptRequestSummary ResetExitReceiptRequestSummary(ExitReceiptRequestSummary exitReceiptRequestSummary)
    {

      #region GetSendPermissions
      var sendPermissionQtys = GetSendPermissions(
              selector: e => new
              {
                SendPermissionQty = e.Qty * e.Unit.ConversionRatio / e.ExitReceiptRequest.Unit.ConversionRatio,
                PreparingSendingQty = e.SendPermissionSummary.PreparingSendingQty * e.Unit.ConversionRatio / e.ExitReceiptRequest.Unit.ConversionRatio,
                SendedQty = e.SendPermissionSummary.SendedQty * e.Unit.ConversionRatio / e.ExitReceiptRequest.Unit.ConversionRatio
              },
              sendPermissionStatusType: SendPermissionStatusType.Accepted,
                  isDelete: false,
                  exitReceiptRequestId: exitReceiptRequestSummary.ExitReceiptRequest.Id);
      var permissionQty = 0d;
      var preparingSendingQty = 0d;
      var sendedQty = 0d;
      if (sendPermissionQtys.Any())
      {
        permissionQty = sendPermissionQtys.Sum(i => i.SendPermissionQty);
        preparingSendingQty = sendPermissionQtys.Sum(i => i.PreparingSendingQty);
        sendedQty = sendPermissionQtys.Sum(i => i.SendedQty);
      }

      #endregion
      #region EditExitReceiptRequestSummary
      EditExitReceiptRequestSummary(
              exitReceiptRequestSummary: exitReceiptRequestSummary,
              rowVersion: exitReceiptRequestSummary.RowVersion,
              permissionQty: permissionQty,
              preparingSendingQty: preparingSendingQty,
              sendedQty: sendedQty);
      #endregion
      #region ResetOrderItemStatus
      var orderItemBlockblock = exitReceiptRequestSummary.ExitReceiptRequest as OrderItemBlock;
      if (orderItemBlockblock != null)
        ResetOrderItemStatus(id: orderItemBlockblock.OrderItemId);
      #endregion
      return exitReceiptRequestSummary;
    }
    #endregion
  }
}
