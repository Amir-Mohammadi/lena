using System;
using System.CodeDom;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ScrumManagement.Exception;
//using Parlar.DAL;
using lena.Models.Common;
using lena.Domains;
using lena.Models.ScrumManagement.ScrumEntity;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement
{
  public partial class ScrumManagement
  {
    #region Add
    public ScrumEntity AddScrumEntity(
        ScrumEntity scrumEntity,
        string name,
        string description,
        string color,
        short departmentId,
        long estimatedTime,
        bool isCommit,
        int? baseEntityId)
    {

      scrumEntity.Name = name;
      scrumEntity.Description = description;
      scrumEntity.Color = color;
      scrumEntity.EstimatedTime = estimatedTime;
      scrumEntity.DepartmentId = departmentId;
      scrumEntity.IsCommit = isCommit;
      scrumEntity.IsDelete = false;
      scrumEntity.IsArchive = false;
      scrumEntity.DateTime = DateTime.Now.ToUniversalTime();
      scrumEntity.BaseEntityId = baseEntityId;
      scrumEntity.Code = "";
      repository.Add(scrumEntity);
      GenerateCode(scrumEntity);
      repository.Update(scrumEntity, scrumEntity.RowVersion);
      return scrumEntity;
    }
    #endregion
    #region Edit
    public ScrumEntity EditScrumEntity(
        ScrumEntity scrumEntity,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<bool> isArchive = null,
        TValue<int?> baseEntityId = null)
    {

      if (code != null)
        scrumEntity.Code = code;
      if (name != null)
        scrumEntity.Name = name;
      if (description != null)
        scrumEntity.Description = description;
      if (color != null)
        scrumEntity.Color = color;
      if (departmentId != null)
        scrumEntity.DepartmentId = departmentId;
      if (estimatedTime != null)
        scrumEntity.EstimatedTime = estimatedTime;
      if (isCommit != null)
        scrumEntity.IsCommit = isCommit;
      if (isDelete != null)
        scrumEntity.IsDelete = isDelete;
      if (isArchive != null)
        scrumEntity.IsArchive = isArchive;
      if (baseEntityId != null)
        scrumEntity.BaseEntityId = baseEntityId;
      repository.Update(scrumEntity, rowVersion: rowVersion);
      return scrumEntity;
    }
    public ScrumEntity EditScrumEntity(
        int id,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<bool> isDelete = null,
        TValue<bool> isArchive = null,
        TValue<int?> baseEntityId = null)
    {

      var scrumEntity = GetScrumEntity(id);
      scrumEntity = EditScrumEntity(scrumEntity: scrumEntity,
                    rowVersion: rowVersion,
                    code: code,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    isDelete: isDelete,
                    isArchive: isArchive
                );
      return scrumEntity;
    }
    #endregion
    #region Remove
    public void RemoveScrumEntity(
        int id,
        byte[] rowVersion)
    {

      var scrumEntity = GetScrumEntity(id);
      RemoveScrumEntity(
                    scrumEntity: scrumEntity,
                    rowVersion: rowVersion);
    }
    public void RemoveScrumEntity(
        ScrumEntity scrumEntity,
        byte[] rowVersion)
    {

      EditScrumEntity(
                scrumEntity: scrumEntity,
                rowVersion: rowVersion,
                isDelete: true);
    }
    #endregion
    #region Archive
    public ScrumEntity ArchiveScrumEntity(
        int id,
        byte[] rowVersion)
    {

      var scrumEntity = GetScrumEntity(id: id);
      return ArchiveScrumEntity(
                    scrumEntity: scrumEntity,
                    rowVersion: rowVersion);
    }
    public ScrumEntity ArchiveScrumEntity(
        ScrumEntity scrumEntity,
        byte[] rowVersion)
    {

      {
        return EditScrumEntity(
                      scrumEntity: scrumEntity,
                      rowVersion: rowVersion,
                      isArchive: true);
      }
    }
    #endregion
    #region Restore
    public ScrumEntity RestoreArchiveScrumEntity(
        int id,
        byte[] rowVersion)
    {

      var scrumEntity = GetScrumEntity(id);
      return RestoreArchiveScrumEntity(
                scrumEntity: scrumEntity,
                rowVersion: rowVersion);
    }
    public ScrumEntity RestoreArchiveScrumEntity(
        ScrumEntity scrumEntity,
        byte[] rowVersion)
    {

      return EditScrumEntity(
                    scrumEntity: scrumEntity,
                    rowVersion: rowVersion,
                    isArchive: false);
    }
    #endregion
    #region Gets
    public IQueryable<ScrumEntity> GetScrumEntities(
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<bool> isCommit = null,
        TValue<long> estimatedTime = null,
        TValue<int> departmentId = null,
        TValue<bool> isDelete = null,
        TValue<bool> isArchive = null,
        TValue<int?> baseEntityId = null)
    {


      var query = repository.GetQuery<ScrumEntity>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (code != null)
        query = query.Where(i => i.Code == code);
      if (name != null)
        query = query.Where(i => i.Name == name);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (color != null)
        query = query.Where(i => i.Color == color);
      if (isCommit != null)
        query = query.Where(i => i.IsCommit == isCommit);
      if (estimatedTime != null)
        query = query.Where(i => i.EstimatedTime == estimatedTime);
      if (departmentId != null)
        query = query.Where(i => i.DepartmentId == departmentId);
      if (isDelete != null)
        query = query.Where(i => i.IsDelete == isDelete);
      if (isArchive != null)
        query = query.Where(i => i.IsArchive == isArchive);
      if (baseEntityId != null)
        query = query.Where(i => i.BaseEntityId == baseEntityId);
      return query;
    }
    #endregion
    #region Get
    public ScrumEntity GetScrumEntity(int id)
    {

      var scrumEntity = GetScrumEntities(id: id).FirstOrDefault();
      if (scrumEntity == null)
        throw new ScrumEntityNotFoundException(id);
      return scrumEntity;
    }
    public ScrumEntity GetScrumEntity(string code)
    {

      var scrumEntity = GetScrumEntities(code: code)

                .FirstOrDefault();
      if (scrumEntity == null)
        throw new ScrumEntityNotFoundException(code);
      return scrumEntity;
    }
    #endregion
    #region GenerateCode
    public void GenerateCode(ScrumEntity scrumEntity)
    {

      var code = "";
      #region ScrumProjectGroup
      if (scrumEntity is ScrumProjectGroup)
      {
        var scrumProjectGroup = (ScrumProjectGroup)scrumEntity;
        if (scrumProjectGroup.BaseEntityId != null)
          code = $"SPG{scrumProjectGroup.BaseEntityId}";
        else
        {
          //var count = GetScrumProjectGroups(departmentId: scrumProjectGroup.DepartmentId)
          //    
          //    
          //    .Count();
          //code = $"SPG-D{scrumProjectGroup.DepartmentId}-{count}";
          code = $"SPG-D{scrumProjectGroup.DepartmentId}";


        }
      }
      #endregion
      #region ScrumProject
      if (scrumEntity is ScrumProject)
      {
        var scrumProject = (ScrumProject)scrumEntity;
        if (scrumProject.BaseEntityId != null)
        {
          var query = GetScrumProjects(
                        scrumProjectGroupId: scrumProject.ScrumProjectGroupId);
          code = $"{scrumProject.ScrumProjectGroup.Code}{query.Count()}";
        }
        else
        {
          var year = 0;
          Math.DivRem(new PersianCalendar().GetYear(DateTime.Now.ToUniversalTime()), 100, out year);
          var scrumProjectGroupCode = scrumProject.ScrumProjectGroup != null ?
                    scrumProject.ScrumProjectGroup.Code :
                    $"SPG-D{scrumProject.DepartmentId}";
          code = $"{scrumProjectGroupCode}Y{year:00}";
        }
      }
      #endregion
      #region ScrumSprint
      if (scrumEntity is ScrumSprint)
      {
        var scrumSprint = (ScrumSprint)scrumEntity;
        if (scrumSprint.BaseEntityId != null)
        {
          var query = GetScrumSprints(
                        scrumProjectId: scrumSprint.ScrumProjectId);
        }
        else
        {

          var year = 0;
          Math.DivRem(new PersianCalendar().GetYear(DateTime.Now.ToUniversalTime()), 100, out year);
          var month = new PersianCalendar().GetMonth(DateTime.Now.ToUniversalTime());
          var scrumProjectCode = scrumSprint.ScrumProject != null ?
                    scrumSprint.ScrumProject.Code :
                    $"SPG-D{scrumSprint.DepartmentId}Y{year:00}";
          code = $"{scrumProjectCode}M{month:00}";
        }
      }
      #endregion
      #region ScrumBackLog
      if (scrumEntity is ScrumBackLog)
      {
        var scrumBackLog = (ScrumBackLog)scrumEntity;
        var query = GetScrumBackLogs(
                      selector: e => e.Id,
                      scrumSprintId: scrumBackLog.ScrumSprintId);
        code = $"{scrumBackLog.ScrumSprint.Code}-{query.Count()}";
      }
      #endregion
      #region ScrumTask
      if (scrumEntity is ScrumTask)
      {
        var scrumTask = (ScrumTask)scrumEntity;
        var query = GetScrumTasks(
                      selector: e => e.Id,
                      scrumBackLogId: scrumTask.ScrumBackLogId);
        code = $"{scrumTask.ScrumBackLog.Code}-{query.Count()}";
      }
      #endregion
      scrumEntity.Code = code;
    }
    #endregion
    #region ToResult
    public ScrumEntityResult ToScrumEntityResult(ScrumEntity scrumEntity)
    {
      var result = new ScrumEntityResult()
      {
        Id = scrumEntity.Id,
        Code = scrumEntity.Code,
        Name = scrumEntity.Name,
        Color = scrumEntity.Color,
        DepartmentId = scrumEntity.DepartmentId,
        DepartmentName = scrumEntity.Department.Name,
        Description = scrumEntity.Description,
        IsDelete = scrumEntity.IsDelete,
        RowVersion = scrumEntity.RowVersion
      };
      return result;
    }
    #endregion
  }
}
