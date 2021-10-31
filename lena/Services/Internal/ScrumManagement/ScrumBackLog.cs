using lena.Models.Common;
using lena.Domains;
using System;
using System.Linq;
using lena.Models;
using lena.Services.Common;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ScrumManagement.Exception;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.ScrumManagement.ScrumBackLog;
using System.Linq.Expressions;
using lena.Models.ScrumManagement.ScrumTask;
using lena.Services.Core;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement
{
  public partial class ScrumManagement
  {
    public ScrumBackLog AddScrumBackLog(ScrumBackLog scrumBackLog,
        string name,
        string description,
        string color,
        short departmentId,
        long estimatedTime,
        bool isCommit,
        int scrumSprintId,
        int? baseEntityId)
    {


      scrumBackLog = scrumBackLog ?? repository.Create<ScrumBackLog>();
      scrumBackLog.ScrumSprintId = scrumSprintId;
      var retValue = AddScrumEntity(scrumEntity: scrumBackLog,
                name: name,
                description: description,
                color: color,
                departmentId: departmentId,
                estimatedTime: estimatedTime,
                isCommit: isCommit,
                baseEntityId: baseEntityId
                );
      return retValue as ScrumBackLog;
    }
    public ScrumBackLog EditScrumBackLog(byte[] rowVersion,
        int id,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<int> scrumSprintId = null
        )
    {

      var scrumBackLog = GetScrumBackLog(id: id);
      return
                EditScrumBackLog(
                        rowVersion: rowVersion,
                        scrumBackLog: scrumBackLog,
                        code: code,
                        name: name,
                        description: description,
                        color: color,
                        departmentId: departmentId,
                        estimatedTime: estimatedTime,
                        isCommit: isCommit,
                        isDelete: isDelete,
                        scrumSprintId: scrumSprintId);
    }
    public ScrumBackLog EditScrumBackLog(
        byte[] rowVersion,
        ScrumBackLog scrumBackLog,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<int> scrumSprintId = null)
    {


      if (scrumSprintId != null)
        scrumBackLog.ScrumSprintId = scrumSprintId;
      var retValue = EditScrumEntity(rowVersion: rowVersion,
                    scrumEntity: scrumBackLog,
                    code: code,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    isDelete: isDelete);
      return retValue as ScrumBackLog;
    }

    public IQueryable<TResult> GetScrumBackLogs<TResult>(
        Expression<Func<ScrumBackLog, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<int> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<bool> isArchive = null,
        TValue<int> scrumSprintId = null,
        TValue<int?> baseEntityId = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null,
        TValue<int> scrumTaskTypeId = null)
    {

      var isScrumSprintIdNull = scrumSprintId == null;
      var baseQuery = GetScrumEntities(
                    id: id,
                    code: code,
                    name: name,
                    description: description,
                    color: color,
                    isCommit: isCommit,
                    estimatedTime: estimatedTime,
                    departmentId: departmentId,
                    isDelete: isDelete,
                    isArchive: isArchive,
                    baseEntityId: baseEntityId);
      var query = baseQuery.OfType<ScrumBackLog>();
      if (scrumTaskTypeId != null)
        query = query.Where(i => i.ScrumTasks.Any(s => s.ScrumTaskTypeId == scrumTaskTypeId));
      if (fromDate != null)
        query = query.Where(i => i.DateTime >= fromDate);
      if (toDate != null)
        query = query.Where(i => i.DateTime <= toDate);
      return query.Select(selector);
    }

    public ScrumBackLog GetScrumBackLog(int id) => GetScrumBackLog(selector: e => e, id: id);

    public TResult GetScrumBackLog<TResult>(
        Expression<Func<ScrumBackLog, TResult>> selector,
        int id)
    {

      var scrumBackLog = GetScrumBackLogs(selector: selector, id: id).SingleOrDefault();
      if (scrumBackLog == null)
        throw new ScrumBackLogNotFoundException(id);
      return scrumBackLog;
    }

    public void DeleteScrumBackLog(int id)
    {

      var scrumBackLog = GetScrumBackLog(id);
      repository.Delete(scrumBackLog);
    }

    public IQueryable<ScrumBackLogResult> SearchScrumBackLogResult(IQueryable<ScrumBackLogResult> query, string search)
    {
      if (string.IsNullOrEmpty(search))
        return query;
      return from item in query
             where
             item.Code.Contains(search) ||
             item.Name.Contains(search) ||
             item.Description.Contains(search) ||
             item.DepartmentName.Contains(search)
             select item;
    }

    public IOrderedQueryable<ScrumBackLogResult> SortScrumBackLogResult(IQueryable<ScrumBackLogResult> query, SortInput<ScrumBackLogSortType> sort)
    {
      switch (sort.SortType)
      {
        case ScrumBackLogSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ScrumBackLogSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        case ScrumBackLogSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);
        case ScrumBackLogSortType.EstimatedTime:
          return query.OrderBy(a => a.EstimatedTime, sort.SortOrder);
        case ScrumBackLogSortType.ScrumSprintName:
          return query.OrderBy(a => a.ScrumSprintName, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public IQueryable<ScrumBackLogResult> ToScrumBackLogResultQuery(IQueryable<ScrumBackLog> query)
    {
      var resultQuery = from scrumBackLog in query
                        let department = scrumBackLog.Department
                        let scrumSprint = scrumBackLog.ScrumSprint
                        select new ScrumBackLogResult
                        {
                          Id = scrumBackLog.Id,
                          Code = scrumBackLog.Code,
                          Name = scrumBackLog.Name,
                          Description = scrumBackLog.Description,
                          Color = scrumBackLog.Color,
                          IsCommit = scrumBackLog.IsCommit,
                          IsDelete = scrumBackLog.IsDelete,
                          DepartmentId = department.Id,
                          DepartmentName = department.Name,
                          ScrumSprintId = scrumBackLog.ScrumSprintId,
                          ScrumSprintName = scrumSprint.Name,
                          EstimatedTime = scrumBackLog.EstimatedTime,
                          RowVersion = scrumBackLog.RowVersion,
                        };
      return resultQuery;
    }

    public Expression<Func<ScrumBackLog, ScrumBackLogResult>> ToScrumBackLogResult =
                           scrumBackLog => new ScrumBackLogResult
                           {
                             Id = scrumBackLog.Id,
                             Code = scrumBackLog.Code,
                             Name = scrumBackLog.Name,
                             Description = scrumBackLog.Description,
                             Color = scrumBackLog.Color,
                             IsCommit = scrumBackLog.IsCommit,
                             IsDelete = scrumBackLog.IsDelete,
                             DepartmentId = scrumBackLog.Department.Id,
                             DepartmentName = scrumBackLog.Department.Name,
                             EstimatedTime = scrumBackLog.EstimatedTime,
                             ScrumSprintId = scrumBackLog.ScrumSprint.Id,
                             ScrumSprintName = scrumBackLog.ScrumSprint.Name,
                             RowVersion = scrumBackLog.RowVersion,
                             DateTime = scrumBackLog.DateTime,
                           };

    public ScrumBackLog ArchiveScrumBackLog(byte[] rowVersion, int id)
    {
      return EditScrumBackLog(id: id, rowVersion: rowVersion, isDelete: true);
    }

    public ScrumBackLog RestoreScrumBackLog(byte[] rowVersion, int id)
    {
      return EditScrumBackLog(id: id, rowVersion: rowVersion, isDelete: false);
    }
  }
}
