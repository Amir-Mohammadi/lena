using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.ApplicationBase.Unit;
using lena.Models.Common;
using lena.Models.Unit;
using lena.Models.WarehouseManagement.Common;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase
{
  public partial class ApplicationBase
  {
    #region Add
    public Unit AddUnit(
        string name,
        bool isMainUnit,
        double conversionRatio,
        bool isActive,
        byte unitTypeId,
        byte decimalDigitCount,
        string symbol)
    {
      var unit = repository.Create<Unit>();
      unit.Name = name;
      unit.IsMainUnit = isMainUnit;
      unit.ConversionRatio = conversionRatio;
      unit.IsActive = isActive;
      unit.UnitTypeId = unitTypeId;
      unit.DecimalDigitCount = decimalDigitCount;
      unit.Symbol = symbol;
      repository.Add(unit);
      return unit;
    }
    #endregion
    #region AddUnitProcess
    public Unit AddUnitProcess(
        string name,
        bool isMainUnit,
        double conversionRatio,
        bool isActive,
        byte unitTypeId,
        byte decimalDigitCount,
        string symbol
        )
    {
      if (isMainUnit == true)
      {
        var unitsOfCurrentType = GetUnits(
                      selector: e => new { e.Id, e.IsMainUnit },
                      unitTypeId: unitTypeId);
        var isThereAnyMainUnitForThisUnitType = unitsOfCurrentType.Any(i => i.IsMainUnit);
        if (isThereAnyMainUnitForThisUnitType)
        {
          throw new ThereIsAlreadyOneMainUnitForThisUnitType();
        }
      }
      return AddUnit(
                    name: name,
                    isMainUnit: isMainUnit,
                    conversionRatio: conversionRatio,
                    isActive: isActive,
                    unitTypeId: unitTypeId,
                    decimalDigitCount: decimalDigitCount,
                    symbol: symbol);
    }
    #endregion
    #region Edit Process
    public Unit EditUnitProcess(
        byte[] rowVersion,
        byte id,
        TValue<string> name = null,
        TValue<bool> isMainUnit = null,
        TValue<double> conversionRatio = null,
        TValue<bool> isActive = null,
        TValue<byte> unitTypeId = null,
        TValue<byte> decimalDigitCount = null,
        TValue<string> symbol = null)
    {
      if (isMainUnit != null)
      {
        var currentUnit = GetUnit(id: id);
        var editUnitTypeId = unitTypeId ?? currentUnit.UnitTypeId;
        var unitsOfCurrentType = GetUnits(
                      selector: e => new { e.Id, e.IsMainUnit },
                      unitTypeId: currentUnit.UnitTypeId,
                      isMainUnit: true);
        var isThereAnyMainUnitForThisUnitType = unitsOfCurrentType.Any(i => i.Id != id);
        if (isMainUnit && isThereAnyMainUnitForThisUnitType)
        {
          throw new ThereIsAlreadyOneMainUnitForThisUnitType();
        }
      }
      var modifiedUnit = EditUnit(
                    rowVersion: rowVersion,
                    id: id,
                    conversionRatio: conversionRatio,
                    isActive: isActive,
                    isMainUnit: isMainUnit,
                    name: name,
                    unitTypeId: unitTypeId,
                    decimalDigitCount: decimalDigitCount,
                    symbol: symbol
                   );
      return modifiedUnit;
    }
    #endregion
    #region Edit
    public Unit EditUnit(
        byte[] rowVersion,
        byte id,
        TValue<string> name = null,
        TValue<bool> isMainUnit = null,
        TValue<double> conversionRatio = null,
        TValue<bool> isActive = null,
        TValue<byte> unitTypeId = null,
        TValue<byte> decimalDigitCount = null,
        TValue<string> symbol = null)
    {
      var unit = GetUnit(id: id);
      if (name != null)
        unit.Name = name;
      if (isMainUnit != null)
        unit.IsMainUnit = isMainUnit;
      if (conversionRatio != null)
        unit.ConversionRatio = conversionRatio;
      if (isActive != null)
        unit.IsActive = isActive;
      if (unitTypeId != null)
        unit.UnitTypeId = unitTypeId;
      if (decimalDigitCount != null)
        unit.DecimalDigitCount = decimalDigitCount;
      if (symbol != null)
        unit.Symbol = symbol;
      repository.Update(entity: unit, rowVersion: unit.RowVersion);
      return unit;
    }
    #endregion
    #region Delete
    public void DeleteUnit(byte id)
    {
      var unit = GetUnit(id: id);
      repository.Delete(unit);
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetUnits<TResult>(
        Expression<Func<Unit, TResult>> selector,
        TValue<byte> id = null,
        TValue<byte> unitTypeId = null,
        TValue<string> name = null,
        TValue<bool> isActive = null,
        TValue<bool> isMainUnit = null,
        TValue<int> stuffId = null,
        TValue<string> searchText = null)
    {
      var query = repository.GetQuery<Unit>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (name != null)
        query = query.Where(i => i.Name == name);
      if (unitTypeId != null)
        query = query.Where(i => i.UnitTypeId == unitTypeId);
      if (isActive != null)
        query = query.Where(i => i.IsActive == isActive);
      if (isMainUnit != null)
        query = query.Where(i => i.IsMainUnit == isMainUnit);
      if (stuffId != null)
      {
        query = from unit in query
                from stuff in unit.UnitType.Stuffs
                where stuff.Id == stuffId
                select unit;
      }
      return query.Select(selector);
    }
    #endregion
    #region Get
    public Unit GetUnit(byte id) => GetUnit(e => e, id: id);
    public TResult GetUnit<TResult>(
        Expression<Func<Unit, TResult>> selector,
        byte id)
    {
      var unit = GetUnits(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (unit == null)
        throw new UnitNotFoundException(id);
      return unit;
    }
    #endregion
    #region SortResult
    public IOrderedQueryable<UnitResult> SortUnitResult(IQueryable<UnitResult> query, SortInput<UnitSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case UnitSortType.Id:
          return query.OrderBy(r => r.Id, sortInput.SortOrder);
        case UnitSortType.Name:
          return query.OrderBy(r => r.Name, sortInput.SortOrder);
        case UnitSortType.IsActive:
          return query.OrderBy(r => r.IsActive, sortInput.SortOrder);
        case UnitSortType.UnitTypeName:
          return query.OrderBy(r => r.UnitTypeName, sortInput.SortOrder);
        case UnitSortType.IsMainUnit:
          return query.OrderBy(r => r.IsMainUnit, sortInput.SortOrder);
        case UnitSortType.ConversionRatio:
          return query.OrderBy(r => r.ConversionRatio, sortInput.SortOrder);
        case UnitSortType.DecimalDigitCount:
          return query.OrderBy(r => r.DecimalDigitCount, sortInput.SortOrder);
        case UnitSortType.Symbol:
          return query.OrderBy(r => r.Symbol, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region SearchResult
    public IQueryable<UnitResult> SearchUnitResult(IQueryable<UnitResult> query, string searchText, AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(i => i.Id.ToString().Contains(searchText) ||
                                 i.Name.Contains(searchText) ||
                                 i.UnitTypeName.Contains(searchText) ||
                                 i.ConversionRatio.ToString().Contains(searchText) ||
                                 i.Symbol.Contains(searchText));
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region SortComboResult
    public IOrderedQueryable<UnitComboResult> SortUnitComboResult(IQueryable<UnitComboResult> query, SortInput<UnitSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case UnitSortType.Id:
          return query.OrderBy(r => r.Id, sortInput.SortOrder);
        case UnitSortType.Name:
          return query.OrderBy(r => r.Name, sortInput.SortOrder);
        case UnitSortType.IsMainUnit:
          return query.OrderBy(r => r.IsMainUnit, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    public Expression<Func<Unit, UnitResult>> ToUnitResult =
        unit => new UnitResult()
        {
          Id = unit.Id,
          Name = unit.Name,
          ConversionRatio = unit.ConversionRatio,
          IsActive = unit.IsActive,
          IsMainUnit = unit.IsMainUnit,
          UnitTypeName = unit.UnitType.Name,
          UnitTypeId = unit.UnitTypeId,
          DecimalDigitCount = unit.DecimalDigitCount,
          RowVersion = unit.RowVersion,
          Symbol = unit.Symbol
        };
    #endregion
    #region ToComboResult
    public Expression<Func<Unit, UnitComboResult>> ToUnitComboResult =
        unit => new UnitComboResult
        {
          Id = unit.Id,
          Name = unit.Name,
          IsMainUnit = unit.IsMainUnit,
          UnitTypeId = unit.UnitTypeId,
          UnitConversionRatio = unit.ConversionRatio,
          DecimalDigitCount = unit.DecimalDigitCount
        };
    #endregion
    #region SumQty
    public SumQtyResult SumQty(byte? targetUnitId, SumQtyItemInput[] sumQtys)
    {
      var result = new SumQtyResult();
      Unit mainUnit = targetUnitId != null ? GetUnit(id: targetUnitId.Value)
               : null;
      double sumQty = 0;
      double sumCanceledQty = 0;
      foreach (var item in sumQtys)
      {
        var unit = GetUnit(item.UnitId);
        mainUnit = mainUnit ?? unit.UnitType.Units.SingleOrDefault(i => i.IsMainUnit);
        if (unit.UnitTypeId != mainUnit.UnitTypeId)
          throw new UnitTypeNotMatchException(unitName1: unit.Name, unitName2: mainUnit.Name);
        // convertRatio = Math.Round(unit.ConversionRatio / mainUnit.ConversionRatio, mainUnit.DecimalDigitCount);
        //convertRatio = mainUnit.ConversionRatio;
        sumQty += Math.Round(item.Qty * unit.ConversionRatio / mainUnit.ConversionRatio, mainUnit.DecimalDigitCount);
        sumCanceledQty += item.CanceledQty * unit.ConversionRatio / mainUnit.ConversionRatio;
      }
      result = new SumQtyResult()
      {
        Qty = sumQty,
        ConvertRatio = mainUnit.ConversionRatio,
        CanceledQty = sumCanceledQty,
        UnitId = mainUnit.Id,
        UnitName = mainUnit.Name
      };
      return result;
    }
    public SumQtyResult SumQty(byte? targetUnitId, SumQtyItemInput sumItem)
    {
      var result = new SumQtyResult();
      Unit mainUnit = targetUnitId != null ? GetUnit(id: targetUnitId.Value)
               : null;
      var unit = GetUnit(sumItem.UnitId);
      mainUnit = mainUnit ?? unit.UnitType.Units.SingleOrDefault(i => i.IsMainUnit);
      if (unit.UnitTypeId != mainUnit.UnitTypeId)
        throw new UnitTypeNotMatchException(unitName1: unit.Name, unitName2: mainUnit.Name);
      var sumQty = Math.Round(sumItem.Qty * unit.ConversionRatio / mainUnit.ConversionRatio, mainUnit.DecimalDigitCount);
      var sumCanceledQty = sumItem.CanceledQty * unit.ConversionRatio / mainUnit.ConversionRatio;
      result = new SumQtyResult()
      {
        Qty = sumQty,
        ConvertRatio = mainUnit.ConversionRatio,
        CanceledQty = sumCanceledQty,
        UnitId = mainUnit.Id,
        UnitName = mainUnit.Name
      };
      return result;
    }
    #endregion
    #region Compare Qty
    public Result CompareQty(QtyItemCompareInput qtyItemsCompareinput)
    {
      double currentQty = 0;
      double targetQty = 0;
      var length = qtyItemsCompareinput.CurrentQtyItemInput.Length;
      var currentQtyItemInput = qtyItemsCompareinput.CurrentQtyItemInput;
      var targetQtyItemInput = qtyItemsCompareinput.TargetQtyItemInput;
      Unit expectedUnit = null;
      for (int index = 0; index < length; index++)
      {
        expectedUnit = GetUnit(id: targetQtyItemInput[index].TargetUnitId);
        var currentUnit = GetUnit(currentQtyItemInput[index].CurrentUnitId);
        currentQty = Math.Round(currentQtyItemInput[index].CurrentQty * currentUnit.ConversionRatio / expectedUnit.ConversionRatio, expectedUnit.DecimalDigitCount);
        targetQty = Math.Round(targetQtyItemInput[index].TargetQty, expectedUnit.DecimalDigitCount);
        if (currentQty > targetQty)
          throw new CurrentItemQtyIsBiggerThanTargetItemQtyException(currentItemQty: currentQty, targetItemQty: targetQty);
      }
      return new Result();
    }
    #endregion
  }
}