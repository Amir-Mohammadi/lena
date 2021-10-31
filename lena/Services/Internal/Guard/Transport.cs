using System;
//using System.Data.Entity;
//using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Guard.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Guard;
using lena.Models.Guard.Transport;
//using System.Data.Entity.Core.Objects;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Guard
{
  public partial class Guard
  {
    #region Add
    public Transport AddTransport(
        Transport transport,
        TransactionBatch transactionBatch,
        string description,
        DateTime transportDateTime,
        string shippingCompanyName,
        string driverName,
        string phoneNumber,
        string carNumber,
        string carInformation,
        int? entranceTransportId,
        TransportType transportType,
        TransportStatus status)
    {

      transport = transport ?? repository.Create<Transport>();
      transport.ShippingCompanyName = shippingCompanyName;
      transport.Description = description;
      transport.DriverName = driverName;
      transport.PhoneNumber = phoneNumber;
      transport.CarNumber = carNumber;
      transport.CarInformation = carInformation;
      transport.TransportDateTime = transportDateTime;
      transport.UserId = App.Providers.Security.CurrentLoginData.UserId;
      transport.Status = status;
      if (entranceTransportId != null)
        transport.EntranceTransport = GetTransport(id: entranceTransportId.Value);
      transport.TransportType = transportType;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: transport,
                    transactionBatch: null,
                    description: description);
      return transport;
    }
    #endregion
    #region AddProcess
    public Transport AddTransportProcess(
        Transport transport,
        TransactionBatch transactionBatch,
        string description,
        DateTime transportDateTime,
        string shippingCompanyName,
        string driverName,
        string phoneNumber,
        string carNumber,
        string carInformation,
        int? entranceTransportId,
        TransportType transportType)
    {

      var status = TransportStatus.NotAction;
      if (transportType == TransportType.Inbound)
      {
        if (transport is InboundCargo)
          status = TransportStatus.NotAction;
        else
          status = TransportStatus.Complated;
      }
      else if (transportType == TransportType.Outbound)
      {
        if (transport is OutboundCargo)
          status = TransportStatus.Waiting;
        else
          status = TransportStatus.Exit;
      }
      return AddTransport(transport: transport,
                    transactionBatch: null,
                    description: description,
                    transportDateTime: transportDateTime,
                    shippingCompanyName: shippingCompanyName,
                    driverName: driverName,
                    phoneNumber: phoneNumber,
                    carNumber: carNumber,
                    carInformation: carInformation,
                    entranceTransportId: entranceTransportId,
                    transportType: transportType,
                    status: status);
    }
    #endregion
    #region Edit
    public Transport EditTransport(
        int id,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<DateTime> transportDateTime = null,
        TValue<string> shippingCompanyName = null,
        TValue<string> driverName = null,
        TValue<string> phoneNumber = null,
        TValue<string> carNumber = null,
        TValue<string> carInformation = null,
        TValue<int?> entranceTransportId = null,
        TValue<int?> outputTransportId = null,
        TValue<TransportStatus> status = null)
    {

      var transport = GetTransport(id: id);
      return EditTransport(
                    transport: transport,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description,
                    transportDateTime: transportDateTime,
                    shippingCompanyName: shippingCompanyName,
                    driverName: driverName,
                    phoneNumber: phoneNumber,
                    carNumber: carNumber,
                    carInformation: carInformation,
                    entranceTransportId: entranceTransportId,
                    outputTransportId: outputTransportId,
                    status: status);
    }
    public Transport EditTransport(
        Transport transport,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<DateTime> transportDateTime = null,
        TValue<string> shippingCompanyName = null,
        TValue<string> driverName = null,
        TValue<string> phoneNumber = null,
        TValue<string> carNumber = null,
        TValue<string> carInformation = null,
        TValue<int?> entranceTransportId = null,
        TValue<int?> outputTransportId = null,
        TValue<TransportStatus> status = null)

    {

      if (transportDateTime != null)
        transport.TransportDateTime = transportDateTime;
      if (shippingCompanyName != null)
        transport.ShippingCompanyName = shippingCompanyName;
      if (driverName != null)
        transport.DriverName = driverName;
      if (phoneNumber != null)
        transport.PhoneNumber = phoneNumber;
      if (carNumber != null)
        transport.CarNumber = carNumber;
      if (carInformation != null)
        transport.CarInformation = carInformation;
      if (status != null)
        transport.Status = status;
      if (entranceTransportId != null)
        if (entranceTransportId.Value == null)
          transport.EntranceTransport = null;
        else
          transport.EntranceTransport = GetTransport(id: entranceTransportId.Value.Value);
      if (outputTransportId != null)
        if (outputTransportId.Value == null)
          transport.EntranceTransport = null;
        else
          transport.OutputTransport = GetTransport(id: outputTransportId.Value.Value);
      App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: transport,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description);
      return transport;
    }
    #endregion
    #region Delete
    public void RemoveTransport(int id, byte[] rowVersion)
    {

      #region RemoveBaseEntity
      var transport = GetTransport(id: id);
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                    baseEntity: transport,
                    transactionBatchId: null,
                    rowVersion: rowVersion);
      #endregion
      if (transport is OutboundCargo)
      {
        var outboundCargo = transport as OutboundCargo;
        #region GetExitReceipts
        var exitReceipts = App.Internals.WarehouseManagement.GetExitReceipts(
                selector: e => new { e.Id, e.RowVersion },
                outboundCargoId: outboundCargo.Id,
                confirmed: true,
                isDelete: false);
        #endregion
        #region Delete OutboundCargoCooperators
        foreach (var exitReceipt in exitReceipts)
        {
          App.Internals.WarehouseManagement.UnConfirmExitReceipt(
                            id: exitReceipt.Id,
                            rowVersion: exitReceipt.RowVersion);
        }
        #endregion
      }
    }
    #endregion
    #region Get
    public TResult GetTransport<TResult>(
        Expression<Func<Transport, TResult>> selector,
        int id)
    {

      var transport = GetTransports(
                    selector: selector,
                    id: id)

                .FirstOrDefault();
      if (transport == null)
        throw new TransportNotFoundException(id);
      return transport;
    }
    public Transport GetTransport(int id) => GetTransport(selector: e => e, id: id);
    #endregion
    #region Gets
    public IQueryable<TResult> GetTransports<TResult>(
        Expression<Func<Transport, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<string> driverName = null,
        TValue<string> phoneNumber = null,
        TValue<string> carNumber = null,
        TValue<string> carInformation = null,
        TValue<string> shippingCompanyName = null,
        TValue<DateTime> transportDateTime = null,
        TValue<int?> entranceTransportId = null,
        TValue<int?> outputTransportId = null,
        TValue<TransportType> transportType = null,
        TValue<int> cooperatorId = null,
        TValue<bool> haveOutputTransport = null,
        TValue<TransportStatus> status = null,
        TValue<TransportStatus[]> statuses = null
        )
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<Transport>();
      if (driverName != null)
        query = query.Where(i => i.DriverName == driverName);
      if (phoneNumber != null)
        query = query.Where(i => i.PhoneNumber == phoneNumber);
      if (carNumber != null)
        query = query.Where(i => i.CarNumber == carNumber);
      if (carInformation != null)
        query = query.Where(i => i.CarInformation == carInformation);
      if (shippingCompanyName != null)
        query = query.Where(i => i.ShippingCompanyName == shippingCompanyName);
      if (transportDateTime != null)
        query = query.Where(i => i.TransportDateTime == transportDateTime);
      if (entranceTransportId != null)
        query = query.Where(i => i.EntranceTransport.Id == entranceTransportId);
      if (outputTransportId != null)
        query = query.Where(i => i.OutputTransport.Id == outputTransportId);
      if (transportType != null)
        query = query.Where(i => i.TransportType == transportType);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (haveOutputTransport != null)
        query = query.Where(i => (((int?)i.OutputTransport.Id) != null) == haveOutputTransport);
      if (status != null)
        query = query.Where(i => i.Status == status);
      if (cooperatorId != null)
      {
        query = query.OfType<InboundCargo>();
        query = query.Where(i => ((InboundCargo)i).InboundCargoCooperators.Any(u => u.CooperatorId == cooperatorId));
      }
      if (statuses != null)
      {
        query = query.Where(i => statuses.Value.Contains(i.Status));
      }

      return query.Select(selector);
    }

    #endregion
    #region ToResult
    public IQueryable<TransportResult> ToTransportResultQuery(IQueryable<Transport> transports)
    {
      var cooperatorNames = from transport in transports.OfType<InboundCargo>()
                            select new
                            {
                              TransportId = transport.Id,
                              CooperatorName = transport.InboundCargoCooperators.Select(i => i.Cooperator.Name)
                            };

      var stuffNames = from transport in transports.OfType<InboundCargo>()
                       select new
                       {
                         TransportId = transport.Id,
                         StuffName = transport.StoreReceipts.Select(i => i.Stuff.Name)
                       };


      var resultQuery = from transport in transports
                        join cn in cooperatorNames
                        on transport.Id equals cn.TransportId into trans1
                        from tran1 in trans1.DefaultIfEmpty()
                        join sn in stuffNames
                        on transport.Id equals sn.TransportId into trans2
                        from tran2 in trans2.DefaultIfEmpty()
                        select new TransportResult
                        {
                          Id = transport.Id,
                          TransportCode = transport.Code,
                          TransportType = transport.TransportType,
                          TransportStatus = transport.Status,
                          DateTime = transport.DateTime,
                          CarInformation = transport.CarInformation,
                          CarNumber = transport.CarNumber,
                          Description = transport.Description,
                          DriverName = transport.DriverName,
                          CooperatorNames = tran1.CooperatorName,
                          StuffNames = tran2.StuffName,
                          TransportDateTime = transport.TransportDateTime,
                          ShippingCompanyName = transport.ShippingCompanyName,
                          UserId = transport.UserId,
                          EmployeeFullName = transport.User.Employee.FirstName + " " + transport.User.Employee.LastName,
                          EntranceTransportId = transport.EntranceTransport != null ? (int?)transport.EntranceTransport.Id : null,
                          OutputTransportId = transport.OutputTransport != null ? (int?)transport.OutputTransport.Id : null,
                          PhoneNumber = transport.PhoneNumber,
                          IsInboundCargo = transport is InboundCargo ? true : false,
                          Status = transport.Status,
                          RowVersion = transport.RowVersion
                        };

      return resultQuery;
    }

    public TransportResult ToTransportResult(Transport transport)
    {
      var result = new TransportResult
      {
        Id = transport.Id,
        TransportCode = transport.Code,
        TransportType = transport.TransportType,
        TransportStatus = transport.Status,
        DateTime = transport.DateTime,
        CarInformation = transport.CarInformation,
        CarNumber = transport.CarNumber,
        Description = transport.Description,
        DriverName = transport.DriverName,
        TransportDateTime = transport.TransportDateTime,
        ShippingCompanyName = transport.ShippingCompanyName,
        UserId = transport.UserId,
        EmployeeFullName = transport.User.Employee.FirstName + " " + transport.User.Employee.LastName,
        EntranceTransportId = transport.EntranceTransport != null ? (int?)transport.EntranceTransport.Id : null,
        OutputTransportId = transport.OutputTransport != null ? (int?)transport.OutputTransport.Id : null,
        PhoneNumber = transport.PhoneNumber,
        IsInboundCargo = transport is InboundCargo ? true : false,
        Status = transport.Status,
        RowVersion = transport.RowVersion
      };
      return result;
    }
    #endregion
    #region ToComboResult
    public Expression<Func<Transport, TransportComboResult>> ToTransportComboResult =
        transport => new TransportComboResult
        {
          Id = transport.Id,
          TransportCode = transport.Code,
          CarNumber = transport.CarNumber,
          DriverName = transport.DriverName,
          PhoneNumber = transport.PhoneNumber,
          ShippingCompanyName = transport.ShippingCompanyName,
          TransportDateTime = transport.TransportDateTime
        };
    #endregion
    #region Sort
    public IOrderedQueryable<TransportResult> SortTransportResult(IQueryable<TransportResult> input, SortInput<TransportSortType> options)
    {
      switch (options.SortType)
      {
        case TransportSortType.Id:
          return input.OrderBy(i => i.Id, options.SortOrder);
        case TransportSortType.TransportCode:
          return input.OrderBy(i => i.TransportCode, options.SortOrder);
        case TransportSortType.DateTime:
          return input.OrderBy(i => i.DateTime, options.SortOrder);
        case TransportSortType.TransportDateTime:
          return input.OrderBy(i => i.TransportDateTime, options.SortOrder);
        case TransportSortType.CarNumber:
          return input.OrderBy(i => i.CarNumber, options.SortOrder);
        case TransportSortType.DriverName:
          return input.OrderBy(i => i.DriverName, options.SortOrder);
        case TransportSortType.ShippingCompanyName:
          return input.OrderBy(i => i.ShippingCompanyName, options.SortOrder);
        case TransportSortType.PhoneNumber:
          return input.OrderBy(i => i.PhoneNumber, options.SortOrder);
        case TransportSortType.TransportType:
          return input.OrderBy(i => i.TransportType, options.SortOrder);
        case TransportSortType.Status:
          return input.OrderBy(i => i.Status, options.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<TransportResult> SearchTransportResult(
        IQueryable<TransportResult> query,
        string search,
        AdvanceSearchItem[] advanceSearchItem,
        DateTime? fromDateTime,
        DateTime? toDateTime
        )
    {
      if (!string.IsNullOrWhiteSpace(search))
      {
        query = from item in query
                where
                item.CarInformation.Contains(search) ||
                item.CarNumber.Contains(search) ||
                item.Description.Contains(search) ||
                item.DriverName.Contains(search) ||
                item.ShippingCompanyName.Contains(search) ||
                item.EmployeeFullName.Contains(search) ||
                item.TransportCode.Contains(search) ||
                item.PhoneNumber.Contains(search)
                select item;
      }

      if (fromDateTime != null)
        query = query.Where(i => i.TransportDateTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.TransportDateTime <= toDateTime);

      if (advanceSearchItem.Any())
      {
        query = query.Where(advanceSearchItem);
      }

      return query;
    }
    #endregion
  }
}
