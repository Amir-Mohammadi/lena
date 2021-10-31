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
using lena.Models.Planning.OperationSequenceMachineTypeParameter;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Get
    public OperationSequenceMachineTypeParameter GetOperationSequenceMachineTypeParameter(int id) =>
        GetOperationSequenceMachineTypeParameter(selector: e => e, id: id);

    public TResult GetOperationSequenceMachineTypeParameter<TResult>(
        Expression<Func<OperationSequenceMachineTypeParameter, TResult>> selector,
        int id)
    {

      var machineTypeParamiter = GetOperationSequenceMachineTypeParameters(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (machineTypeParamiter == null)
        throw new OperationSequenceMachineTypeParameterNotFoundException(id: id);
      return machineTypeParamiter;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetOperationSequenceMachineTypeParameters<TResult>(
        Expression<Func<OperationSequenceMachineTypeParameter, TResult>> selector,
        TValue<int> id = null,
        TValue<int> machineTypeParameterId = null,
        TValue<int> operationSequenceId = null)
    {

      var machineTypeParameters = repository.GetQuery<OperationSequenceMachineTypeParameter>();
      if (id != null)
        machineTypeParameters = machineTypeParameters.Where(i => i.Id == id);
      if (machineTypeParameterId != null)
        machineTypeParameters = machineTypeParameters.Where(i => i.MachineTypeParameterId == machineTypeParameterId);
      if (operationSequenceId != null)
        machineTypeParameters = machineTypeParameters.Where(i => i.OperationSequenceId == operationSequenceId);

      return machineTypeParameters.Select(selector);
    }
    #endregion

    #region Add
    public OperationSequenceMachineTypeParameter AddOperationSequenceMachineTypeParameter(
        int machineTypeParameterId,
        int operationSequenceId,
        double value)
    {

      var machineTypeParameter = repository.Create<OperationSequenceMachineTypeParameter>();
      machineTypeParameter.MachineTypeParameterId = machineTypeParameterId;
      machineTypeParameter.OperationSequenceId = operationSequenceId;
      machineTypeParameter.Value = value;
      repository.Add(machineTypeParameter);

      return machineTypeParameter;
    }
    #endregion

    #region Edit
    public OperationSequenceMachineTypeParameter EditOperationSequenceMachineTypeParameter(
        int id,
        byte[] rowVersion,
        TValue<int> machineTypeParameterId = null,
        TValue<int> operationSequenceId = null,
        TValue<double> value = null)
    {

      var machineTypeParameter = GetOperationSequenceMachineTypeParameter(id: id);

      return EditOperationSequenceMachineTypeParameter(
                    operationSequenceMachineTypeParameter: machineTypeParameter,
                    rowVersion: rowVersion,
                    machineTypeParameterId: machineTypeParameterId,
                    operationSequenceId: operationSequenceId,
                    value: value);
    }

    public OperationSequenceMachineTypeParameter EditOperationSequenceMachineTypeParameter(
        OperationSequenceMachineTypeParameter operationSequenceMachineTypeParameter,
        byte[] rowVersion,
        TValue<int> machineTypeParameterId = null,
        TValue<int> operationSequenceId = null,
        TValue<double> value = null)
    {

      if (machineTypeParameterId != null)
        operationSequenceMachineTypeParameter.MachineTypeParameterId = machineTypeParameterId;
      if (operationSequenceId != null)
        operationSequenceMachineTypeParameter.OperationSequenceId = operationSequenceId;
      if (value != null)
        operationSequenceMachineTypeParameter.Value = value;

      repository.Update(rowVersion: rowVersion, entity: operationSequenceMachineTypeParameter);
      return operationSequenceMachineTypeParameter;
    }
    #endregion

    #region Delete
    public void DeleteOperationSequenceMachineTypeParameter(int id)
    {

      var operationSequenceMachineTypeParameter = GetOperationSequenceMachineTypeParameter(id: id);
      if (operationSequenceMachineTypeParameter == null)
        throw new OperationSequenceMachineTypeParameterNotFoundException(id);

      repository.Delete(operationSequenceMachineTypeParameter);
    }

    public void DeleteOperationSequenceMachineTypeParameter(OperationSequenceMachineTypeParameter operationSequenceMachineTypeParameter)
    {

      if (operationSequenceMachineTypeParameter == null)
        throw new ArgumentNullException(paramName: nameof(operationSequenceMachineTypeParameter));

      repository.Delete(operationSequenceMachineTypeParameter);
    }
    #endregion

    #region ToResult
    public Expression<Func<OperationSequenceMachineTypeParameter, OperationSequenceMachineTypeParameterResult>> ToOperationSequenceMachineTypeParameterResult =>
      operationSequenceMachineTypeParameter => new OperationSequenceMachineTypeParameterResult
      {
        Id = operationSequenceMachineTypeParameter.Id,
        MachineTypeParameterId = operationSequenceMachineTypeParameter.MachineTypeParameterId,
        MachineTypeParameterName = operationSequenceMachineTypeParameter.MachineTypeParameter.Name,
        OperationSequenceId = operationSequenceMachineTypeParameter.OperationSequenceId,
        Value = operationSequenceMachineTypeParameter.Value,
        RowVersion = operationSequenceMachineTypeParameter.RowVersion
      };
    #endregion

  }
}
