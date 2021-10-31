using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Domains;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Common;
using lena.Models.QualityControl;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using lena.Domains.Enums;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region Add Process
    public QualityControlTestEquipment AddQualityControlTestEquipmentProcess(
        long qualityControlTestId,
        string name,
        string description)
    {
      var testEquipment = AddTestEquipment(
                name: name, description: description);
      var qualityControlTestEquipment = AddQualityControlTestEquipment(
                qualityControlTestId: qualityControlTestId,
                testEquipmentId: testEquipment.Id);
      repository.Add(qualityControlTestEquipment);
      return qualityControlTestEquipment;
    }
    #endregion
    #region Add
    public QualityControlTestEquipment AddQualityControlTestEquipment(
        int testEquipmentId,
        long qualityControlTestId)
    {
      var qualityControlTestEquipment = repository.Create<QualityControlTestEquipment>();
      qualityControlTestEquipment.QualityControlTestId = qualityControlTestId;
      qualityControlTestEquipment.QualityControlTest = GetQualityControlTest(id: qualityControlTestId);
      qualityControlTestEquipment.TestEquipmentId = testEquipmentId;
      repository.Add(qualityControlTestEquipment);
      return qualityControlTestEquipment;
    }
    #endregion
    #region Get
    public QualityControlTestEquipment GetQualityControlTestEquipment(int id) => GetQualityControlTestEquipment(selector: e => e, id: id);
    public TResult GetQualityControlTestEquipment<TResult>(
        Expression<Func<QualityControlTestEquipment, TResult>> selector,
        int id)
    {
      var qualityControlTestEquipment = GetQualityControlTestEquipments(
                selector: selector,
                id: id).FirstOrDefault();
      if (qualityControlTestEquipment == null)
        throw new QualityControlTestEquipmentNotFoundException(id);
      return qualityControlTestEquipment;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetQualityControlTestEquipments<TResult>(
        Expression<Func<QualityControlTestEquipment, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<int> testEquipmentId = null,
        TValue<long> qualityControlTestId = null
        )
    {
      var qualityControlTestEquipments = repository.GetQuery<QualityControlTestEquipment>();
      if (testEquipmentId != null)
        qualityControlTestEquipments = qualityControlTestEquipments.Where(x => x.TestEquipmentId == testEquipmentId);
      if (qualityControlTestId != null)
        qualityControlTestEquipments = qualityControlTestEquipments.Where(i => i.QualityControlTestId == qualityControlTestId);
      if (name != null)
        qualityControlTestEquipments = qualityControlTestEquipments.Where(i => i.TestEquipment.Name == name);
      return qualityControlTestEquipments.Select(selector);
    }
    #endregion
    #region Remove
    public void RemoveQualityControlTestEquipment(
        int id,
        byte[] rowVersion,
        TValue<int> testEquipmentId = null,
        TValue<int> qualityControlTestId = null)
    {
      var qualityControlTestEquipment = GetQualityControlTestEquipment(id: id);
      DeleteTestEquipment(id: qualityControlTestEquipment.TestEquipmentId);
    }
    #endregion
    #region Delete
    public void DeleteQualityControlTestEquipment(
        int testEquipmentId,
        int qualityControlTestId)
    {
      var qualityControlTestEquipments = GetQualityControlTestEquipments(
                e => e,
                testEquipmentId: testEquipmentId,
                qualityControlTestId: qualityControlTestId);
      if (qualityControlTestEquipments.Any())
        repository.Delete(qualityControlTestEquipments.FirstOrDefault());
    }
    #endregion
    #region Edit
    public QualityControlTestEquipment EditQualityControlTestEquipment(
        int id,
        TValue<long> qualityControlTestId = null,
        TValue<int> testEquipmentId = null)
    {
      var qualityControlTestEquipment = GetQualityControlTestEquipment(id: id);
      if (qualityControlTestId != null)
        qualityControlTestEquipment.QualityControlTestId = qualityControlTestId;
      if (testEquipmentId != null)
        qualityControlTestEquipment.TestEquipmentId = testEquipmentId;
      repository.Update(qualityControlTestEquipment, qualityControlTestEquipment.RowVersion);
      return qualityControlTestEquipment;
    }
    #endregion
    #region Search
    public IQueryable<QualityControlTestEquipmentResult> SearchQualityControlTestEquipment(IQueryable<QualityControlTestEquipmentResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.QualityControlTestName.Contains(searchText));
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region SortCombo
    public IOrderedQueryable<QualityControlTestEquipmentComboResult> SortQualityControlTestEquipmentComboResult(
        IQueryable<QualityControlTestEquipmentComboResult> query,
        SortInput<QualityControlTestEquipmentSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlTestEquipmentSortType.TestEquipmentId:
          return query.OrderBy(a => a.TestEquipmentId, sort.SortOrder);
        case QualityControlTestEquipmentSortType.QualityControlTestName:
          return query.OrderBy(a => a.QualityControlTestName, sort.SortOrder);
        case QualityControlTestEquipmentSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Sort
    public IOrderedQueryable<QualityControlTestEquipmentResult> SortQualityControlTestEquipmentResult(IQueryable<QualityControlTestEquipmentResult> query,
        SortInput<QualityControlTestEquipmentSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlTestEquipmentSortType.TestEquipmentId:
          return query.OrderBy(a => a.TestEquipmentId, sort.SortOrder);
        case QualityControlTestEquipmentSortType.QualityControlTestName:
          return query.OrderBy(a => a.QualityControlTestName, sort.SortOrder);
        case QualityControlTestEquipmentSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToQualityControlTestEquipmentResult
    public Expression<Func<QualityControlTestEquipment, QualityControlTestEquipmentResult>> ToQualityControlTestEquipmentResult =
        qualityControlTestEquipment => new QualityControlTestEquipmentResult
        {
          TestEquipmentId = qualityControlTestEquipment.TestEquipmentId,
          Name = qualityControlTestEquipment.TestEquipment.Name,
          Description = qualityControlTestEquipment.TestEquipment.Description,
          QualityControlTestId = qualityControlTestEquipment.QualityControlTestId,
          QualityControlTestName = qualityControlTestEquipment.QualityControlTest.Name,
          RowVersion = qualityControlTestEquipment.RowVersion
        };
    #endregion
    #region ToQualityControlTestEquipmentComboResult
    public Expression<Func<QualityControlTestEquipment, QualityControlTestEquipmentComboResult>> ToQualityControlTestEquipmentComboResult =
        qualityControlTestEquipment => new QualityControlTestEquipmentComboResult
        {
          TestEquipmentId = qualityControlTestEquipment.TestEquipmentId,
          Name = qualityControlTestEquipment.TestEquipment.Name,
          QualityControlTestId = qualityControlTestEquipment.QualityControlTestId,
          QualityControlTestName = qualityControlTestEquipment.QualityControlTest.Name,
        };
    #endregion
  }
}