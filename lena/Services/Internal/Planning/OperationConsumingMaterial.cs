using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Planning.OperationConsumingMaterial;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public IQueryable<TResult> GetOperationConsumingMaterials<TResult>(
            Expression<Func<OperationConsumingMaterial, TResult>> selector,
            TValue<int> id = null,
            TValue<int> billOfMaterialDetailId = null,
            TValue<double> value = null,
            TValue<int> operationSequenceId = null,
            TValue<int> stuffId = null,
            TValue<int> operationId = null,
            TValue<int[]> operationSequenceIds = null
        )
    {

      var query = repository.GetQuery<OperationConsumingMaterial>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (billOfMaterialDetailId != null)
        query = query.Where(i => i.BillOfMaterialDetailId == billOfMaterialDetailId);
      if (value != null)
        query = query.Where(i => i.Value == value);
      if (operationSequenceId != null)
        query = query.Where(i => i.OperationSequenceId == operationSequenceId);
      if (stuffId != null)
        query = query.Where(i => i.BillOfMaterialDetail.StuffId == stuffId);
      if (operationId != null)
        query = query.Where(i => i.OperationSequence.OperationId == operationId);
      if (operationSequenceIds != null)
        query = query.Where(i => operationSequenceIds.Value.Contains(i.OperationSequenceId));
      return query.Select(selector);
    }

    public OperationConsumingMaterial GetOperationConsumingMaterial(int id) =>
        GetOperationConsumingMaterial(selector: e => e, id: id);
    public TResult GetOperationConsumingMaterial<TResult>(
        Expression<Func<OperationConsumingMaterial, TResult>> selector,
        int id)
    {

      var operationConsumingMaterial = GetOperationConsumingMaterials(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (operationConsumingMaterial == null)
        throw new OperationConsumingMaterialNotFoundException(id);
      return operationConsumingMaterial;
    }
    public OperationConsumingMaterial AddOperationConsumingMaterial(
            int billOfMaterialDetailId,
            double value,
            int operationSequenceId,
            bool limitedSerialBuffer)
    {

      var operationConsumingMaterial = repository.Create<OperationConsumingMaterial>();
      operationConsumingMaterial.BillOfMaterialDetailId = billOfMaterialDetailId;
      operationConsumingMaterial.Value = value;
      operationConsumingMaterial.LimitedSerialBuffer = limitedSerialBuffer;
      operationConsumingMaterial.OperationSequenceId = operationSequenceId;
      repository.Add(operationConsumingMaterial);
      return operationConsumingMaterial;
    }
    public OperationConsumingMaterial EditOperationConsumingMaterial(
        byte[] rowVersion,
        int id,
        TValue<int> billOfMaterialDetailId = null,
        TValue<double> value = null,
        TValue<bool> limitedSerialBuffer = null,
        TValue<int> operationSequenceId = null)
    {

      var operationConsumingMaterial = GetOperationConsumingMaterial(id: id);
      if (billOfMaterialDetailId != null)
        operationConsumingMaterial.BillOfMaterialDetailId = billOfMaterialDetailId;
      if (limitedSerialBuffer != null)
        operationConsumingMaterial.LimitedSerialBuffer = limitedSerialBuffer;
      if (value != null)
        operationConsumingMaterial.Value = value;
      if (operationSequenceId != null)
        operationConsumingMaterial.OperationSequenceId = operationSequenceId;
      repository.Update(rowVersion: rowVersion, entity: operationConsumingMaterial);
      return operationConsumingMaterial;
    }
    public void DeleteOperationConsumingMaterial(int id)
    {

      var operationConsumingMaterial = GetOperationConsumingMaterial(id: id);
      repository.Delete(operationConsumingMaterial);
    }

    public Expression<Func<OperationConsumingMaterial, OperationConsumingMaterialResult>> ToOperationConsumingMaterialResult =

            operationConsumingMaterial => new OperationConsumingMaterialResult
            {
              Id = operationConsumingMaterial.Id,
              BillOfMaterialDetailId = operationConsumingMaterial.BillOfMaterialDetailId,
              Value = operationConsumingMaterial.Value,
              OperationSequenceId = operationConsumingMaterial.OperationSequenceId,
              RowVersion = operationConsumingMaterial.RowVersion
            };

    public Expression<Func<OperationConsumingMaterial, FullOperationConsumingMaterialResult>> ToFullOperationConsumingMaterialResult =
        operationConsumingMaterial => new FullOperationConsumingMaterialResult
        {
          Id = operationConsumingMaterial.Id,
          BillOfMaterialDetailId = operationConsumingMaterial.BillOfMaterialDetailId,
          BillOfMaterialDetailType = operationConsumingMaterial.BillOfMaterialDetail.BillOfMaterialDetailType,
          SemiProductBillOfMaterialVersion = operationConsumingMaterial.BillOfMaterialDetail.SemiProductBillOfMaterialVersion,
          Value = operationConsumingMaterial.BillOfMaterialDetail.Value,
          Consumed = operationConsumingMaterial.Value,
          UnitId = operationConsumingMaterial.BillOfMaterialDetail.UnitId,
          UnitName = operationConsumingMaterial.BillOfMaterialDetail.Unit.Name,
          UnitConversionRatio = operationConsumingMaterial.BillOfMaterialDetail.Unit.ConversionRatio,
          OperationSequenceId = operationConsumingMaterial.OperationSequenceId,
          StuffId = operationConsumingMaterial.BillOfMaterialDetail.StuffId,
          StuffCode = operationConsumingMaterial.BillOfMaterialDetail.Stuff.Code,
          StuffName = operationConsumingMaterial.BillOfMaterialDetail.Stuff.Name,
          ForQty = operationConsumingMaterial.BillOfMaterialDetail.ForQty,
          IsPackingMaterial = operationConsumingMaterial.BillOfMaterialDetail.IsPackingMaterial,
          LimitedSerialBuffer = operationConsumingMaterial.LimitedSerialBuffer,
          RowVersion = operationConsumingMaterial.RowVersion
        };

    public IQueryable<OperationConsumingMaterialResult> ToOperationConsumingMaterialResultQuery(IQueryable<OperationConsumingMaterial> query)
    {
      return from operationConsumingMaterial in query
             select new OperationConsumingMaterialResult
             {
               Id = operationConsumingMaterial.Id,
               BillOfMaterialDetailId = operationConsumingMaterial.BillOfMaterialDetailId,
               Value = operationConsumingMaterial.Value,
               OperationSequenceId = operationConsumingMaterial.OperationSequenceId,
               RowVersion = operationConsumingMaterial.RowVersion
             };
    }
    public IQueryable<OperationConsumingMaterialResult> SearchOperationConsumingMaterialResult(IQueryable<OperationConsumingMaterialResult> query, TValue<string> search)
    {
      if (search == null) return query;
      return from operationConsumingMaterial in query
               //where
               //operationConsumingMaterial.BillOfMaterialDetailId.Contains(search) ||
               //operationConsumingMaterial.Value.Contains(search) ||
               //operationConsumingMaterial.OperationSequenceId.Contains(search) ||
             select operationConsumingMaterial;
    }
    public IOrderedQueryable<OperationConsumingMaterialResult> SortOperationConsumingMaterialResult(IQueryable<OperationConsumingMaterialResult> query, SortInput<OperationConsumingMaterialSortType> sort)
    {
      switch (sort.SortType)
      {
        case OperationConsumingMaterialSortType.OperationSequenceId:
          return query.OrderBy(a => a.OperationSequenceId, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
