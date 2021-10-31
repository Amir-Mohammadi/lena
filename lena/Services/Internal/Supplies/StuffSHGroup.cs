using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models;
using lena.Models.ApplicationBase.StuffHSGroup;
using lena.Models.Common;
using lena.Models.StuffHSGroup;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public StuffHSGroup AddStuffHSGroup(
        string code,
        string title,
        string description)
    {

      if (string.IsNullOrEmpty(code))
        throw new ArgumentNullException(code);
      var StuffHSGroup = repository.Create<StuffHSGroup>();
      StuffHSGroup.Code = code;
      StuffHSGroup.Title = title;
      StuffHSGroup.Description = description;
      repository.Add(StuffHSGroup);
      return StuffHSGroup;
    }
    #endregion
    #region Edit
    public StuffHSGroup EditStuffHSGroup(
        int id,
        byte[] rowVersion,
        TValue<string> code = null,
         TValue<string> title = null,
        TValue<string> description = null
    )
    {

      StuffHSGroup StuffHSGroup = GetStuffHSGroup(id: id);
      if (StuffHSGroup == null)
        throw new StuffHSGroupNotFoundException(id);
      if (code != null)
        StuffHSGroup.Code = code;
      if (title != null)
        StuffHSGroup.Title = title;
      if (description != null)
        StuffHSGroup.Description = description;
      repository.Update(entity: StuffHSGroup, rowVersion: StuffHSGroup.RowVersion);
      return StuffHSGroup;
    }
    #endregion
    #region Delete
    public void DeleteStuffHSGroup(int id)
    {

      var StuffHSGroup = GetStuffHSGroup(id: id);
      if (StuffHSGroup == null)
        throw new StuffHSGroupNotFoundException(id);
      repository.Delete(StuffHSGroup);
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStuffHSGroups<TResult>(
        Expression<Func<StuffHSGroup, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
          TValue<string> title = null,
        TValue<string> description = null
    )
    {

      var query = repository.GetQuery<StuffHSGroup>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (code != null)
        query = query.Where(i => i.Code == code);
      if (title != null)
        query = query.Where(i => i.Title == title);
      if (description != null)
        query = query.Where(i => i.Description == description);
      return query.Select(selector);
    }


    #endregion
    #region Get
    public StuffHSGroup GetStuffHSGroup(int id) => GetStuffHSGroup(selector: e => e, id: id);
    public TResult GetStuffHSGroup<TResult>(
        Expression<Func<StuffHSGroup, TResult>> selector,
        int id)
    {

      var StuffHSGroup = GetStuffHSGroups(
                selector: selector,
                id: id)


                .FirstOrDefault();
      if (StuffHSGroup == null)
        throw new StuffHSGroupNotFoundException(id);
      return StuffHSGroup;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StuffHSGroupResult> SortStuffHSGroupResult(IQueryable<StuffHSGroupResult> input,
        SortInput<StuffHSGroupSortType> options)
    {
      switch (options.SortType)
      {
        case StuffHSGroupSortType.Code:
          return input.OrderBy(i => i.Code, options.SortOrder);
        case StuffHSGroupSortType.Title:
          return input.OrderBy(i => i.Title, options.SortOrder);
        case StuffHSGroupSortType.Description:
          return input.OrderBy(i => i.Description, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    #endregion
    #region ToResult

    public Expression<Func<StuffHSGroup, StuffHSGroupResult>> ToStuffHSGroupResult =
        StuffHSGroup => new StuffHSGroupResult()
        {
          Id = StuffHSGroup.Id,
          Code = StuffHSGroup.Code,
          Title = StuffHSGroup.Title,
          Description = StuffHSGroup.Description,
          RowVersion = StuffHSGroup.RowVersion
        };
    #endregion
    #region Search
    public IQueryable<StuffHSGroupResult> SearchStuffHSGroupResult(
        IQueryable<StuffHSGroupResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(r =>
                r.Code.Contains(searchText) ||
                r.Title.Contains(searchText) ||
                r.Description.Contains(searchText));
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }

    #endregion
    #region ToComboResult
    public Expression<Func<StuffHSGroup, StuffHSGroupComboResult>> ToStuffHSGroupComboResult =
        StuffHSGroup => new StuffHSGroupComboResult()
        {
          Id = StuffHSGroup.Id,
          Code = StuffHSGroup.Code,
          Title = StuffHSGroup.Title,

        };
    #endregion
    #region SortCombo
    public IOrderedQueryable<StuffHSGroupComboResult> SortStuffHSGroupComboResult(
        IQueryable<StuffHSGroupComboResult> query, SortInput<StuffHSGroupComboSortType> type)
    {
      switch (type.SortType)
      {
        case StuffHSGroupComboSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case StuffHSGroupComboSortType.Code:
          return query.OrderBy(a => a.Code, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}
