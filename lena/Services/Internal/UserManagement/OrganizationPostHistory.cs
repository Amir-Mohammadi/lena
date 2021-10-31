using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.UserManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.UserManagement.OganizationPostHistory;
using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {
    #region Gets
    public IQueryable<TResult> GetOrganizationPostHistories<TResult>(
        Expression<Func<OrganizationPostHistory, TResult>> selector,
        TValue<int> id = null,
        TValue<int> employeeId = null,
          TValue<int> organizationPostId = null,
          TValue<string> description = null,
          TValue<DateTime> startDate = null,
          TValue<int> creatorId = null

    )
    {

      var query = repository.GetQuery<OrganizationPostHistory>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (employeeId != null)
        query = query.Where(i => i.EmployeeId == employeeId);
      if (organizationPostId != null)
        query = query.Where(i => i.OrganizationPostId == organizationPostId);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (startDate != null)
        query = query.Where(i => i.StartDate == startDate);
      if (creatorId != null)
        query = query.Where(i => i.CreatorId == creatorId);

      return query.Select(selector);
    }
    #endregion
    #region Get
    public OrganizationPostHistory GetOrganizationPostHistory(int id) => GetOrganizationPostHistory(selector: e => e, id: id);
    public TResult GetOrganizationPostHistory<TResult>(Expression<Func<OrganizationPostHistory, TResult>> selector, int id)
    {

      var postHistory = GetOrganizationPostHistories(
                selector: selector,
                id: id).SingleOrDefault();
      if (postHistory == null)
        throw new OrganizationPostHistoryNotFoundException(id);
      return postHistory;
    }
    #endregion
    #region Add
    public OrganizationPostHistory AddOrganizationPostHistory(
    int employeeId,
    int organizationPostId,
    string description,
    DateTime startDate)
    {

      var postHistory = repository.Create<OrganizationPostHistory>();
      postHistory.EmployeeId = employeeId;
      postHistory.OrganizationPostId = organizationPostId;
      postHistory.Description = description;
      postHistory.StartDate = startDate;
      postHistory.CreationTime = DateTime.UtcNow;
      postHistory.CreatorId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Add(postHistory);
      return postHistory;
    }
    #endregion
    #region Delete
    public void DeleteOrganizationPostHistory(int id)
    {

      var post = GetOrganizationPostHistory(id);
      repository.Delete(post);
    }
    #endregion


    #region ToResult
    public Expression<Func<OrganizationPostHistory, OrganizationPostHistoryResult>> ToOrganizationPostHistoryResult =
       result => new OrganizationPostHistoryResult()
       {
         Id = result.Id,
         EmployeeCode = result.Employee.Code,
         EmployeeId = result.EmployeeId,
         EmployeeFullName = result.Employee.FirstName + " " + result.Employee.LastName,
         OrganizationPostId = result.OrganizationPostId,
         OrganizationPostName = result.OrganizationPost.Title,
         StartDate = result.StartDate,
         Description = result.Description,
         RowVersion = result.RowVersion,
         CreationTime = result.CreationTime,
         CreatorId = result.CreatorId,
         CreatorFullName = result.Creator.Employee.FirstName + " " + result.Creator.Employee.LastName,
       };

    #endregion
    #region Search
    public IQueryable<OrganizationPostHistoryResult> SearchOrganizationPostHistory(
        IQueryable<OrganizationPostHistoryResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                item.OrganizationPostName.Contains(searchText) ||
                item.EmployeeFullName.Contains(searchText) ||
                item.Description.Contains(searchText) ||
                item.EmployeeCode.Contains(searchText)

                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }

    #endregion
    #region SortResult
    public IOrderedQueryable<OrganizationPostHistoryResult> SortOrganizationPostHistoryResult(IQueryable<OrganizationPostHistoryResult> query, SortInput<OrganizationPostHistorySortType> sort)
    {
      switch (sort.SortType)
      {
        case OrganizationPostHistorySortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case OrganizationPostHistorySortType.EmployeeCode:
          return query.OrderBy(a => a.EmployeeCode, sort.SortOrder);
        case OrganizationPostHistorySortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case OrganizationPostHistorySortType.OrganizationPostName:
          return query.OrderBy(a => a.OrganizationPostName, sort.SortOrder);
        case OrganizationPostHistorySortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case OrganizationPostHistorySortType.StartDate:
          return query.OrderBy(a => a.StartDate, sort.SortOrder);
        case OrganizationPostHistorySortType.CreatorFullName:
          return query.OrderBy(a => a.CreatorFullName, sort.SortOrder);
        case OrganizationPostHistorySortType.CreationTime:
          return query.OrderBy(a => a.CreationTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Edit
    public OrganizationPostHistory EditOrganizationPostHistory(
        int id,
        byte[] rowVersion,
        TValue<int> employeeId = null,
        TValue<int> organizationPostId = null,
        TValue<string> description = null,
        TValue<DateTime> startDate = null)
    {

      var postHistory = GetOrganizationPostHistory(id: id);
      if (employeeId != null)
        postHistory.EmployeeId = employeeId;
      if (organizationPostId != null)
        postHistory.OrganizationPostId = organizationPostId;
      if (description != null)
        postHistory.Description = description;
      if (startDate != null)
        postHistory.StartDate = startDate;
      repository.Update(postHistory, rowVersion);
      return postHistory;
    }
    #endregion
  }
}
