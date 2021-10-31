using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Supplies.Customhouse;
using lena.Services.Core.Exceptions;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {

    #region Get
    public Customhouse GetCustomhouse(int id) => GetCustomhouse(selector: e => e, id: id);
    public TResult GetCustomhouse<TResult>(
        Expression<Func<Customhouse, TResult>> selector,
        int id)
    {

      var customhouse = GetCustomhouses(selector: selector,
                id: id).FirstOrDefault();
      if (customhouse == null)
        throw new RecordNotFoundException(id, typeof(Customhouse));
      return customhouse;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetCustomhouses<TResult>(
        Expression<Func<Customhouse, TResult>> selector,
        TValue<int> id = null,
        TValue<string> title = null)
    {

      var query = repository.GetQuery<Customhouse>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (title != null)
        query = query.Where(x => x.Title == title);
      return query.Select(selector);
    }
    #endregion

    #region Add
    public Customhouse AddCustomhouse(
        string title)
    {

      var entity = repository.Create<Customhouse>();
      entity.Title = title;
      repository.Add(entity);
      return entity;
    }
    #endregion

    #region Edit
    public Customhouse EditCustomhouse(
        int id,
        byte[] rowVersion,
        TValue<string> title = null)
    {

      var Customhouse = GetCustomhouse(id: id);
      if (title != null)
        Customhouse.Title = title;
      repository.Update(rowVersion: rowVersion, entity: Customhouse);
      return Customhouse;
    }
    #endregion

    #region Delete
    public void DeleteCustomhouse(int id)
    {

      var Customhouse = GetCustomhouse(id: id);
      repository.Delete(Customhouse);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<CustomhouseResult> SortCustomhouseResult(
        IQueryable<CustomhouseResult> query,
        SortInput<CustomhouseSortType> sort)
    {
      switch (sort.SortType)
      {
        case CustomhouseSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case CustomhouseSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<CustomhouseResult> SearchCustomhouseResult(
        IQueryable<CustomhouseResult> query,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where item.Title.Contains(search)
                select item;
      return query;
    }
    #endregion

    #region ToResult
    public Expression<Func<Customhouse, CustomhouseResult>> ToCustomhouseResult =
        Customhouse => new CustomhouseResult
        {
          Id = Customhouse.Id,
          Title = Customhouse.Title,
          RowVersion = Customhouse.RowVersion
        };
    #endregion

    #region ToComboResult
    public Expression<Func<Customhouse, CustomhouseComboResult>> ToCustomhouseComboResult =
        Customhouse => new CustomhouseComboResult()
        {
          Id = Customhouse.Id,
          Title = Customhouse.Title,

        };
    #endregion
    #region SortCombo
    public IOrderedQueryable<CustomhouseComboResult> SortCustomhouseComboResult(
        IQueryable<CustomhouseComboResult> query, SortInput<CustomhouseComboSortType> type)
    {
      switch (type.SortType)
      {
        case CustomhouseComboSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case CustomhouseComboSortType.Title:
          return query.OrderBy(a => a.Title, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

  }
}
