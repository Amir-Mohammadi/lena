using System;
using System.Linq;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Planning.Machine;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public void DeleteMachine(int id)
    {
      repository.Delete(
                GetMachine(id: id));
    }
    public IQueryable<Machine> GetMachines(
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<int> machineTypeId = null,
        TValue<string> description = null,
        TValue<int[]> machineTypeIds = null)
    {
      var isIdNull = id == null;
      var isNameNull = name == null;
      var isMachineTypeId = machineTypeId == null;
      var isDescriptionNull = description == null;
      var machines = from item in repository.GetQuery<Machine>()
                     where (isIdNull || item.Id == id) &&
                                 (isNameNull || item.Name == name) &&
                                 (isMachineTypeId || item.MachineTypeId == machineTypeId) &&
                                 (isDescriptionNull || item.Description == description)
                     select item;
      if (machineTypeIds != null)
        machines = machines.Where(i => machineTypeIds.Value.Contains(i.MachineTypeId));
      return machines;
    }
    public Machine GetMachine(int id)
    {
      var machine = GetMachines(id: id);
      if (machine == null)
        throw new MachineNotFoundException(id);
      return machine;
    }
    public Machine AddMachine(
        string name,
        short machineTypeId,
        string description)
    {
      var machine = repository.Create<Machine>();
      machine.Name = name;
      machine.MachineTypeId = machineTypeId;
      machine.Description = description;
      repository.Add(machine);
      return machine;
    }
    public Machine EditMachine(
        int id,
        byte[] rowVersion,
        TValue<string> name = null,
        TValue<short> machineTypeId = null,
        TValue<string> description = null)
    {
      var machine = GetMachine(id);
      return EditMachine(
                    machine: machine,
                    rowVersion: rowVersion,
                    name: name,
                    machineTypeId: machineTypeId,
                    description: description);
    }
    public Machine EditMachine(
        Machine machine,
        byte[] rowVersion,
        TValue<string> name = null,
        TValue<short> machineTypeId = null,
        TValue<string> description = null)
    {
      if (name != null)
        machine.Name = name;
      if (machineTypeId != null)
        machine.MachineTypeId = machineTypeId;
      if (description != null)
        machine.Description = description;
      repository.Update(entity: machine, rowVersion: rowVersion);
      return machine;
    }
    public MachineResult ToMachineResult(Machine machine)
    {
      return new MachineResult
      {
        Id = machine.Id,
        Name = machine.Name,
        Description = machine.Description,
        MachineTypeId = machine.MachineTypeId,
        MachineTypeName = machine.MachineType.Name,
        RowVersion = machine.RowVersion
      };
    }
    public MachineComboResult ToMachineComboResult(Machine machine)
    {
      return new MachineComboResult
      {
        Id = machine.Id,
        Name = machine.Name
      };
    }
    public IQueryable<MachineComboResult> ToMachineComboResultQuery(IQueryable<Machine> query)
    {
      return from machine in query
             select new MachineComboResult
             {
               Id = machine.Id,
               Name = machine.Name,
             };
    }
    public IQueryable<MachineResult> ToMachineResultQuery(IQueryable<Machine> query)
    {
      return from machine in query
             let machineType = machine.MachineType
             select new MachineResult
             {
               Id = machine.Id,
               Name = machine.Name,
               Description = machine.Description,
               MachineTypeId = machine.MachineTypeId,
               MachineTypeName = machineType.Name,
               RowVersion = machine.RowVersion,
             };
    }
    public IOrderedQueryable<MachineResult> SortMachineResult(IQueryable<MachineResult> query, SortInput<MachineSortType> options)
    {
      switch (options.SortType)
      {
        case MachineSortType.Id:
          return query.OrderBy(a => a.Id, options.SortOrder);
        case MachineSortType.Name:
          return query.OrderBy(a => a.Name, options.SortOrder);
        case MachineSortType.MachineTypeName:
          return query.OrderBy(a => a.MachineTypeName, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<MachineResult> SearchMachine(
        IQueryable<MachineResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems,
        int? machineTypeId = null
        )
    {
      if (!string.IsNullOrEmpty(searchText))
      {
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                item.Name.Contains(searchText) ||
                item.MachineTypeName.Contains(searchText) ||
                item.Description.Contains(searchText)
                select item;
      }
      if (machineTypeId.HasValue)
      {
        query = from item in query
                where (item.MachineTypeId == machineTypeId)
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