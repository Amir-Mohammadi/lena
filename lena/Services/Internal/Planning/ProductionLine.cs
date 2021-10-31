using System;
using System.Linq;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Planning.ProductionLine;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public IQueryable<ProductionLine> GetProductionLines(
        TValue<int> id = null,
        TValue<int> sortIndex = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<int> productivityImpactFactor = null,
        TValue<int> departmentId = null,
        TValue<int> productWarehouseId = null,
        TValue<int> consumeWarehouseId = null,
        TValue<bool> confirmationDetailedCode = null,
        TValue<int> repairUnitId = null)
    {

      var query = repository.GetQuery<ProductionLine>();
      if (id != null) query = query.Where(i => i.Id == id);
      if (sortIndex != null) query = query.Where(i => i.SortIndex == sortIndex);
      if (name != null) query = query.Where(i => i.Name == name);
      if (productivityImpactFactor != null) query = query.Where(i => i.ProductivityImpactFactor == productivityImpactFactor);
      if (description != null) query = query.Where(i => i.Description == description);
      if (departmentId != null) query = query.Where(i => i.DepartmentId == departmentId);
      if (productWarehouseId != null) query = query.Where(i => i.ProductWarehouseId == productWarehouseId);
      if (consumeWarehouseId != null) query = query.Where(i => i.ConsumeWarehouseId == consumeWarehouseId);
      if (confirmationDetailedCode != null) query = query.Where(i => i.ConfirmationDetailedCode == confirmationDetailedCode);
      if (repairUnitId != null)
        query = query.Where(i => i.ProductionLineRepairUnitId == repairUnitId);

      return query;
    }
    public ProductionLine GetProductionLine(int id)
    {

      var productionLine = GetProductionLines(id: id)


                .SingleOrDefault();
      if (productionLine == null)
        throw new ProductionLineNotFoundException(id);
      return productionLine;
    }
    public ProductionLine AddProductionLine(
        int sortIndex,
        string name,
        string detailedCode,
        int productivityImpactFactor,
        string description,
        short departmentId,
        short consumeWarehouseId,
        short productWarehouseId,
        int? adminUserGroupId,
        int? repairUnitId)
    {

      var productionLine = repository.Create<ProductionLine>();
      productionLine.SortIndex = sortIndex;
      productionLine.Name = name;
      productionLine.ConfirmationDetailedCode = false;
      productionLine.DetailedCode = detailedCode;
      productionLine.ProductivityImpactFactor = productivityImpactFactor;
      productionLine.Description = description;
      productionLine.DepartmentId = departmentId;
      productionLine.ConsumeWarehouseId = consumeWarehouseId;
      productionLine.ProductWarehouseId = productWarehouseId;
      productionLine.AdminUserGroupId = adminUserGroupId;
      productionLine.ProductionLineRepairUnitId = repairUnitId;

      repository.Add(productionLine);
      return productionLine;
    }
    public ProductionLine AddProductionLineProcess(
        int sortIndex,
        string name,
        string detailedCode,
        int productivityImpactFactor,
        string description,
        short departmentId,
        short consumeWarehouseId,
        short productWarehouseId,
        int? adminUserGroupId,
        int[] productionSteps,
        int? repairUnitId)
    {

      var productionLine = AddProductionLine(
                    sortIndex: sortIndex,
                    name: name,
                    detailedCode: detailedCode,
                    productivityImpactFactor: productivityImpactFactor,
                    description: description,
                    departmentId: departmentId,
                    consumeWarehouseId: consumeWarehouseId,
                    productWarehouseId: productWarehouseId,
                    adminUserGroupId: adminUserGroupId,
                    repairUnitId: repairUnitId
                    );
      if (productionSteps != null)
        foreach (var productionStepId in productionSteps)
          AddProductionLineProductionStep(
                        productionStepId: productionStepId,
                        productionLineId: productionLine.Id);
      return productionLine;
    }
    public ProductionLine EditProductionLine(
        int id,
        byte[] rowVersion,
        TValue<int> sortIndex = null,
        TValue<string> name = null,
        TValue<string> detailedCode = null,
        TValue<int> productivityImpactFactor = null,
        TValue<string> description = null,
        TValue<short> departmentId = null,
        TValue<short> consumeWarehouseId = null,
        TValue<short> productWarehouseId = null,
        TValue<int?> repairUnitId = null)
    {

      var productionLine = GetProductionLine(id);
      if (productionLine == null)
        throw new ProductionLineNotFoundException(id);
      if (sortIndex != null)
        productionLine.SortIndex = sortIndex;
      if (name != null)
        productionLine.Name = name;
      if (detailedCode != null)
        productionLine.DetailedCode = detailedCode;
      if (productivityImpactFactor != null)
        productionLine.ProductivityImpactFactor = productivityImpactFactor;
      if (description != null)
        productionLine.Description = description;
      if (departmentId != null)
        productionLine.DepartmentId = departmentId;
      if (consumeWarehouseId != null)
        productionLine.ConsumeWarehouseId = consumeWarehouseId;
      if (productWarehouseId != null)
        productionLine.ProductWarehouseId = productWarehouseId;
      if (repairUnitId != null)
        productionLine.ProductionLineRepairUnitId = repairUnitId;

      repository.Update(productionLine, rowVersion: rowVersion);
      return productionLine;
    }
    public ProductionLine EditProductionLineProcess(
        int id,
        byte[] rowVersion,
        TValue<int> sortIndex = null,
        TValue<string> name = null,
        TValue<string> detailedCode = null,
        TValue<int> productivityImpactFactor = null,
        TValue<string> description = null,
        TValue<short> departmentId = null,
        TValue<short> consumeWarehouseId = null,
        TValue<short> productWarehouseId = null,
        TValue<int[]> addedProductionSteps = null,
        TValue<int[]> deletedProductionSteps = null,
        TValue<int?> repairUnitId = null)
    {

      var productionLine = EditProductionLine(
                    id: id,
                    rowVersion: rowVersion,
                    sortIndex: sortIndex,
                    name: name,
                    detailedCode: detailedCode,
                    productivityImpactFactor: productivityImpactFactor,
                    description: description,
                    departmentId: departmentId,
                    consumeWarehouseId: consumeWarehouseId,
                    productWarehouseId: productWarehouseId,
                    repairUnitId: repairUnitId);
      if (addedProductionSteps != null)
        foreach (var productionStepId in addedProductionSteps.Value)
          AddProductionLineProductionStep(
                        productionStepId: productionStepId,
                        productionLineId: productionLine.Id);
      if (deletedProductionSteps != null)
        foreach (var productionStepId in deletedProductionSteps.Value)
          DeleteProductionLineProductionStep(
                        productionStepId: productionStepId,
                        productionLineId: productionLine.Id);


      return productionLine;
    }
    public void DeleteProductionLine(int id)
    {

      var productionLine = GetProductionLine(id: id);
      if (productionLine == null)
        throw new ProductionLineNotFoundException(id);
      repository.Delete(productionLine);
    }
    public IQueryable<ProductionLineResult> ToProductionLineResultQuery(IQueryable<ProductionLine> query)
    {
      var resultQuery = from productionLine in query
                        select new ProductionLineResult()
                        {
                          Id = productionLine.Id,
                          DetailedCode = productionLine.DetailedCode,
                          ConfirmationDetailedCode = productionLine.ConfirmationDetailedCode,
                          SortIndex = productionLine.SortIndex,
                          Name = productionLine.Name,
                          ProductivityImpactFactor = productionLine.ProductivityImpactFactor,
                          Description = productionLine.Description,
                          DepartmentId = productionLine.DepartmentId,
                          ConsumeWarehouseId = productionLine.ConsumeWarehouseId,
                          ProductWarehouseId = productionLine.ProductWarehouseId,
                          DepartmentName = productionLine.Department.Name,
                          ConsumeWarehouseName = productionLine.ConsumeWarehouse.Name,
                          ProductWarehouseName = productionLine.ProductWarehouse.Name,
                          Barcode = "PL" + productionLine.Id.ToString(),
                          RepairUnitId = productionLine.ProductionLineRepairUnitId,
                          RepairUnitName = productionLine.ProductionLineRepairUnit.Name,
                          RepairUnitWarehouseId = productionLine.ProductionLineRepairUnit.WarehouseId,
                          RepairUnitWarehouseName = productionLine.ProductionLineRepairUnit.Warehouse.Name,
                          RowVersion = productionLine.RowVersion
                        };
      return resultQuery;
    }

    public IQueryable<ProductionLineComboResult> ToProductionLineComboResultQuery(IQueryable<ProductionLine> query)
    {
      var resultQuery = from productionLine in query
                        select new ProductionLineComboResult()
                        {
                          Id = productionLine.Id,
                          Name = productionLine.Name
                        };
      return resultQuery;
    }
    public ProductionLineResult ToProductionLineResult(ProductionLine productionLine)
    {
      var result = new ProductionLineResult()
      {
        Id = productionLine.Id,
        Name = productionLine.Name,
        DetailedCode = productionLine.DetailedCode,
        ConfirmationDetailedCode = productionLine.ConfirmationDetailedCode,
        ProductivityImpactFactor = productionLine.ProductivityImpactFactor,
        SortIndex = productionLine.SortIndex,
        Description = productionLine.Description,
        DepartmentId = productionLine.DepartmentId,
        ConsumeWarehouseId = productionLine.ConsumeWarehouseId,
        ProductWarehouseId = productionLine.ProductWarehouseId,
        DepartmentName = productionLine.Department.Name,
        ConsumeWarehouseName = productionLine.ConsumeWarehouse.Name,
        ProductWarehouseName = productionLine.ProductWarehouse.Name,
        RepairUnitId = productionLine.ProductionLineRepairUnitId,
        RepairUnitName = productionLine.ProductionLineRepairUnit?.Name ?? "",
        RepairUnitWarehouseId = productionLine.ProductionLineRepairUnitId,
        RepairUnitWarehouseName = productionLine.ProductionLineRepairUnit != null ? productionLine.ProductionLineRepairUnit.Warehouse.Name : "",
        ProductionSteps = productionLine.ProductionLineProductionSteps.Select(i => i.ProductionStepId).ToArray(),
        Barcode = "PL" + productionLine.Id.ToString(),
        RowVersion = productionLine.RowVersion
      };
      return result;
    }

    public IOrderedQueryable<ProductionLineResult> SortProductionLineResult(
        IQueryable<ProductionLineResult> input,
        SortInput<ProductionLineSortType> options)
    {
      switch (options.SortType)
      {
        case ProductionLineSortType.Id:
          return input.OrderBy(i => i.Id, options.SortOrder);
        case ProductionLineSortType.Name:
          return input.OrderBy(i => i.Name, options.SortOrder);
        case ProductionLineSortType.ProductivityImpactFactor:
          return input.OrderBy(i => i.ProductivityImpactFactor, options.SortOrder);
        case ProductionLineSortType.SortIndex:
          return input.OrderBy(i => i.SortIndex, options.SortOrder);
        case ProductionLineSortType.ConsumeWarehouseName:
          return input.OrderBy(i => i.ConsumeWarehouseName, options.SortOrder);
        case ProductionLineSortType.ProductWarehouseName:
          return input.OrderBy(i => i.ProductWarehouseName, options.SortOrder);
        case ProductionLineSortType.DepartmentName:
          return input.OrderBy(i => i.DepartmentName, options.SortOrder);
        case ProductionLineSortType.RepairUnitName:
          return input.OrderBy(i => i.RepairUnitName, options.SortOrder);
        case ProductionLineSortType.RepairUnitWarehouseName:
          return input.OrderBy(i => i.RepairUnitWarehouseName, options.SortOrder);
        case ProductionLineSortType.DetailedCode:
          return input.OrderBy(i => i.DetailedCode, options.SortOrder);


        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public IQueryable<ProductionLineResult> SearchProductionLine(
        IQueryable<ProductionLineResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems
       )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from productionLine in query
                where productionLine.Name.Contains(searchText) ||
                productionLine.Id.ToString().Contains(searchText) ||
                productionLine.SortIndex.ToString().Contains(searchText) ||
                productionLine.DepartmentName.Contains(searchText) ||
                productionLine.Description.Contains(searchText) ||
                productionLine.ConsumeWarehouseName.Contains(searchText) ||
                productionLine.ProductWarehouseName.Contains(searchText) ||
                productionLine.RepairUnitName.Contains(searchText) ||
                productionLine.RepairUnitWarehouseName.Contains(searchText)
                select productionLine;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }

    #region ConfirmationOfProductionLineDetailedCode
    public ProductionLine ConfirmationOfProductionLineDetailedCode(
        int id,
        string detailedCode,
        byte[] rowVersion)
    {

      var productionLine = GetProductionLine(id: id);
      if (detailedCode != null)
        productionLine.DetailedCode = detailedCode;
      productionLine.ConfirmationDetailedCode = true;
      repository.Update(productionLine, rowVersion);
      return productionLine;
    }
    #endregion

    #region DisapprovalOfProductionLineDetailedCode
    public ProductionLine DisapprovalOfProductionLineDetailedCode(
        int id,
        byte[] rowVersion,
        TValue<string> detailedCode)
    {

      var productionLine = GetProductionLine(id: id);
      if (detailedCode != null)
        productionLine.DetailedCode = detailedCode;
      productionLine.ConfirmationDetailedCode = false;
      repository.Update(productionLine, rowVersion);
      return productionLine;
    }
    #endregion
  }
}




