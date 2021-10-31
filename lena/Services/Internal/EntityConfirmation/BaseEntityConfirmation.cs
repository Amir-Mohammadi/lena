using lena.Services.Core.Exceptions;
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
// using ExcelLibrary.BinaryFileFormat;
using lena.Services.Core;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.EntityConfirmation
{
  public partial class Confirmation
  {
    #region Add 
    public BaseEntityConfirmation AddBaseEntityConfirmation(
    int baseEntityConfirmTypeId,
    int confirmingEntityId,
    string confirmDescription,
    ConfirmationStatus status)
    {

      var entity = repository.Create<BaseEntityConfirmation>();
      entity.BaseEntityConfirmTypeId = baseEntityConfirmTypeId;
      entity.UserId = App.Providers.Security.CurrentLoginData.UserId;
      entity.ConfirmerId = App.Providers.Security.CurrentLoginData.UserId;
      entity.ConfirmDescription = confirmDescription;
      entity.Status = status;
      entity.ConfirmingEntityId = confirmingEntityId;
      entity.ConfirmDateTime = DateTime.UtcNow;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: entity,
                    transactionBatch: null,
                    description: "");
      return entity;
    }
    #endregion
    #region Edit
    public BaseEntityConfirmation EditBaseEntityConfirmation(
        int id,
        byte[] rowVersion,
        TValue<int> baseEntityConfirmTypeId = null,
        TValue<string> confirmDescription = null,
        TValue<ConfirmationStatus> status = null,
        TValue<int> confirmingEntityId = null)
    {

      var baseEntityConfirmation = GetBaseEntityConfirmation(id);
      return EditBaseEntityConfirmation(
                    baseEntityConfirmation: baseEntityConfirmation,
                    rowVersion: rowVersion,
                    baseEntityConfirmTypeId: baseEntityConfirmTypeId,
                    confirmDescription: confirmDescription,
                    status: status,
                    confirmingEntityId: confirmingEntityId);
    }
    public BaseEntityConfirmation EditBaseEntityConfirmation(
        BaseEntityConfirmation baseEntityConfirmation,
        byte[] rowVersion,
        TValue<int> baseEntityConfirmTypeId = null,
        TValue<string> confirmDescription = null,
        TValue<ConfirmationStatus> status = null,
        TValue<int> confirmingEntityId = null,
        TValue<bool> isDelete = null)
    {

      if (baseEntityConfirmTypeId != null)
        baseEntityConfirmation.BaseEntityConfirmTypeId = baseEntityConfirmTypeId;
      if (confirmDescription != null)
        baseEntityConfirmation.ConfirmDescription = confirmDescription;
      if (status != null)
        baseEntityConfirmation.Status = status;
      if (confirmingEntityId != null)
        baseEntityConfirmation.ConfirmingEntityId = confirmingEntityId;
      App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: baseEntityConfirmation,
                    isDelete: isDelete,
                    rowVersion: rowVersion);
      return baseEntityConfirmation;
    }
    #endregion
    #region Delete
    public void DeleteBaseEntityConfirmation(int id)
    {

      var record = GetBaseEntityConfirmation(id);
      #region Edit BaseEntity
      App.Internals.ApplicationBase.EditBaseEntity(baseEntity: record,
              rowVersion: record.RowVersion,
              isDelete: true);
      #endregion
      //repository.Delete(record);
    }
    #endregion
    #region Get
    public BaseEntityConfirmation GetBaseEntityConfirmation(int id) =>
        GetBaseEntityConfirmation(selector: e => e, id: id);
    public TResult GetBaseEntityConfirmation<TResult>(
        Expression<Func<BaseEntityConfirmation, TResult>> selector,
        int id)
    {

      var record = GetBaseEntityConfirmations(
                    selector: selector,
                    id: id)


            .FirstOrDefault();
      if (record == null)
        throw new RecordNotFoundException(id, typeof(BaseEntityConfirmation));
      return record;
    }
    #endregion
    #region Get with ConfirmingEntityId
    public BaseEntityConfirmation GetBaseEntityConfirmation(int id, int confirmingEntityId) =>
        GetBaseEntityConfirmation(selector: e => e, id: id, confirmingEntityId: confirmingEntityId);
    public TResult GetBaseEntityConfirmation<TResult>(
        Expression<Func<BaseEntityConfirmation, TResult>> selector,
        int id,
        int confirmingEntityId)
    {

      var record = GetBaseEntityConfirmations(
                    selector: selector,
                    id: id,
                    confirmingEntityId: confirmingEntityId)


                .FirstOrDefault();
      if (record == null)
        throw new RecordNotFoundException(id, typeof(BaseEntityConfirmation));
      return record;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetBaseEntityConfirmations<TResult>(
       Expression<Func<BaseEntityConfirmation, TResult>> selector,
       TValue<int> id = null,
       TValue<int> baseEntityConfirmTypeId = null,
       TValue<int> confirmerId = null,
       TValue<string> confirmDescription = null,
       TValue<ConfirmationStatus> status = null,
       TValue<int> confirmingEntityId = null,
       TValue<DateTime> confirmDateTime = null)
    {

      var query = repository.GetQuery<BaseEntityConfirmation>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (baseEntityConfirmTypeId != null)
        query = query.Where(i => i.BaseEntityConfirmTypeId == baseEntityConfirmTypeId);
      if (confirmerId != null)
        query = query.Where(i => i.UserId == confirmerId);
      if (confirmDescription != null)
        query = query.Where(i => i.ConfirmDescription == confirmDescription);
      if (status != null)
        query = query.Where(i => i.Status == status);
      if (confirmingEntityId != null)
        query = query.Where(x => x.ConfirmingEntityId == confirmingEntityId);
      if (confirmDateTime != null)
        query = query.Where(x => x.ConfirmDateTime == confirmDateTime);
      return query.Select(selector);
    }
    #endregion
    #region ToResult
    public IQueryable<BaseEntityConfirmationResult> ToBaseEntityConfirmationResultQuery(IQueryable<BaseEntityConfirmation> query)
    {
      return query.Select(x => new BaseEntityConfirmationResult()
      {
        BaseEntityConfirmTypeId = x.BaseEntityConfirmTypeId,
        ConfirmDescription = x.ConfirmDescription,
        Status = x.Status,
        UserFullName = x.User.Employee.FirstName + " " + x.User.Employee.LastName,
        UserId = x.UserId,
        ConfirmingEntityId = x.ConfirmingEntityId,
        ConfirmDateTime = x.ConfirmDateTime,
      });
    }
    #endregion
    #region Accept 
    public BaseEntityConfirmation AcceptBaseEntityConfirmation(
        int baseEntityConfirmTypeId,
        int confirmingEntityId,
        string confirmDescription)
    {

      var baseEntityConfirmation = AddBaseEntityConfirmation(
                    baseEntityConfirmTypeId: baseEntityConfirmTypeId,
                    confirmDescription: confirmDescription,
                    status: ConfirmationStatus.Accepted,
                    confirmingEntityId: confirmingEntityId);
      return baseEntityConfirmation;
    }
    #endregion
    #region Reject 
    public BaseEntityConfirmation RejectBaseEntityConfirmation(
        int baseEntityConfirmTypeId,
        int confirmingEntityId,
        string confirmDescription)
    {

      var baseEntityConfirmation = AddBaseEntityConfirmation(
                    baseEntityConfirmTypeId: baseEntityConfirmTypeId,
                    confirmDescription: confirmDescription,
                    status: ConfirmationStatus.Rejected,
                    confirmingEntityId: confirmingEntityId);
      return baseEntityConfirmation;
    }
    #endregion
  }
}