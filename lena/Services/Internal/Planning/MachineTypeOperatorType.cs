using System;
using System.Linq;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Planning.MachineTypeOperatorType;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public void DeleteMachineTypeOperatorType(int id)
    {
      var machineTypeOperatorType = GetMachineTypeOperatorType(id: id);
      repository.Delete(machineTypeOperatorType);
    }
    public IQueryable<MachineTypeOperatorType> GetMachineTypeOperatorTypes(
        TValue<int> id = null,
        TValue<string> title = null,
        TValue<int> machineTypeId = null,
        TValue<int> operatorTypeId = null,
        TValue<bool> isNecessary = null,
        TValue<bool> isActive = null)
    {
      var isIdNull = id == null;
      var isTitleNull = title == null;
      var isMachineTypeIdNull = machineTypeId == null;
      var isOperatorTypeIdNull = operatorTypeId == null;
      var isIsNecessaryNull = isNecessary == null;
      var isIsActiveNull = isActive == null;
      var machineTypeOperatorTypes = from item in repository.GetQuery<MachineTypeOperatorType>()
                                     where
                                               (isIdNull || item.Id == id) &&
                                               (isTitleNull || item.Title == title) &&
                                               (isMachineTypeIdNull || item.MachineTypeId == machineTypeId) &&
                                               (isOperatorTypeIdNull || item.OperatorTypeId == operatorTypeId) &&
                                               (isIsNecessaryNull || item.IsNecessary == isNecessary) &&
                                               (isIsActiveNull || item.IsActive == isActive)
                                     select item;
      return machineTypeOperatorTypes;
    }
    public MachineTypeOperatorType GetMachineTypeOperatorType(int id)
    {
      var machineTypeOperatorType = GetMachineTypeOperatorTypes(id: id)


                .FirstOrDefault();
      if (machineTypeOperatorType == null)
        throw new MachineTypeOperatorTypeNotFoundException(id: id);
      return machineTypeOperatorType;
    }
    public MachineTypeOperatorType AddMachineTypeOperatorType(
        string title,
        short machineTypeId,
        short operatorTypeId,
        bool isNecessary)
    {
      var machineTypeOperatorType = repository.Create<MachineTypeOperatorType>();
      machineTypeOperatorType.Title = title;
      machineTypeOperatorType.MachineTypeId = machineTypeId;
      machineTypeOperatorType.OperatorTypeId = operatorTypeId;
      machineTypeOperatorType.IsNecessary = isNecessary;
      machineTypeOperatorType.IsActive = true;
      repository.Add(machineTypeOperatorType);
      return machineTypeOperatorType;
    }
    public MachineTypeOperatorType EditMachineTypeOperatorType(
        byte[] rowVersion,
        int id,
        TValue<string> title = null,
        TValue<short> machineTypeId = null,
        TValue<short> operatorTypeId = null,
        TValue<bool> isNecessary = null,
        TValue<bool> isActive = null)
    {
      var machineTypeOperatorType = GetMachineTypeOperatorType(id: id);
      if (title != null)
        machineTypeOperatorType.Title = title;
      if (machineTypeId != null)
        machineTypeOperatorType.MachineTypeId = machineTypeId;
      if (operatorTypeId != null)
        machineTypeOperatorType.OperatorTypeId = operatorTypeId;
      if (isNecessary != null)
        machineTypeOperatorType.IsNecessary = isNecessary;
      if (isActive != null)
        machineTypeOperatorType.IsActive = isActive;
      repository.Update(entity: machineTypeOperatorType, rowVersion: rowVersion);
      return machineTypeOperatorType;
    }
    public MachineTypeOperatorType ActiveMachineTypeOperatorType(
        byte[] rowVersion,
        int id
        )
    {
      EditMachineTypeOperatorType(rowVersion: rowVersion, id: id, isActive: true);
    }
    public MachineTypeOperatorType DeactiveMachineTypeOperatorType(
        byte[] rowVersion,
        int id)
    {
      EditMachineTypeOperatorType(rowVersion: rowVersion, id: id, isActive: false);
    }
    public MachineTypeOperatorTypeResult ToMachineTypeOperatorTypeResult(MachineTypeOperatorType machineTypeOperatorType)
    {
      var machineType = machineTypeOperatorType.MachineType;
      var @operatorType = machineTypeOperatorType.OperatorType;
      return new MachineTypeOperatorTypeResult
      {
        Id = machineTypeOperatorType.Id,
        Title = machineTypeOperatorType.Title,
        MachineTypeId = machineTypeOperatorType.MachineTypeId,
        MachineTypeName = machineType.Name,
        OperatorTypeId = machineTypeOperatorType.OperatorTypeId,
        OperatorTypeName = @operatorType.Name,
        IsNecessary = machineTypeOperatorType.IsNecessary,
        IsActive = machineTypeOperatorType.IsActive,
        RowVersion = machineTypeOperatorType.RowVersion,
      };
    }
    public IQueryable<MachineTypeOperatorTypeResult> ToMachineTypeOperatorTypeResultQuery(IQueryable<MachineTypeOperatorType> query)
    {
      return from machineTypeOperatorType in query
             let machineType = machineTypeOperatorType.MachineType
             let @operatorType = machineTypeOperatorType.OperatorType
             select new MachineTypeOperatorTypeResult
             {
               Id = machineTypeOperatorType.Id,
               Title = machineTypeOperatorType.Title,
               MachineTypeId = machineTypeOperatorType.MachineTypeId,
               MachineTypeName = machineType.Name,
               OperatorTypeId = machineTypeOperatorType.OperatorTypeId,
               OperatorTypeName = @operatorType.Name,
               IsNecessary = machineTypeOperatorType.IsNecessary,
               IsActive = machineTypeOperatorType.IsActive,
               RowVersion = machineTypeOperatorType.RowVersion,
             };
    }
    public IQueryable<MachineTypeOperatorTypeResult> SearchMachineTypeOperatorTypeResultQuery(IQueryable<MachineTypeOperatorTypeResult> query, string searchText)
    {
      if (searchText == null)
        return query;
      return from machineTypeOperatorType in query
             where machineTypeOperatorType.Title.Contains(searchText) ||
                   machineTypeOperatorType.MachineTypeName.Contains(searchText) ||
                   machineTypeOperatorType.OperatorTypeName.Contains(searchText)
             select machineTypeOperatorType;
    }
    public IOrderedQueryable<MachineTypeOperatorTypeResult> SortMachineTypeOperatorTypeResult(IQueryable<MachineTypeOperatorTypeResult> query, SortInput<MachineTypeOperatorTypeSortType> options)
    {
      switch (options.SortType)
      {
        case MachineTypeOperatorTypeSortType.Id:
          return query.OrderBy(a => a.Id, options.SortOrder);
        case MachineTypeOperatorTypeSortType.Title:
          return query.OrderBy(a => a.Title, options.SortOrder);
        case MachineTypeOperatorTypeSortType.IsActive:
          return query.OrderBy(a => a.IsActive, options.SortOrder);
        case MachineTypeOperatorTypeSortType.IsNecessary:
          return query.OrderBy(a => a.IsNecessary, options.SortOrder);
        case MachineTypeOperatorTypeSortType.MachineTypeName:
          return query.OrderBy(a => a.MachineTypeName, options.SortOrder);
        case MachineTypeOperatorTypeSortType.OperatorTypeName:
          return query.OrderBy(a => a.OperatorTypeName, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}