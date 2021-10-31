using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Production.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Production.ProductionOperatorEmployeeBan;
using System;
using System.Linq;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Add
    public ProductionOperatorEmployeeBan AddProductionOperatorEmployeeBan(
        int productionOperatorId,
        int employeeId)
    {

      var productionOperatorEmployeeBan = repository.Create<ProductionOperatorEmployeeBan>();
      productionOperatorEmployeeBan.ProductionOperatorId = productionOperatorId;
      productionOperatorEmployeeBan.EmployeeId = employeeId;
      productionOperatorEmployeeBan.BanDateTime = DateTime.UtcNow;
      productionOperatorEmployeeBan.IsBan = true;
      repository.Add(productionOperatorEmployeeBan);

      return productionOperatorEmployeeBan;
    }
    #endregion

    #region Edit

    public ProductionOperatorEmployeeBan EditProductionOperatorEmployeeBan(
       int id,
       byte[] rowVersion,
       TValue<bool> isBan = null,
       TValue<int> revokeUserId = null,
       TValue<DateTime> revokeDateTime = null)
    {

      var productionOperatorEmployeeBan = GetProductionOperatorEmployeeBan(id: id);

      EditProductionOperatorEmployeeBan(
                productionOperatorEmployeeBan: productionOperatorEmployeeBan,
                rowVersion: rowVersion,
                isBan: isBan,
                revokeUserId: revokeUserId,
                revokeDateTime: revokeDateTime);

      return productionOperatorEmployeeBan;

    }

    public ProductionOperatorEmployeeBan EditProductionOperatorEmployeeBan(
        ProductionOperatorEmployeeBan productionOperatorEmployeeBan,
        byte[] rowVersion,
        TValue<bool> isBan = null,
        TValue<int> revokeUserId = null,
        TValue<DateTime> revokeDateTime = null)
    {

      if (isBan != null)
        productionOperatorEmployeeBan.IsBan = isBan;
      if (revokeUserId != null)
        productionOperatorEmployeeBan.RevokeUserId = revokeUserId;
      if (revokeDateTime != null)
        productionOperatorEmployeeBan.RevokeDateTime = revokeDateTime;


      repository.Update(entity: productionOperatorEmployeeBan, rowVersion: rowVersion);

      return productionOperatorEmployeeBan;

    }
    #endregion

    #region Get
    public ProductionOperatorEmployeeBan GetProductionOperatorEmployeeBan(int id) => GetProductionOperatorEmployeeBan(id: id, selector: e => e);
    public TResult GetProductionOperatorEmployeeBan<TResult>(
        Expression<Func<ProductionOperatorEmployeeBan, TResult>> selector,
        int id)
    {

      var production = GetProductionOperatorEmployeeBans(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (production == null)
        throw new ProductionOperatorEmployeeBanNotFoundException(id: id);
      return production;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProductionOperatorEmployeeBans<TResult>(
        Expression<Func<ProductionOperatorEmployeeBan, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<int> productionOperatorId = null,
        TValue<int?[]> productionOperatorIds = null,
        TValue<bool> isBan = null,
        TValue<int> employeeId = null,
        TValue<int[]> employeeIds = null)
    {

      var productionOperatorEmployeeBans = repository.GetQuery<ProductionOperatorEmployeeBan>();

      if (id != null)
        productionOperatorEmployeeBans = productionOperatorEmployeeBans.Where(i => i.Id == id);
      if (ids != null)
        productionOperatorEmployeeBans = productionOperatorEmployeeBans.Where(i => ids.Value.Contains(i.Id));
      if (productionOperatorId != null)
        productionOperatorEmployeeBans = productionOperatorEmployeeBans.Where(i => i.ProductionOperatorId == productionOperatorId);
      if (productionOperatorIds != null)
        productionOperatorEmployeeBans = productionOperatorEmployeeBans.Where(i => productionOperatorIds.Value.Contains(i.ProductionOperatorId));
      if (employeeId != null)
        productionOperatorEmployeeBans = productionOperatorEmployeeBans.Where(i => i.EmployeeId == employeeId);
      if (employeeIds != null)
        productionOperatorEmployeeBans = productionOperatorEmployeeBans.Where(i => employeeIds.Value.Contains(i.EmployeeId));
      if (isBan != null)
        productionOperatorEmployeeBans = productionOperatorEmployeeBans.Where(i => i.IsBan == isBan);

      return productionOperatorEmployeeBans.Select(selector);
    }
    #endregion

    #region Delete
    public void DeleteProductionOperatorEmployeeBan(ProductionOperatorEmployeeBan productionOperatorEmployeeBan)
    {

      repository.Delete(productionOperatorEmployeeBan);
    }
    #endregion

    #region ToResult
    public Expression<Func<ProductionOperatorEmployeeBan, ProductionOperatorEmployeeBanResult>> ToProductionOperatorEmployeeBanResult =
                item => new ProductionOperatorEmployeeBanResult
                {
                  Id = item.Id,
                  ProductionOperatorId = item.ProductionOperatorId,
                  OperationId = item.ProductionOperator.OperationId,
                  EmployeeId = item.EmployeeId,
                  IsBan = item.IsBan,
                  RowVersion = item.RowVersion
                };
    #endregion

    #region ResolveProductionOperatorEmployeeBanProcess
    public void ResolveProductionOperatorEmployeeBanProcess(int id, byte[] rowVersion)
    {


      EditProductionOperatorEmployeeBan(
                id: id,
                rowVersion: rowVersion,
                isBan: false,
                revokeUserId: App.Providers.Security.CurrentLoginData.UserId,
                revokeDateTime: DateTime.UtcNow);
    }
    #endregion


    #region CheckBanEmployees
    public void CheckBanEmployees(int?[] productionOperatorIds, int[] employeeIds)
    {

      var employeeBans = GetProductionOperatorEmployeeBans(
                selector: e => e,
                employeeIds: employeeIds,
                productionOperatorIds: productionOperatorIds,
                isBan: true);

      if (employeeBans.Any())
      {
        var employeeBan = employeeBans.Select(m =>
              new
              {
                OperationTitle = m.ProductionOperator.Operation.Title,
                EmployeeFullName = m.Employee.FirstName + " " + m.Employee.LastName,
                ProductionOrderCode = m.ProductionOperator.ProductionOrder.Code
              })
              .FirstOrDefault();

        throw new TheEmployeeIsBannedException(
                  productionOrderCode: employeeBan.ProductionOrderCode,
                  employeeFullName: employeeBan.EmployeeFullName,
                  operationTitle: employeeBan.OperationTitle);
      }
    }
    #endregion
  }
}
