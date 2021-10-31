using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Production.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Production.RepairUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Get
    public ProductionLineRepairUnit GetProductionLineRepairUnit(int productionLineRepairUnitId)
        => GetProductionLineRepairUnit(selector: e => e, id: productionLineRepairUnitId);
    public TResult GetProductionLineRepairUnit<TResult>(
        Expression<Func<ProductionLineRepairUnit, TResult>> selector,
        int id)
    {

      var ProductionLineRepairUnit = GetProductionLineRepairUnits(
                   selector: selector,
                   id: id)


               .FirstOrDefault();
      if (ProductionLineRepairUnit == null)
        throw new ProductionLineRepairUnitNotFoundException(id: id);
      return ProductionLineRepairUnit;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionLineRepairUnits<TResult>(
            Expression<Func<ProductionLineRepairUnit, TResult>> selector,
            TValue<int> id = null,
            TValue<string> name = null,
            TValue<int> warehouseId = null,
            TValue<int> userId = null
            )
    {

      var query = repository.GetQuery<ProductionLineRepairUnit>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (name != null)
        query = query.Where(x => x.Name == name);
      if (warehouseId != null)
        query = query.Where(x => x.WarehouseId == warehouseId);
      if (userId != null)
        query = query.Where(x => x.UserId == userId);

      return query.Select(selector);
    }
    #endregion
    #region Add
    public ProductionLineRepairUnit AddProductionLineRepairUnit(
                string name,
                short warehouseId,
                TValue<int> userId = null,
                TValue<DateTime> dateTime = null
                )
    {


      var repairUnit = repository.Create<ProductionLineRepairUnit>();
      repairUnit.Name = name;
      repairUnit.WarehouseId = warehouseId;
      repairUnit.UserId = userId ?? App.Providers.Security.CurrentLoginData.UserId;
      repairUnit.CreationTime = dateTime ?? DateTime.UtcNow;

      repository.Add(repairUnit);
      return repairUnit;
    }

    public ProductionLineRepairUnit EditProductionLineRepairUnit(
                int id,
                byte[] rowVersion,
                TValue<string> name = null,
                 TValue<short> warehouseId = null,
                 TValue<int> productionLineId = null
                )
    {


      var repairUnit = GetProductionLineRepairUnit(id);
      if (name != null)
        repairUnit.Name = name;
      if (warehouseId != null)
        repairUnit.WarehouseId = warehouseId;

      repository.Update(repairUnit, rowVersion);
      return repairUnit;
    }
    #endregion

    #region Delete
    public void DeleteProductionLineRepairUnit(int repairUnitId, byte[] rowVersion)
    {

      var repairUnitEntity = GetProductionLineRepairUnit(productionLineRepairUnitId: repairUnitId);

      repository.Delete(repairUnitEntity);

    }
    public void DeleteProductionLineRepairUnit(ProductionLineRepairUnit ProductionLineRepairUnit)
    {

      repository.Delete(ProductionLineRepairUnit);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ProductionLineRepairUnitResult> SortProductionLineRepairUnitResult(
        IQueryable<ProductionLineRepairUnitResult> query,
        SortInput<ProductionLineRepairUnitSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProductionLineRepairUnitSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ProductionLineRepairUnitSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        case ProductionLineRepairUnitSortType.UserName:
          return query.OrderBy(a => a.UserName, sort.SortOrder);
        case ProductionLineRepairUnitSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case ProductionLineRepairUnitSortType.CreationTime:
          return query.OrderBy(a => a.CreationTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<ProductionLineRepairUnitResult> SearchProductionLineRepairUnitResult(
        IQueryable<ProductionLineRepairUnitResult> query,
        string searchText)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                    item.Name.Contains(searchText) ||
                    item.UserName.Contains(searchText) ||
                    item.EmployeeFullName.Contains(searchText)
                select item;
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<ProductionLineRepairUnit, ProductionLineRepairUnitResult>> ToProductionLineRepairUnitResult =
                item => new ProductionLineRepairUnitResult
                {
                  Id = item.Id,
                  Name = item.Name,
                  UserId = item.UserId,
                  UserName = item.User.UserName,
                  EmployeeId = item.User.Employee.Id,
                  EmployeeFullName = item.User.Employee.FirstName + " " + item.User.Employee.LastName,
                  WarehouseId = item.WarehouseId,
                  WarehouseName = item.Warehouse.Name,
                  CreationTime = item.CreationTime,
                  RowVersion = item.RowVersion
                };

    public Expression<Func<ProductionLineRepairUnit, ProductionLineRepairUnitComboResult>> ToProductionLineRepairUnitComboResult =
              item => new ProductionLineRepairUnitComboResult
              {
                Id = item.Id,
                Name = item.Name,
                RowVersion = item.RowVersion
              };
    #endregion
  }
}
