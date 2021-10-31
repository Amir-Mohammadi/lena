using System.Linq;
//using lena.Services.Core.Foundation.Action;
using lena.Models.Common;
using lena.Domains;
using System.Linq.Expressions;
using System;
using lena.Models.Common;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Domains.Enums.SortTypes;
using lena.Models.QualityControl.StuffQualityControlTest;
using lena.Services.Core;
using lena.Models;
using System.Collections.Generic;
using lena.Domains.Enums;
using lena.Models.QualityControl.StuffQualityControlTestCondition;
using lena.Models.QualityControl;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region SaveProccess
    public void SaveStuffQualityControlTests(
        int stuffId,
        AddStuffQualityControlTestDocument[] addQualityControlTestInputs,
        EditStuffQualityControlTestDocument[] editQualityControlTestInputs,
        string correctiveReaction,
        string measurementMethod,
        string frequency,
        long[] deleteQualityControlTestIds)
    {
      foreach (var deleteQualityControlTestId in deleteQualityControlTestIds)
      {
        #region GetStuffQualityControlTestConditions
        var stuffQualityControlTestConditions = GetStuffQualityControlTestConditions(
            e => e,
            stuffId: stuffId,
            qualityControlTestId: deleteQualityControlTestId);
        #endregion
        #region RemoveStuffQualityControlTestCondition
        foreach (var item in stuffQualityControlTestConditions.ToList())
        {
          RemoveStuffQualityControlTestCondition(item);
        }
        #endregion
        #region GetStuffQualityControlTestEquipments
        var stuffQualityControlTestEquipments = GetStuffQualityControlTestEquipments(
            e => e,
            stuffId: stuffId,
            qualityControlTestId: deleteQualityControlTestId);
        #endregion
        #region RemoveStuffQualityControlTestEquipment
        foreach (var item in stuffQualityControlTestEquipments.ToList())
        {
          RemoveStuffQualityControlTestEquipment(item);
        }
        #endregion
        #region GetStuffQualityControlTestImportanceDegrees
        var stuffQualityControlTestImportanceDegrees = GetStuffQualityControlTestImportanceDegrees(
            e => e,
            stuffId: stuffId,
            qualityControlTestId: deleteQualityControlTestId);
        #endregion
        #region RemoveStuffQualityControlTestImportanceDegree
        foreach (var item in stuffQualityControlTestImportanceDegrees.ToList())
        {
          RemoveStuffQualityControlTestImportanceDegree(item);
        }
        #endregion
        #region GetStuffQualityControlTestOperations
        var stuffQualityControlTestOperations = GetStuffQualityControlTestOperations(
            e => e,
            stuffId: stuffId,
            qualityControlTestId: deleteQualityControlTestId);
        #endregion
        #region RemoveStuffQualityControlTestOperation
        foreach (var item in stuffQualityControlTestOperations.ToList())
        {
          RemoveStuffQualityControlTestOperation(item);
        }
        #endregion
        #region DeleteStuffQualityControlTest
        DeleteStuffQualityControlTest(
                stuffId: stuffId,
                qualityControlTestId: deleteQualityControlTestId);
        #endregion
      }
      foreach (var input in addQualityControlTestInputs)
      {
        var duplicatedQualityControlTests = GetStuffQualityControlTests(
                  selector: e => e,
                  stuffId: stuffId,
                  qualityControlTestId: input.QualityControlTestId);
        if (duplicatedQualityControlTests.Any())
          throw new DuplicatedQualityControlTestException();
        AddStuffQualityControlTest(
                      stuffId: stuffId,
                      qualityControlTestId: input.QualityControlTestId,
                      fileKey: input.FileKey,
                      measurementMethod: measurementMethod,
                      frequency: frequency,
                      correctiveReaction: correctiveReaction
                      );
        #region AddStuffQualityControlTestCondition
        foreach (var item in input.AddStuffQualityControlTestConditionInputs)
        {
          AddStuffQualityControlTestCondition(
                    stuffId: item.StuffId,
                    qualityControlTestId: item.QualityControlTestId,
                    qualityControlConditionTestConditionId: item.QualityControlConditionTestConditionId,
                    qualityControlTestConditionQualityControlTestId: item.QualityControlTestConditionQualityControlTestId,
                     min: item.Min,
                     max: item.Max,
                     qualityControlTestUnitId: item.QualityControlTestUnitId,
                     description: item.Description,
                     acceptanceLimit: item.AcceptanceLimit,
                     toleranceType: item.ToleranceType);
        }
        #endregion
        #region AddStuffQualityControlTestEquipment
        foreach (var item in input.AddStuffQualityControlTestEquipmentInputs)
        {
          AddStuffQualityControlTestEquipment(
                    stuffId: item.StuffId,
                    qualityControlTestId: item.QualityControlTestId,
                    qualityControlEquipmentTestEquipmentId: item.QualityControlEquipmentTestEquipmentId,
                    qualityControlTestEquipmentQualityControlTestId: item.QualityControlTestEquipmentQualityControlTestId);
        }
        #endregion
        #region AddStuffQualityControlTestImportancedegree
        if (input.AddStuffQualityControlTestImportanceDegreeInputs != null)
        {
          foreach (var item in input.AddStuffQualityControlTestImportanceDegreeInputs)
          {
            AddStuffQualityControlTestImportanceDegree(
                      stuffId: item.StuffId,
                      qualityControlTestId: item.QualityControlTestId,
                      qualityControlImportanceDegreeTestImportanceDegreeId: item.QualityControlImportanceDegreeTestImportanceDegreeId,
                      qualityControlTestImportanceDegreeQualityControlTestId: item.QualityControlTestImportanceDegreeQualityControlTestId);
          }
        }
        #endregion
        #region AddStuffQualityControlTestOperation
        foreach (var item in input.AddStuffQualityControlTestOperationInputs)
        {
          AddStuffQualityControlTestOperation(
                    stuffId: item.StuffId,
                    qualityControlTestId: item.QualityControlTestId,
                    qualityControlOperationTestOperationId: item.QualityControlOperationTestOperationId,
                    qualityControlTestOperationQualityControlTestId: item.QualityControlTestOperationQualityControlTestId);
        }
        #endregion
      }
      foreach (var input in editQualityControlTestInputs)
      {
        EditStuffQualityControlTestProcess(
                      stuffId: stuffId,
                      qualityControlTestId: input.QualityControlTestId,
                      fileKey: input.FileKey,
                      measurementMethod: measurementMethod,
                      frequency: frequency,
                      correctiveReaction: correctiveReaction,
                      addStuffQualityControlTestConditionInputs: input.AddStuffQualityControlTestConditionInputs,
                      deleteStuffQualityControlTestConditionInputs: input.DeleteStuffQualityControlTestConditionInputs,
                      addStuffQualityControlTestEquipmentInputs: input.AddStuffQualityControlTestEquipmentInputs,
                      deleteStuffQualityControlTestEquipmentInputs: input.DeleteStuffQualityControlTestEquipmentInputs,
                      addStuffQualityControlTestImportanceDegreeInputs: input.AddStuffQualityControlTestImportanceDegreeInputs,
                      deleteStuffQualityControlTestImportanceDegreeInputs: input.DeleteStuffQualityControlTestImportanceDegreeInputs,
                      addStuffQualityControlTestOperationInputs: input.AddStuffQualityControlTestOperationInputs,
                      deleteStuffQualityControlTestOperationInputs: input.DeleteStuffQualityControlTestOperationInputs
                      );
      }
    }
    #endregion
    #region Add
    public StuffQualityControlTest AddStuffQualityControlTest(
        int stuffId,
        long qualityControlTestId,
        string fileKey,
        string measurementMethod,
        string frequency,
        string correctiveReaction)
    {
      Document document = null;
      UploadFileData uploadFileData = null;
      if (!string.IsNullOrWhiteSpace(fileKey))
        uploadFileData = App.Providers.Session.GetAs<UploadFileData>(fileKey);
      if (uploadFileData != null)
      {
        document = App.Internals.ApplicationBase.AddDocument(name: uploadFileData.FileName, fileStream: uploadFileData.FileData);
      }
      var entity = repository.Create<StuffQualityControlTest>();
      entity.StuffId = stuffId;
      entity.QualityControlTestId = qualityControlTestId;
      entity.DocumentId = document?.Id;
      entity.MeasurementMethod = measurementMethod;
      entity.Frequency = frequency;
      entity.CorrectiveReaction = correctiveReaction;
      repository.Add(entity);
      return entity;
    }
    #endregion
    #region Edit Process
    public StuffQualityControlTest EditStuffQualityControlTestProcess(
        int stuffId,
        long qualityControlTestId,
        string fileKey,
        string measurementMethod,
        string frequency,
        string correctiveReaction,
        AddStuffQualityControlTestConditionInput[] addStuffQualityControlTestConditionInputs,
        DeleteStuffQualityControlTestConditionInput[] deleteStuffQualityControlTestConditionInputs,
        AddStuffQualityControlTestEquipmentInput[] addStuffQualityControlTestEquipmentInputs,
        DeleteStuffQualityControlTestEquipmentInput[] deleteStuffQualityControlTestEquipmentInputs,
        AddStuffQualityControlTestImportanceDegreeInput[] addStuffQualityControlTestImportanceDegreeInputs,
        DeleteStuffQualityControlTestImportanceDegreeInput[] deleteStuffQualityControlTestImportanceDegreeInputs,
        AddStuffQualityControlTestOperationInput[] addStuffQualityControlTestOperationInputs,
        DeleteStuffQualityControlTestOperationInput[] deleteStuffQualityControlTestOperationInputs)
    {
      var stuffQcTest = EditStuffQualityControlTest(
                        stuffId: stuffId,
                        qualityControlTestId: qualityControlTestId,
                        fileKey: fileKey,
                        measurementMethod: measurementMethod,
                        frequency: frequency,
                        correctiveReaction: correctiveReaction);
      #region DeleteStuffQualityControlTestCondition
      foreach (var item in deleteStuffQualityControlTestConditionInputs)
      {
        DeleteStuffQualityControlTestCondition(
                   stuffId: item.StuffId,
                   qualityControlTestId: item.QualityControlTestId,
                   qualityControlConditionTestConditionId: item.QualityControlConditionTestConditionId,
                   qualityControlTestConditionQualityControlTestId: item.QualityControlTestConditionQualityControlTestId);
      }
      #endregion
      #region AddStuffQualityControlTestCondition
      foreach (var item in addStuffQualityControlTestConditionInputs)
      {
        AddStuffQualityControlTestCondition(
                   stuffId: item.StuffId,
                   qualityControlTestId: item.QualityControlTestId,
                   qualityControlConditionTestConditionId: item.QualityControlConditionTestConditionId,
                   qualityControlTestConditionQualityControlTestId: item.QualityControlTestConditionQualityControlTestId,
                    min: item.Min,
                    max: item.Max,
                    qualityControlTestUnitId: item.QualityControlTestUnitId,
                    description: item.Description,
                    acceptanceLimit: item.AcceptanceLimit,
                    toleranceType: item.ToleranceType);
      }
      #endregion
      #region DeleteStuffQualityControlTestEquipment
      foreach (var item in deleteStuffQualityControlTestEquipmentInputs)
      {
        DeleteStuffQualityControlTestEquipment(
                   stuffId: item.StuffId,
                   qualityControlTestId: item.QualityControlTestId,
                   qualityControlEquipmentTestEquipmentId: item.QualityControlEquipmentTestEquipmentId,
                   qualityControlTestEquipmentQualityControlTestId: item.QualityControlTestEquipmentQualityControlTestId);
      }
      #endregion
      #region AddStuffQualityControlTestEquipment
      foreach (var item in addStuffQualityControlTestEquipmentInputs)
      {
        AddStuffQualityControlTestEquipment(
                   stuffId: item.StuffId,
                   qualityControlTestId: item.QualityControlTestId,
                   qualityControlEquipmentTestEquipmentId: item.QualityControlEquipmentTestEquipmentId,
                   qualityControlTestEquipmentQualityControlTestId: item.QualityControlTestEquipmentQualityControlTestId);
      }
      #endregion
      #region DeleteStuffQualityControlTestImportanceDegree
      foreach (var item in deleteStuffQualityControlTestImportanceDegreeInputs)
      {
        DeleteStuffQualityControlTestImportanceDegree(
                   stuffId: item.StuffId,
                   qualityControlTestId: item.QualityControlTestId,
                   qualityControlImportanceDegreeTestImportanceDegreeId: item.QualityControlImportanceDegreeTestImportanceDegreeId,
                   qualityControlTestImportanceDegreeQualityControlTestId: item.QualityControlTestImportanceDegreeQualityControlTestId);
      }
      #endregion
      #region AddStuffQualityControlTestImportanceDegree
      foreach (var item in addStuffQualityControlTestImportanceDegreeInputs)
      {
        AddStuffQualityControlTestImportanceDegree(
                   stuffId: item.StuffId,
                   qualityControlTestId: item.QualityControlTestId,
                   qualityControlImportanceDegreeTestImportanceDegreeId: item.QualityControlImportanceDegreeTestImportanceDegreeId,
                   qualityControlTestImportanceDegreeQualityControlTestId: item.QualityControlTestImportanceDegreeQualityControlTestId);
      }
      #endregion
      #region DeleteStuffQualityControlTestOperation
      foreach (var item in deleteStuffQualityControlTestOperationInputs)
      {
        DeleteStuffQualityControlTestOperation(
                   stuffId: item.StuffId,
                   qualityControlTestId: item.QualityControlTestId,
                   qualityControlOperationTestOperationId: item.QualityControlOperationTestOperationId,
                   qualityControlTestOperationQualityControlTestId: item.QualityControlTestOperationQualityControlTestId);
      }
      #endregion
      #region AddStuffQualityControlTestOperation
      foreach (var item in addStuffQualityControlTestOperationInputs)
      {
        AddStuffQualityControlTestOperation(
                   stuffId: item.StuffId,
                   qualityControlTestId: item.QualityControlTestId,
                   qualityControlOperationTestOperationId: item.QualityControlOperationTestOperationId,
                   qualityControlTestOperationQualityControlTestId: item.QualityControlTestOperationQualityControlTestId);
      }
      #endregion
      return stuffQcTest;
    }
    #endregion
    #region Edit
    public StuffQualityControlTest EditStuffQualityControlTest(
        int stuffId,
        long qualityControlTestId,
        string fileKey,
        string measurementMethod,
        string frequency,
        string correctiveReaction)
    {
      Document document = null;
      UploadFileData uploadFileData = null;
      if (!string.IsNullOrWhiteSpace(fileKey))
        uploadFileData = App.Providers.Session.GetAs<UploadFileData>(fileKey);
      if (uploadFileData != null)
      {
        document = App.Internals.ApplicationBase.AddDocument(name: uploadFileData.FileName, fileStream: uploadFileData.FileData);
      }
      var qcTest = GetStuffQualityControlTest(
                        stuffId: stuffId,
                        qualityControlTestId: qualityControlTestId);
      if (document != null && document.Id != null)
        qcTest.DocumentId = document.Id;
      if (measurementMethod != null)
        qcTest.MeasurementMethod = measurementMethod;
      if (frequency != null)
        qcTest.Frequency = frequency;
      if (correctiveReaction != null)
        qcTest.CorrectiveReaction = correctiveReaction;
      repository.Update(entity: qcTest, rowVersion: qcTest.RowVersion);
      return qcTest;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStuffQualityControlTests<TResult>(
        Expression<Func<StuffQualityControlTest, TResult>> selector,
        TValue<int> stuffId = null,
        TValue<long> qualityControlTestId = null)
    {
      var query = repository.GetQuery<StuffQualityControlTest>();
      if (stuffId != null)
        query = query.Where(x => x.StuffId == stuffId);
      if (qualityControlTestId != null)
        query = query.Where(x => x.QualityControlTestId == qualityControlTestId);
      return query.Select(selector);
    }
    public StuffQualityControlTesterResult[] GetStuffQualityControlTesters(TValue<int[]> stuffIds)
    {
      var stuffs = repository.GetQuery<Stuff>()
            .Where(i => stuffIds.Value.Contains(i.Id))
            .Select(i => new { StuffId = i.Id, i.QualityControlDepartmentId });
      var result = new List<StuffQualityControlTesterResult>();
      foreach (var stuffId in stuffIds.Value)
      {
        var stuff = stuffs.FirstOrDefault(i => i.StuffId == stuffId);
        var deprtmentEmployees = App.Internals.UserManagement
              .GetEmployees(
                  selector: App.Internals.UserManagement.ToEmployeeComboResult,
                  departmentId: stuff.QualityControlDepartmentId)
              .ToArray();
        result.Add(new StuffQualityControlTesterResult()
        {
          StuffId = stuffId,
          Employees = deprtmentEmployees
        });
      }
      return result.ToArray();
    }
    #endregion
    #region Get
    public StuffQualityControlTest GetStuffQualityControlTest(
        int stuffId,
        long qualityControlTestId) => GetStuffQualityControlTest(
        selector: e => e,
        stuffId: stuffId,
        qualityControlTestId: qualityControlTestId);
    public TResult GetStuffQualityControlTest<TResult>(
        Expression<Func<StuffQualityControlTest, TResult>> selector,
        int stuffId,
        long qualityControlTestId)
    {
      var stuffQualityControlTest = GetStuffQualityControlTests(
                selector: selector,
                stuffId: stuffId,
                qualityControlTestId: qualityControlTestId)
                .FirstOrDefault();
      if (stuffQualityControlTest == null)
        throw new StuffQualityControlTestNotFoundException(stuffId: stuffId,
                  qualityControlTestId: qualityControlTestId);
      return stuffQualityControlTest;
    }
    #endregion
    #region Delete
    public void DeleteStuffQualityControlTest(
        int stuffId,
        long qualityControlTestId)
    {
      var stuffQualityControlTest = GetStuffQualityControlTest(
                    stuffId: stuffId,
                    qualityControlTestId: qualityControlTestId);
      repository.Delete(stuffQualityControlTest);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StuffQualityControlTestResult> SortStuffQualityControlTestResult(
        IQueryable<StuffQualityControlTestResult> query,
        SortInput<StuffQualityControlTestSortType> sort)
    {
      switch (sort.SortType)
      {
        case StuffQualityControlTestSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case StuffQualityControlTestSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case StuffQualityControlTestSortType.QualityControlTestName:
          return query.OrderBy(a => a.QualityControlTestName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<StuffQualityControlTestResult> SearchStuffQualityControlTestResult(
        IQueryable<StuffQualityControlTestResult> query,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                item.StuffCode.Contains(search) ||
                item.StuffName.Contains(search) ||
                item.QualityControlTestName.Contains(search) ||
                item.QualityControlTestDescription.Contains(search) ||
                item.MeasurementMethod.Contains(search) ||
                item.Frequency.Contains(search) ||
                item.CorrectiveReaction.Contains(search)
                select item;
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<StuffQualityControlTest, StuffQualityControlTestResult>> ToStuffQualityControlTestResult =
        stuffQualityControlTest => new StuffQualityControlTestResult
        {
          StuffId = stuffQualityControlTest.StuffId,
          StuffCode = stuffQualityControlTest.Stuff.Code,
          StuffName = stuffQualityControlTest.Stuff.Name,
          QualityControlTestId = stuffQualityControlTest.QualityControlTestId,
          QualityControlTestCode = stuffQualityControlTest.QualityControlTest.Code,
          QualityControlTestName = stuffQualityControlTest.QualityControlTest.Name,
          QualityControlTestDescription = stuffQualityControlTest.QualityControlTest.Description,
          MeasurementMethod = stuffQualityControlTest.MeasurementMethod,
          Frequency = stuffQualityControlTest.Frequency,
          CorrectiveReaction = stuffQualityControlTest.CorrectiveReaction,
          RowVersion = stuffQualityControlTest.RowVersion,
          DocumentId = stuffQualityControlTest.DocumentId,
          StuffQualityControlTestConditionResult = stuffQualityControlTest.StuffQualityControlTestConditions.AsQueryable().Select(App.Internals.QualityControl.ToStuffQualityControlTestConditionResult),
          StuffQualityControlTestEquipmentResult = stuffQualityControlTest.StuffQualityControlTestEquipments.AsQueryable().Select(App.Internals.QualityControl.ToStuffQualityControlTestEquipmentResult),
          StuffQualityControlTestImportanceDegreeResult = stuffQualityControlTest.StuffQualityControlTestImportanceDegrees.AsQueryable().Select(App.Internals.QualityControl.ToStuffQualityControlTestImportanceDegreeResult),
          StuffQualityControlTestOperationResult = stuffQualityControlTest.StuffQualityControlTestOperations.AsQueryable().Select(App.Internals.QualityControl.ToStuffQualityControlTestOperationResult),
        };
    #endregion
  }
}