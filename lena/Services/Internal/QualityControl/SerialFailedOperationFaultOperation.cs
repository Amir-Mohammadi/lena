using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Models.QualityControl.SerialFailedOperationFaultOperationEmployee;
using lena.Models.QualityControl.SerialFailedOperationFualtOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region Add
    public SerialFailedOperationFaultOperation AddSerialFailedOperationFaultOperation(
        int serialFailedOperationId,
        short operationId)
    {

      var serialFailedOperationFaultOperation = repository.Create<SerialFailedOperationFaultOperation>();
      serialFailedOperationFaultOperation.OperationId = operationId;
      serialFailedOperationFaultOperation.SerialFailedOperationId = serialFailedOperationId;
      repository.Add(serialFailedOperationFaultOperation);
      return serialFailedOperationFaultOperation;
    }
    #endregion

    #region SaveSerialFailedOperationFaultOperation
    public void SaveSerialFailedOperationFaultOperationProcess(
        int serialFailedOperationId,
        string description,
        AddSerialFailedOperationFaultOperationInput[] addSerialFailedOperationFaultOperationInputs,
        int[] deletedSerailFailedOperationFaultIds,
        DeleteSerialFailedOperationFaultOperationEmployeeInput[] deleteSerialFailedOperationFaultOperationEmployeeInputs,
        AddSerialFailedOperationFaultOperationEmployeeInput[] addSerialFailedOperationFaultOperationEmployeeInputs)
    {

      #region GetSerialFailedOperationAndRelatedItemAndUpdateSerialFailedOperation
      var serialFailedOperation = GetSerialFailedOperation(
          e => e,
          id: serialFailedOperationId);



      if (serialFailedOperation.IsRepaired)
      {
        throw new TheSerialFailedOperationRepairedException(id: serialFailedOperationId);
      }

      EditSerialFailedOperation(
                serialFailedOperation: serialFailedOperation,
                rowVersion: serialFailedOperation.RowVersion,
                reviewerUserId: App.Providers.Security.CurrentLoginData.UserId,
                description: description,
                reviewedDateTime: DateTime.UtcNow);

      var deletedBanIds = new List<int>();

      var serialFailedOperations = GetSerialFailedOperations(
                selector: e => e,
                id: serialFailedOperationId);

      var serialFailedOperationFaultOperations = serialFailedOperation.SerialFailedOperationFaultOperations.ToList();

      var serialFailedOperationFaultOperationEmployees = GetSerialFailedOperatonFaultOperationEmployees(
                selector: e => e,
                serialFailedOperationFaultOperationIds: serialFailedOperationFaultOperations.Select(m => m.Id).ToArray())


                .ToList();


      #endregion

      #region AddOperationAndRelatedEmployee
      foreach (var item in addSerialFailedOperationFaultOperationInputs)
      {

        var serialFailedOperationFaultOperation = AddSerialFailedOperationFaultOperation(
                 serialFailedOperationId: serialFailedOperationId,
                 operationId: item.OperationId);

        foreach (var subItem in item.AddSerialFailedOperationFaultOperationEmployeeInputs)
        {
          AddSerialFailedOperationFaultOperationEmployee(
                    productionOperationEmployeeId: subItem.ProductionOperationEmployeeId,
                    serialFailedOperationFaultOperationId: serialFailedOperationFaultOperation.Id);
        }
      }
      #endregion

      #region AddEmployeeForExistOperation
      foreach (var item in addSerialFailedOperationFaultOperationEmployeeInputs)
      {
        AddSerialFailedOperationFaultOperationEmployee(
                    productionOperationEmployeeId: item.ProductionOperationEmployeeId,
                    serialFailedOperationFaultOperationId: item.SerialFailedOperationFaultOperationId.Value);
      }

      #endregion

      #region DeleteEmployeeForExistOperation
      foreach (var item in deleteSerialFailedOperationFaultOperationEmployeeInputs)
      {

        var serialFailedOperationFaultOperationEmployee = serialFailedOperationFaultOperationEmployees.FirstOrDefault(
                  m => m.SerialFailedOperationFaultOperationId == item.SerialFailedOperationFaultOperationId &&
                  m.ProductionOperationEmployeeId == item.ProductionOperationEmployeeId);

        if (serialFailedOperationFaultOperationEmployee != null)
        {
          if (serialFailedOperationFaultOperationEmployee.ProductionOperatorEmployeeBanId != null)
          {
            deletedBanIds.Add(serialFailedOperationFaultOperationEmployee.ProductionOperatorEmployeeBanId.Value);

            var employeeFaults = GetSerialFailedOperatonFaultOperationEmployees(
                      selector: e => e,
                      productionOperatorEmployeeBanId: serialFailedOperationFaultOperationEmployee.ProductionOperatorEmployeeBanId)


                      .ToList();


            foreach (var employeeFault in employeeFaults)
            {

              EditSerialFailedOperationFaultOperationEmployee(
                        serialFailedOperationFaultOperationEmployee: employeeFault,
                        rowVersion: employeeFault.RowVersion,
                        productionOperatorEmployeeBanId: new TValue<int?>(null));
            }

            DeleteSerialFailedOperationFaultOperationEmployee(
                  serialFailedOperationFaultOperationEmployee: serialFailedOperationFaultOperationEmployee);

          }
          else
          {
            DeleteSerialFailedOperationFaultOperationEmployee(
                      serialFailedOperationFaultOperationId: item.SerialFailedOperationFaultOperationId,
                      productionOperationEmployeeId: item.ProductionOperationEmployeeId);
          }
        }
      }

      #endregion

      #region DeleteOperationAndRelatedEmployee
      foreach (var item in deletedSerailFailedOperationFaultIds)
      {
        var faultOperation = serialFailedOperationFaultOperations.FirstOrDefault(m => m.Id == item);
        if (faultOperation != null)
        {
          var faultEmployee = serialFailedOperationFaultOperationEmployees.FirstOrDefault(m => m.SerialFailedOperationFaultOperationId == item && m.ProductionOperatorEmployeeBanId != null);

          if (faultEmployee != null)
          {
            deletedBanIds.Add(faultEmployee.ProductionOperatorEmployeeBanId.Value);

            var faultEmployeesFullResults = GetSerialFailedOperatonFaultOperationEmployees(
                       selector: e => e,
                       productionOperatorEmployeeBanId: faultEmployee.ProductionOperatorEmployeeBanId.Value);

            foreach (var faultEmployeesFullResult in faultEmployeesFullResults)
            {

              EditSerialFailedOperationFaultOperationEmployee(
                        serialFailedOperationFaultOperationEmployee: faultEmployeesFullResult,
                        rowVersion: faultEmployeesFullResult.RowVersion,
                        productionOperatorEmployeeBanId: new TValue<int?>(null));
            }

          }
          DeleteSerialFailedOperationFaultOperation(serialFailedOperationFaultOperation: faultOperation);
        }

      }
      #endregion

      #region DeleteEmployeeBansIfExist
      if (deletedBanIds.Any())
      {
        var operatorBans = App.Internals.Production.GetProductionOperatorEmployeeBans(
                  selector: e => e,
                  ids: deletedBanIds.ToArray());

        foreach (var item in deletedBanIds)
        {
          var productionOperatorEmployeeBan = operatorBans.FirstOrDefault(m => m.Id == item);
          if (productionOperatorEmployeeBan != null)
          {
            App.Internals.Production.DeleteProductionOperatorEmployeeBan(productionOperatorEmployeeBan: productionOperatorEmployeeBan);
          }
        }
      }
      #endregion

      #region CheckAndApplyEmployeeBan
      if (addSerialFailedOperationFaultOperationInputs.Any() ||
      deletedSerailFailedOperationFaultIds.Any() ||
      deleteSerialFailedOperationFaultOperationEmployeeInputs.Any() ||
      addSerialFailedOperationFaultOperationEmployeeInputs.Any())
      {
        var productionOrderId = serialFailedOperation.ProductionOperation.Production.ProductionOrderId;

        var productionOperators = App.Internals.Production.GetProductionOperators(
                 selector: e => e,
                 productionOrderId: productionOrderId);

        var operationIds = addSerialFailedOperationFaultOperationInputs.Select(m => m.OperationId).Distinct().ToArray();

        var faultOperationEmployees = GetSerialFailedOperatonFaultOperationEmployees(
                  selector: e => new
                  {
                    OperationId = e.SerialFailedOperationFaultOperation.OperationId,
                    ProductionOperationEmployeeId = e.ProductionOperationEmployeeId,
                    EmployeeId = e.ProductionOperationEmployee.EmployeeId,
                    SerialFailedOperationFaultOperationId = e.SerialFailedOperationFaultOperationId,
                    RowVersion = e.RowVersion,
                  },
                  productionOrderId: productionOrderId,
                  productionOperatorEmployeeBanId: new TValue<int?>(null));


        var groupFaultOprerationEmployees = from faultOperationEmployee in faultOperationEmployees
                                            group faultOperationEmployee by new { faultOperationEmployee.OperationId, faultOperationEmployee.ProductionOperationEmployeeId } into g
                                            select new
                                            {
                                              OperationId = g.Key.OperationId,
                                              ProductionOperationEmployeeId = g.Key.ProductionOperationEmployeeId,
                                              FaultCount = g.Count(),

                                            };


        var joinOperationFaulEmployees = from productionOperator in productionOperators
                                         join groupFaultOprerationEmployee in groupFaultOprerationEmployees on productionOperator.OperationId equals groupFaultOprerationEmployee.OperationId
                                         select new
                                         {
                                           ProductionOperatorId = productionOperator.Id,
                                           ProductionOperationEmployeeId = groupFaultOprerationEmployee.ProductionOperationEmployeeId,
                                           OperationId = groupFaultOprerationEmployee.OperationId,
                                           FaultCount = groupFaultOprerationEmployee.FaultCount,
                                           WrongLimitCount = productionOperator.WrongLimitCount
                                         };


        var operationFaulEmployeeResult = joinOperationFaulEmployees.Where(m => m.FaultCount >= m.WrongLimitCount).ToList();

        if (operationFaulEmployeeResult.Any())
        {
          var faultEmployeeResult = faultOperationEmployees.ToList();

          foreach (var item in operationFaulEmployeeResult)
          {
            var employeeId = faultEmployeeResult.FirstOrDefault(
                      m => m.OperationId == item.OperationId &&
                      m.ProductionOperationEmployeeId == item.ProductionOperationEmployeeId).EmployeeId;


            var productionOperatorEmployeeBanId = App.Internals.Production.AddProductionOperatorEmployeeBan(
                          productionOperatorId: item.ProductionOperatorId,
                          employeeId: employeeId);

            var editFaultEmployees = faultEmployeeResult.Where(
                      m => m.OperationId == item.OperationId &&
                      m.ProductionOperationEmployeeId == item.ProductionOperationEmployeeId);

            var productionOperationEmployeeIds = editFaultEmployees.Select(m => m.ProductionOperationEmployeeId).ToArray();

            var serialFailedOperationFaultOperationIds = editFaultEmployees.Select(m => m.SerialFailedOperationFaultOperationId).ToArray();

            var fOperationEmployees = GetSerialFailedOperatonFaultOperationEmployees(
                      selector: e => e,
                      productionOperationEmployeeIds: productionOperationEmployeeIds,
                      serialFailedOperationFaultOperationIds: serialFailedOperationFaultOperationIds)


                      .ToList();

            foreach (var faultItem in editFaultEmployees)
            {

              var fOperationEmployee = fOperationEmployees.FirstOrDefault(
                        m => m.SerialFailedOperationFaultOperationId == faultItem.SerialFailedOperationFaultOperationId &&
                        m.ProductionOperationEmployeeId == faultItem.ProductionOperationEmployeeId);

              if (fOperationEmployee != null)
              {
                EditSerialFailedOperationFaultOperationEmployee(
                          serialFailedOperationFaultOperationEmployee: fOperationEmployee,
                          rowVersion: fOperationEmployee.RowVersion,
                          productionOperatorEmployeeBanId: productionOperatorEmployeeBanId.Id);
              }

            }
          }

        }
      }
      #endregion
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetSerialFailedOperatonFaultOperations<TResult>(
        Expression<Func<SerialFailedOperationFaultOperation, TResult>> selector,
        TValue<int> id = null)
    {

      var query = repository.GetQuery<SerialFailedOperationFaultOperation>();
      if (id != null)
        query = query.Where(x => x.Id == id);

      return query.Select(selector);
    }
    #endregion

    #region Delete
    public void DeleteSerialFailedOperationFaultOperation(SerialFailedOperationFaultOperation serialFailedOperationFaultOperation)
    {

      repository.Delete(serialFailedOperationFaultOperation);
    }
    #endregion
  }
}
