using System;
using System.Linq;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Planning.WorkStation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public IQueryable<WorkStation> GetWorkStations(
        TValue<short> id = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<int> productionLineId = null)
    {

      var isIdNull = id == null;
      var isTitleNull = name == null;
      var isDescriptionNull = description == null;
      var isProductionLineIdNull = productionLineId == null;
      var workStations = from item in repository.GetQuery<WorkStation>()
                         where (isIdNull || item.Id == id) &&
                                         (isTitleNull || item.Name == name) &&
                                         (isDescriptionNull || item.Description == description) &&
                                         (isProductionLineIdNull || item.ProductionLineId == productionLineId)
                         select item;
      return workStations;
    }
    public WorkStation GetWorkStation(short id)
    {

      var workStation = GetWorkStations(id: id).SingleOrDefault();
      if (workStation == null)
        throw new WorkStationNotFoundException(id);
      return workStation;
    }
    public WorkStation AddWorkStation(string name,
        string description,
        int productionLineId)
    {

      var workStation = repository.Create<WorkStation>();
      workStation.Name = name;
      workStation.Description = description;
      workStation.ProductionLineId = productionLineId; ;
      repository.Add(workStation);
      return workStation;
    }
    public WorkStation AddWorkStationProcess(
        string name,
        string description,
        int productionLineId)
    {

      var workStation =
                AddWorkStation(name: name,
                    description: description,
                    productionLineId: productionLineId);
      return workStation;
    }
    public WorkStation EditWorkStation(
        byte[] rowVersion,
        short id,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<int> productionLineId = null)
    {

      var workStation = GetWorkStation(id);
      if (workStation == null)
        throw new WorkStationNotFoundException(id);
      if (name != null)
        workStation.Name = name;
      if (description != null)
        workStation.Description = description;
      if (productionLineId != null)
        workStation.ProductionLineId = productionLineId;
      repository.Update(workStation, rowVersion: rowVersion);
      return workStation;
    }
    public WorkStation EditWorkStationProcess(
        short id,
        byte[] rowVersion,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<int> productionLineId = null)
    {

      var workStation = EditWorkStation(
                    rowVersion: rowVersion,
                    id: id,
                    name: name,
                    description: description,
                    productionLineId: productionLineId);
      return workStation;
    }
    public void DeleteWorkStation(short id)
    {

      var workStation = GetWorkStation(id: id);
      if (workStation == null)
        throw new WorkStationNotFoundException(id);
      //if (workStation.Memberships.Any())
      //    throw new   WorkStationDeleteFailedRemoveMembershipException(id);
      //if (workStation.Permissions.Any())
      //    throw new WorkStationDeleteFailedRemovePermissionException(id);
      repository.Delete(workStation);
    }
    public IQueryable<WorkStationResult> ToWorkStationResultQuery(IQueryable<WorkStation> query)
    {
      var resultQuery = from workStation in query
                        select new WorkStationResult()
                        {
                          Id = workStation.Id,
                          Name = workStation.Name,
                          Description = workStation.Description,
                          ProductionLineId = workStation.ProductionLineId,
                          ProductionLineName = workStation.ProductionLine.Name,
                          RowVersion = workStation.RowVersion
                        };
      return resultQuery;
    }

    public IQueryable<WorkStationComboResult> ToWorkStationComboResultQuery(IQueryable<WorkStation> query)
    {
      var resultQuery = from workStation in query
                        select new WorkStationComboResult()
                        {
                          Id = workStation.Id,
                          Name = workStation.Name
                        };
      return resultQuery;
    }
    public WorkStationResult ToWorkStationResult(WorkStation workStation)
    {
      var result = new WorkStationResult()
      {
        Id = workStation.Id,
        Name = workStation.Name,
        Description = workStation.Description,
        ProductionLineId = workStation.ProductionLineId,
        ProductionLineName = workStation.ProductionLine.Name,
        RowVersion = workStation.RowVersion
      };
      return result;
    }

    public IOrderedQueryable<WorkStationResult> SortWorkStationResult(IQueryable<WorkStationResult> input, SortInput<WorkStationSortType> options)
    {
      switch (options.SortType)
      {
        case WorkStationSortType.Id:
          return input.OrderBy(i => i.Id, options.SortOrder);
        case WorkStationSortType.Name:
          return input.OrderBy(i => i.Name, options.SortOrder);
        case WorkStationSortType.ProductionLineName:
          return input.OrderBy(i => i.ProductionLineName, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public IQueryable<WorkStationResult> SearchWorkStation(
       IQueryable<WorkStationResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems
       )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                item.Name.Contains(searchText) ||
                item.Description.Contains(searchText) ||
                item.ProductionLineName.Contains(searchText)
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




