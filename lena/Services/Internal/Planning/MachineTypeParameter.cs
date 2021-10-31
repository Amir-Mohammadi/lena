using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Planning.Machine;
using lena.Models.Planning.MachineTypeParameter;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Get
    public MachineTypeParameter GetMachineTypeParameter(int id) =>
        GetMachineTypeParameter(selector: e => e, id: id);

    public TResult GetMachineTypeParameter<TResult>(
        Expression<Func<MachineTypeParameter, TResult>> selector,
        int id)
    {

      var machineTypeParamiter = GetMachineTypeParameters(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (machineTypeParamiter == null)
        throw new MachineTypeParameterNotFoundException(id: id);
      return machineTypeParamiter;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetMachineTypeParameters<TResult>(
        Expression<Func<MachineTypeParameter, TResult>> selector,
        TValue<int> id = null,
        TValue<int> machineTypeId = null,
        TValue<int> operationSequenceId = null,
        TValue<string> name = null)
    {

      var machineTypeParameters = repository.GetQuery<MachineTypeParameter>();
      if (id != null)
        machineTypeParameters = machineTypeParameters.Where(i => i.Id == id);
      if (machineTypeId != null)
        machineTypeParameters = machineTypeParameters.Where(i => i.MachineTypeId == machineTypeId);
      if (name != null)
        machineTypeParameters = machineTypeParameters.Where(i => i.Name == name);

      return machineTypeParameters.Select(selector);
    }
    #endregion

    #region Add
    public MachineTypeParameter AddMachineTypeParameter(
        short machineTypeId,
        string name)
    {

      var machineTypeParameter = repository.Create<MachineTypeParameter>();
      machineTypeParameter.MachineTypeId = machineTypeId;
      machineTypeParameter.Name = name;
      repository.Add(machineTypeParameter);

      return machineTypeParameter;
    }
    #endregion

    #region Edit
    public MachineTypeParameter EditMachineTypeParameter(
        int id,
        byte[] rowVersion,
        TValue<short> machineTypeId = null,
        TValue<string> name = null)
    {

      var machineTypeParameter = GetMachineTypeParameter(id: id);

      return EditMachineTypeParameter(
                    machineTypeParameter: machineTypeParameter,
                    rowVersion: rowVersion,
                    machineTypeId: machineTypeId,
                    name: name);
    }

    public MachineTypeParameter EditMachineTypeParameter(
        MachineTypeParameter machineTypeParameter,
        byte[] rowVersion,
        TValue<short> machineTypeId = null,
        TValue<string> name = null)
    {

      if (machineTypeId != null) machineTypeParameter.MachineTypeId = machineTypeId;
      if (name != null) machineTypeParameter.Name = name;

      repository.Update(rowVersion: rowVersion, entity: machineTypeParameter);
      return machineTypeParameter;
    }
    #endregion

    #region Delete
    public void DeleteMachineTypeParameter(int id)
    {

      var machineTypeParameter = GetMachineTypeParameter(id: id);
      if (machineTypeParameter == null)
        throw new MachineTypeParameterNotFoundException(id);

      repository.Delete(machineTypeParameter);
    }
    #endregion

    #region ToResult
    public Expression<Func<MachineTypeParameter, MachineTypeParameterResult>> ToMachineTypeParameterResult =>
      machineTypeParameter => new MachineTypeParameterResult
      {
        Id = machineTypeParameter.Id,
        Name = machineTypeParameter.Name,
        MachineTypeId = machineTypeParameter.MachineTypeId,
        MachineTypeName = machineTypeParameter.MachineType.Name,
        RowVersion = machineTypeParameter.RowVersion
      };
    #endregion

    #region Search
    public IQueryable<MachineTypeParameterResult> SearchMachineTypeParameter(
        IQueryable<MachineTypeParameterResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
      {
        query = from item in query
                where
                    item.Name.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<MachineTypeParameterResult> SortMachineTypeParameterResult(
        IQueryable<MachineTypeParameterResult> query,
        SortInput<MachineTypeParameterSortType> sort)
    {
      switch (sort.SortType)
      {
        case MachineTypeParameterSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case MachineTypeParameterSortType.MachineTypeName:
          return query.OrderBy(a => a.MachineTypeName, sort.SortOrder);
        case MachineTypeParameterSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}
