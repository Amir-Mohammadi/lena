using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
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
    public TestEquipment AddTestEquipment(
        string name,
        string description)
    {

      var testEquipment = repository.Create<TestEquipment>();

      var duplicatedEquipments = GetTestEquipments(
                selector: e => e,
                name: name,
                description: description);

      if (duplicatedEquipments.Any())
        throw new DuplicatedTestEquipmentException();

      testEquipment.Name = name;
      testEquipment.Description = description;
      repository.Add(testEquipment);
      return testEquipment;
    }
    #endregion

    #region Edit
    public TestEquipment EditTestEquipment(
        byte[] rowVersion,
        int id,
        TValue<string> name = null,
        TValue<string> description = null
        )
    {

      var testEquipment = GetTestEquipment(id: id);
      if (name != null)
        testEquipment.Name = name;
      if (description != null)
        testEquipment.Description = description;
      repository.Update(testEquipment, testEquipment.RowVersion);
      return testEquipment;
    }
    #endregion

    #region Search
    public IQueryable<TestEquipmentResult> SearchTestEquipment(IQueryable<TestEquipmentResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Name.Contains(searchText) ||
            item.Description.Contains(searchText));
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<TestEquipmentResult> SortTestEquipmentResult(IQueryable<TestEquipmentResult> query,
        SortInput<TestEquipmentSortType> sort)
    {
      switch (sort.SortType)
      {
        case TestEquipmentSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Get
    public TestEquipment GetTestEquipment(int id) => GetTestEquipment(selector: e => e, id: id);
    public TResult GetTestEquipment<TResult>(
        Expression<Func<TestEquipment, TResult>> selector,
        int id)
    {

      var testEquipment = GetTestEquipments(
                selector: selector,
                id: id).FirstOrDefault();
      if (testEquipment == null)
        throw new TestEquipmentNotFoundException(id);
      return testEquipment;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetTestEquipments<TResult>(
        Expression<Func<TestEquipment, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<string> description = null)
    {

      var testEquipments = repository.GetQuery<TestEquipment>();
      if (id != null)
        testEquipments = testEquipments.Where(x => x.Id == id);

      if (name != null)
        testEquipments = testEquipments.Where(i => i.Name == name);
      if (description != null)
        testEquipments = testEquipments.Where(i => i.Description == description);
      return testEquipments.Select(selector);
    }
    #endregion

    #region Delete
    public void DeleteTestEquipment(int id)
    {

      var testEquipment = GetTestEquipment(id: id);
      repository.Delete(testEquipment);
    }
    #endregion

    #region ToResult
    public Expression<Func<TestEquipment, TestEquipmentResult>> ToTestEquipmentResult =
        testEquipment => new TestEquipmentResult
        {
          Id = testEquipment.Id,
          Name = testEquipment.Name,
          Description = testEquipment.Description,
          RowVersion = testEquipment.RowVersion
        };
    #endregion
  }

}
