using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.ApplicationBase.StuffDefinitionRequests;
using lena.Models.Common;
using System;
//using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase
{
  public partial class ApplicationBase
  {
    #region Add
    public StuffDefinitionRequest AddStuffDefinitionRequest(
        string name,
        string noun,
        string title,
        string description,
        byte unitTypeId,
        StuffType stuffType,
        int stuffPurchaseCategoryId
        )
    {
      var request = repository.Create<StuffDefinitionRequest>();
      request.Name = name;
      request.Noun = noun;
      request.Title = title;
      request.Description = description;
      request.UnitTypeId = unitTypeId;
      request.StuffType = stuffType;
      request.DefinitionStatus = StuffDefinitionStatus.Requested;
      request.StuffPurchaseCategoryId = stuffPurchaseCategoryId;
      request.UserId = App.Providers.Security.CurrentLoginData.UserId;
      request.DateTime = DateTime.UtcNow;
      repository.Add(request);
      return request;
    }
    #endregion
    #region Edit
    public StuffDefinitionRequest EditStuffDefinitionRequest(
        byte[] rowVersion,
        int id,
        TValue<string> name = null,
        TValue<string> noun = null,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<StuffType> stuffType = null,
        TValue<byte> unitTypeId = null,
        TValue<int> stuffPurchaseCategoryId = null,
        TValue<StuffDefinitionStatus> definitionStatus = null,
        TValue<string> confirmDescription = null
        )
    {
      var request = GetStuffDefinitionRequest(id: id);
      if (name != null)
        request.Name = name;
      if (noun != null)
        request.Noun = noun;
      if (title != null)
        request.Title = title;
      if (description != null)
        request.Description = description;
      if (unitTypeId != null)
        request.UnitTypeId = unitTypeId;
      if (stuffType != null)
        request.StuffType = stuffType;
      if (definitionStatus != null)
      {
        request.DefinitionStatus = definitionStatus;
        if (definitionStatus == StuffDefinitionStatus.Confirmed || definitionStatus == StuffDefinitionStatus.Rejected)
        {
          request.ConfirmDateTime = DateTime.UtcNow;
          request.ConfirmerUserId = App.Providers.Security.CurrentLoginData.UserId;
        }
      }
      if (confirmDescription != null)
        request.ConfirmDescription = confirmDescription;
      if (stuffPurchaseCategoryId != null)
        request.StuffPurchaseCategoryId = stuffPurchaseCategoryId;
      repository.Update(entity: request, rowVersion: request.RowVersion);
      return request;
    }
    #endregion
    #region Delete
    public void DeleteStuffDefinitionRequest(int id)
    {

      var stuff = GetStuffDefinitionRequest(id: id);
      repository.Delete(stuff);
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStuffDefinitionRequests<TResult>(
      Expression<Func<StuffDefinitionRequest, TResult>> selector,
      TValue<int> id = null,
      TValue<string> name = null,
      TValue<string> description = null,
      TValue<int> stuffCategoryId = null,
      TValue<StuffType> stuffType = null,
      TValue<int> unitTypeId = null,
      TValue<int> stuffPurchaseCategoryId = null,
      TValue<StuffDefinitionStatus> definitionStatus = null,
      TValue<string> stuffCode = null)
    {
      var query = repository.GetQuery<StuffDefinitionRequest>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (name != null)
        query = query.Where(i => i.Name == name);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (unitTypeId != null)
        query = query.Where(i => i.UnitTypeId == unitTypeId);
      if (stuffType != null)
        query = query.Where(i => i.StuffType == stuffType);
      if (stuffPurchaseCategoryId != null)
        query = query.Where(i => i.StuffPurchaseCategoryId == stuffPurchaseCategoryId);
      if (definitionStatus != null)
        query = query.Where(i => i.DefinitionStatus == definitionStatus);
      if (stuffCode != null)
        query = query.Where(i => i.Stuff.Code == stuffCode);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public StuffDefinitionRequest GetStuffDefinitionRequest(int id) => GetStuffDefinitionRequest(selector: e => e, id: id);
    public TResult GetStuffDefinitionRequest<TResult>(Expression<Func<StuffDefinitionRequest, TResult>> selector, int id)
    {
      var stuff = GetStuffDefinitionRequests(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (stuff == null)
        throw new StuffDefinitionRequestNotFoundException(id);
      return stuff;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StuffDefinitionRequestResult> SortStuffDefinitionRequestResult(IQueryable<StuffDefinitionRequestResult> input,
        SortInput<StuffDefinitionRequestSortType> options)
    {
      switch (options.SortType)
      {
        case StuffDefinitionRequestSortType.Id:
          return input.OrderBy(i => i.Id, options.SortOrder);
        case StuffDefinitionRequestSortType.StuffCode:
          return input.OrderBy(i => i.StuffCode, options.SortOrder);
        case StuffDefinitionRequestSortType.Name:
          return input.OrderBy(i => i.Name, options.SortOrder);
        case StuffDefinitionRequestSortType.Noun:
          return input.OrderBy(i => i.Noun, options.SortOrder);
        case StuffDefinitionRequestSortType.Title:
          return input.OrderBy(i => i.Title, options.SortOrder);
        case StuffDefinitionRequestSortType.StuffPurchaseCategoryName:
          return input.OrderBy(i => i.StuffPurchaseCategoryName, options.SortOrder);
        case StuffDefinitionRequestSortType.StuffTypeName:
          return input.OrderBy(i => i.StuffType, options.SortOrder);
        case StuffDefinitionRequestSortType.UnitTypeName:
          return input.OrderBy(i => i.UnitTypeName, options.SortOrder);
        case StuffDefinitionRequestSortType.DefinitionStatus:
          return input.OrderBy(i => i.DefinitionStatus, options.SortOrder);
        case StuffDefinitionRequestSortType.Description:
          return input.OrderBy(i => i.Description, options.SortOrder);
        case StuffDefinitionRequestSortType.DateTime:
          return input.OrderBy(i => i.DateTime, options.SortOrder);
        case StuffDefinitionRequestSortType.EmployeeFullName:
          return input.OrderBy(i => i.EmployeeFullName, options.SortOrder);
        case StuffDefinitionRequestSortType.ConfirmationDescription:
          return input.OrderBy(i => i.ConfirmationDescription, options.SortOrder);
        case StuffDefinitionRequestSortType.ConfirmDateTime:
          return input.OrderBy(i => i.ConfirmDateTime, options.SortOrder);
        case StuffDefinitionRequestSortType.ConfirmerFullName:
          return input.OrderBy(i => i.ConfirmerFullName, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<StuffDefinitionRequestComboResult> SortStuffDefinitionRequestComboResult(IQueryable<StuffDefinitionRequestComboResult> input,
        SortInput<StuffDefinitionRequestSortType> options)
    {
      switch (options.SortType)
      {
        case StuffDefinitionRequestSortType.Name:
          return input.OrderBy(i => i.Name, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    public Expression<Func<StuffDefinitionRequest, StuffDefinitionRequestResult>> ToStuffDefinitionRequestResult =>
    request => new StuffDefinitionRequestResult
    {
      Id = request.Id,
      StuffCode = request.Stuff.Code,
      Name = request.Name,
      Noun = request.Noun,
      Title = request.Title,
      UnitTypeName = request.UnitType.Name,
      Description = request.Description,
      UnitTypeId = request.UnitTypeId,
      StuffType = request.StuffType,
      StuffPurchaseCategoryId = request.StuffPurchaseCategoryId,
      StuffPurchaseCategoryName = request.StuffPurchaseCategory.Title,
      DefinitionStatus = request.DefinitionStatus,
      DateTime = request.DateTime,
      UserId = request.UserId,
      UserName = request.User.UserName,
      EmployeeFullName = request.User.Employee.FirstName + " " + request.User.Employee.LastName,
      ConfirmationDescription = request.ConfirmDescription,
      ConfirmDateTime = request.ConfirmDateTime,
      ConfirmerFullName = request.ConfirmerUser.Employee.FirstName + " " + request.ConfirmerUser.Employee.LastName,
      ConfirmerUserId = request.UserId,
      RowVersion = request.RowVersion
    };
    #endregion
    #region ToStuffDefinitionRequestComboResult
    public Expression<Func<StuffDefinitionRequest, StuffDefinitionRequestComboResult>> ToStuffDefinitionRequestComboResult =
        request => new StuffDefinitionRequestComboResult()
        {
          Id = request.Id,
          Name = request.Name,
          RowVersion = request.RowVersion
        };
    #endregion
    #region Search
    public IQueryable<StuffDefinitionRequestResult> SearchStuffDefinitionRequestResultQuery(
        IQueryable<StuffDefinitionRequestResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from stuff in query
                where
                      stuff.Name.Contains(searchText) ||
                      stuff.Title.Contains(searchText) ||
                      stuff.UnitTypeName.Contains(searchText) ||
                      stuff.Noun.Contains(searchText) ||
                      stuff.Description.Contains(searchText)
                select stuff;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    public IQueryable<StuffDefinitionRequestComboResult> SearchStuffDefinitionRequestComboResultQuery(
        IQueryable<StuffDefinitionRequestComboResult> query,
        string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from stuff in query
                where
                      stuff.Name.Contains(searchText)
                select stuff;
      return query;
    }
    #endregion
  }
}