using System;
using System.Linq;
using lena.Services.Common;
using lena.Models.Common;
using lena.Domains;
using System.Linq.Expressions;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Internals.Supplies.Exception;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Supplies.EnactmentActionProcess;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public EnactmentActionProcess AddEnactmentActionProcess(
        string code,
        string name)
    {
      var enactmentActionProcess = repository.Create<EnactmentActionProcess>();
      enactmentActionProcess.Code = code.ToUpper();
      enactmentActionProcess.Name = name;
      repository.Add(enactmentActionProcess);
      return enactmentActionProcess;

    }
    #endregion
    #region Edit
    public EnactmentActionProcess EditEnactmentActionProcess(
       int EnactmentActionProcessId,
       byte[] rowVersion,
       TValue<string> code = null,
       TValue<string> name = null)
    {
      var enactmentActionProcess = GetEnactmentActionProcess(EnactmentActionProcessId);
      if (code != null)
        enactmentActionProcess.Code = code.Value.ToUpper();
      if (name != null)
        enactmentActionProcess.Name = name;
      repository.Update(enactmentActionProcess, rowVersion);
      return enactmentActionProcess;

    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetEnactmentActionProcesses<TResult>(
        Expression<Func<EnactmentActionProcess, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<string> name = null
        )
    {
      var query = repository.GetQuery<EnactmentActionProcess>();
      if (id != null)
        query = query.Where(a => a.Id == id);
      if (code != null)
        query = query.Where(a => a.Code == code);
      if (name != null)
        query = query.Where(a => a.Name == name);
      return query.Select(selector);
    }
    #endregion
    #region Get      
    public EnactmentActionProcess GetEnactmentActionProcess(int id) => GetEnactmentActionProcess(selector: e => e, id: id);
    public TResult GetEnactmentActionProcess<TResult>(
        Expression<Func<EnactmentActionProcess, TResult>> selector,
        int id)
    {
      var enactmentActionProcess = GetEnactmentActionProcesses(selector: selector, id: id)


            .FirstOrDefault();
      if (enactmentActionProcess == null)
        throw new EnactmentActionProcessNotFoundException(id);
      return enactmentActionProcess;
    }
    #endregion
    #region ToResult
    public Expression<Func<EnactmentActionProcess, EnactmentActionProcessResult>> ToEnactmentActionProcessResult =
        (EnactmentActionProcess) => new EnactmentActionProcessResult
        {
          Id = EnactmentActionProcess.Id,
          Code = EnactmentActionProcess.Code,
          Name = EnactmentActionProcess.Name,
          RowVersion = EnactmentActionProcess.RowVersion
        };
    #endregion
    #region Search
    public IQueryable<EnactmentActionProcessResult> SearchEnactmentActionProcessesResults(
        IQueryable<EnactmentActionProcessResult> query,
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
    #region Sort
    public IQueryable<EnactmentActionProcessResult> SortEnactmentActionProcessResult(IQueryable<EnactmentActionProcessResult> query,
        SortInput<EnactmentActionProcessSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case EnactmentActionProcessSortType.Id:
          query = query.OrderBy(x => x.Id, sortInput.SortOrder);
          break;
        case EnactmentActionProcessSortType.Code:
          query = query.OrderBy(x => x.Code, sortInput.SortOrder);
          break;
        case EnactmentActionProcessSortType.Name:
          query = query.OrderBy(x => x.Name, sortInput.SortOrder);
          break;
        default:
          query = query.OrderBy(x => x.Id, sortInput.SortOrder);
          break;
      }
      return query;
    }
    #endregion
    #region Delete
    public void DeleteEnactmentActionProcess(int id)
    {
      var enactmentActionProcess = GetEnactmentActionProcess(id);
      if (enactmentActionProcess.EnactmentActionProcessLogs.Any())
      {
        throw new CannotDeleteEnactmentActionProcessHasBeenUsedInLogs(enactmentActionProcess.Id, enactmentActionProcess.Name);
      }
      repository.Delete(enactmentActionProcess);
    }
    #endregion
  }
}