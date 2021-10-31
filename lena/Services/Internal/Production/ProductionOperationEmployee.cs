//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Production.Exception;
using lena.Models.Common;
using lena.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core;
using lena.Models.Production.ProductionOperationEmployee;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Get
    public ProductionOperationEmployee GetProductionOperationEmployee(int id) => GetProductionOperationEmployee(selector: e => e, id: id);
    public TResult GetProductionOperationEmployee<TResult>(
        Expression<Func<ProductionOperationEmployee, TResult>> selector,
        int id)
    {

      var productionOperationEmployee = GetProductionOperationEmployees(
                   selector: selector,
                   id: id)



               .FirstOrDefault();
      if (productionOperationEmployee == null)
        throw new ProductionOperationEmployeeNotFoundException(id);
      return productionOperationEmployee;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionOperationEmployees<TResult>(
            Expression<Func<ProductionOperationEmployee, TResult>> selector,
            TValue<int> id = null,
            TValue<int[]> ids = null,
            TValue<int> employeeId = null,
            TValue<int[]> employeeIds = null,
            TValue<int> productionOperationEmployeeGroupId = null)
    {

      var query = repository.GetQuery<ProductionOperationEmployee>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (ids != null)
        query = query.Where(x => ids.Value.Contains(x.Id));
      if (employeeId != null)
        query = query.Where(x => x.EmployeeId == employeeId);
      if (employeeIds != null)
        query = query.Where(x => employeeIds.Value.Contains(x.EmployeeId));
      if (productionOperationEmployeeGroupId != null)
        query = query.Where(x => x.ProductionOperationEmployeeGroupId == productionOperationEmployeeGroupId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public ProductionOperationEmployee AddProductionOperationEmployee(
            int employeeId,
            int productionOperationEmployeeGroupId)
    {

      var productionOperationEmployee = repository.Create<ProductionOperationEmployee>();
      productionOperationEmployee.EmployeeId = employeeId;
      productionOperationEmployee.ProductionOperationEmployeeGroupId = productionOperationEmployeeGroupId;
      repository.Add(productionOperationEmployee);
      return productionOperationEmployee;
    }
    #endregion

    #region Delete
    public void DeleteProductionOperationEmployee(int id)
    {

      var productionOperationEmployee = GetProductionOperationEmployee(id: id);
      repository.Delete(productionOperationEmployee);
    }
    #endregion

    //#region ToResult

    //public Expression<Func<ProductionOperationEmployee, ProductionOperationEmployeeResult>> ToProductionOperationEmployeeResult =
    //    entity => new ProductionOperationEmployeeResult()
    //    {
    //        Id = entity.Id,
    //        ProductionOperationId = entity.ProductionOperationId,
    //        OperationCode = entity.ProductionOperation.Operation.Code,
    //        OperationId = entity.ProductionOperation.OperationId,
    //        OperationName = entity.ProductionOperation.Operation.Title,
    //        EmployeeId = entity.EmployeeId,
    //        EmployeeFullName = entity.Employee.FirstName + ' ' + entity.Employee.LastName,
    //        EmployeeCode = entity.Employee.Code,

    //    };



    //#endregion
  }
}
