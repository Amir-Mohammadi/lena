using System;
using System.Linq;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Planning.Machine;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public void DeleteMachineType(short id)
    {
      DeleteWorkStation(id);
    }
    public IQueryable<MachineType> GetMachineTypes(TValue<int> id = null, TValue<string> name = null, TValue<int> workStationId = null, TValue<string> description = null)
    {
      var query = GetWorkStationParts(id: id, name: name, workStationId: workStationId, description: description);
      var result = query.OfType<MachineType>();
      return result;
    }
    public MachineType GetMachineType(int id)
    {
      var machineType = GetMachineTypes(id: id).FirstOrDefault();
      if (machineType == null)
        throw new MachineTypeNotFoundException(id: id);
      return machineType;
    }
    public MachineType AddMachineType(
        string name,
        short workStationId,
        string description)
    {
      var machineType = repository.Create<MachineType>();
      AddWorkStationPart(
                workStationPart: machineType,
                name: name,
                workStationId: workStationId,
                description: description);
      return machineType;
    }
    public MachineType EditMachineType(
        byte[] rowVersion,
        int id,
        TValue<string> name = null,
        TValue<short> workStationId = null,
        TValue<string> description = null)
    {
      var machineType = GetMachineType(id: id);
      EditWorkStationPart(
                workStationPart: machineType,
                rowVersion: rowVersion,
                name: name,
                description: description,
                workStationId: workStationId);
      return machineType;
    }
    public MachineTypeResult ToMachineTypeResult(MachineType machineType)
    {
      return new MachineTypeResult
      {
        Id = machineType.Id,
        Name = machineType.Name,
        ProductionLineId = machineType.WorkStation.ProductionLineId,
        ProductionLineName = machineType.WorkStation.ProductionLine.Name,
        DepartmentId = machineType.WorkStation.ProductionLine.DepartmentId,
        DepartmentName = machineType.WorkStation.ProductionLine.Department.Name,
        Description = machineType.Description,
        WorkStationId = machineType.WorkStationId,
        WorkStationName = machineType.WorkStation.Name,
        WorkStationPartType = WorkStationPartType.MachineType,
        RowVersion = machineType.RowVersion,
      };
    }
    public IQueryable<MachineTypeResult> ToMachineTypeResultQuery(IQueryable<MachineType> query)
    {
      return from machineType in query
             let workStation = machineType.WorkStation
             let productionLine = workStation.ProductionLine
             let department = productionLine.Department
             select new MachineTypeResult
             {
               Id = machineType.Id,
               Name = machineType.Name,
               Description = machineType.Description,
               DepartmentId = department.Id,
               DepartmentName = department.Name,
               ProductionLineId = workStation.ProductionLineId,
               ProductionLineName = productionLine.Name,
               WorkStationId = machineType.WorkStationId,
               WorkStationName = workStation.Name,
               WorkStationPartType = WorkStationPartType.MachineType,
               RowVersion = machineType.RowVersion,
             };
    }
    public IQueryable<MachineTypeComboResult> ToMachineTypeComboResultQuery(IQueryable<MachineType> query)
    {
      return from machineType in query
             let workStation = machineType.WorkStation
             select new MachineTypeComboResult
             {
               Id = machineType.Id,
               Name = machineType.Name,
               WorkStationId = machineType.WorkStationId
             };
    }
    public IOrderedQueryable<MachineTypeResult> SortMachineTypeResult(IQueryable<MachineTypeResult> query,
        SortInput<MachineTypeSortType> options)
    {
      switch (options.SortType)
      {
        case MachineTypeSortType.ProductionLineName:
          return query.OrderBy(a => a.ProductionLineName, options.SortOrder);
        case MachineTypeSortType.Description:
          return query.OrderBy(a => a.Description, options.SortOrder);
        case MachineTypeSortType.WorkStationName:
          return query.OrderBy(a => a.WorkStationName, options.SortOrder);
        case MachineTypeSortType.Name:
          return query.OrderBy(a => a.Name, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}