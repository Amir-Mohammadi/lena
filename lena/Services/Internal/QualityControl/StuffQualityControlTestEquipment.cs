using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.QualityControl;
using System;
using System.Linq;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {

    #region Add
    public StuffQualityControlTestEquipment AddStuffQualityControlTestEquipment(
        int stuffId,
        long qualityControlTestId,
        int qualityControlEquipmentTestEquipmentId,
        long qualityControlTestEquipmentQualityControlTestId)
    {

      var stuffQualityControlTestEquipment = repository.Create<StuffQualityControlTestEquipment>();
      stuffQualityControlTestEquipment.StuffId = stuffId;
      stuffQualityControlTestEquipment.QualityControlTestId = qualityControlTestId;
      stuffQualityControlTestEquipment.QualityControlEquipmentTestEquipmentId = qualityControlEquipmentTestEquipmentId;
      stuffQualityControlTestEquipment.QualityControlTestEquipmentQualityControlTestId = qualityControlTestEquipmentQualityControlTestId;

      repository.Add(stuffQualityControlTestEquipment);
      return stuffQualityControlTestEquipment;
    }
    #endregion

    #region Search
    public IQueryable<StuffQualityControlTestEquipmentResult> SearchStuffQualityControlTestEquipment(IQueryable<StuffQualityControlTestEquipmentResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.StuffId.ToString().Contains(searchText));
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<StuffQualityControlTestEquipmentResult> SortStuffQualityControlTestEquipmentResult(IQueryable<StuffQualityControlTestEquipmentResult> query,
        SortInput<StuffQualityControlTestEquipmentSortType> sort)
    {
      switch (sort.SortType)
      {
        case StuffQualityControlTestEquipmentSortType.StuffId:
          return query.OrderBy(a => a.StuffId, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetStuffQualityControlTestEquipments<TResult>(
        Expression<Func<StuffQualityControlTestEquipment, TResult>> selector,
        TValue<int> stuffId = null,
        TValue<long> qualityControlTestId = null,
        TValue<int> qualityControlEquipmentTestEquipmentId = null,
        TValue<long> qualityControlTestEquipmentQualityControlTestId = null)
    {

      var stuffQualityControlTestEquipments = repository.GetQuery<StuffQualityControlTestEquipment>();
      if (stuffId != null)
        stuffQualityControlTestEquipments = stuffQualityControlTestEquipments.Where(m => m.StuffId == stuffId);
      if (qualityControlTestId != null)
        stuffQualityControlTestEquipments = stuffQualityControlTestEquipments.Where(m => m.QualityControlTestId == qualityControlTestId);
      if (qualityControlEquipmentTestEquipmentId != null)
        stuffQualityControlTestEquipments = stuffQualityControlTestEquipments.Where(m => m.QualityControlEquipmentTestEquipmentId == qualityControlEquipmentTestEquipmentId);
      if (qualityControlTestEquipmentQualityControlTestId != null)
        stuffQualityControlTestEquipments = stuffQualityControlTestEquipments.Where(m => m.QualityControlTestEquipmentQualityControlTestId == qualityControlTestEquipmentQualityControlTestId);
      return stuffQualityControlTestEquipments.Select(selector);
    }
    #endregion

    #region Remove
    public void RemoveStuffQualityControlTestEquipment(
        StuffQualityControlTestEquipment stuffQualityControlTestEquipment)
    {

      repository.Delete(stuffQualityControlTestEquipment);
    }
    #endregion

    #region Delete
    public void DeleteStuffQualityControlTestEquipment(
        int stuffId,
        long qualityControlTestId,
        int qualityControlEquipmentTestEquipmentId,
        long qualityControlTestEquipmentQualityControlTestId)
    {

      var stuffQualityControlTestEquipment = GetStuffQualityControlTestEquipments(
                e => e,
                stuffId: stuffId,
                qualityControlTestId: qualityControlTestId,
                qualityControlEquipmentTestEquipmentId: qualityControlEquipmentTestEquipmentId,
                qualityControlTestEquipmentQualityControlTestId: qualityControlTestEquipmentQualityControlTestId)
            .FirstOrDefault();
      repository.Delete(stuffQualityControlTestEquipment);
    }
    #endregion

    #region ToResult
    public Expression<Func<StuffQualityControlTestEquipment, StuffQualityControlTestEquipmentResult>> ToStuffQualityControlTestEquipmentResult =
        stuffQualityControlTestEquipment => new StuffQualityControlTestEquipmentResult
        {
          Name = stuffQualityControlTestEquipment.QualityControlTestEquipment.TestEquipment.Name,
          Description = stuffQualityControlTestEquipment.QualityControlTestEquipment.TestEquipment.Description,
          StuffId = stuffQualityControlTestEquipment.StuffId,
          QualityControlTestId = stuffQualityControlTestEquipment.QualityControlTestId,
          QualityControlEquipmentTestEquipmentId = stuffQualityControlTestEquipment.QualityControlEquipmentTestEquipmentId,
          QualityControlTestEquipmentQualityControlTestId = stuffQualityControlTestEquipment.QualityControlTestEquipmentQualityControlTestId,

          RowVersion = stuffQualityControlTestEquipment.RowVersion
        };
    #endregion
  }

}
