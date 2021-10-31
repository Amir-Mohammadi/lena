using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.LadingCustomhouseStatus;
//using System.Data.Entity.SqlServer;
//using System.Data.Entity;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public LadingCustomhouseStatus AddLadingCustomhouseStatus(
        string code,
        string name)
    {
      var ladingCustomhouseStatus = repository.Create<LadingCustomhouseStatus>();
      ladingCustomhouseStatus.Code = code.ToUpper();
      ladingCustomhouseStatus.Name = name;
      repository.Add(ladingCustomhouseStatus);
      return ladingCustomhouseStatus;

    }
    #endregion
    public LadingCustomhouseStatus EditLadingCustomhouseStatus(
       int ladingCustomhouseStatusId,
       byte[] rowVersion,
       TValue<string> code = null,
       TValue<string> name = null)
    {
      var ladingCustomhouseStatus = GetLadingCustomhouseStatus(ladingCustomhouseStatusId);
      if (code != null)
        ladingCustomhouseStatus.Code = code.Value.ToUpper();
      if (name != null)
        ladingCustomhouseStatus.Name = name;
      repository.Update(ladingCustomhouseStatus, rowVersion);
      return ladingCustomhouseStatus;
    }
    #region Gets
    public IQueryable<TResult> GetLadingCustomhouseStatuses<TResult>(
        Expression<Func<LadingCustomhouseStatus, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<string> name = null
        )
    {
      var query = repository.GetQuery<LadingCustomhouseStatus>();
      if (code != null)
        query = query.Where(a => a.Code == code);
      if (name != null)
        query = query.Where(a => a.Name == name);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public LadingCustomhouseStatus GetLadingCustomhouseStatus(int id) => GetLadingCustomhouseStatus(selector: e => e, id: id);
    public TResult GetLadingCustomhouseStatus<TResult>(
           Expression<Func<LadingCustomhouseStatus, TResult>> selector,
        int id)
    {
      var ladingCustomhouseStatus = GetLadingCustomhouseStatuses(selector: selector, id: id)
            .FirstOrDefault();
      if (ladingCustomhouseStatus == null)
        throw new LadingCustomhouseStatusNotFoundException(id);
      return ladingCustomhouseStatus;
    }
    #endregion
    #region ToResult
    public Expression<Func<LadingCustomhouseStatus, LadingCustomhouseStatusResult>> ToLadingCustomhouseStatusResult =
        (ladingCustomhouseStatus) => new LadingCustomhouseStatusResult
        {
          Id = ladingCustomhouseStatus.Id,
          Code = ladingCustomhouseStatus.Code,
          Name = ladingCustomhouseStatus.Name,
          RowVersion = ladingCustomhouseStatus.RowVersion
        };
    #endregion
    #region Search
    public IQueryable<LadingCustomhouseStatusResult> SearchLadingCustomhouseStatusesResults(
        IQueryable<LadingCustomhouseStatusResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems
        )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = query.Where(i => i.Code.Contains(searchText) || i.Name.Contains(searchText));
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    public IQueryable<LadingCustomhouseStatusResult> SortLadingCustomhouseStatusResult(IQueryable<LadingCustomhouseStatusResult> query,
        SortInput<LadingCustomhouseStatusSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case LadingCustomhouseStatusSortType.Id:
          query = query.OrderBy(x => x.Id, sortInput.SortOrder);
          break;
        case LadingCustomhouseStatusSortType.Code:
          query = query.OrderBy(x => x.Code, sortInput.SortOrder);
          break;
        case LadingCustomhouseStatusSortType.Name:
          query = query.OrderBy(x => x.Name, sortInput.SortOrder);
          break;
        default:
          query = query.OrderBy(x => x.Id, sortInput.SortOrder);
          break;
      }
      return query;
    }
    #region Delete
    public void DeleteLadingCustomhouseStatus(int id)
    {
      var ladingCustomhouseStatus = GetLadingCustomhouseStatus(id);
      if (ladingCustomhouseStatus.LadingCustomhouseLogs.Any())
      {
        throw new CannotDeleteLadingCustomhouseStatusHasBeenUsedInLogs(ladingCustomhouseStatus.Id, ladingCustomhouseStatus.Name);
      }
      repository.Delete(ladingCustomhouseStatus);
    }
    #endregion
  }
}