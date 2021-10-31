using System.Linq;
using lena.Services.Core;
using lena.Services.Core.Foundation;
////using lena.Services.Core.Foundation.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using System.Linq.Expressions;
using System;
using lena.Models.QualityControl.QualityControlTest;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.QualityControl.QualityControlTestCondition;
using lena.Models.QualityControl.TestCondition;
using lena.Models.QualityControl;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region Get
    public QualityControlTest GetQualityControlTest(long id) => GetQualityControlTest(selector: e => e, id: id);
    public TResult GetQualityControlTest<TResult>(
        Expression<Func<QualityControlTest, TResult>> selector,
        long id)
    {

      var qualityControlTest = GetQualityControlTests(selector: selector, id: id)


                .FirstOrDefault();
      if (qualityControlTest == null)
        throw new QualityControlTestNotFoundException(id);
      return qualityControlTest;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetQualityControlTests<TResult>(
        Expression<Func<QualityControlTest, TResult>> selector,
        TValue<long> id = null,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null)
    {

      var query = repository.GetQuery<QualityControlTest>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (code != null)
        query = query.Where(x => x.Code == code);
      if (name != null)
        query = query.Where(x => x.Name == name);
      if (description != null)
        query = query.Where(x => x.Description == description);
      return query.Select(selector);
    }
    #endregion
    #region AddProcess
    public QualityControlTest AddQualityControlTestProcess(
        string code,
        string name,
        string description,
        AddTestConditionInput[] addTestConditionInputs,
        AddTestEquipmentInput[] addTestEquipmentInputs,
        AddTestImportanceDegreeInput[] addTestImportanceDegreeInputs,
        AddTestOperationInput[] addTestOperationInputs)
    {

      var qualityControlTest = AddQualityControlTest(code: code,
            name: name,
            description: description);
      #region AddQualityControlTestCondition
      foreach (var item in addTestConditionInputs)
      {
        var duplicatedItems = GetQualityControlTestConditions(
                  selector: e => e,
                  testConditionId: item.Id,
                  qualityControlTestId: qualityControlTest.Id);

        if (duplicatedItems.Any())
          throw new DuplicatedQualityControlTestConditionException();


        AddQualityControlTestCondition(
                  testConditionId: item.Id,
                  qualityControlTestId: qualityControlTest.Id);
      }
      #endregion
      #region AddQualityControlTestEquipment
      foreach (var item in addTestEquipmentInputs)
      {
        var duplicatedItems = GetQualityControlTestEquipments(
                  selector: e => e,
                  testEquipmentId: item.Id,
                  qualityControlTestId: qualityControlTest.Id);

        if (duplicatedItems.Any())
          throw new DuplicatedQualityControlTestEquipmentException();


        AddQualityControlTestEquipment(
                  testEquipmentId: item.Id,
                  qualityControlTestId: qualityControlTest.Id);
      }
      #endregion
      #region AddQualityControlTestImportanceDegree
      foreach (var item in addTestImportanceDegreeInputs)
      {
        var duplicatedItems = GetQualityControlTestImportanceDegrees(
                  selector: e => e,
                  testImportanceDegreeId: item.Id,
                  qualityControlTestId: qualityControlTest.Id);

        if (duplicatedItems.Any())
          throw new DuplicatedQualityControlTestImportanceDegreeException();


        AddQualityControlTestImportanceDegree(
                  testImportanceDegreeId: item.Id,
                  qualityControlTestId: qualityControlTest.Id);
      }
      #endregion
      #region AddQualityControlTestOperation
      foreach (var item in addTestOperationInputs)
      {
        var duplicatedItems = GetQualityControlTestOperations(
                  selector: e => e,
                  testOperationId: item.Id,
                  qualityControlTestId: qualityControlTest.Id);

        if (duplicatedItems.Any())
          throw new DuplicatedQualityControlTestOperationException();


        AddQualityControlTestOperation(
                  testOperationId: item.Id,
                  qualityControlTestId: qualityControlTest.Id);
      }
      #endregion
      return qualityControlTest;
    }
    #endregion
    #region Add
    public QualityControlTest AddQualityControlTest(
        string code,
        string name,
        string description)
    {

      var entity = repository.Create<QualityControlTest>();
      entity.Code = code;
      entity.Name = name;
      entity.Description = description;
      repository.Add(entity);
      return entity;
    }
    #endregion

    #region Edit Process
    public QualityControlTest EditQualityControlTestProcess(
        long id,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        AddTestConditionInput[] addTestConditions = null,
        DeleteTestConditionInput[] deleteTestConditions = null,
        AddTestEquipmentInput[] addTestEquipments = null,
        DeleteTestEquipmentInput[] deleteTestEquipments = null,
        AddTestImportanceDegreeInput[] addTestImportanceDegrees = null,
        DeleteTestImportanceDegreeInput[] deleteTestImportanceDegrees = null,
        AddTestOperationInput[] addTestOperations = null,
        DeleteTestOperationInput[] deleteTestOperations = null)
    {

      #region GetQualityControlTest
      var qualityControlTest = GetQualityControlTest(id: id);
      #endregion

      #region DeleteQualityControlTestCondition
      foreach (var item in deleteTestConditions)
      {
        DeleteQualityControlTestCondition(
                  testConditionId: item.TestConditionId,
                  qualityControlTestId: item.QualityControlTestId);
      }
      #endregion

      #region AddQualityControlTestCondition
      foreach (var item in addTestConditions)
      {
        var duplicatedItems = GetQualityControlTestConditions(
                  selector: e => e,
                  testConditionId: item.Id,
                  qualityControlTestId: qualityControlTest.Id);

        if (duplicatedItems.Any())
          throw new DuplicatedQualityControlTestConditionException();


        AddQualityControlTestCondition(
                  testConditionId: item.Id,
                  qualityControlTestId: qualityControlTest.Id);
      }
      #endregion
      #region DeleteQualityControlTestEquipment
      foreach (var item in deleteTestEquipments)
      {
        DeleteQualityControlTestEquipment(
                  testEquipmentId: item.TestEquipmentId,
                  qualityControlTestId: item.QualityControlTestId);
      }
      #endregion

      #region AddQualityControlTestEquipment
      foreach (var item in addTestEquipments)
      {
        var duplicatedItems = GetQualityControlTestEquipments(
                  selector: e => e,
                  testEquipmentId: item.Id,
                  qualityControlTestId: qualityControlTest.Id);

        if (duplicatedItems.Any())
          throw new DuplicatedQualityControlTestEquipmentException();


        AddQualityControlTestEquipment(
                  testEquipmentId: item.Id,
                  qualityControlTestId: qualityControlTest.Id);
      }
      #endregion
      #region DeleteQualityControlTestImportanceDegree
      foreach (var item in deleteTestImportanceDegrees)
      {
        DeleteQualityControlTestImportanceDegree(
                  testImportanceDegreeId: item.TestImportanceDegreeId,
                  qualityControlTestId: item.QualityControlTestId);
      }
      #endregion

      #region AddQualityControlTestImportanceDegree
      foreach (var item in addTestImportanceDegrees)
      {
        var duplicatedItems = GetQualityControlTestImportanceDegrees(
                  selector: e => e,
                  testImportanceDegreeId: item.Id,
                  qualityControlTestId: qualityControlTest.Id);

        if (duplicatedItems.Any())
          throw new DuplicatedQualityControlTestImportanceDegreeException();


        AddQualityControlTestImportanceDegree(
                  testImportanceDegreeId: item.Id,
                  qualityControlTestId: qualityControlTest.Id);
      }
      #endregion
      #region DeleteQualityControlTestOperation
      foreach (var item in deleteTestOperations)
      {
        DeleteQualityControlTestOperation(
                  testOperationId: item.TestOperationId,
                  qualityControlTestId: item.QualityControlTestId);
      }
      #endregion

      #region AddQualityControlTestOperation
      foreach (var item in addTestOperations)
      {
        var duplicatedItems = GetQualityControlTestOperations(
                  selector: e => e,
                  testOperationId: item.Id,
                  qualityControlTestId: qualityControlTest.Id);

        if (duplicatedItems.Any())
          throw new DuplicatedQualityControlTestOperationException();


        AddQualityControlTestOperation(
                  testOperationId: item.Id,
                  qualityControlTestId: qualityControlTest.Id);
      }
      #endregion

      #region EditQualityControlTest
      qualityControlTest = EditQualityControlTest(
          id: qualityControlTest.Id,
          rowVersion: qualityControlTest.RowVersion,
          code: code,
          name: name,
          description: description);
      #endregion


      return qualityControlTest;
    }
    #endregion

    #region Edit
    public QualityControlTest EditQualityControlTest(
        long id,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null)
    {

      var qualityControlTest = GetQualityControlTest(id: id);
      if (code != null)
        qualityControlTest.Code = code;
      if (name != null)
        qualityControlTest.Name = name;
      if (description != null)
        qualityControlTest.Description = description;
      repository.Update(rowVersion: rowVersion, entity: qualityControlTest);
      return qualityControlTest;
    }
    #endregion

    #region Remove
    public void RemoveQualityControlTest(int id)
    {

      var qualityControlTest = GetQualityControlTest(id: id);

      DeleteQualityControlTest(id: (int)qualityControlTest.Id);
    }
    #endregion

    #region Delete
    public void DeleteQualityControlTest(int id)
    {

      var qualityControlTest = GetQualityControlTest(id: id);
      repository.Delete(qualityControlTest);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<QualityControlTestResult> SortQualityControlTestResult(
        IQueryable<QualityControlTestResult> query,
        SortInput<QualityControlTestSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlTestSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case QualityControlTestSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        case QualityControlTestSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<QualityControlTestResult> SearchQualityControlTestResult(
        IQueryable<QualityControlTestResult> query,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where item.Name.Contains(search)
                select item;
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<QualityControlTest, QualityControlTestResult>> ToQualityControlTestResult =
        qualityControlTest => new QualityControlTestResult
        {
          Id = qualityControlTest.Id,
          Code = qualityControlTest.Code,
          Name = qualityControlTest.Name,
          Description = qualityControlTest.Description,
          QualityControlTestConditions = qualityControlTest.QualityControlTestConditions.AsQueryable().Select(App.Internals.QualityControl.ToQualityControlTestConditionResult),
          QualityControlTestEquipments = qualityControlTest.QualityControlTestEquipments.AsQueryable().Select(App.Internals.QualityControl.ToQualityControlTestEquipmentResult),
          QualityControlTestImportanceDegrees = qualityControlTest.QualityControlTestImportanceDegrees.AsQueryable().Select(App.Internals.QualityControl.ToQualityControlTestImportanceDegreeResult),
          QualityControlTestOperations = qualityControlTest.QualityControlTestOperations.AsQueryable().Select(App.Internals.QualityControl.ToQualityControlTestOperationResult),
          RowVersion = qualityControlTest.RowVersion
        };
    #endregion
  }
}
