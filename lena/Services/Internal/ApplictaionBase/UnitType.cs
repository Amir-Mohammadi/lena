using System;
using System.Linq;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.UnitType;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase
{
  public partial class ApplicationBase
  {
    public UnitType AddUnitType(string name, bool isActive)
    {

      var unitType = repository.Create<UnitType>();
      unitType.Name = name;
      unitType.IsActive = isActive;
      repository.Add(unitType);
      return unitType;
    }
    public UnitType EditUnitType(byte[] rowVersion, int id, TValue<string> name = null, TValue<bool> isActive = null)
    {

      var unitType = GetUnitType(id: id);
      if (name != null)
        unitType.Name = name;
      if (isActive != null)
        unitType.IsActive = isActive;
      repository.Update(entity: unitType, rowVersion: unitType.RowVersion);
      return unitType;
    }
    public void DeleteUnitType(int id)
    {

      var unitType = GetUnitType(id: id);
      repository.Delete(unitType);
    }
    public IQueryable<UnitType> GetUnitTypes(TValue<int> id = null, TValue<string> name = null)
    {

      var isIdNUll = id == null;
      var isNameNull = name == null;
      var unitTypes = from unitType in repository.GetQuery<UnitType>()
                      where (isIdNUll || unitType.Id == id) &&
                                  (isNameNull || unitType.Name == name)
                      select unitType;
      return unitTypes;
    }
    public UnitType GetUnitType(int id)
    {

      var unitType = GetUnitTypes(id: id).FirstOrDefault();
      if (unitType == null)
        throw new UnitTypeNotFoundException(id);
      return unitType;
    }
    public IOrderedQueryable<UnitTypeResult> SortUnitTypeResult(IQueryable<UnitTypeResult> query, SortInput<UnitTypeSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case UnitTypeSortType.Id:
          return query.OrderBy(r => r.Id, sortInput.SortOrder);
        case UnitTypeSortType.Name:
          return query.OrderBy(r => r.Name, sortInput.SortOrder);
        case UnitTypeSortType.IsActive:
          return query.OrderBy(r => r.IsActive, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<UnitTypeResult> ToUnitTypeResultQuery(IQueryable<UnitType> query)
    {
      var result = from item in query
                   select new UnitTypeResult()
                   {
                     Id = item.Id,
                     Name = item.Name,
                     IsActive = item.IsActive,
                     RowVersion = item.RowVersion
                   };
      return result;
    }
    public UnitTypeResult ToUnitTypeResult(UnitType unitType)
    {
      var result = new UnitTypeResult()
      {
        Id = unitType.Id,
        Name = unitType.Name,
        IsActive = unitType.IsActive,
        RowVersion = unitType.RowVersion
      };
      return result;
    }

    public IQueryable<UnitTypeResult> SearchUnitTypeResult(
      IQueryable<UnitTypeResult> query,
      string searchText,
      AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where item.Name.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }

  }
}