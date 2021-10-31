using System;
using System.Linq;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Planning.WorkStationOperation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public IQueryable<WorkStationOperation> GetWorkStationOperations(
        TValue<int> operationId = null,
        TValue<short> workStationId = null)
    {

      var query = repository.GetQuery<WorkStationOperation>();
      if (operationId != null)
        query = query.Where(x => x.OperationId == operationId.Value);
      if (workStationId != null)
        query = query.Where(x => x.WorkStationId == workStationId.Value);
      return query;
    }

    public IQueryable<CrossWorkStationOperationResult> GetCrossWorkStationOperations(
        TValue<int> operationId = null,
        TValue<short> workStationId = null,
        TValue<bool> isExist = null)
    {

      var workStations = GetWorkStations(id: workStationId);
      var operations = GetOperations(
                    selector: e => e,
                    id: operationId);
      var workStationOperations = GetWorkStationOperations(
                    operationId: operationId,
                    workStationId: workStationId);
      var query = from workStation in workStations
                  from operation in operations
                  join tWorkStationOperation in workStationOperations
                            on new
                            {
                              WorkStationId = workStation.Id,
                              operationId = operation.Id

                            } equals new
                            {
                              WorkStationId = tWorkStationOperation.WorkStationId,
                              operationId = tWorkStationOperation.OperationId
                            }
                        into tWorkStationOperations
                  from workStationOperation in tWorkStationOperations.DefaultIfEmpty()

                  select new CrossWorkStationOperationResult()
                  {
                    WorkStationId = workStation.Id,
                    WorkStationName = workStation.Name,
                    OperationId = operation.Id,
                    OperationCode = operation.Code,
                    OperationTitle = operation.Title,
                    IsExist = workStationOperation.WorkStationId == workStation.Id
                  };
      if (isExist != null)
        query = query.Where(i => i.IsExist == isExist);
      return query;
    }
    public WorkStationOperation GetWorkStationOperation(short workStationId, int operationId)
    {

      var workStationOperation = GetWorkStationOperations(
                operationId: operationId,
                workStationId: workStationId)

            .SingleOrDefault();
      if (workStationOperation == null)
        throw new WorkStationOperationNotFoundException(
                  workStationId: workStationId,
                  operationId: operationId);
      return workStationOperation;
    }

    public WorkStationOperation AddWorkStationOperation(
        short operationId,
        short workStationId)
    {

      var workStationOperation = repository.Create<WorkStationOperation>();
      workStationOperation.OperationId = operationId;
      workStationOperation.WorkStationId = workStationId;
      repository.Add(workStationOperation);
      return workStationOperation;
    }
    public void DeleteWorkStationOperation(short workStationId, int operationId)
    {

      var workStationOperation = GetWorkStationOperation(
                    workStationId: workStationId,
                    operationId: operationId);
      if (workStationOperation == null)
        throw new WorkStationOperationNotFoundException(
                  workStationId: workStationId,
                  operationId: operationId);
      repository.Delete(workStationOperation);
    }
    public IQueryable<WorkStationOperationResult> ToWorkStationOperationResultQuery(IQueryable<WorkStationOperation> query)
    {
      var resultQuery = from workStationOperation in query
                        select new WorkStationOperationResult()
                        {
                          OperationId = workStationOperation.OperationId,
                          OperationCode = workStationOperation.Operation.Code,
                          OperationTitle = workStationOperation.Operation.Title,
                          WorkStationId = workStationOperation.WorkStationId,
                          WorkStationName = workStationOperation.WorkStation.Name,
                          RowVersion = workStationOperation.RowVersion,
                        };
      return resultQuery;
    }


    public WorkStationOperationResult ToWorkStationOperationResult(WorkStationOperation workStationOperation)
    {

      var result = new WorkStationOperationResult()
      {
        OperationId = workStationOperation.OperationId,
        OperationCode = workStationOperation.Operation.Code,
        OperationTitle = workStationOperation.Operation.Title,
        WorkStationId = workStationOperation.WorkStationId,
        WorkStationName = workStationOperation.WorkStation.Name,
        RowVersion = workStationOperation.RowVersion,
      };
      return result;
    }
    public IOrderedQueryable<WorkStationOperationResult> SortWorkStationOperationResult(
        IQueryable<WorkStationOperationResult> input,
        SortInput<WorkStationOperationSortType> options)
    {
      switch (options.SortType)
      {
        case WorkStationOperationSortType.WorkStationName:
          return input.OrderBy(i => i.WorkStationName, options.SortOrder);
        case WorkStationOperationSortType.OperationCode:
          return input.OrderBy(i => i.OperationCode, options.SortOrder);
        case WorkStationOperationSortType.OperationTitle:
          return input.OrderBy(i => i.OperationTitle, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public IOrderedQueryable<CrossWorkStationOperationResult> SortCrossWorkStationOperationResult(
        IQueryable<CrossWorkStationOperationResult> input,
        SortInput<CrossWorkStationOperationSortType> options)
    {
      switch (options.SortType)
      {
        case CrossWorkStationOperationSortType.WorkStationName:
          return input.OrderBy(i => i.WorkStationName, options.SortOrder);
        case CrossWorkStationOperationSortType.OperationCode:
          return input.OrderBy(i => i.OperationCode, options.SortOrder);
        case CrossWorkStationOperationSortType.OperationTitle:
          return input.OrderBy(i => i.OperationTitle, options.SortOrder);
        case CrossWorkStationOperationSortType.IsExist:
          return input.OrderBy(i => i.IsExist, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public IQueryable<CrossWorkStationOperationResult> SearchCrossWorkStationOperation(
        IQueryable<CrossWorkStationOperationResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems
        )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.OperationCode.Contains(searchText) ||
                item.OperationTitle.Contains(searchText) ||
                item.WorkStationName.Contains(searchText)
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
