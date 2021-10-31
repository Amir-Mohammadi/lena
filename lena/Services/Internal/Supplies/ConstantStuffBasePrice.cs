using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains;
using lena.Models.Supplies.StuffBasePrice;
using lena.Services.Core;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region AddProcess
    public StuffBasePrice AddConstantStuffBasePriceProcess(
        StuffBasePrice stuffBasePrice,
        byte currencyId,
        double price,
        int stuffId,
        string description,
        TValue<int> purchaseOrderId = null)
    {

      PurchaseOrder purchaseOrder = null;
      if (purchaseOrderId != null)
        purchaseOrder = App.Internals.Supplies.GetPurchaseOrder(id: purchaseOrderId);

      stuffBasePrice = stuffBasePrice ?? repository.Create<StuffBasePrice>();
      stuffBasePrice.MainPrice = price;
      stuffBasePrice.Description = description;
      stuffBasePrice.PurchaseOrder = purchaseOrder;
      AddStuffBasePriceProcess(
            stuffBasePrice: stuffBasePrice,
            currencyId: currencyId,
                price: price,
                stuffId: stuffId,
                stuffBasePriceType: StuffBasePriceType.Constant,
                description: description);
      return stuffBasePrice;
    }
    public void AddConstantStuffsBasePriceProcess(
        int? stuffPriceId,
        byte[] stuffPriceRowVersion,
        byte currencyId,
        double price,
        int[] stuffIds,
        int? purchaseOrderId,
        string description)
    {

      foreach (var stuff in stuffIds)
      {
        AddConstantStuffBasePriceProcess(
                      stuffBasePrice: null,
                      currencyId: currencyId,
                      price: price,
                      stuffId: stuff,
                      purchaseOrderId: purchaseOrderId,
                      description: description);
      }
      if (stuffPriceId.HasValue)
      {
        #region Check StuffPriceStatus Is Not Archived To Confirm
        var stuffPrice = GetStuffPrice(id: stuffPriceId.Value);

        if (stuffPrice.Status != StuffPriceStatus.Archived)
        {
          #region Check PermissionToConfirmStuffPrice                   
          var confirmStuffPricePermission =
              App.Internals.UserManagement.CheckPermission(
                actionName: Models.StaticData.StaticActionName.ConfirmStuffPrice,
                actionParameters: null
                );

          if (confirmStuffPricePermission.AccessType == AccessType.Allowed)
          {
            #region ConfirmStuffPriceProcess
            ConfirmStuffPriceProcess(
             id: stuffPriceId.Value,
             rowVersion: stuffPriceRowVersion,
             description: description);
            #endregion
          }
          #endregion
        }
        #endregion
      }
    }
    #endregion
    #region EditProcess
    public StuffBasePrice EditConstantStuffBasePriceProcess(
        int id,
        byte[] rowVersion,
        byte currencyId,
        double price,
        int? purchaseOrderId,
        string description)
    {

      #region Archive

      var stuffPrice = ArchiveStuffPriceProcess(id: id,
              rowVersion: rowVersion);
      #endregion
      #region Add New StuffBasePrice
      return AddConstantStuffBasePriceProcess(
              stuffBasePrice: null,
              currencyId: currencyId,
              price: price,
              purchaseOrderId: purchaseOrderId,
              stuffId: stuffPrice.StuffId,
              description: stuffPrice.Description);
      #endregion
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetConstantStuffBasePrices<TResult>(
        Expression<Func<StuffBasePrice, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<int> stuffId = null,
        TValue<StuffPriceStatus> status = null,
        TValue<int> currencyId = null,
        TValue<int> userId = null,
        TValue<string> description = null)
    {

      var baseQuery = GetStuffBasePrices(
                    selector: e => e,
                    id: id,
                    code: code,
                    stuffId: stuffId,
                    status: status,
                    currencyId: currencyId,
                    userId: userId,
                    stuffBasePriceType: StuffBasePriceType.Constant,
                    description: description);
      var query = baseQuery.OfType<StuffBasePrice>();
      return query.Select(selector);
    }

    #endregion
    #region Get
    public StuffBasePrice GetConstantStuffBasePrice(int id) => GetConstantStuffBasePrice(selector: e => e, id: id);
    public TResult GetConstantStuffBasePrice<TResult>(Expression<Func<StuffBasePrice, TResult>> selector, int id)
    {

      var stuffBasePrice = GetConstantStuffBasePrices(
                    selector: selector,
                    id: id)


                .FirstOrDefault();

      if (stuffBasePrice == null)
        throw new ConstantStuffBasePriceNotFoundException(id);
      return stuffBasePrice;
    }

    #endregion
  }
}

