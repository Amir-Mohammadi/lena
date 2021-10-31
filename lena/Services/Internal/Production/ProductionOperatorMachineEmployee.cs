using System;
////using lena.Services.Core.Foundation.Action;
using lena.Models.Common;
using lena.Domains;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Production.Exception;
using lena.Models.Production.ProductionOperatorMachineEmployee;
using System.Collections.Generic;
//using Microsoft.Ajax.Utilities;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Add
    public ProductionOperatorMachineEmployee AddProductionOperatorMachineEmployee(
        int? employeeId,
        short? machineId,
        int productionOperatorId,
        int? productionTerminalId,
        string description)
    {
      if (productionTerminalId != null)
        CheckTerminalArrangement(
                  productionTerminalId: productionTerminalId.Value,
                  productionOperatorId: productionOperatorId);
      var productionOperatorMachineEmployee = repository.Create<ProductionOperatorMachineEmployee>();
      productionOperatorMachineEmployee.EmployeeId = employeeId;
      productionOperatorMachineEmployee.MachineId = machineId;
      productionOperatorMachineEmployee.ProductionOperatorId = productionOperatorId;
      productionOperatorMachineEmployee.Description = description;
      productionOperatorMachineEmployee.ProductionTerminalId = productionTerminalId;
      repository.Add(productionOperatorMachineEmployee);
      return productionOperatorMachineEmployee;
    }
    public List<ProductionOperatorMachineEmployee> AddProductionOperatorMachineEmployees(
        AddProductionOperatorMachineEmployeeInput[] productionOperatorMachineEmployeeItems)
    {
      var productionOperatorMachineEmployees = new List<ProductionOperatorMachineEmployee>();
      if (productionOperatorMachineEmployeeItems != null)
        foreach (var item in productionOperatorMachineEmployeeItems)
        {
          if (item.ProductionTerminalId != null)
            CheckTerminalArrangement(
                      productionTerminalId: item.ProductionTerminalId.Value,
                      productionOperatorId: item.ProductionOperatorId);
          var employeeExsit = GetProductionOperatorMachineEmployees(selector: e => e.Id,
                    employeeId: item.EmployeeId,
                    productionTerminalId: item.ProductionTerminalId,
                     productionOperatorId: item.ProductionOperatorId,
                     machineId: item.MachineId
                    )
                .Any();
          if (employeeExsit)
            throw new DuplicateProductionOperatorMachineEmployeeException(item.EmployeeId, item.ProductionOperatorId);
          var productionOperatorMachineEmployee = repository.Create<ProductionOperatorMachineEmployee>();
          productionOperatorMachineEmployee.EmployeeId = item.EmployeeId;
          productionOperatorMachineEmployee.MachineId = item.MachineId;
          productionOperatorMachineEmployee.ProductionOperatorId = item.ProductionOperatorId;
          productionOperatorMachineEmployee.Description = item.Description;
          productionOperatorMachineEmployee.ProductionTerminalId = item.ProductionTerminalId;
          repository.Add(productionOperatorMachineEmployee);
        }
      return productionOperatorMachineEmployees;
    }
    #endregion
    #region Edit
    public ProductionOperatorMachineEmployee EditProductionOperatorMachineEmployee(
        int id,
        byte[] rowVersion,
        TValue<int?> employeeId = null,
        TValue<short?> machineId = null,
        TValue<int> productionOperatorId = null,
        TValue<int?> productionTerminalId = null,
        TValue<string> description = null)
    {
      var ProductionOperatorMachineEmployee = GetProductionOperatorMachineEmployee(id: id);
      if (productionTerminalId?.Value != null)
        CheckTerminalArrangement(
                  productionTerminalId: productionTerminalId.Value.Value,
                  productionOperatorId: productionOperatorId);
      return EditProductionOperatorMachineEmployee(
                    productionOperatorMachineEmployee: ProductionOperatorMachineEmployee,
                    rowVersion: rowVersion,
                    employeeId: employeeId,
                    machineId: machineId,
                    productionOperatorId: productionOperatorId,
                    productionTerminalId: productionTerminalId,
                    description: description);
    }
    public ProductionOperatorMachineEmployee EditProductionOperatorMachineEmployee(
        ProductionOperatorMachineEmployee productionOperatorMachineEmployee,
        byte[] rowVersion,
        TValue<int?> employeeId = null,
        TValue<short?> machineId = null,
        TValue<int> productionOperatorId = null,
        TValue<int?> productionTerminalId = null,
        TValue<string> description = null)
    {
      if (employeeId != null)
        productionOperatorMachineEmployee.EmployeeId = (employeeId == 0 ? null : employeeId);
      if (machineId != null)
        productionOperatorMachineEmployee.MachineId = machineId;
      if (productionOperatorId != null)
        productionOperatorMachineEmployee.ProductionOperatorId = productionOperatorId;
      if (productionTerminalId != null)
        productionOperatorMachineEmployee.ProductionTerminalId = productionTerminalId;
      if (description != null)
        productionOperatorMachineEmployee.Description = description;
      repository.Update<ProductionOperatorMachineEmployee>(productionOperatorMachineEmployee, rowVersion);
      return productionOperatorMachineEmployee;
    }
    #endregion
    #region Get
    public ProductionOperatorMachineEmployee GetProductionOperatorMachineEmployee(int id)
    {
      var ProductionOperatorMachineEmployee = GetProductionOperatorMachineEmployees(
                    selector: e => e,
                    id: id)


                .FirstOrDefault();
      if (ProductionOperatorMachineEmployee == null)
        throw new ProductionOperatorMachineEmployeeNotFoundException(id);
      return ProductionOperatorMachineEmployee;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionOperatorMachineEmployees<TResult>(
       Expression<Func<ProductionOperatorMachineEmployee, TResult>> selector,
       TValue<int> id = null,
       TValue<int?> employeeId = null,
       TValue<int?> machineId = null,
       TValue<int?> productionTerminalId = null,
       TValue<int> productionOperatorId = null,
       TValue<int> productionOrderId = null
       )
    {
      var pOperator = repository.GetQuery<ProductionOperatorMachineEmployee>();
      if (id != null)
        pOperator = pOperator.Where(i => i.Id == id);
      if (employeeId != null)
        pOperator = pOperator.Where(x => x.EmployeeId == employeeId);
      if (machineId != null)
        pOperator = pOperator.Where(x => x.MachineId == machineId);
      if (productionOperatorId != null)
        pOperator = pOperator.Where(x => x.ProductionOperatorId == productionOperatorId);
      if (productionTerminalId != null)
        pOperator = pOperator.Where(x => x.ProductionTerminalId == productionTerminalId);
      if (productionOrderId != null)
        pOperator = pOperator.Where(x => x.ProductionOperator.ProductionOrderId == productionOrderId);
      return pOperator.Select(selector);
    }
    #endregion
    #region Delete
    public void DeleteProductionOperatorMachineEmployee(int id)
    {
      var ProductionOperatorMachineEmployee = GetProductionOperatorMachineEmployee(id: id);
      repository.Delete(ProductionOperatorMachineEmployee);
    }
    #endregion
    #region ToResult
    public Expression<Func<ProductionOperatorMachineEmployee, ProductionOperatorMachineEmployeeResult>> ToProductionOperatorMachineEmployeeResult =
        entity => new ProductionOperatorMachineEmployeeResult()
        {
          Id = entity.Id,
          Index = entity.ProductionOperator.OperationSequence.Index,
          EmployeeId = entity.EmployeeId,
          EmployeeFullName = entity.Employee.FirstName + " " + entity.Employee.LastName,
          MachineId = entity.MachineId,
          MachineName = entity.Machine.Name,
          ProductionOperatorId = entity.ProductionOperatorId,
          ProductionTerminalId = entity.ProductionTerminalId,
          ProductionTerminalDescription = entity.ProductionTerminal.Description,
          Description = entity.Description,
          RowVersion = entity.RowVersion
        };
    #endregion
    #region CheckTerminalArrangement
    private bool CheckTerminalArrangement(
        int productionTerminalId,
        int productionOperatorId)
    {
      var productionOperator = GetProductionOperator(
                    selector: e => e,
                    id: productionOperatorId);
      var productionOrder = GetProductionOrder(
                    selector: e => e,
                    id: productionOperator.ProductionOrderId);
      var productionOperatorMachineEmployees = productionOrder.ProductionOperators
                .SelectMany(x => x.ProductionOperatorMachineEmployees).ToList();
      var sequenceIndex = productionOperator.OperationSequence.Index;
      var prevOperator = productionOperatorMachineEmployees.Where(x =>
                x.ProductionOperator.OperationSequence.Index == sequenceIndex - 1);
      var pastOperator = productionOperatorMachineEmployees.Where(x =>
                x.ProductionOperator.OperationSequence.Index == sequenceIndex + 1);
      if (prevOperator?.Any(x =>
                    x.ProductionTerminalId == productionTerminalId) == false
                && pastOperator?.Any(x =>
                    x.ProductionTerminalId == productionTerminalId) == false
                && productionOperator?.ProductionOperatorMachineEmployees.Any(x =>
                    x.ProductionTerminalId == productionTerminalId) == false
                && productionOperatorMachineEmployees.Any(x => x.ProductionTerminalId == productionTerminalId))
      {
        throw new ProductionOperatorMachineEmployeeNotSetTerminalIdException(productionTerminalId);
      }
      return true;

    }
    #endregion
  }
}