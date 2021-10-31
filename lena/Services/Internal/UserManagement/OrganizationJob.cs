using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.UserManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.UserManagement.OrganizationJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {
    #region Gets
    public IQueryable<TResult> GetOrganizationJobs<TResult>(
        Expression<Func<OrganizationJob, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
          TValue<string> title = null,
          TValue<string> description = null,
          TValue<bool> isActive = null,
          TValue<int> organizationPostId = null


    )
    {

      var query = repository.GetQuery<OrganizationJob>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (code != null)
        query = query.Where(i => i.Code == code);
      if (title != null)
        query = query.Where(i => i.Title == title);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (isActive != null)
        query = query.Where(i => i.IsActive == isActive);
      if (organizationPostId != null)
        query = query.Where(i => i.OranizationPostId == organizationPostId);

      return query.Select(selector);
    }
    #endregion
    #region Get
    public OrganizationJob GetOrganizationJob(int id) => GetOrganizationJob(selector: e => e, id: id);
    public TResult GetOrganizationJob<TResult>(Expression<Func<OrganizationJob, TResult>> selector, int id)
    {

      var bank = GetOrganizationJobs(
                selector: selector,
                id: id).SingleOrDefault();
      if (bank == null)
        throw new OrganizationJobNotFoundException(id);
      return bank;
    }
    #endregion
    #region Add
    public OrganizationJob AddOrganizationJob(
    string code,
    string title,
    string description,
    bool isActive,
    int? organizationPostId)
    {

      var job = repository.Create<OrganizationJob>();
      job.Title = title;
      job.Code = code;
      job.Description = description;
      job.IsActive = isActive;
      job.OranizationPostId = organizationPostId;
      job.CreationTime = DateTime.UtcNow;
      job.CreatorId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Add(job);
      return job;
    }
    #endregion
    #region Delete
    public void DeleteOrganizationJob(int id)
    {

      var job = GetOrganizationJob(id);
      repository.Delete(job);
    }
    #endregion
    #region ToComboResult
    public Expression<Func<OrganizationJob, OrganizationJobComboResult>> ToOrganizationJobComboResult =
        OrganizationJob => new OrganizationJobComboResult()
        {
          Id = OrganizationJob.Id,
          Title = OrganizationJob.Title,

        };
    #endregion

    #region ToResult
    public Expression<Func<OrganizationJob, OrganizationJobResult>> ToOrganizationJobResult =
       result => new OrganizationJobResult()
       {
         Id = result.Id,
         Title = result.Title,
         Description = result.Description,
         IsActive = result.IsActive,
         OrganizationPostId = result.OranizationPostId,
         OrganizationPostName = result.OrganizationPost.Title,
         RowVersion = result.RowVersion,
         Code = result.Code,
         CreationTime = result.CreationTime,
         CreatorId = result.CreatorId,
         CreatorFullName = result.Creator.Employee.FirstName + " " + result.Creator.Employee.LastName,
       };

    #endregion
    #region Search
    public IQueryable<OrganizationJobResult> SearchOrganizationJob(
        IQueryable<OrganizationJobResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                item.Title.Contains(searchText) ||
                item.Description.Contains(searchText) ||
                item.Code.Contains(searchText) ||
                item.CreatorFullName.Contains(searchText)
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
    public IOrderedQueryable<OrganizationJobResult> SortOrganizationJobResult(IQueryable<OrganizationJobResult> query, SortInput<OrganizationJobSortType> sort)
    {
      switch (sort.SortType)
      {
        case OrganizationJobSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case OrganizationJobSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case OrganizationJobSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        case OrganizationJobSortType.IsActive:
          return query.OrderBy(a => a.IsActive, sort.SortOrder);
        case OrganizationJobSortType.OrganizationPostName:
          return query.OrderBy(a => a.OrganizationPostName, sort.SortOrder);
        case OrganizationJobSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case OrganizationJobSortType.CreatorFullName:
          return query.OrderBy(a => a.CreatorFullName, sort.SortOrder);
        case OrganizationJobSortType.CreationTime:
          return query.OrderBy(a => a.CreationTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Edit
    public OrganizationJob EditOrganizationJob(
        int id,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<string> description = null,
        TValue<string> title = null,
        TValue<bool> isActive = null,
        TValue<int> organizationPostId = null)
    {

      var job = GetOrganizationJob(id: id);
      if (title != null)
        job.Title = title;
      if (description != null)
        job.Description = description;
      if (isActive != null)
        job.IsActive = isActive;
      if (code != null)
        job.Code = code;
      if (organizationPostId != null)
        job.OranizationPostId = organizationPostId;

      repository.Update(job, rowVersion);
      return job;
    }
    #endregion
  }
}
