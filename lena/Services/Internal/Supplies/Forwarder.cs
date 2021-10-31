using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Supplies.Forwarder;
using lena.Models.Common;
using lena.Services.Internals.Supplies.Exception;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public Forwarder AddForwarder(
               string name,
               bool isActive)
    {

      var forwarder = repository.Create<Forwarder>();
      forwarder.Name = name;
      forwarder.IsActive = isActive;
      repository.Add(forwarder);
      return forwarder;
    }
    #endregion

    #region Get
    public Forwarder GetForwarder(int id) => GetForwarder(selector: e => e, id: id);
    public TResult GetForwarder<TResult>(
        Expression<Func<Forwarder, TResult>> selector,
        int id)
    {

      var result = GetForwarders(
                selector: selector,
                id: id).FirstOrDefault();
      if (result == null)
        throw new ForwarderNotFoundException(id);
      return result;
    }


    #endregion
    #region Gets
    public IQueryable<TResult> GetForwarders<TResult>(
        Expression<Func<Forwarder, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<bool> isActive = null
        )
    {

      var query = repository.GetQuery<Forwarder>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (name != null)
        query = query.Where(i => i.Name == name);
      if (isActive != null)
        query = query.Where(i => i.IsActive == isActive);

      return query.Select(selector);
    }
    #endregion

    #region Remove Forwarder
    public void RemoveForwarder(int id, byte[] rowVersion)
    {

      var forwarder = GetForwarder(id: id);

    }
    #endregion

    #region Delete Forwarder
    public void DeleteForwarder(
        int id,
        byte[] rowVersion)
    {

      #region RemoveForwarder
      RemoveForwarder(
              id: id,
              rowVersion: rowVersion);
      #endregion

    }
    #endregion


    #region EditProcess

    public Forwarder EditForwarder(
        int id,
        byte[] rowVersion,
        TValue<string> name = null,
        TValue<bool> isActive = null)
    {

      var forwarder = GetForwarder(id: id);
      if (name != null)
        forwarder.Name = name;
      if (isActive != null)
        forwarder.IsActive = isActive;
      repository.Update(forwarder, rowVersion);
      return forwarder;
    }
    #endregion

    #region IsActiveChgange

    public Forwarder ActiveChangeForwarder(
        int id,
        byte[] rowVersion,
        TValue<bool> isActive = null)
    {

      var forwarder = GetForwarder(id: id);
      if (isActive != null)
        forwarder.IsActive = isActive;
      repository.Update(forwarder, rowVersion);
      return forwarder;
    }
    #endregion
    #region Search
    public IQueryable<ForwarderResult> SearchForwarder(IQueryable<ForwarderResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems

        )
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Name.Contains(searchText));

      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region SortCombo
    public IOrderedQueryable<ForwarderComboResult> SortForwarderComboResult(
        IQueryable<ForwarderComboResult> query, SortInput<ForwarderSortType> type)
    {
      switch (type.SortType)
      {
        case ForwarderSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case ForwarderSortType.Name:
          return query.OrderBy(a => a.Name, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ForwarderResult> SortForwarderResult(IQueryable<ForwarderResult> query,
        SortInput<ForwarderSortType> sort)
    {
      switch (sort.SortType)
      {
        case ForwarderSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ForwarderSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToForwarderResult
    public Expression<Func<Forwarder, ForwarderResult>> ToForwarderResult =
        Forwarder => new ForwarderResult
        {
          Id = Forwarder.Id,
          Name = Forwarder.Name,
          RowVersion = Forwarder.RowVersion,
          IsActive = Forwarder.IsActive
        };
    #endregion
    #region ToForwarderComboResult
    public Expression<Func<Forwarder, ForwarderComboResult>> ToForwarderComboResult =
        Forwarder => new ForwarderComboResult
        {
          Id = Forwarder.Id,
          Name = Forwarder.Name,
          IsActive = Forwarder.IsActive

        };
    #endregion

  }
}
