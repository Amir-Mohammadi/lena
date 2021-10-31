//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region Add
    public SerialFailedOperationFaultOperationEmployee AddSerialFailedOperationFaultOperationEmployee(
        int productionOperationEmployeeId,
        int serialFailedOperationFaultOperationId)
    {

      var serialFailedOperationFaultOperationEmployee = repository.Create<SerialFailedOperationFaultOperationEmployee>();
      serialFailedOperationFaultOperationEmployee.ProductionOperationEmployeeId = productionOperationEmployeeId;
      serialFailedOperationFaultOperationEmployee.SerialFailedOperationFaultOperationId = serialFailedOperationFaultOperationId;
      repository.Add(serialFailedOperationFaultOperationEmployee);
      return serialFailedOperationFaultOperationEmployee;
    }
    #endregion

    #region Edit
    public SerialFailedOperationFaultOperationEmployee EditSerialFailedOperationFaultOperationEmployee(
        int serialFailedOperationFaultOperationId,
        int productionOperationEmployeeId,
        byte[] rowVersion,
        TValue<int?> productionOperatorEmployeeBanId = null)
    {

      var serialFailedOperationFaultOperationEmployee = GetSerialFailedOperatonFaultOperationEmployee(
                serialFailedOperationFaultOperationId: serialFailedOperationFaultOperationId,
                productionOperationEmployeeId: productionOperationEmployeeId);

      return EditSerialFailedOperationFaultOperationEmployee(
                    serialFailedOperationFaultOperationEmployee: serialFailedOperationFaultOperationEmployee,
                    rowVersion: rowVersion,
                    productionOperatorEmployeeBanId: productionOperatorEmployeeBanId);
    }

    public SerialFailedOperationFaultOperationEmployee EditSerialFailedOperationFaultOperationEmployee(
        SerialFailedOperationFaultOperationEmployee serialFailedOperationFaultOperationEmployee,
        byte[] rowVersion,
        TValue<int?> productionOperatorEmployeeBanId = null)
    {


      if (productionOperatorEmployeeBanId != null)
        serialFailedOperationFaultOperationEmployee.ProductionOperatorEmployeeBanId = productionOperatorEmployeeBanId;


      repository.Update(entity: serialFailedOperationFaultOperationEmployee, rowVersion: rowVersion);

      return serialFailedOperationFaultOperationEmployee;
    }

    #endregion

    #region Delete
    public void DeleteSerialFailedOperationFaultOperationEmployee(int serialFailedOperationFaultOperationId, int productionOperationEmployeeId)
    {

      var serialFailedOperationFaultEmployee = GetSerialFailedOperatonFaultOperationEmployee(
             serialFailedOperationFaultOperationId: serialFailedOperationFaultOperationId,
             productionOperationEmployeeId: productionOperationEmployeeId);

      repository.Delete(serialFailedOperationFaultEmployee);
    }

    public void DeleteSerialFailedOperationFaultOperationEmployee(SerialFailedOperationFaultOperationEmployee serialFailedOperationFaultOperationEmployee)
    {

      repository.Delete(serialFailedOperationFaultOperationEmployee);
    }
    #endregion

    #region Get
    public SerialFailedOperationFaultOperationEmployee GetSerialFailedOperatonFaultOperationEmployee(
        int serialFailedOperationFaultOperationId,
        int productionOperationEmployeeId) =>
        GetSerialFailedOperatonFaultOperationEmployee(
            selector: e => e,
            serialFailedOperationFaultOperationId: serialFailedOperationFaultOperationId,
            productionOperationEmployeeId: productionOperationEmployeeId);

    public TResult GetSerialFailedOperatonFaultOperationEmployee<TResult>(
        Expression<Func<SerialFailedOperationFaultOperationEmployee, TResult>> selector,
        int serialFailedOperationFaultOperationId,
        int productionOperationEmployeeId)
    {

      var serialFailedOperationFaultEmployee = GetSerialFailedOperatonFaultOperationEmployees(
                selector: selector,
                serialFailedOperationFaultOperationId: serialFailedOperationFaultOperationId,
                productionOperationEmployeeId: productionOperationEmployeeId)


                .FirstOrDefault();
      if (serialFailedOperationFaultEmployee == null)
        throw new SerialFailedOperationFaultOperationEmployeeNotFoundException(
                  serialFailedOperationFaultOperationId: serialFailedOperationFaultOperationId,
                  productionOperationEmployeeId: productionOperationEmployeeId);

      return serialFailedOperationFaultEmployee;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetSerialFailedOperatonFaultOperationEmployees<TResult>(
        Expression<Func<SerialFailedOperationFaultOperationEmployee, TResult>> selector,
        TValue<int> serialFailedOperationFaultOperationId = null,
        TValue<int> productionOperationEmployeeId = null,
        TValue<int> serialFailedOperationId = null,
        TValue<int> productionOrderId = null,
        TValue<int?> productionOperatorEmployeeBanId = null,
        TValue<int[]> productionOperationEmployeeIds = null,
        TValue<int[]> serialFailedOperationFaultOperationIds = null,
        TValue<int[]> operationIds = null)
    {

      var query = repository.GetQuery<SerialFailedOperationFaultOperationEmployee>();
      if (serialFailedOperationFaultOperationId != null)
        query = query.Where(x => x.SerialFailedOperationFaultOperationId == serialFailedOperationFaultOperationId);

      if (productionOperationEmployeeId != null)
        query = query.Where(x => x.ProductionOperationEmployeeId == productionOperationEmployeeId);

      if (productionOperationEmployeeIds != null)
        query = query.Where(x => productionOperationEmployeeIds.Value.Contains(x.ProductionOperationEmployeeId));

      if (serialFailedOperationFaultOperationIds != null)
        query = query.Where(x => serialFailedOperationFaultOperationIds.Value.Contains(x.SerialFailedOperationFaultOperationId));

      if (serialFailedOperationId != null)
        query = query.Where(x => x.SerialFailedOperationFaultOperation.SerialFailedOperationId == serialFailedOperationId);

      if (productionOperatorEmployeeBanId != null)
        query = query.Where(x => x.ProductionOperatorEmployeeBanId == productionOperatorEmployeeBanId);

      if (productionOrderId != null)
        query = query.Where(x => x.SerialFailedOperationFaultOperation.SerialFailedOperation.ProductionOrderId == productionOrderId);

      if (operationIds != null)
        query = query.Where(x => operationIds.Value.Contains(x.SerialFailedOperationFaultOperation.OperationId));

      return query.Select(selector);
    }
    #endregion
  }
}
