using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Common;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {

    #region Get SendPermissionId
    public SendPermissionSummary GetSendPermissionSummaryBySendPermissionId(int sendPermissionId) =>
    GetSendPermissionSummaryBySendPermissionId(selector: e => e, sendPermissionId: sendPermissionId);
    public TResult GetSendPermissionSummaryBySendPermissionId<TResult>(
        Expression<Func<SendPermissionSummary, TResult>> selector,
        int sendPermissionId)
    {

      var sendPermissionSummary = GetSendPermissionSummarys(
                    selector: selector,
                    sendPermissionId: sendPermissionId)


                .FirstOrDefault();
      if (sendPermissionSummary == null)
        throw new SendPermissionSummaryForSendPermissionNotFoundException(sendPermissionId: sendPermissionId);
      return sendPermissionSummary;
    }
    #endregion
    #region Get
    public SendPermissionSummary GetSendPermissionSummary(int id) => GetSendPermissionSummary(selector: e => e, id: id);
    public TResult GetSendPermissionSummary<TResult>(
        Expression<Func<SendPermissionSummary, TResult>> selector,
        int id)
    {

      var sendPermissionSummary = GetSendPermissionSummarys(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (sendPermissionSummary == null)
        throw new SendPermissionSummaryNotFoundException(id: id);
      return sendPermissionSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetSendPermissionSummarys<TResult>(
            Expression<Func<SendPermissionSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> preparingSendingQty = null,
            TValue<double> sendedQty = null,
            TValue<int> sendPermissionId = null)
    {

      var query = repository.GetQuery<SendPermissionSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (preparingSendingQty != null)
        query = query.Where(x => x.PreparingSendingQty == preparingSendingQty);
      if (sendedQty != null)
        query = query.Where(x => x.SendedQty == sendedQty);
      if (sendPermissionId != null)
        query = query.Where(x => x.SendPermission.Id == sendPermissionId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public SendPermissionSummary AddSendPermissionSummary(
        double preparingSendingQty,
        double sendedQty,
        int sendPermissionId)
    {

      var sendPermissionSummary = repository.Create<SendPermissionSummary>();
      sendPermissionSummary.PreparingSendingQty = preparingSendingQty;
      sendPermissionSummary.SendedQty = sendedQty;
      sendPermissionSummary.SendPermission = GetSendPermission(id: sendPermissionId);
      repository.Add(sendPermissionSummary);
      return sendPermissionSummary;
    }
    #endregion
    #region Edit
    public SendPermissionSummary EditSendPermissionSummary(
        int id,
        byte[] rowVersion,
        TValue<double> preparingSendingQty = null,
        TValue<double> sendedQty = null)
    {

      var sendPermissionSummary = GetSendPermissionSummary(id: id);
      return EditSendPermissionSummary(
                    sendPermissionSummary: sendPermissionSummary,
                    rowVersion: rowVersion,
                    preparingSendingQty: preparingSendingQty,
                    sendedQty: sendedQty);

    }

    public SendPermissionSummary EditSendPermissionSummary(
                SendPermissionSummary sendPermissionSummary,
                byte[] rowVersion,
                TValue<double> preparingSendingQty = null,
                TValue<double> sendedQty = null)
    {


      if (preparingSendingQty != null)
        sendPermissionSummary.PreparingSendingQty = preparingSendingQty;
      if (sendedQty != null)
        sendPermissionSummary.SendedQty = sendedQty;
      repository.Update(rowVersion: rowVersion, entity: sendPermissionSummary);
      return sendPermissionSummary;
    }

    #endregion
    #region Delete
    public void DeleteSendPermissionSummary(int id)
    {

      var sendPermissionSummary = GetSendPermissionSummary(id: id);
      repository.Delete(sendPermissionSummary);
    }
    #endregion
    #region Reset
    public SendPermissionSummary ResetSendPermissionSummaryBySendPermissionId(int sendPermissionId)
    {

      var sendPermissionSummary = GetSendPermissionSummaryBySendPermissionId(sendPermissionId: sendPermissionId); ; return ResetSendPermissionSummary(sendPermissionSummary: sendPermissionSummary);

    }
    public SendPermissionSummary ResetSendPermissionSummary(int id)
    {

      var sendPermissionSummary = GetSendPermissionSummary(id: id); ; return ResetSendPermissionSummary(sendPermissionSummary: sendPermissionSummary);

    }

    public SendPermissionSummary ResetSendPermissionSummary(SendPermissionSummary sendPermissionSummary)
    {

      #region GetSendProducts
      var sendProductQtys = App.Internals.WarehouseManagement.GetSendProducts(
              selector: e => e.PreparingSending.Qty * e.PreparingSending.Unit.ConversionRatio /
                             e.PreparingSending.SendPermission.Unit.ConversionRatio,
              isDelete: false,
              sendPermissionId: sendPermissionSummary.SendPermission.Id);
      var sendedQty = sendProductQtys.Any() ? sendProductQtys.Sum() : 0;
      #endregion
      #region GetPreparingSendings
      var preparingSendingQtys = App.Internals.WarehouseManagement.GetPreparingSendings(
              selector: e => e.Qty * e.Unit.ConversionRatio /
                             e.SendPermission.Unit.ConversionRatio,
              isDelete: false,
              sendPermissionId: sendPermissionSummary.SendPermission.Id);
      var preparingSendingQty = preparingSendingQtys.Any() ? preparingSendingQtys.Sum() : 0;
      #endregion
      #region EditSendPermissionSummary
      EditSendPermissionSummary(
              sendPermissionSummary: sendPermissionSummary,
              rowVersion: sendPermissionSummary.RowVersion,
              preparingSendingQty: preparingSendingQty,
              sendedQty: sendedQty);
      #endregion
      #region ResetExitReceiptStatus
      ResetExitReceiptRequestStatus(id: sendPermissionSummary.SendPermission.ExitReceiptRequestId);
      #endregion
      return sendPermissionSummary;
    }

    #endregion
  }
}
