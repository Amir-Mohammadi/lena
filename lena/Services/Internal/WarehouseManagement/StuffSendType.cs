using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.SendProduct;
using lena.Models.WarehouseManagement.StuffSndeType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Get
    public ExitReceiptRequestType GetExitReceiptRequestType(int id) => GetExitReceiptRequestType(selector: e => e, id: id);
    public TResult GetExitReceiptRequestType<TResult>(
        Expression<Func<ExitReceiptRequestType, TResult>> selector,
        int id)
    {

      var tagType = GetExitReceiptRequestTypes(selector: selector, id: id)


                .FirstOrDefault();
      if (tagType == null)
        throw new ExitReceiptRequestTypeNotFoundException(id);
      return tagType;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetExitReceiptRequestTypes<TResult>(
        Expression<Func<ExitReceiptRequestType, TResult>> selector,
        TValue<int> id = null,
        TValue<string> title = null,
        TValue<bool> autoConfirm = null,
        TValue<bool> IsActive = null)
    {

      var query = repository.GetQuery<ExitReceiptRequestType>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (title != null)
        query = query.Where(x => x.Title == title);
      if (IsActive != null)
        query = query.Where(x => x.IsActive == IsActive);
      if (autoConfirm != null)
        query = query.Where(x => x.AutoConfirm == autoConfirm);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public ExitReceiptRequestType AddExitReceiptRequestType(int id, string title, bool autoConfirm, bool isActive, string desciption)
    {

      var typeRecord = repository.GetQuery<ExitReceiptRequestType>().FirstOrDefault(x => x.Id == id);
      if (typeRecord == null)
      {
        var entity = repository.Create<ExitReceiptRequestType>();
        entity.Id = id;
        entity.Title = title;
        entity.AutoConfirm = autoConfirm;
        entity.IsActive = isActive;
        repository.Add(entity);
        return entity;
      }

      return typeRecord;

    }
    #endregion
    #region Edit
    public ExitReceiptRequestType EditExitReceiptRequestType(
        int id,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<bool> autoConfirm = null,
        TValue<bool> isActive = null)
    {

      var entity = GetExitReceiptRequestType(id: id);
      if (title != null)
        entity.Title = title;
      if (autoConfirm != null)
        entity.AutoConfirm = autoConfirm;
      if (isActive != null)
        entity.IsActive = isActive;

      repository.Update(rowVersion: rowVersion, entity: entity);
      return entity;
    }
    #endregion
    #region Delete
    public void DeleteExitReceiptRequestType(int id)
    {

      var entity = GetExitReceiptRequestType(id: id);
      repository.Delete(entity);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ExitReceiptRequestTypeResult> SortExitReceiptRequestTypeResult(
        IQueryable<ExitReceiptRequestTypeResult> query,
        SortInput<ExitReceiptRequestTypeSortType> sort)
    {
      switch (sort.SortType)
      {
        case ExitReceiptRequestTypeSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ExitReceiptRequestTypeSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        case ExitReceiptRequestTypeSortType.IsActive:
          return query.OrderBy(x => x.IsActive);
        case ExitReceiptRequestTypeSortType.AutoConfirm:
          return query.OrderBy(x => x.AutoConfirm);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<ExitReceiptRequestTypeResult> SearchExitReceiptRequestTypeResult(
        IQueryable<ExitReceiptRequestTypeResult> query,
        string search,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where item.Title.Contains(search)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<ExitReceiptRequestType, ExitReceiptRequestTypeResult>> ToExitReceiptRequestTypeResult =
        exitReceiptRequestType => new ExitReceiptRequestTypeResult
        {
          Id = exitReceiptRequestType.Id,
          Title = exitReceiptRequestType.Title,
          AutoConfirm = exitReceiptRequestType.AutoConfirm,
          IsActive = exitReceiptRequestType.IsActive,
          RowVersion = exitReceiptRequestType.RowVersion
        };
    public Expression<Func<ExitReceiptRequestType, ExitReceiptRequestTypeComboResult>> ToExitReceiptRequestTypeComboResult =
        exitReceiptRequestType => new ExitReceiptRequestTypeComboResult
        {
          Id = exitReceiptRequestType.Id,
          Name = exitReceiptRequestType.Title,
          RowVersion = exitReceiptRequestType.RowVersion
        };
    #endregion
  }
}
