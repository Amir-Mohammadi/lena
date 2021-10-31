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
using lena.Models.Planning.WorkStationPart;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public void DeleteWorkStationPart(int id)
    {
      repository.Delete(
                GetWorkStationPart(id: id));
    }
    public IQueryable<WorkStationPart> GetWorkStationParts(TValue<int> id = null, TValue<string> name = null, TValue<int> workStationId = null, TValue<string> description = null)
    {
      var isIdNull = id == null;
      var isNameNull = name == null;
      var isWorkStationId = workStationId == null;
      var isDescriptionNull = description == null;
      var workStationparts = from item in repository.GetQuery<WorkStationPart>()
                             where (isIdNull || item.Id == id) &&
                                         (isNameNull || item.Name == name) &&
                                         (isWorkStationId || item.WorkStationId == workStationId) &&
                                         (isDescriptionNull || item.Description == description)
                             select item;
      return workStationparts;
    }
    public WorkStationPart GetWorkStationPart(int id)
    {
      var workStationPart = GetWorkStationParts(id: id);
      if (workStationPart == null)
        throw new WorkStationPartNotFoundException();
      return workStationPart;
    }
    public WorkStationPart AddWorkStationPart(
        WorkStationPart workStationPart,
        string name,
        short workStationId,
        string description)
    {
      workStationPart.Name = name;
      workStationPart.WorkStationId = workStationId;
      workStationPart.Description = description;
      repository.Add(workStationPart);
      return workStationPart;
    }
    public WorkStationPart EditWorkStationPart(
        WorkStationPart workStationPart,
        byte[] rowVersion,
        TValue<string> name = null,
        TValue<short> workStationId = null,
        TValue<string> description = null)
    {
      if (name != null)
        workStationPart.Name = name;
      if (workStationId != null)
        workStationPart.WorkStationId = workStationId;
      if (description != null)
        workStationPart.Description = description;
      repository.Update(entity: workStationPart, rowVersion: rowVersion);
      return workStationPart;
    }
    public WorkStationPartResult ToWorkStationPartResult(WorkStationPart workStationPart)
    {
      return new WorkStationPartResult
      {
        ProductionLineId = workStationPart.WorkStation.ProductionLineId,
        ProductionLineName = workStationPart.WorkStation.ProductionLine.Name,
        DepartmentId = workStationPart.WorkStation.ProductionLine.DepartmentId,
        DepartmentName = workStationPart.WorkStation.ProductionLine.Department.Name,
        Description = workStationPart.Description,
        WorkStationId = workStationPart.WorkStationId,
        WorkStationName = workStationPart.WorkStation.Name,
        Name = workStationPart.Name,
        RowVersion = workStationPart.RowVersion,
        Id = workStationPart.Id,
        WorkStationPartType = workStationPart is MachineType ? WorkStationPartType.MachineType : WorkStationPartType.OperatorType
      };
    }
    public WorkStationPartComboResult ToWorkStationPartComboResult(WorkStationPart workStationPart)
    {
      return new WorkStationPartComboResult
      {
        Id = workStationPart.Id,
        Name = workStationPart.Name
      };
    }
    public IQueryable<WorkStationPartComboResult> ToWorkStationPartComboResultQuery(IQueryable<WorkStationPart> query)
    {
      return from workStationPart in query
             select new WorkStationPartComboResult
             {
               Id = workStationPart.Id,
               Name = workStationPart.Name,
             };
    }
    public IQueryable<WorkStationPartResult> ToWorkStationPartResultQuery(IQueryable<WorkStationPart> query)
    {
      return from workStationPart in query
             let workStation = workStationPart.WorkStation
             let productionLine = workStation.ProductionLine
             let department = productionLine.Department
             let isMachineType = workStationPart is MachineType
             let isOperatorType = workStationPart is OperatorType
             select new WorkStationPartResult
             {
               DepartmentId = department.Id,
               DepartmentName = department.Name,
               Description = workStationPart.Description,
               WorkStationId = workStationPart.WorkStationId,
               WorkStationName = workStation.Name,
               ProductionLineId = workStation.ProductionLineId,
               ProductionLineName = productionLine.Name,
               Id = workStationPart.Id,
               Name = workStationPart.Name,
               WorkStationPartType = isMachineType ? WorkStationPartType.MachineType : WorkStationPartType.OperatorType,
               RowVersion = workStationPart.RowVersion,
             };
    }
    public IOrderedQueryable<WorkStationPartResult> SortWorkStationPartResult(IQueryable<WorkStationPartResult> query, SortInput<WorkStationPartSortType> options)
    {
      switch (options.SortType)
      {
        case WorkStationPartSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, options.SortOrder);
        case WorkStationPartSortType.ProductionLineName:
          return query.OrderBy(a => a.ProductionLineName, options.SortOrder);
        case WorkStationPartSortType.WorkStationName:
          return query.OrderBy(a => a.WorkStationName, options.SortOrder);
        case WorkStationPartSortType.Name:
          return query.OrderBy(a => a.Name, options.SortOrder);
        case WorkStationPartSortType.WorkStationPartType:
          return query.OrderBy(a => a.WorkStationPartType, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<WorkStationPartResult> SearchWorkStationPart(
        IQueryable<WorkStationPartResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems,
       WorkStationPartType? workStationPartType = null
       )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Name.Contains(searchText) ||
                item.Description.Contains(searchText) ||
                item.ProductionLineName.Contains(searchText) ||
                item.WorkStationName.Contains(searchText) ||
                item.DepartmentName.Contains(searchText)
                select item;
      }
      if (workStationPartType != null)
      {
        query = from workStationPart in query
                where workStationPart.WorkStationPartType == workStationPartType
                select workStationPart;
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
  }
}