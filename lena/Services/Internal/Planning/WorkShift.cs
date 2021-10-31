using System;
using System.Linq;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Planning.WorkShift;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public IQueryable<WorkShift> GetWorkShifts(TValue<int> id = null, TValue<string> name = null)
    {

      var isIdNull = id == null;
      var isTitleNull = name == null;
      var workShifts = from item in repository.GetQuery<WorkShift>()
                       where (isIdNull || item.Id == id) &&
                                   (isTitleNull || item.Name == name)
                       select item;
      return workShifts;
    }
    public WorkShift GetWorkShift(int id)
    {

      var workShift = GetWorkShifts(id: id).SingleOrDefault();
      if (workShift == null)
        throw new WorkShiftNotFoundException(id);
      return workShift;
    }
    public WorkShift AddWorkShift(string name)
    {

      var workShift = repository.Create<WorkShift>();
      workShift.Name = name;
      repository.Add(workShift);
      return workShift;
    }
    public WorkShift EditWorkShift(byte[] rowVersion, int id, TValue<string> name = null)
    {

      var workShift = GetWorkShift(id);
      if (workShift == null)
        throw new WorkShiftNotFoundException(id);
      if (name != null)
        workShift.Name = name;
      repository.Update(workShift, rowVersion: rowVersion);
      return workShift;
    }
    public void DeleteWorkShift(int id)
    {

      var workShift = GetWorkShift(id: id);
      if (workShift == null)
        throw new WorkShiftNotFoundException(id);
      repository.Delete(workShift);
    }
    public IQueryable<WorkShiftResult> ToWorkShiftResultQuery(IQueryable<WorkShift> query)
    {
      var resultQuery = from workShift in query
                        select new WorkShiftResult()
                        {
                          Id = workShift.Id,
                          Name = workShift.Name,
                          RowVersion = workShift.RowVersion
                        };
      return resultQuery;
    }
    public WorkShiftResult ToWorkShiftResult(WorkShift workShift)
    {
      var result = new WorkShiftResult()
      {
        Id = workShift.Id,
        Name = workShift.Name,
        RowVersion = workShift.RowVersion
      };
      return result;
    }
    public IOrderedQueryable<WorkShiftResult> SortWorkShiftResult(IQueryable<WorkShiftResult> input, SortInput<WorkShiftSortType> options)
    {
      switch (options.SortType)
      {
        case WorkShiftSortType.Id:
          return input.OrderBy(i => i.Id, options.SortOrder);
        case WorkShiftSortType.Name:
          return input.OrderBy(i => i.Name, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public IQueryable<WorkShiftResult> SearchWorkShiftResultQuery(
        IQueryable<WorkShiftResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from workShift in query
                where workShift.Name.Contains(searchText)
                select workShift;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }

    public IQueryable<WorkShiftComboResult> ToWorkShiftComboResult(IQueryable<WorkShift> query)
    {
      var result = from workShift in query
                   select new WorkShiftComboResult()
                   {
                     Id = workShift.Id,
                     Name = workShift.Name
                   };
      return result;
    }
  }
}
