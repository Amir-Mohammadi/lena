using lena.Services.Core.Exceptions;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Models.EntityConfirmation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.EntityConfirmation
{
  public partial class Confirmation
  {
    public BaseEntityConfirmType AddBaseEntityConfirmType(
        short id,
        short departmentId,
        EntityType confirmType,
        int? userId,
        string confirmPageUrl)
    {
      var entity = repository.Create<BaseEntityConfirmType>();
      entity.Id = id;
      entity.DepartmentId = departmentId;
      entity.UserId = userId;
      entity.ConfirmType = confirmType;
      entity.ConfirmPageUrl = confirmPageUrl;
      repository.Add(entity);
      return entity;
    }
    public BaseEntityConfirmType EditBaseEntityConfirmType(
        int id,
        byte[] rowVersion,
       TValue<EntityType> confirmType = null,
       TValue<short> departmentId = null,
       TValue<int?> userId = null,
       TValue<string> confirmPageUrl = null,
        BaseEntityConfirmType baseEntityConfirmType = null)
    {
      var entity = baseEntityConfirmType ?? GetBaseEntityConfirmType(id);
      if (departmentId != null)
        entity.DepartmentId = departmentId;
      if (userId != null)
        entity.UserId = userId;
      if (confirmType != null)
        entity.ConfirmType = confirmType;
      if (confirmPageUrl != null)
        entity.ConfirmPageUrl = confirmPageUrl;
      repository.Update(entity, rowVersion);
      return entity;
    }
    public void DeleteBaseEntityConfirmType(int id)
    {
      var record = GetBaseEntityConfirmType(id);
      repository.Delete<BaseEntityConfirmType>(record);
    }
    public BaseEntityConfirmType GetBaseEntityConfirmType(int id)
    {
      var record = GetBaseEntityConfirmTypes(e => e, id: id)
            .FirstOrDefault();
      if (record == null)
        throw new RecordNotFoundException(id, typeof(BaseEntityConfirmType));
      return record;
    }
    public IQueryable<BaseEntityConfirmType> GetBaseEntityConfirmTypes(
      TValue<int> id,
      TValue<EntityType> confirmType = null,
      TValue<int> departmentId = null,
      TValue<int?> userId = null,
      TValue<string> confirmPageUrl = null)
    {
      var query = repository.GetQuery<BaseEntityConfirmType>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (confirmType != null)
        query = query.Where(i => i.ConfirmType == confirmType);
      if (departmentId != null)
        query = query.Where(i => i.DepartmentId == departmentId);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (confirmPageUrl != null)
        query = query.Where(i => i.ConfirmPageUrl == confirmPageUrl);
      return query;
    }
    public IQueryable<TResult> GetBaseEntityConfirmTypes<TResult>(
       Expression<Func<BaseEntityConfirmType, TResult>> selector,
       TValue<int> id = null,
       TValue<EntityType> confirmType = null,
       TValue<int> departmentId = null,
       TValue<int?> userId = null,
       TValue<string> confirmPageUrl = null)
    {
      var query = repository.GetQuery<BaseEntityConfirmType>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (confirmType != null)
        query = query.Where(i => i.ConfirmType == confirmType);
      if (departmentId != null)
        query = query.Where(i => i.DepartmentId == departmentId);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (confirmPageUrl != null)
        query = query.Where(i => i.ConfirmPageUrl == confirmPageUrl);
      return query.Select(selector);
    }
    public BaseEntityConfirmTypeResult ToBaseEntityConfirmTypeResult(BaseEntityConfirmType confirm)
    {
      return new BaseEntityConfirmTypeResult
      {
        Id = confirm.Id,
        ConfirmPageUrl = confirm.ConfirmPageUrl,
        ConfirmType = confirm.ConfirmType,
        DepartmentId = confirm.DepartmentId,
        DepartmentName = confirm.Department.Name,
        UserId = confirm.UserId,
        UserFullName = confirm.User.Employee.FirstName + " " + confirm.User.Employee.LastName
      };
    }
    public IQueryable<BaseEntityConfirmTypeResult> ToBaseEntityConfirmTypeResultQuery(IQueryable<BaseEntityConfirmType> query)
    {
      return query.Select(x => new BaseEntityConfirmTypeResult()
      {
        Id = x.Id,
        UserId = x.UserId,
        ConfirmPageUrl = x.ConfirmPageUrl,
        ConfirmType = x.ConfirmType,
        DepartmentId = x.DepartmentId,
        DepartmentName = x.Department.Name,
        UserFullName = x.User.Employee.FirstName + " " + x.User.Employee.LastName
      });
    }
  }
}