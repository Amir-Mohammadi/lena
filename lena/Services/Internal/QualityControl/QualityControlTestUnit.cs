using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.QualityControl.QualityControlTestUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{

  public partial class QualityControl
  {
    public QualityControlTestUnit AddQualityControlTestUnit(string name, bool isActive, string description)
    {

      var entity = repository.Create<QualityControlTestUnit>();
      entity.Name = name;
      entity.IsActive = isActive;
      entity.Description = description;
      repository.Add(entity);
      return entity;
    }
    public QualityControlTestUnit EditQualityControlTestUnit(byte[] rowVersion, int id, TValue<string> name = null, TValue<bool> isActive = null, TValue<string> description = null)
    {

      var entity = GetQualityControlTestUnit(id: id);

      if (name != null)
        entity.Name = name;

      if (isActive != null)
        entity.IsActive = isActive;
      if (description != null)
        entity.Description = description;

      repository.Update(entity: entity, rowVersion: rowVersion);

      return entity;
    }
    public void DeleteQualityControlTestUnit(int id)
    {

      var entity = GetQualityControlTestUnit(id: id);
      repository.Delete(entity);
    }

    public IQueryable<QualityControlTestUnit> GetQualityControlTestUnits(
       TValue<int> id = null,
       TValue<string> name = null,
       TValue<bool> isActive = null)
    {

      var query = repository.GetQuery<QualityControlTestUnit>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (name != null)
        query = query.Where(i => i.Name == name);
      if (isActive != null)
        query = query.Where(i => i.IsActive == isActive);

      return query;
    }

    public QualityControlTestUnit GetQualityControlTestUnit(int id)
    {

      var qualityControlTestUnit = GetQualityControlTestUnits(id: id).FirstOrDefault();
      if (qualityControlTestUnit == null)
        throw new QualityControlTestUnitNotFoundException(id);
      return qualityControlTestUnit;
    }
    public IOrderedQueryable<QualityControlTestUnitResult> SortQualityControlTestUnitResult(IQueryable<QualityControlTestUnitResult> query, SortInput<QualityControlTestUnitSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case QualityControlTestUnitSortType.Id:
          return query.OrderBy(r => r.Id, sortInput.SortOrder);
        case QualityControlTestUnitSortType.Name:
          return query.OrderBy(r => r.Name, sortInput.SortOrder);
        case QualityControlTestUnitSortType.IsActive:
          return query.OrderBy(r => r.IsActive, sortInput.SortOrder);
        case QualityControlTestUnitSortType.Description:
          return query.OrderBy(r => r.Description, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }


    public IQueryable<QualityControlTestUnitResult> ToQualityControlTestUnitResultQuery(IQueryable<QualityControlTestUnit> query)
    {
      var result = from qcTestUnit in query
                   select new QualityControlTestUnitResult()
                   {
                     Id = qcTestUnit.Id,
                     Name = qcTestUnit.Name,
                     IsActive = qcTestUnit.IsActive,
                     Description = qcTestUnit.Description,
                     RowVersion = qcTestUnit.RowVersion
                   };
      return result;
    }

    public QualityControlTestUnitResult ToQualityControlTestUnitResult(QualityControlTestUnit qcTestUnit)
    {
      var result = new QualityControlTestUnitResult()
      {
        Id = qcTestUnit.Id,
        Name = qcTestUnit.Name,
        IsActive = qcTestUnit.IsActive,
        Description = qcTestUnit.Description,
        RowVersion = qcTestUnit.RowVersion
      };
      return result;
    }

    public IQueryable<QualityControlTestUnitResult> SearchQualityControlTestUnitResult(
       IQueryable<QualityControlTestUnitResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where item.Name.Contains(searchText) ||
                      item.Id.ToString().Contains(searchText) ||
                      item.Description.Contains(searchText)
                select item;

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);

      return query;
    }

  }
}
