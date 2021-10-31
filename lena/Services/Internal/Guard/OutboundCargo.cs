using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Services.Internals.Guard.Exception;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Guard.OutboundCargo;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Guard
{
  public partial class Guard
  {
    #region Add
    public OutboundCargo AddOutboundCargo(
        OutboundCargo outboundCargo,
        TransactionBatch transactionBatch,
        DateTime transportDateTime,
        string shippingCompanyName,
        string driverName,
        string carNumber,
        string phoneNumber,
        string carInformation,
        string description,
        int entranceTransportId)
    {

      outboundCargo = outboundCargo ?? repository.Create<OutboundCargo>();
      AddTransportProcess(
                    transport: outboundCargo,
                    transactionBatch: transactionBatch,
                    description: description,
                    transportDateTime: transportDateTime,
                    shippingCompanyName: shippingCompanyName,
                    driverName: driverName,
                    phoneNumber: phoneNumber,
                    carNumber: carNumber,
                    carInformation: carInformation,
                    entranceTransportId: entranceTransportId,
                    transportType: TransportType.Outbound);
      return outboundCargo;
    }
    #endregion
    #region Edit
    public OutboundCargo EditOutboundCargo(
        int id,
        byte[] rowVersion,
        TValue<DateTime> transportDateTime = null,
        TValue<string> shippingCompanyName = null,
        TValue<string> driverName = null,
        TValue<string> phoneNumber = null,
        TValue<string> carNumber = null,
        TValue<string> carInformation = null,
        TValue<string> description = null,
        TValue<int> entranceTransportId = null,
        TValue<TransportStatus> status = null)
    {

      var outboundCargo = GetOutboundCargo(id);
      EditOutboundCargo(
                    outboundCargo: outboundCargo,
                    rowVersion: rowVersion,
                    transportDateTime: transportDateTime,
                    shippingCompanyName: shippingCompanyName,
                    driverName: driverName,
                    phoneNumber: phoneNumber,
                    carNumber: carNumber,
                    carInformation: carInformation,
                    description: description,
                    entranceTransportId: entranceTransportId == null ? null : (int?)entranceTransportId,
                    status: status);
      return outboundCargo;
    }
    public OutboundCargo EditOutboundCargo(
        OutboundCargo outboundCargo,
        byte[] rowVersion,
        TValue<DateTime> transportDateTime = null,
        TValue<string> shippingCompanyName = null,
        TValue<string> driverName = null,
        TValue<string> phoneNumber = null,
        TValue<string> carNumber = null,
        TValue<string> carInformation = null,
        TValue<string> description = null,
        TValue<int?> entranceTransportId = null,
        TValue<TransportStatus> status = null)
    {

      EditTransport(
                    transport: outboundCargo,
                    rowVersion: rowVersion,
                    transportDateTime: transportDateTime,
                    shippingCompanyName: shippingCompanyName,
                    driverName: driverName,
                    phoneNumber: phoneNumber,
                    carNumber: carNumber,
                    carInformation: carInformation,
                    description: description,
                    entranceTransportId: entranceTransportId,
                    status: status);
      return outboundCargo;
    }
    #endregion
    #region Delete
    public void DeleteOutboundCargo(int id)
    {

      var outboundCargo = GetOutboundCargo(id);
      repository.Delete(outboundCargo);
    }
    #endregion
    #region Get
    public TResult GetOutboundCargo<TResult>(
        Expression<Func<OutboundCargo, TResult>> selector,
        int id)
    {

      var outboundCargo = GetOutboundCargos(
                    selector: selector,
                    id: id)

                .FirstOrDefault();
      if (outboundCargo == null)
        throw new OutboundCargoNotFoundException(id);
      return outboundCargo;
    }
    public OutboundCargo GetOutboundCargo(int id) => GetOutboundCargo(selector: e => e, id: id);
    #endregion
    #region Gets
    public IQueryable<TResult> GetOutboundCargos<TResult>(
        Expression<Func<OutboundCargo, TResult>> selector,
        TValue<int> id = null,
        TValue<string> driverName = null,
        TValue<string> phoneNumber = null,
        TValue<string> carNumber = null,
        TValue<string> carInformation = null,
        TValue<string> shippingCompanyName = null,
        TValue<string> description = null,
        TValue<DateTime> dateTime = null,
        TValue<int> userId = null,
        TValue<DateTime> transportDateTime = null,
        TValue<int?> entranceTransportId = null,
        TValue<int?> outputTransportId = null)
    {

      var baseQuery = GetTransports(
                    selector: e => e,
                    id: id,
                    driverName: driverName,
                    phoneNumber: phoneNumber,
                    carNumber: carNumber,
                    carInformation: carInformation,
                    shippingCompanyName: shippingCompanyName,
                    description: description,
                    userId: userId,
                    transportDateTime: transportDateTime,
                    entranceTransportId: entranceTransportId,
                    outputTransportId: outputTransportId);
      var query = baseQuery.OfType<OutboundCargo>();
      return query.Select(selector);
    }
    #endregion
    #region ToResult
    public Expression<Func<OutboundCargo, OutboundCargoResult>> ToOutboundCargoResult =
        outboundCargo => new OutboundCargoResult
        {
          Id = outboundCargo.Id,
          DateTime = outboundCargo.DateTime,
          CarInformation = outboundCargo.CarInformation,
          CarNumber = outboundCargo.CarNumber,
          Description = outboundCargo.Description,
          DriverName = outboundCargo.DriverName,
          TransportDateTime = outboundCargo.TransportDateTime,
          ShippingCompanyName = outboundCargo.ShippingCompanyName,
          UserId = outboundCargo.UserId,
          EmployeeFullName = outboundCargo.User.Employee.FirstName + " " + outboundCargo.User.Employee.LastName,
          RowVersion = outboundCargo.RowVersion
        };
    #endregion
    #region ToFullResult
    public Expression<Func<OutboundCargo, OutboundCargoFullResult>> ToOutboundCargoFullResult =
        outboundCargo => new OutboundCargoFullResult
        {
          Id = outboundCargo.Id,
          DateTime = outboundCargo.DateTime,
          CarInformation = outboundCargo.CarInformation,
          CarNumber = outboundCargo.CarNumber,
          Description = outboundCargo.Description,
          DriverName = outboundCargo.DriverName,
          PhoneNumber = outboundCargo.PhoneNumber,
          TransportDateTime = outboundCargo.TransportDateTime,
          ShippingCompanyName = outboundCargo.ShippingCompanyName,
          EntranceTransportId = outboundCargo.EntranceTransport.Id,
          ExitReceipts = outboundCargo.ExitReceipts.AsQueryable().Select(App.Internals.WarehouseManagement.ToExitReceiptFullResult),
          RowVersion = outboundCargo.RowVersion
        };
    #endregion
    #region AddProcess
    public OutboundCargo AddOutboundCargoProcess(
        DateTime transportDateTime,
        string shippingCompanyName,
        string driverName,
        string phoneNumber,
        string carNumber,
        string carInformation,
        string description,
        int entranceTransportId,
        AddOutboundCargoExitReceiptInput[] addOutboundCargoExitReceipts)
    {

      #region Add OutboundCargo

      var outboundCargo = AddOutboundCargo(
              outboundCargo: null,
              transactionBatch: null,
              transportDateTime: transportDateTime,
              shippingCompanyName: shippingCompanyName,
              driverName: driverName,
              phoneNumber: phoneNumber,
              carNumber: carNumber,
              carInformation: carInformation,
              description: description,
              entranceTransportId: entranceTransportId);
      #endregion
      #region Add ExitReceipt
      foreach (var addOutboundCargoExitReceipt in addOutboundCargoExitReceipts)
      {
        if (addOutboundCargoExitReceipt.Price > 0)
        {
          var exitReceipt = App.Internals.WarehouseManagement.GetExitReceipts(
                                    selector: App.Internals.WarehouseManagement.ToExitReceiptFullResult,
                                    id: addOutboundCargoExitReceipt.Id,
                                    isDelete: false)

                                  .FirstOrDefault();
          foreach (var sendProduct in exitReceipt.SendProducts)
          {

            FinancialTransactionBatch financialTransactionBatch = null;
            if (sendProduct.CooperatorId == null) throw new ProviderIsNotDefinedException();

            var cooperatorFinancialAccount = App.Internals.Accounting.GetCooperatorFinancialAccounts(
                          selector: e => e,
                          cooperatorId: sendProduct.CooperatorId)


                      .FirstOrDefault();

            if (cooperatorFinancialAccount == null)
              throw new CooperatorHasNoFinancialAccountException(cooperatorId: sendProduct.CooperatorId);

            #region FinancialTransactionBatch
            financialTransactionBatch = App.Internals.Accounting.AddFinancialTransactionBatch();
            #endregion

            App.Internals.Accounting.AddFinancialTransactionProcess(
            financialTransaction: null,
            amount: addOutboundCargoExitReceipt.Price.Value * sendProduct.Qty,
            effectDateTime: transportDateTime,
            description: null,
            financialAccountId: cooperatorFinancialAccount.Id,
            financialTransactionType: Models.StaticData.StaticFinancialTransactionTypes.GivebackExitReceipt,
            financialTransactionBatchId: financialTransactionBatch.Id,
            referenceFinancialTransaction: null);

          }
        }
        App.Internals.WarehouseManagement.ConfirmExitReceipt(
                      id: addOutboundCargoExitReceipt.Id,
                      rowVersion: addOutboundCargoExitReceipt.RowVersion,
                      outboundCargoId: outboundCargo.Id);
      }
      #endregion
      return outboundCargo;
    }
    #endregion
    #region EditProcess
    public OutboundCargo EditOutboundCargoProcess(
        int id,
        byte[] rowVersion,
        DateTime transportDateTime,
        string shippingCompanyName,
        string driverName,
        string phoneNumber,
        string carNumber,
        string carInformation,
        string description,
        int entranceTransportId,
        AddOutboundCargoExitReceiptInput[] addOutboundCargoExitReceipts,
        DeleteOutboundCargoExitReceiptInput[] deleteOutboundCargoExitReceipts)
    {

      #region Edit OutboundCargo

      var outboundCargo = EditOutboundCargo(
              id: id,
              rowVersion: rowVersion,
              transportDateTime: transportDateTime,
              shippingCompanyName: shippingCompanyName,
              driverName: driverName,
              phoneNumber: phoneNumber,
              carNumber: carNumber,
              carInformation: carInformation,
              description: description,
              entranceTransportId: entranceTransportId);
      #endregion
      #region Add ExitReceipt
      foreach (var addOutboundCargoExitReceipt in addOutboundCargoExitReceipts)
      {
        App.Internals.WarehouseManagement.ConfirmExitReceipt(
                      id: addOutboundCargoExitReceipt.Id,
                      rowVersion: addOutboundCargoExitReceipt.RowVersion,
                      outboundCargoId: outboundCargo.Id);
      }
      #endregion
      #region Delete OutboundCargoCooperators
      foreach (var deleteOutboundCargoExitReceipt in deleteOutboundCargoExitReceipts)
      {
        App.Internals.WarehouseManagement.UnConfirmExitReceipt(
                      id: deleteOutboundCargoExitReceipt.Id,
                      rowVersion: deleteOutboundCargoExitReceipt.RowVersion);

      }
      #endregion
      return outboundCargo;
    }
    #endregion

    #region Complate
    public OutboundCargo ComplateOutboundCargoProcess(
        int id,
        byte[] rowVersion)
    {

      #region Edit OutboundCargo

      var outboundCargo = EditOutboundCargo(
              id: id,
              rowVersion: rowVersion,
              status: TransportStatus.Exit);
      #endregion
      return outboundCargo;
    }
    #endregion
  }
}
