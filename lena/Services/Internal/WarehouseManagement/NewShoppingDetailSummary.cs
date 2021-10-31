using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region GetByNewShoppingDetailId 
    public NewShoppingDetailSummary GetNewShoppingDetailSummaryByNewShoppingDetailId(int newShoppingDetailId) => GetNewShoppingDetailSummaryByNewShoppingDetailId(selector: e => e, newShoppingDetailId: newShoppingDetailId);
    public TResult GetNewShoppingDetailSummaryByNewShoppingDetailId<TResult>(
        Expression<Func<NewShoppingDetailSummary, TResult>> selector,
        int newShoppingDetailId)
    {

      var newShoppingDetailSummary = GetNewShoppingDetailSummarys(
                    selector: selector,
                    newShoppingDetailId: newShoppingDetailId)


                .FirstOrDefault();
      if (newShoppingDetailSummary == null)
        throw new NewShoppingDetailSummaryForNewShoppingDetailNotFoundException(newShoppingDetailId: newShoppingDetailId);
      return newShoppingDetailSummary;
    }
    #endregion
    #region Get
    public NewShoppingDetailSummary GetNewShoppingDetailSummary(int id) => GetNewShoppingDetailSummary(selector: e => e, id: id);
    public TResult GetNewShoppingDetailSummary<TResult>(
        Expression<Func<NewShoppingDetailSummary, TResult>> selector,
        int id)
    {

      var newShoppingDetailSummary = GetNewShoppingDetailSummarys(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (newShoppingDetailSummary == null)
        throw new NewShoppingDetailSummaryNotFoundException(id: id);
      return newShoppingDetailSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetNewShoppingDetailSummarys<TResult>(
            Expression<Func<NewShoppingDetailSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> qualityControlPassedQty = null,
            TValue<double> qualityControlFailedQty = null,
            TValue<double> qualityControlConsumedQty = null,
            TValue<int> newShoppingDetailId = null
            )
    {

      var query = repository.GetQuery<NewShoppingDetailSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (qualityControlPassedQty != null)
        query = query.Where(x => x.QualityControlPassedQty == qualityControlPassedQty);
      if (qualityControlFailedQty != null)
        query = query.Where(x => x.QualityControlFailedQty == qualityControlFailedQty);
      if (qualityControlConsumedQty != null)
        query = query.Where(x => x.QualityControlConsumedQty == qualityControlConsumedQty);
      if (newShoppingDetailId != null)
        query = query.Where(x => x.NewShoppingDetail.Id == newShoppingDetailId);

      return query.Select(selector);
    }
    #endregion
    #region Add

    public NewShoppingDetailSummary AddNewShoppingDetailSummary(
        double qualityControlPassedQty,
        double qualityControlFailedQty,
        double qualityControlConsumedQty,
        int newShoppingDetailId)
    {

      var newShoppingDetailSummary = repository.Create<NewShoppingDetailSummary>();
      newShoppingDetailSummary.QualityControlPassedQty = qualityControlPassedQty;
      newShoppingDetailSummary.QualityControlFailedQty = qualityControlFailedQty;
      newShoppingDetailSummary.QualityControlConsumedQty = qualityControlConsumedQty;
      newShoppingDetailSummary.NewShoppingDetail = GetNewShoppingDetail(id: newShoppingDetailId);
      repository.Add(newShoppingDetailSummary);
      return newShoppingDetailSummary;
    }

    #endregion
    #region Edit
    public NewShoppingDetailSummary EditNewShoppingDetailSummary(
        int id,
        byte[] rowVersion,
        TValue<double> qualityControlPassedQty = null,
        TValue<double> qualityControlFailedQty = null,
        TValue<double> qualityControlConsumedQty = null,
        TValue<int> newShoppingDetailId = null)
    {

      var newShoppingDetailSummary = GetNewShoppingDetailSummary(id: id);
      return EditNewShoppingDetailSummary(
                    newShoppingDetailSummary: newShoppingDetailSummary,
                    rowVersion: rowVersion,
                    qualityControlPassedQty: qualityControlPassedQty,
                    qualityControlFailedQty: qualityControlFailedQty,
                    qualityControlConsumedQty: qualityControlConsumedQty,
                    newShoppingDetailId: newShoppingDetailId);
    }

    public NewShoppingDetailSummary EditNewShoppingDetailSummary(
                NewShoppingDetailSummary newShoppingDetailSummary,
                byte[] rowVersion,
                TValue<double> qualityControlPassedQty = null,
                TValue<double> qualityControlFailedQty = null,
                TValue<double> qualityControlConsumedQty = null,
                TValue<int> newShoppingDetailId = null)
    {

      if (newShoppingDetailId != null)
        newShoppingDetailSummary.NewShoppingDetail.Id = newShoppingDetailId;
      if (qualityControlPassedQty != null)
        newShoppingDetailSummary.QualityControlPassedQty = qualityControlPassedQty;
      if (qualityControlFailedQty != null)
        newShoppingDetailSummary.QualityControlFailedQty = qualityControlFailedQty;
      if (qualityControlConsumedQty != null)
        newShoppingDetailSummary.QualityControlConsumedQty = qualityControlConsumedQty;
      repository.Update(rowVersion: rowVersion, entity: newShoppingDetailSummary);
      return newShoppingDetailSummary;
    }

    #endregion
    #region Delete
    public void DeleteNewShoppingDetailSummary(int id)
    {

      var newShoppingDetailSummary = GetNewShoppingDetailSummary(id: id);
      repository.Delete(newShoppingDetailSummary);
    }
    #endregion
  }
}
