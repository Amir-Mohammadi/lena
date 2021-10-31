using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.UserManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.UserManagement.DepartmentManager;

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
    public IQueryable<TResult> GetDepartmentManagers<TResult>(
        Expression<Func<DepartmentManager, TResult>> selector,
        TValue<int> id = null,
        TValue<int> departmentId = null,
        TValue<int> organizationPostId = null,
        TValue<DateTime> dateTime = null
        )
    {

      var query = repository.GetQuery<DepartmentManager>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (departmentId != null)
        query = query.Where(i => i.Department.Id == departmentId);
      if (organizationPostId != null)
        query = query.Where(i => i.OrganizationPost.Id == organizationPostId);
      if (dateTime != null)
        query = query.Where(i => i.DateTime <= dateTime);





      return query.Select(selector);
    }
    #endregion

    #region Get
    public DepartmentManager GetDepartmentManager(int id) => GetDepartmentManager(selector: e => e, id: id);
    public TResult GetDepartmentManager<TResult>(Expression<Func<DepartmentManager, TResult>> selector, int id)
    {

      var manager = GetDepartmentManagers(
               selector: selector,
                id: id).FirstOrDefault();
      if (manager == null)
        throw new DepartmentManagerNotFoundException(id);
      return manager;
    }
    #endregion

    #region Add
    public DepartmentManager AddDepartmentManager(
        int id,
        short departmentId,
        int organizationPostId)
    {

      var manager = repository.Create<DepartmentManager>();
      manager.Id = id;
      manager.Department = App.Internals.ApplicationBase.GetDepartment(
                id: departmentId); ; manager.OrganizationPost = App.Internals.UserManagement.GetOrganizationPost(id: organizationPostId);
      manager.UserId = App.Providers.Security.CurrentLoginData.UserId;
      manager.DateTime = DateTime.UtcNow;

      repository.Add(manager);
      return manager;
    }
    #endregion

    #region Delete
    public void DeleteDepartmentManager(int id)
    {

      var data = GetDepartmentManager(id: id);
      var departmentmanagers = GetDepartmentManagers(selector: e => e, id: id);



      repository.Delete(data);
    }
    #endregion

    #region ToResult
    public Expression<Func<DepartmentManager, DepartmentManagerResult>> ToDepartmentManagerResult =
       result => new DepartmentManagerResult()
       {
         Id = result.Id,
         DepartmentId = result.Department.Id,
         DepartmentName = result.Department.Name,
         OrganizationPostId = result.OrganizationPost.Id,
         OrganizationPostName = result.OrganizationPost.Title,
         UserId = result.User.Id,
         UserName = result.User.Employee.FirstName + " " + result.User.Employee.LastName,
         DateTime = result.DateTime,
         RowVersion = result.RowVersion

       };

    #endregion

    #region Search
    public IQueryable<DepartmentManagerResult> SearchDepartmentManager(
        IQueryable<DepartmentManagerResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems,
       TValue<int> departmentId,
       TValue<int> organizationPostId)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                item.DepartmentName.Contains(searchText) ||
                item.OrganizationPostName.Contains(searchText) ||
                item.UserName.Contains(searchText)

                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      if (departmentId != null)
        query = query.Where(i => i.DepartmentId == departmentId);
      if (organizationPostId != null)
        query = query.Where(i => i.OrganizationPostId == organizationPostId);



      return query;
    }

    #endregion

    #region Sort
    public IOrderedQueryable<DepartmentManagerResult> SortDepartmentManagerResult(IQueryable<DepartmentManagerResult> query, SortInput<DepartmentManagerSortType> sort)
    {
      switch (sort.SortType)
      {
        case DepartmentManagerSortType.UserId:
          return query.OrderBy(a => a.UserId, sort.SortOrder);
        case DepartmentManagerSortType.UserName:
          return query.OrderBy(a => a.UserName, sort.SortOrder);
        case DepartmentManagerSortType.DepartmentId:
          return query.OrderBy(a => a.DepartmentId, sort.SortOrder);
        case DepartmentManagerSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);
        case DepartmentManagerSortType.OrganizationPostId:
          return query.OrderBy(a => a.OrganizationPostId, sort.SortOrder);
        case DepartmentManagerSortType.OrganizationPostName:
          return query.OrderBy(a => a.OrganizationPostName, sort.SortOrder);
        case DepartmentManagerSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Edit
    public DepartmentManager EditDepartmentManager(
       int id,
        byte[] rowVersion,
        TValue<short> departmentId = null,
        TValue<int> organizationPostId = null)
    {

      var departmentManager = GetDepartmentManager(id: id);
      return EditDepartmentManager(
                   departmentManager: departmentManager,
                    departmentId: departmentId,
                    organizationPostId: organizationPostId);
    }

    public DepartmentManager EditDepartmentManager(
        DepartmentManager departmentManager,
        TValue<short> departmentId = null,
        TValue<int> organizationPostId = null)
    {

      if (departmentId != null)
      {
        departmentManager.Department = App.Internals.ApplicationBase.GetDepartment(id: departmentId);
      }
      if (organizationPostId != null)
      {
        departmentManager.OrganizationPost = App.Internals.UserManagement.GetOrganizationPost(id: organizationPostId);
      }



      repository.Update(entity: departmentManager, rowVersion: departmentManager.RowVersion);
      return departmentManager;
    }

    #endregion
  }
}

