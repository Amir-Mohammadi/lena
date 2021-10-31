using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Guard.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Guard.InboundCargo;
using lena.Models.Guard.InboundCargoCooperator;
using lena.Models.Guard;
using lena.Models.Guard.Transport;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Guard
{
  public partial class Guard
  {
    #region Add
    public InboundCargo AddInboundCargo(
        InboundCargo inboundCargo,
        TransactionBatch transactionBatch,
        DateTime transportDateTime,
        string shippingCompanyName,
        string driverName,
        string phoneNumber,
        string carNumber,
        string carInformation,
        string description,
        short boxCount)
    {
      inboundCargo = inboundCargo ?? repository.Create<InboundCargo>();
      inboundCargo.BoxCount = boxCount;
      AddTransport(
                    transport: inboundCargo,
                    transactionBatch: transactionBatch,
                    description: description,
                    transportDateTime: transportDateTime,
                    shippingCompanyName: shippingCompanyName,
                    driverName: driverName,
                    phoneNumber: phoneNumber,
                    carNumber: carNumber,
                    carInformation: carInformation,
                    entranceTransportId: null,
                    transportType: TransportType.Inbound,
                    status: TransportStatus.NotAction);
      return inboundCargo;
    }
    #endregion
    #region Edit
    public InboundCargo EditInboundCargo(
        int inboundCargoId,
        byte[] rowVersion,
        TValue<DateTime> transportDateTime = null,
        TValue<string> shippingCompanyName = null,
        TValue<string> driverName = null,
        TValue<string> phoneNumber = null,
        TValue<string> carNumber = null,
        TValue<string> carInformation = null,
        TValue<string> description = null,
        TValue<int?> outputTransportId = null,
        TValue<short> boxCount = null,
        TValue<TransportStatus> status = null)
    {
      var inboundCargo = GetInboundCargo(inboundCargoId);
      EditInboundCargo(
                    inboundCargo: inboundCargo,
                    rowVersion: rowVersion,
                    transportDateTime: transportDateTime,
                    shippingCompanyName: shippingCompanyName,
                    driverName: driverName,
                    phoneNumber: phoneNumber,
                    carNumber: carNumber,
                    carInformation: carInformation,
                    description: description,
                    outputTransportId: outputTransportId,
                    boxCount: boxCount,
                    status: status);
      return inboundCargo;
    }
    public InboundCargo EditInboundCargo(
        InboundCargo inboundCargo,
        byte[] rowVersion,
        TValue<DateTime> transportDateTime = null,
        TValue<string> shippingCompanyName = null,
        TValue<string> driverName = null,
        TValue<string> phoneNumber = null,
        TValue<string> carNumber = null,
        TValue<string> carInformation = null,
        TValue<string> description = null,
        TValue<int?> outputTransportId = null,
        TValue<short> boxCount = null,
        TValue<TransportStatus> status = null)
    {
      if (boxCount != null)
        inboundCargo.BoxCount = boxCount;
      if (status != null)
        inboundCargo.Status = status;
      EditTransport(
                    transport: inboundCargo,
                    rowVersion: rowVersion,
                    description: description,
                    transportDateTime: transportDateTime,
                    shippingCompanyName: shippingCompanyName,
                    driverName: driverName,
                    phoneNumber: phoneNumber,
                    carNumber: carNumber,
                    carInformation: carInformation,
                    outputTransportId: outputTransportId);
      return inboundCargo;
    }
    #endregion
    #region Delete
    public void DeleteInboundCargo(int id)
    {
      var inboundCargo = GetInboundCargo(id);
      repository.Delete(inboundCargo);
    }
    #endregion
    #region Get
    public TResult GetInboundCargo<TResult>(
        Expression<Func<InboundCargo, TResult>> selector,
        int id)
    {
      var inboundCargo = GetInboundCargos(
                    selector: selector,
                    id: id)

                .FirstOrDefault();
      if (inboundCargo == null)
        throw new InboundCargoNotFoundException(id);
      return inboundCargo;
    }
    public InboundCargo GetInboundCargo(int id) => GetInboundCargo(selector: e => e, id: id);
    #endregion
    #region Gets
    public IQueryable<TResult> GetInboundCargos<TResult>(
        Expression<Func<InboundCargo, TResult>> selector,
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
        TValue<int?> outputTransportId = null,
        TValue<short> boxCount = null)
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
      var query = baseQuery.OfType<InboundCargo>();
      if (boxCount != null)
        query = query.Where(i => i.BoxCount == boxCount);
      return query.Select(selector);
    }
    #endregion
    #region GetDrivers
    public IQueryable<TResult> GetDrivers<TResult>(
        Expression<Func<Transport, TResult>> selector,
        TValue<string> description = null,
          TValue<string> driverName = null,
          TValue<string> phoneNumber = null,
          TValue<string> carNumber = null,
          TValue<string> carInformation = null,
          TValue<string> shippingCompanyName = null,
          TValue<DateTime> transportDateTime = null
      )
    {
      var query = repository.GetQuery<Transport>();
      if (driverName != null)
        query = query.Where(i => i.DriverName == driverName);
      if (phoneNumber != null)
        query = query.Where(i => i.PhoneNumber == phoneNumber);
      if (carNumber != null)
        query = query.Where(i => i.CarNumber == carNumber);
      if (carInformation != null
        ) query = query.Where(i => i.CarInformation == carInformation);
      if (shippingCompanyName != null)
        query = query.Where(i => i.ShippingCompanyName == shippingCompanyName);
      if (transportDateTime != null)
        query = query.Where(i => i.TransportDateTime == transportDateTime);
      //var q =  query.Distinct().Select(selector);
      var result = query
       .GroupBy(u => new { driverName = u.DriverName })
        .Select(u => u.OrderByDescending(i => i.DriverName).FirstOrDefault());
      return result.Select(selector);
    }
    #endregion
    #region ToResult
    public Expression<Func<InboundCargo, InboundCargoResult>> ToInboundCargoResult =
        inboundCargo => new InboundCargoResult
        {
          Id = inboundCargo.Id,
          DateTime = inboundCargo.DateTime,
          BoxCount = inboundCargo.BoxCount,
          CarInformation = inboundCargo.CarInformation,
          CarNumber = inboundCargo.CarNumber,
          Description = inboundCargo.Description,
          DriverName = inboundCargo.DriverName,
          PhoneNumber = inboundCargo.PhoneNumber,
          TransportDateTime = inboundCargo.TransportDateTime,
          ShippingCompanyName = inboundCargo.ShippingCompanyName,
          UserId = inboundCargo.UserId,
          EmployeeFullName = inboundCargo.User.Employee.FirstName + " " + inboundCargo.User.Employee.LastName,
          RowVersion = inboundCargo.RowVersion
        };
    #endregion
    #region ToDriverResult
    public Expression<Func<Transport, DriverComboResult>> ToDriverComboResult =
        driver => new DriverComboResult
        {
          CarNumber = driver.CarNumber,
          PhoneNumber = driver.PhoneNumber,
          ShippingCompanyName = driver.ShippingCompanyName,
          DriverName = driver.DriverName,
          CarInformation = driver.CarInformation,
          Description = driver.Description
        };
    #endregion
    #region ToFullResult
    public Expression<Func<InboundCargo, InboundCargoFullResult>> ToInboundCargoFullResult =
        inboundCargo => new InboundCargoFullResult
        {
          Id = inboundCargo.Id,
          DateTime = inboundCargo.DateTime,
          BoxCount = inboundCargo.BoxCount,
          CarInformation = inboundCargo.CarInformation,
          CarNumber = inboundCargo.CarNumber,
          PhoneNumber = inboundCargo.PhoneNumber,
          Description = inboundCargo.Description,
          DriverName = inboundCargo.DriverName,
          TransportDateTime = inboundCargo.TransportDateTime,
          ShippingCompanyName = inboundCargo.ShippingCompanyName,
          InboundCargoCooperators = inboundCargo.InboundCargoCooperators.AsQueryable().Select(App.Internals.Guard.ToInboundCargoCooperatorResult),
          RowVersion = inboundCargo.RowVersion
        };
    #endregion
    #region AddProcess
    public InboundCargo AddInboundCargoProcess(
        DateTime transportDateTime,
        string shippingCompanyName,
        string driverName,
        string phoneNumber,
        string carNumber,
        string carInformation,
        string description,
        short boxCount,
        AddInboundCargoCooperatorInput[] addInboundCargoCooperators
        )
    {
      #region Add InboundCargo
      var inboundCargo = AddInboundCargo(
              inboundCargo: null,
              transactionBatch: null,
              transportDateTime: transportDateTime,
              shippingCompanyName: shippingCompanyName,
              driverName: driverName,
              phoneNumber: phoneNumber,
              carNumber: carNumber,
              carInformation: carInformation,
              description: description,
              boxCount: boxCount);
      #endregion
      #region Add InboundCargoCooperators
      foreach (var addInboundCargoCooperator in addInboundCargoCooperators)
      {
        AddInboundCargoCooperator(
                      cooperatorId: addInboundCargoCooperator.CooperatorId,
                      inboundCargoId: inboundCargo.Id);
      }
      #endregion
      #region AddStoreReceiptTask
      AddStoreReceiptTask(inboundCargo: inboundCargo);
      #endregion
      return inboundCargo;
    }
    #endregion
    #region EditProcess
    public InboundCargo EditInboundCargoProcess(
        int id,
        byte[] rowVersion,
        string description,
        DateTime transportDateTime,
        string shippingCompanyName,
        string driverName,
        string phoneNumber,
        string carNumber,
        string carInformation,
        int? outputTransportId,
        short boxCount,
        AddInboundCargoCooperatorInput[] addInboundCargoCooperators,
        DeleteInboundCargoCooperatorInput[] deleteInboundCargoCooperators
    )
    {
      #region Edit InboundCargo
      var inboundCargo = EditInboundCargo(
              inboundCargoId: id,
              rowVersion: rowVersion,
              transportDateTime: transportDateTime,
              shippingCompanyName: shippingCompanyName,
              driverName: driverName,
              phoneNumber: phoneNumber,
              carNumber: carNumber,
              carInformation: carInformation,
              description: description,
              outputTransportId: outputTransportId,
              boxCount: boxCount);
      #endregion
      #region Add InboundCargoCooperators
      foreach (var addInboundCargoCooperator in addInboundCargoCooperators)
      {
        AddInboundCargoCooperator(
                  cooperatorId: addInboundCargoCooperator.CooperatorId,
                  inboundCargoId: inboundCargo.Id);
      }
      #endregion
      #region Delete InboundCargoCooperators
      foreach (var deleteInboundCargoCooperator in deleteInboundCargoCooperators)
      {
        DeleteInboundCargoCooperator(
                      cooperatorId: deleteInboundCargoCooperator.CooperatorId,
                      inboundCargoId: inboundCargo.Id);
      }
      #endregion
      return inboundCargo;
    }
    #endregion
    #region ComplateEditProcess
    public InboundCargo ComplateInboundCargoProcess(
        int id,
        byte[] rowVersion)
    {
      #region Edit InboundCargo
      var inboundCargo = EditInboundCargo(
              inboundCargoId: id,
              rowVersion: rowVersion,
              status: TransportStatus.Complated);
      #endregion
      return inboundCargo;
    }
    #endregion
    #region IncomplateInboundCargoProcess
    public InboundCargo IncomplateInboundCargoProcess(
        int id,
        byte[] rowVersion)
    {
      #region Edit InboundCargo
      var inboundCargo = EditInboundCargo(
              inboundCargoId: id,
              rowVersion: rowVersion,
              status: TransportStatus.Incomplated);
      #endregion
      return inboundCargo;
    }
    #endregion
    #region AddStoreReceiptTask
    public void AddStoreReceiptTask(InboundCargo inboundCargo)
    {
      #region GetOrAddCommonProjectGroup
      var projectGroup = App.Internals.ScrumManagement.GetOrAddCommonScrumProjectGroup(
              departmentId: (int)Departments.Warehouse);
      #endregion
      #region GetOrAddCommonProject
      var project = App.Internals.ProjectManagement.GetOrAddCommonProject(
              departmentId: (int)Departments.Warehouse);
      #endregion
      #region GetOrAddCommonProjectStep
      var projectStep = App.Internals.ProjectManagement.GetOrAddCommonProjectStep(
              departmentId: (int)Departments.Warehouse);
      #endregion
      #region Add ProjectWork
      var projectWork = App.Internals.ProjectManagement.AddProjectWork(
              projectWork: null,
              name: $"پروسه ورود کالا {inboundCargo.Code}",
              description: "",
              color: "",
              departmentId: (int)Departments.Warehouse,
              estimatedTime: 18000,
              isCommit: false,
              projectStepId: projectStep.Id,
              baseEntityId: inboundCargo.Id
          );
      #endregion
      #region Add ProductionMaterialRequestTask
      //check projectWork not null
      if (projectWork != null)
      {
        #region Get DescriptionForTask
        var shippingCompanyName = inboundCargo.ShippingCompanyName;
        var boxNo = inboundCargo.BoxCount;
        #endregion
        App.Internals.ProjectManagement.AddProjectWorkItem(
                projectWorkItem: null,
                name: $"ثبت رسید ورود کالا {inboundCargo.Code}",
                description: $"شرکت حمل و نقل:{shippingCompanyName}, تعداد بسته:{boxNo}",
                color: "",
                departmentId: (int)Departments.Warehouse,
                estimatedTime: 10800,
                isCommit: false,
                scrumTaskTypeId: (int)ScrumTaskTypes.StoreReceipt,
                userId: null,
                spentTime: 0,
                remainedTime: 0,
                scrumTaskStep: ScrumTaskStep.ToDo,
                projectWorkId: projectWork.Id,
                baseEntityId: inboundCargo.Id);
      }
      #endregion
    }
    public void AddStoreReceiptTask(int inboundCargoId)
    {
      var inboundCargo = GetInboundCargo(id: inboundCargoId); ; AddStoreReceiptTask(inboundCargo: inboundCargo);
    }
    #endregion
  }
}