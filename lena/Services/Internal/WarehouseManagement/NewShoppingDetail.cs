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
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {

    #region Add
    public NewShoppingDetail AddNewShoppingDetail(
       NewShoppingDetail newShoppingDetail,
       TransactionBatch transactionBatch,
       int newShoppingId,
       int ladingItemDetailId,
       double qty,
       byte unitId,
       string description)
    {

      newShoppingDetail = newShoppingDetail ?? repository.Create<NewShoppingDetail>();
      newShoppingDetail.NewShoppingId = newShoppingId;
      newShoppingDetail.LadingItemDetailId = ladingItemDetailId;
      newShoppingDetail.Qty = qty;
      newShoppingDetail.UnitId = unitId;
      App.Internals.ApplicationBase.AddBaseEntity(
                baseEntity: newShoppingDetail,
                  transactionBatch: transactionBatch,
                  description: description);
      return newShoppingDetail;
    }
    #endregion
    #region Edit
    public NewShoppingDetail EditNewShoppingDetail(
        int id,
        byte[] rowVersion,
        TValue<int> newShoppingId = null,
        TValue<int> ladingItemDetailId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<string> description = null,
        TValue<bool> isDelete = null)
    {

      NewShoppingDetail newShoppingDetail = GetNewShoppingDetail(id: id);
      return EditNewShoppingDetail(
                newShoppingDetail: newShoppingDetail,
                newShoppingId: newShoppingId,
                ladingItemDetailId: ladingItemDetailId,
                rowVersion: rowVersion,
                qty: qty,
                unitId: unitId,
                description: description,
                isDelete: isDelete);



    }
    public NewShoppingDetail EditNewShoppingDetail(
        NewShoppingDetail newShoppingDetail,
        byte[] rowVersion,
        TValue<int> newShoppingId = null,
        TValue<int> ladingItemDetailId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<string> description = null,
        TValue<bool> isDelete = null)
    {

      if (newShoppingId != null)
        newShoppingDetail.NewShoppingId = newShoppingId;
      if (ladingItemDetailId != null)
        newShoppingDetail.LadingItemDetailId = ladingItemDetailId;
      if (qty != null)
        newShoppingDetail.Qty = qty;
      if (unitId != null)
        newShoppingDetail.UnitId = unitId;
      if (description != null)
        newShoppingDetail.Description = description;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: newShoppingDetail,
                    description: description,
                    isDelete: isDelete,
                    rowVersion: rowVersion);
      return retValue as NewShoppingDetail;
    }
    #endregion
    #region Get
    public NewShoppingDetail GetNewShoppingDetail(int id) => GetNewShoppingDetail(e => e, id: id);
    public TResult GetNewShoppingDetail<TResult>(
        Expression<Func<NewShoppingDetail, TResult>> selector,
        int id)
    {

      var newShoppingDetail = GetNewShoppingDetails(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (newShoppingDetail == null)
        throw new NewShoppingDetailNotFoundException(id);
      return newShoppingDetail;
    }
    public NewShoppingDetail GetNewShoppingDetail(string code) => GetNewShoppingDetail(e => e, code: code);
    public TResult GetNewShoppingDetail<TResult>(
        Expression<Func<NewShoppingDetail, TResult>> selector,
        string code)
    {

      var newShoppingDetail = GetNewShoppingDetails(
                    selector: selector,
                    code: code)


                .FirstOrDefault();
      if (newShoppingDetail == null)
        throw new NewShoppingDetailNotFoundException(code: code);
      return newShoppingDetail;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetNewShoppingDetails<TResult>(
        Expression<Func<NewShoppingDetail, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> newShoppingId = null,
        TValue<int> ladingItemId = null,
        TValue<int> ladingItemDetailId = null,
        TValue<double> qty = null,
        TValue<int> unitId = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<NewShoppingDetail>();
      if (newShoppingId != null)
        query = query.Where(r => r.NewShoppingId == newShoppingId);
      if (ladingItemDetailId != null)
        query = query.Where(r => r.LadingItemDetailId == ladingItemDetailId);
      if (qty != null)
        query = query.Where(r => Math.Abs(r.Qty - qty) < 0.000001);
      if (unitId != null)
        query = query.Where(r => r.UnitId == unitId);
      if (ladingItemId != null)
        query = query.Where(r => r.LadingItemDetail.LadingItemId == ladingItemId);

      return query.Select(selector);
    }
    #endregion
    #region Remove
    public void RemoveNewShoppingDetail(int id, byte[] rowVersion)
    {

      EditNewShoppingDetail(
                id: id,
                rowVersion: rowVersion,
                isDelete: true);
    }
    #endregion
    #region RemoveProcess
    public void RemoveNewShoppingDetailProcess(
        int id,
        byte[] rowVersion,
        TransactionBatch transactionBatch)
    {

      #region RemoveNewShopping
      RemoveNewShoppingDetail(
              id: id,
              rowVersion: rowVersion);
      #endregion
    }
    #endregion
    #region ResetStatus
    // public NewShoppingDetail ResetNewShoppingDetailStatus(int newShoppingDetailId)
    // {
    //     
    //         var newShoppingDetail = GetNewShoppingDetail(id: newShoppingDetailId);
    //         return ResetNewShoppingDetailStatus(newShoppingDetail: newShoppingDetail)
    //             
    //;
    //     });
    // }
    // public NewShoppingDetail ResetNewShoppingDetailStatus(NewShoppingDetail newShoppingDetail)
    // {
    //     
    //         #region ResetOrderItemSummary
    //         var newShoppingDetailSummary = ResetNewShoppingDetailSummaryByNewShoppingDetailId(
    //                 newShoppingDetailId: newShoppingDetail.Id)
    //             
    //;
    //         #endregion
    //         return newShoppingDetail;
    //     });
    // }
    #endregion
  }
}
