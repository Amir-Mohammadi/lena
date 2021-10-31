using System;
using System.Linq;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Planning.OperatorType;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public void DeleteOperatorType(short id)
    {
      DeleteWorkStation(id);
    }
    public IQueryable<OperatorType> GetOperatorTypes(TValue<int> id = null, TValue<string> name = null, TValue<int> workStationId = null, TValue<string> description = null, TValue<int?> machineTypeId = null)
    {
      var query = GetWorkStationParts(id: id, name: name, workStationId: workStationId, description: description);
      var result = query.OfType<OperatorType>();
      if (machineTypeId != null && machineTypeId.Value != null)
        result = result.Where(r => r.MachineTypeOperatorTypes.Any(i => i.MachineTypeId == machineTypeId.Value));
      return result;
    }
    public OperatorType GetOperatorType(int id)
    {
      var @operatorType = GetOperatorTypes(id: id).FirstOrDefault();
      if (@operatorType == null)
        throw new OperatorTypeNotFoundException(id: id);
      return @operatorType;
    }
    public OperatorType AddOperatorType(string name, short workStationId, string description)
    {
      var @operatorType = repository.Create<OperatorType>();
      AddWorkStationPart(
                workStationPart: @operatorType,
                name: name,
                workStationId: workStationId,
                description: description);
      return @operatorType;
    }
    public OperatorType EditOperatorType(
        byte[] rowVersion,
        int id,
        TValue<string> name = null,
        TValue<short> workStationId = null,
        TValue<string> description = null)
    {
      var op = GetOperatorType(id: id);
      EditWorkStationPart(
                workStationPart: op,
                rowVersion: rowVersion,
                name: name,
                description: description,
                workStationId: workStationId);
      return op;
    }
    public OperatorTypeResult ToOperatorTypeResult(OperatorType operatorType)
    {
      return new OperatorTypeResult
      {
        Id = operatorType.Id,
        Name = operatorType.Name,
        ProductionLineId = operatorType.WorkStation.ProductionLineId,
        ProductionLineName = operatorType.WorkStation.ProductionLine.Name,
        DepartmentId = operatorType.WorkStation.ProductionLine.DepartmentId,
        DepartmentName = operatorType.WorkStation.ProductionLine.Department.Name,
        Description = operatorType.Description,
        WorkStationId = operatorType.WorkStationId,
        WorkStationName = operatorType.WorkStation.Name,
        WorkStationPartType = WorkStationPartType.OperatorType,
        RowVersion = operatorType.RowVersion,
      };
    }
    public IQueryable<OperatorTypeResult> ToOperatorTypeResultQuery(IQueryable<OperatorType> query)
    {
      return from operatorType in query
             let workstation = operatorType.WorkStation
             let productionLine = workstation.ProductionLine
             let department = productionLine.Department
             select new OperatorTypeResult
             {
               Id = operatorType.Id,
               Name = operatorType.Name,
               ProductionLineId = productionLine.Id,
               ProductionLineName = productionLine.Name,
               Description = operatorType.Description,
               DepartmentId = department.Id,
               DepartmentName = department.Name,
               WorkStationId = operatorType.WorkStationId,
               WorkStationName = workstation.Name,
               WorkStationPartType = WorkStationPartType.OperatorType,
               RowVersion = operatorType.RowVersion,
             };
    }
    public IQueryable<OperatorTypeComboResult> ToOperatorTypeComboResultQuery(IQueryable<OperatorType> query)
    {
      return from operatorType in query
             select new OperatorTypeComboResult
             {
               Id = operatorType.Id,
               Name = operatorType.Name,
             };
    }
    public IOrderedQueryable<OperatorTypeResult> SortOperatorTypeResult(IQueryable<OperatorTypeResult> query, SortInput<OperatorTypeSortType> options)
    {
      switch (options.SortType)
      {
        case OperatorTypeSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, options.SortOrder);
        case OperatorTypeSortType.Description:
          return query.OrderBy(a => a.Description, options.SortOrder);
        case OperatorTypeSortType.WorkStationName:
          return query.OrderBy(a => a.WorkStationName, options.SortOrder);
        case OperatorTypeSortType.Name:
          return query.OrderBy(a => a.Name, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}