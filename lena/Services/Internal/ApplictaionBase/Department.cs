using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using lena.Services.Common;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.ApplicationBase.Department;
using lena.Models.Common;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase
{
  public partial class ApplicationBase
  {
    public Department AddDepartment(string name, short? parentDepartmentId)
    {
      var department = repository.Create<Department>();
      department.Name = name;
      department.ParentDepartmentId = parentDepartmentId;
      repository.Add(department);
      return department;
    }
    public Department EditDepartment(
        byte[] rowVersion,
        short id,
        TValue<string> name = null,
        TValue<short?> parentDepartmentId = null)
    {
      var department = GetDepartment(id: id);
      if (department.Id == parentDepartmentId)
        throw new InvalidParentDepartmentException(department.Id);
      var childDepartments = GetDepartments(recursiveParentDepartmentId: department.Id);
      if (childDepartments.Any(x => x.ParentDepartmentId == id))
        throw new InvalidDepartmentParentException(id);
      if (name != null)
        department.Name = name;
      if (parentDepartmentId != null)
        department.ParentDepartmentId = parentDepartmentId;
      repository.Update(entity: department, rowVersion: department.RowVersion);
      return department;
    }
    public void DeleteDepartment(short id)
    {
      var department = GetDepartment(id: id);
      repository.Delete(department);
    }
    public IQueryable<Department> GetDepartments(
        TValue<short> id = null,
        TValue<short[]> ids = null,
        TValue<string> name = null,
        TValue<short?> parentDepartmentId = null,
        TValue<short?> recursiveParentDepartmentId = null,
        string searchText = null
        )
    {
      var query = repository.GetQuery<Department>();
      if (recursiveParentDepartmentId != null)
        query = from item in query
                let parent1 = item.ParentDepartment
                let parent2 = parent1.ParentDepartment
                let parent3 = parent2.ParentDepartment
                let parent4 = parent3.ParentDepartment
                let parent5 = parent4.ParentDepartment
                let parent6 = parent5.ParentDepartment
                let parent7 = parent6.ParentDepartment
                let parent8 = parent7.ParentDepartment
                let parent9 = parent8.ParentDepartment
                where item.Id == recursiveParentDepartmentId ||
                      item.ParentDepartmentId == recursiveParentDepartmentId ||
                      parent1.ParentDepartmentId == recursiveParentDepartmentId ||
                      parent2.ParentDepartmentId == recursiveParentDepartmentId ||
                      parent3.ParentDepartmentId == recursiveParentDepartmentId ||
                      parent4.ParentDepartmentId == recursiveParentDepartmentId ||
                      parent5.ParentDepartmentId == recursiveParentDepartmentId ||
                      parent6.ParentDepartmentId == recursiveParentDepartmentId ||
                      parent7.ParentDepartmentId == recursiveParentDepartmentId ||
                      parent8.ParentDepartmentId == recursiveParentDepartmentId ||
                      parent9.ParentDepartmentId == recursiveParentDepartmentId
                select item;
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (ids != null)
        query = query.Where(x => ids.Value.Contains(x.Id));
      if (!string.IsNullOrEmpty(name))
        query = query.Where(x => x.Name == name);
      if (parentDepartmentId != null)
        query = query.Where(x => x.ParentDepartmentId == parentDepartmentId);
      return query;
    }
    public IQueryable<Department> GetUserDepartments()
    {
      var currentDepartmentId = App.Providers.Security.CurrentLoginData.DepartmentId;
      var result = GetDepartments(recursiveParentDepartmentId: currentDepartmentId.Value);
      return result.AsQueryable();
    }
    public List<DepartmentTreeResult> GetDepartmentsTree(
    TValue<int> id = null,
    TValue<string> name = null,
    TValue<bool> isActive = null,
    TValue<string> searchText = null)
    {
      var query = repository.GetQuery<Department>()
            .Where(i => i.ParentDepartmentId == null);
      if (searchText != null && !string.IsNullOrEmpty(searchText))
        query = query.Where(x => x.Name.Contains(searchText));
      var rootDepartments = query.ToList();
      var result = new List<DepartmentTreeResult>();
      foreach (var item in rootDepartments)
      {
        if (id != null && id.Value != item.Id)
          continue;
        if (name != null && name.Value != item.Name)
          continue;
        //if (isActive != null && isActive.Value != item.IsActive)
        //    continue;
        var nCat = new DepartmentTreeResult()
        {
          Id = item.Id,
          Name = item.Name,
          //IsActive = item.IsActive,
          RowVersion = item.RowVersion,
          CategoryLevel = 1,
          ChildDepartments = GetDepartmentsTree(item, 1, id, name, isActive, searchText)
        };
        result.Add(nCat);
      }
      return result;
    }
    private IList<DepartmentTreeResult> GetDepartmentsTree(
        Department rootDepartment,
        int categoryLevel,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<bool> isActive = null,
        TValue<string> searchText = null)
    {
      var result = new List<DepartmentTreeResult>();
      foreach (var item in rootDepartment.ChildDepartments)
      {
        if (id != null && id.Value != item.Id)
          continue;
        if (name != null && name.Value != item.Name)
          continue;
        //if (isActive != null && isActive.Value != item.IsActive)
        //    continue;
        if (searchText != null && !string.IsNullOrEmpty(searchText))
          if (!item.Name.Contains(searchText))
            continue;
        var catResult = new DepartmentTreeResult()
        {
          Id = item.Id,
          Name = item.Name,
          //IsActive = item.IsActive,
          RowVersion = item.RowVersion,
          CategoryLevel = categoryLevel + 1,
          ChildDepartments = GetDepartmentsTree(item, categoryLevel + 1, id, name, isActive)
        };
        result.Add(catResult);
      }
      return result;
    }
    public Department GetDepartment(short id)
    {
      var department = GetDepartments(id: id).FirstOrDefault();
      if (department == null)
        throw new DepartmentNotFoundException(id);
      return department;
    }
    public IOrderedQueryable<DepartmentResult> SortDepartmentResult(IQueryable<DepartmentResult> query, SortInput<DepartmentSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case DepartmentSortType.Id:
          return query.OrderBy(r => r.Id, sortInput.SortOrder);
        case DepartmentSortType.Name:
          return query.OrderBy(r => r.Name, sortInput.SortOrder);
        case DepartmentSortType.ParentDepartmentName:
          return query.OrderBy(r => r.ParentDepartmentName, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<DepartmentTreeResult> SortDepartmentTreeResult(IQueryable<DepartmentTreeResult> input,
      SortInput<DepartmentSortType> options)
    {
      switch (options.SortType)
      {
        case DepartmentSortType.Name:
          return input.OrderBy(i => i.Name, options.SortOrder);
        //case DepartmentSortType.IsActive:
        //    return input.OrderBy(i => i.IsActive, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<DepartmentResult> ToDepartmentResultQuery(IQueryable<Department> query)
    {
      var result = from department in query
                   select new DepartmentResult()
                   {
                     Id = department.Id,
                     Name = department.Name,
                     ParentDepartmentId = department.ParentDepartmentId,
                     ParentDepartmentName = department.ParentDepartment == null ? "" : department.ParentDepartment.Name,
                     RowVersion = department.RowVersion
                   };
      return result;
    }
    public IQueryable<DepartmentComboResult> ToDepartmentComboResult(IQueryable<Department> query)
    {
      var result = from department in query
                   select new DepartmentComboResult()
                   {
                     Id = department.Id,
                     Name = department.Name
                   };
      return result;
    }
    public DepartmentResult ToDepartmentResult(Department department)
    {
      var result = new DepartmentResult()
      {
        Id = department.Id,
        Name = department.Name,
        ParentDepartmentId = department.ParentDepartmentId,
        ParentDepartmentName = department.ParentDepartment == null ? "" : department.ParentDepartment.Name,
        RowVersion = department.RowVersion
      };
      return result;
    }
    public IQueryable<DepartmentResult> SearchDepartmentResult(
       IQueryable<DepartmentResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where item.Name.Contains(searchText) ||
                      item.Id.ToString().Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
  }
}