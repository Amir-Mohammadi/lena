using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains;
using lena.Models.Supplies.StuffBasePriceCustoms;
using lena.Models.Supplies.StuffBasePriceTransport;
using lena.Services.Core;
using lena.Models.Supplies.StuffBasePrice;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region AddProcess
    public StuffBasePrice AddComputionalStuffBasePriceProcess(
        StuffBasePrice stuffBasePrice,
        byte currencyId,
        double mainPrice,
        int stuffId,
        string description,
        AddStuffBasePriceCustomsInput stuffBasePriceCustoms,
        AddStuffBasePriceTransportInput stuffBasePriceTransport,
        TValue<int> purchaseOrderId = null)
    {

      PurchaseOrder purchaseOrder = null;
      if (purchaseOrderId != null)
      {
        purchaseOrder = App.Internals.Supplies.GetPurchaseOrder(id: purchaseOrderId);
      }
      stuffBasePrice = stuffBasePrice ?? repository.Create<StuffBasePrice>();
      stuffBasePrice.PurchaseOrder = purchaseOrder;
      var price = mainPrice;
      #region Add
      AddComputionalStuffBasePrice(
          stuffBasePrice: stuffBasePrice,
          currencyId: currencyId,
          price: price,
          description: description,
          mainPrice: mainPrice,
          stuffId: stuffId);
      #endregion
      #region AddCustoms
      App.Internals.Supplies.AddStuffBasePriceCustoms(
                    price: stuffBasePriceCustoms.Price,
                    currencyId: stuffBasePriceCustoms.CurrencyId,
                    howToBuyId: stuffBasePriceCustoms.HowToBuyId,
                    howToBuyRatio: stuffBasePriceCustoms.HowToBuyRatio,
                    tariff: stuffBasePriceCustoms.Tariff,
                    percent: stuffBasePriceCustoms.Percent,
                    weight: stuffBasePriceCustoms.Weight,
                    type: stuffBasePriceCustoms.Type,
                    stuffBasePrice: stuffBasePrice);
      #endregion
      #region AddTransport
      App.Internals.Supplies.AddStuffBasePriceTransport(
          price: stuffBasePriceTransport.Price,
          percent: stuffBasePriceTransport.Percent,
          computeType: stuffBasePriceTransport.ComputeType,
          type: stuffBasePriceTransport.Type,
          stuffBasePrice: stuffBasePrice
          );
      #endregion
      return stuffBasePrice;
    }

    public void AddComputionalStuffsBasePriceProcess(
        int stuffPriceId,
        byte[] stuffPriceRowVersion,
        byte currencyId,
        double mainPrice,
        int[] stuffIds,
        string description,
        int? purchaseOrderId,
        AddStuffBasePriceCustomsInput stuffBasePriceCustoms,
        AddStuffBasePriceTransportInput stuffBasePriceTransport)
    {


      foreach (var stuffId in stuffIds)
      {
        #region Add ComputionalStuffPriceProcess

        AddComputionalStuffBasePriceProcess(
                stuffBasePrice: null,
                currencyId: currencyId,
                mainPrice: mainPrice,
                stuffId: stuffId,
                description: description,
                purchaseOrderId: purchaseOrderId,
                stuffBasePriceCustoms: stuffBasePriceCustoms,
                stuffBasePriceTransport: stuffBasePriceTransport);
        #endregion

        #region Check StuffPriceStatus Is Not Archived To Confirm
        var stuffPrice = GetStuffPrices(
                selector: e => e,
                id: stuffPriceId)


            .FirstOrDefault();

        if (stuffPrice == null) return;

        if (stuffPrice.Status != StuffPriceStatus.Archived)
        {
          #region Check PermissionToConfirmStuffPrice                   
          var confirmStuffPricePermission =
              App.Internals.UserManagement.CheckPermission(
                actionName: Models.StaticData.StaticActionName.ConfirmStuffPrice,
                actionParameters: null);

          if (confirmStuffPricePermission.AccessType == AccessType.Allowed)
          {
            #region ConfirmStuffPriceProcess
            ConfirmStuffPriceProcess(
             id: stuffPriceId,
             rowVersion: stuffPriceRowVersion,
             description: null);
            #endregion
          }
          #endregion
        }
        #endregion
      }

    }

    #endregion


    #region Add
    public StuffBasePrice AddComputionalStuffBasePrice(
        StuffBasePrice stuffBasePrice,
        byte currencyId,
        double price,
        double mainPrice,
        string description,
        int stuffId)
    {

      stuffBasePrice = stuffBasePrice ?? repository.Create<StuffBasePrice>();
      stuffBasePrice.MainPrice = mainPrice;
      AddStuffBasePriceProcess(
                stuffBasePrice: stuffBasePrice,
                currencyId: currencyId,
                description: description,
                price: price,
                stuffId: stuffId,
                stuffBasePriceType: StuffBasePriceType.Computional);
      return stuffBasePrice;
    }
    #endregion
    #region EditProcess
    public StuffBasePrice EditComputionalStuffBasePriceProcess(
        int id,
        byte[] rowVersion,
        byte currencyId,
        double mainPrice,
        string description,
        int? purchaseOrderId,
        AddStuffBasePriceCustomsInput stuffBasePriceCustoms,
        AddStuffBasePriceTransportInput stuffBasePriceTransport)
    {

      #region Archive
      var stuffPrice = ArchiveStuffPriceProcess(id: id,
              rowVersion: rowVersion);
      #endregion
      #region Add New StuffBasePrice
      return AddComputionalStuffBasePriceProcess(
              stuffBasePrice: null,
              currencyId: currencyId,
              mainPrice: mainPrice,
              description: description,
              stuffId: stuffPrice.StuffId,
              stuffBasePriceCustoms: stuffBasePriceCustoms,
              purchaseOrderId: purchaseOrderId,
              stuffBasePriceTransport: stuffBasePriceTransport);
      #endregion
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetComputionalStuffBasePrices<TResult>(
        Expression<Func<StuffBasePrice, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<int> stuffId = null,
        TValue<StuffPriceStatus> status = null,
        TValue<int> currencyId = null,
        TValue<int> userId = null)
    {

      var baseQuery = GetStuffBasePrices(
                    selector: e => e,
                    id: id,
                    code: code,
                    stuffId: stuffId,
                    status: status,
                    currencyId: currencyId,
                    userId: userId,
                    stuffBasePriceType: StuffBasePriceType.Computional);
      var query = baseQuery.OfType<StuffBasePrice>();
      return query.Select(selector);
    }
    #endregion
    #region Get
    public StuffBasePrice GetComputionalStuffBasePrice(int id) => GetStuffBasePrice(selector: e => e, id: id);
    public TResult GetComputionalStuffBasePrice<TResult>(Expression<Func<StuffBasePrice, TResult>> selector, int id)
    {

      var stuffBasePrice = GetComputionalStuffBasePrices(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (stuffBasePrice == null)
        throw new ComputionalStuffBasePriceNotFoundException(id);
      return stuffBasePrice;
    }
    #endregion
  }
}
