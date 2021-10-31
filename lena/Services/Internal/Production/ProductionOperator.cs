using System;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.UserManagement.Exception;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Production.ProductionOperator;
using lena.Models.Production.ProductionOperatorMachineEmployee;
using lena.Models.Production.StuffProductionFaultType;
using lena.Services.Internals.Production.Exception;
using lena.Domains.Enums;
using lena.Models.StaticData;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Add
    public ProductionOperator AddProductionOperatorProcess(
       int? operationSequenceId,
       short operationId,
       int defaultTime,
       short? machineTypeOperatorTypeId,
       short? operatorTypeId,
       int productionOrderId,
       int wrongLimitCount
       //int? productionTerminalId
       )
    {
      #region CheckPermissionForAddWrongLimitCount
      if (wrongLimitCount != 0)
      {
        var checkPermissionResult = App.Internals.UserManagement.CheckPermission(
            actionName: StaticActionName.EditWrongLimitCount,
            actionParameters: null);
        if (checkPermissionResult.AccessType == AccessType.Denied)
        {
          wrongLimitCount = 0;
        }
      }
      #endregion
      var productionOperators = GetProductionOperators(
          selector: e => e,
          operationId: operationId,
          productionOrderId: productionOrderId);
      #region CheckOpenPackage
      var production = GetProductions(
          selector: e => e,
          productionOrderId: productionOrderId,
          type: ProductionType.Partial)
          .OrderByDescending(i => i.Id)
          .FirstOrDefault();
      if (production != null)
      {
        var serialBuffer = App.Internals.WarehouseManagement.GetSerialBuffers(
                  selector: e => e,
                  stuffSerialCode: production.StuffSerialCode,
                  stuffId: production.StuffSerialStuffId,
                  serialBufferType: SerialBufferType.Production)
                  .FirstOrDefault();
        if (serialBuffer != null)
        {
          throw new OpenPackageIsExistException(productionOrderId, production.Id);
        }
      }
      #endregion
      if (productionOperators.Any())
        throw new DuplicatedProductionOperatorException(operationId, productionOrderId);
      var productionOperator = AddProductionOperator(
                operationSequenceId: operationSequenceId,
                   operationId: operationId,
                   defaultTime: defaultTime,
                    machineTypeOperatorTypeId: machineTypeOperatorTypeId,
                    operatorTypeId: operatorTypeId,
                    wrongLimitCount: wrongLimitCount,
                   productionOrderId: productionOrderId);
      return productionOperator;
    }
    public ProductionOperator AddProductionOperator(
        int? operationSequenceId,
        short operationId,
        int defaultTime,
        short? machineTypeOperatorTypeId,
        short? operatorTypeId,
        int productionOrderId,
        int wrongLimitCount = 0
        //int? productionTerminalId
        )
    {
      var productionOperator = repository.Create<ProductionOperator>();
      productionOperator.OperationSequenceId = operationSequenceId;
      productionOperator.OperationId = operationId;
      productionOperator.DefaultTime = defaultTime;
      productionOperator.MachineTypeOperatorTypeId = machineTypeOperatorTypeId;
      productionOperator.OperatorTypeId = operatorTypeId;
      productionOperator.ProductionOrderId = productionOrderId;
      productionOperator.WrongLimitCount = wrongLimitCount;
      //productionOperator.ProductionTerminalId = productionTerminalId;
      repository.Add(productionOperator);
      return productionOperator;
    }
    internal object GetStuffProductionFaultTypes(Expression<Func<StuffProductionFaultType, StuffProductionFaultTypeResult>> selector, object stuffId, object productionFaultTypeId)
    {
      throw new NotImplementedException();
    }
    #endregion
    #region AddProcess
    public ProductionOperator AddProductionOperatorProcess(
        int? operationSequenceId,
        short operationId,
        int defaultTime,
        short? machineTypeOperatorTypeId,
        short? operatorTypeId,
        int productionOrderId,
        int wrongLimitCount,
        //  int? productionTerminalId,
        AddProductionOperatorMachineEmployeeInput[] addProductionOperatorEmployees)
    {
      if (wrongLimitCount != 0)
      {
        var checkPermissionResult = App.Internals.UserManagement.CheckPermission(
            actionName: StaticActionName.EditWrongLimitCount,
            actionParameters: null);
        if (checkPermissionResult.AccessType == AccessType.Denied)
        {
          wrongLimitCount = 0;
        }
      }
      var productionOperator = AddProductionOperator(
                    operationSequenceId: operationSequenceId,
                    operationId: operationId,
                    defaultTime: defaultTime,
                    machineTypeOperatorTypeId: machineTypeOperatorTypeId,
                    operatorTypeId: operatorTypeId,
                    productionOrderId: productionOrderId,
                    wrongLimitCount: wrongLimitCount
                   // productionTerminalId: productionTerminalId
                   );
      #region AddProdictionOperatorEmployees
      foreach (var addProductionOperatorEmployee in addProductionOperatorEmployees)
      {
        AddProductionOperatorMachineEmployee(
                        employeeId: addProductionOperatorEmployee.EmployeeId,
                        machineId: addProductionOperatorEmployee.MachineId,
                        productionOperatorId: productionOperator.Id,
                        productionTerminalId: addProductionOperatorEmployee.ProductionTerminalId,
                        description: addProductionOperatorEmployee.Description);
      }
      #endregion
      return productionOperator;
    }
    internal object AddStuffProductionFaultType(object stuffId, object productionFaultTypeId)
    {
      throw new NotImplementedException();
    }
    #endregion
    #region Edit
    public ProductionOperator EditProductionOperator(
        int id,
        byte[] rowVersion,
        TValue<int?> operationSequenceId = null,
        TValue<short> operationId = null,
        TValue<int> defaultTime = null,
        TValue<short?> machineTypeOperatorTypeId = null,
        TValue<short?> operatorTypeId = null,
        TValue<int> productionOrderId = null,
        TValue<int?> productionTerminalId = null,
        TValue<int> wrongLimitCount = null
        )
    {
      var productionOperator = GetProductionOperator(id: id);
      return EditProductionOperator(
                    productionOperator: productionOperator,
                    rowVersion: rowVersion,
                    operationSequenceId: operationSequenceId,
                    operationId: operationId,
                    defaultTime: defaultTime,
                    machineTypeOperatorTypeId: machineTypeOperatorTypeId,
                    operatorTypeId: operatorTypeId,
                    productionOrderId: productionOrderId,
                    wrongLimitCount: wrongLimitCount
                 //   productionTerminalId: productionTerminalId
                 );
    }
    public ProductionOperator EditProductionOperator(
        ProductionOperator productionOperator,
        byte[] rowVersion,
        TValue<int?> operationSequenceId = null,
        TValue<short> operationId = null,
        TValue<int> defaultTime = null,
        TValue<short?> machineTypeOperatorTypeId = null,
        TValue<short?> operatorTypeId = null,
        TValue<int> productionOrderId = null,
        TValue<int> wrongLimitCount = null
        //  TValue<int?> productionTerminalId = null
        )
    {
      if (operationSequenceId != null)
        productionOperator.OperationSequenceId = operationSequenceId;
      if (operationId != null)
        productionOperator.OperationId = operationId;
      if (defaultTime != null)
        productionOperator.DefaultTime = defaultTime;
      if (machineTypeOperatorTypeId != null)
        productionOperator.MachineTypeOperatorTypeId = machineTypeOperatorTypeId;
      if (operatorTypeId != null)
        productionOperator.OperatorTypeId = operatorTypeId;
      if (productionOrderId != null)
        productionOperator.ProductionOrderId = productionOrderId;
      if (wrongLimitCount != null)
        productionOperator.WrongLimitCount = wrongLimitCount;
      //if (productionTerminalId != null)
      //    productionOperator.ProductionTerminalId = productionTerminalId;
      repository.Update<ProductionOperator>(productionOperator, rowVersion);
      return productionOperator;
    }
    #endregion
    #region EditProcess
    public ProductionOperator EditProductionOperatorProcess(
        int id,
        byte[] rowVersion,
        TValue<int?> operationSequenceId = null,
        TValue<short> operationId = null,
        TValue<int> defaultTime = null,
        TValue<short?> machineTypeOperatorTypeId = null,
        TValue<short?> operatorTypeId = null,
        TValue<int> wrongLimitCount = null)
    {
      var productionOperator = GetProductionOperator(id: id);
      #region CheckPermissionForEditWrongLimitCount
      if (productionOperator.WrongLimitCount != wrongLimitCount)
      {
        var checkPermissionResult = App.Internals.UserManagement.CheckPermission(
                  actionName: StaticActionName.EditWrongLimitCount,
                  actionParameters: null);
        if (checkPermissionResult.AccessType == AccessType.Denied)
        {
          throw new AccessDeniedForEditLimitWrongCountException();
        }
      }
      #endregion
      #region CheckOpenPackage
      var production = GetProductions(
          selector: e => e,
          productionOrderId: productionOperator.ProductionOrderId,
          type: ProductionType.Partial)
          .OrderByDescending(i => i.Id)
          .FirstOrDefault();
      if (production != null)
      {
        var serialBuffer = App.Internals.WarehouseManagement.GetSerialBuffers(
                  selector: e => e,
                  stuffSerialCode: production.StuffSerialCode,
                  stuffId: production.StuffSerialStuffId,
                  serialBufferType: SerialBufferType.Production)
                  .FirstOrDefault();
        if (serialBuffer != null)
        {
          throw new OpenPackageIsExistException(productionOperator.Id, production.Id);
        }
      }
      #endregion
      return EditProductionOperator(
                  id: id,
                  rowVersion: rowVersion,
                  operationSequenceId: operationSequenceId,
                  operationId: operationId,
                  defaultTime: defaultTime,
                  machineTypeOperatorTypeId: machineTypeOperatorTypeId,
                  operatorTypeId: operatorTypeId,
                  wrongLimitCount: wrongLimitCount
              );
    }
    public ProductionOperator EditProductionOperatorProcess(
        int id,
        byte[] rowVersion,
        TValue<int?> operationSequenceId = null,
        TValue<short> operationId = null,
        TValue<int> defaultTime = null,
        TValue<short?> machineTypeOperatorTypeId = null,
        TValue<short?> operatorTypeId = null,
        TValue<int> productionOrderId = null,
        TValue<int?> productionTerminalId = null,
        TValue<int> wrongLimitCount = null,
        AddProductionOperatorMachineEmployeeInput[] addProductionOperatorMachineEmployees = null,
        EditProductionOperatorMachineEmployeeInput[] editProductionOperatorMachineEmployees = null,
        DeleteProductionOperatorMachineEmployeeInput[] deleteProductionOperatorMachineEmployees = null)
    {
      var productionOperator = GetProductionOperator(id: id);
      if (productionOperator.WrongLimitCount != wrongLimitCount)
      {
        var checkPermissionResult = App.Internals.UserManagement.CheckPermission(
                  actionName: StaticActionName.EditWrongLimitCount,
                  actionParameters: null);
        if (checkPermissionResult.AccessType == AccessType.Denied)
        {
          throw new AccessDeniedForEditLimitWrongCountException();
        }
      }
      return EditProductionOperatorProcess(
                    productionOperator: productionOperator,
                    rowVersion: rowVersion,
                    operationSequenceId: operationSequenceId,
                    operationId: operationId,
                    defaultTime: defaultTime,
                    machineTypeOperatorTypeId: machineTypeOperatorTypeId,
                    operatorTypeId: operatorTypeId,
                    productionOrderId: productionOrderId,
                    productionTerminalId: productionTerminalId,
                    wrongLimitCount: wrongLimitCount,
                    addProductionOperatorMachineEmployees: addProductionOperatorMachineEmployees,
                    editProductionOperatorMachineEmployees: editProductionOperatorMachineEmployees,
                    deleteProductionOperatorMachineEmployees: deleteProductionOperatorMachineEmployees);
    }
    public ProductionOperator EditProductionOperatorProcess(
        ProductionOperator productionOperator,
        byte[] rowVersion,
        TValue<int?> operationSequenceId = null,
        TValue<short> operationId = null,
        TValue<int> defaultTime = null,
        TValue<short?> machineTypeOperatorTypeId = null,
        TValue<short?> operatorTypeId = null,
        TValue<int> productionOrderId = null,
        TValue<int?> productionTerminalId = null,
        TValue<int> wrongLimitCount = null,
        AddProductionOperatorMachineEmployeeInput[] addProductionOperatorMachineEmployees = null,
        EditProductionOperatorMachineEmployeeInput[] editProductionOperatorMachineEmployees = null,
        DeleteProductionOperatorMachineEmployeeInput[] deleteProductionOperatorMachineEmployees = null)
    {
      EditProductionOperator(
                    productionOperator: productionOperator,
                    rowVersion: rowVersion,
                    operationSequenceId: operationSequenceId,
                    operationId: operationId,
                    defaultTime: defaultTime,
                    machineTypeOperatorTypeId: machineTypeOperatorTypeId,
                    operatorTypeId: operatorTypeId,
                    productionOrderId: productionOrderId,
                    wrongLimitCount: wrongLimitCount
                    //productionTerminalId: productionTerminalId
                    );
      #region AddProdictionOperatorEmployees
      if (addProductionOperatorMachineEmployees != null)
        foreach (var addProductionOperatorMachineEmployee in addProductionOperatorMachineEmployees)
          AddProductionOperatorMachineEmployee(
                        employeeId: addProductionOperatorMachineEmployee.EmployeeId,
                        machineId: addProductionOperatorMachineEmployee.MachineId,
                        productionOperatorId: productionOperator.Id,
                        productionTerminalId: addProductionOperatorMachineEmployee.ProductionTerminalId,
                        description: addProductionOperatorMachineEmployee.Description);
      #endregion
      #region EditProdictionOperatorEmployees
      if (editProductionOperatorMachineEmployees != null)
        foreach (var editProductionOperatorMachineEmployee in editProductionOperatorMachineEmployees)
          EditProductionOperatorMachineEmployee(
                        id: editProductionOperatorMachineEmployee.Id,
                        rowVersion: editProductionOperatorMachineEmployee.RowVersion,
                        employeeId: editProductionOperatorMachineEmployee.EmployeeId,
                        machineId: editProductionOperatorMachineEmployee.MachineId,
                        productionOperatorId: productionOperator.Id,
                        productionTerminalId: editProductionOperatorMachineEmployee.ProductionTerminalId,
                        description: editProductionOperatorMachineEmployee.Description);
      #endregion
      #region DeleteProdictionOperatorEmployees
      if (deleteProductionOperatorMachineEmployees != null)
        foreach (var deleteProductionOperatorMachineEmployee in deleteProductionOperatorMachineEmployees)
          DeleteProductionOperatorMachineEmployee(id: deleteProductionOperatorMachineEmployee.Id);
      #endregion
      return productionOperator;
    }
    #endregion
    #region Get
    public ProductionOperator GetProductionOperator(int id) => GetProductionOperator(selector: e => e, id: id);
    public TResult GetProductionOperator<TResult>(
       Expression<Func<ProductionOperator, TResult>> selector,
        int id)
    {
      var productionOperator = GetProductionOperators(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (productionOperator == null)
        throw new ProductionOperatorNotFoundException(id);
      return productionOperator;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionOperators<TResult>(
       Expression<Func<ProductionOperator, TResult>> selector,
       TValue<int> id = null,
       TValue<int?> operationSequenceId = null,
       TValue<int> operationId = null,
       TValue<long> defaultTime = null,
       TValue<int?> machineTypeOperatorTypeId = null,
       TValue<int> operatorTypeId = null,
       TValue<int> productionOrderId = null,
       TValue<int?> productionTerminalId = null
       )
    {
      var pOperator = repository.GetQuery<ProductionOperator>();
      if (id != null)
        pOperator = pOperator.Where(i => i.Id == id);
      if (operationSequenceId != null)
        pOperator = pOperator.Where(x => x.OperationSequenceId == operationSequenceId);
      if (operationId != null)
        pOperator = pOperator.Where(x => x.OperationId == operationId);
      if (defaultTime != null)
        pOperator = pOperator.Where(x => x.DefaultTime == defaultTime);
      if (machineTypeOperatorTypeId != null)
        pOperator = pOperator.Where(x => x.MachineTypeOperatorTypeId == machineTypeOperatorTypeId);
      if (operatorTypeId != null)
        pOperator = pOperator.Where(x => x.OperatorTypeId == operatorTypeId);
      if (productionOrderId != null)
        pOperator = pOperator.Where(x => x.ProductionOrderId == productionOrderId);
      if (productionTerminalId != null)
        pOperator = pOperator.Where(x => x.ProductionOperatorMachineEmployees.Any(i => i.ProductionTerminalId == productionTerminalId));
      return pOperator.Select(selector);
    }
    #endregion
    #region Delete
    public void DeleteProductionOperatorProcess(int id)
    {
      var productionOperator = GetProductionOperator(id: id);
      #region CheckOpenPackage
      var production = GetProductions(
          selector: e => e,
          productionOrderId: productionOperator.ProductionOrderId,
          type: ProductionType.Partial)
          .OrderByDescending(i => i.Id)
          .FirstOrDefault();
      if (production != null)
      {
        var serialBuffer = App.Internals.WarehouseManagement.GetSerialBuffers(
                  selector: e => e,
                  stuffSerialCode: production.StuffSerialCode,
                  stuffId: production.StuffSerialStuffId,
                  serialBufferType: SerialBufferType.Production)
                  .FirstOrDefault();
        if (serialBuffer != null)
        {
          throw new OpenPackageIsExistException(productionOperator.Id, production.Id);
        }
      }
      DeleteProductionOperator(productionOperator);
      #endregion
    }
    public void DeleteProductionOperator(int id)
    {
      var productionOperator = GetProductionOperator(id: id);
      //todo 2  ProductionOperator deleteDetailProcess
      repository.Delete(productionOperator);
    }
    public void DeleteProductionOperator(ProductionOperator productionOperator)
    {
      //todo 2  ProductionOperator deleteDetailProcess
      repository.Delete(productionOperator);
    }
    #endregion
    #region ToResult
    public Expression<Func<ProductionOperator, ProductionOperatorResult>> ToProductionOperatorResult =
        entity => new ProductionOperatorResult()
        {
          Id = entity.Id,
          Index = entity.OperationSequence.Index,
          OperationSequenceId = entity.OperationSequenceId,
          OperationId = entity.OperationId,
          OperationTitle = entity.Operation.Title,
          DefaultTime = entity.DefaultTime,
          ProductionOrderId = entity.ProductionOrderId,
          ProductionOrderCode = entity.ProductionOrder.Code,
          MachineTypeOperatorTypeId = entity.MachineTypeOperatorTypeId,
          MachineTypeOperatorTypeTitle = entity.MachineTypeOperatorType.Title,
          OperatorTypeId = entity.OperatorTypeId,
          WrongLimitCount = entity.WrongLimitCount,
          OperatorTypeName = entity.OperatorType.Name,
          ProductionOperatorEmployeeBans = entity.ProductionOperatorEmployeeBans.AsQueryable()
            .Where(m => m.IsBan == true)
            .Select(App.Internals.Production.ToProductionOperatorEmployeeBanResult),
          OperationSequenceMachineTypeParameters = entity.OperationSequence.OperationSequenceMachineTypeParameters.AsQueryable()
                .Select(App.Internals.Planning.ToOperationSequenceMachineTypeParameterResult),
          ProductionOperatorMachineEmployees = entity.ProductionOperatorMachineEmployees.AsQueryable()
                .Select(App.Internals.Production.ToProductionOperatorMachineEmployeeResult),
          RowVersion = entity.RowVersion
        };
    #endregion
  }
}