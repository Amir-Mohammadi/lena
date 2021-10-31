using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
//using System.Web.UI.WebControls;
using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Planning.BillOfMaterialDocumentType;
using lena.Models.UserManagement.SecurityAction;
using lena.Models.UserManagement.User;
using lena.Models.UserManagement.UserGroup;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Get
    public BillOfMaterialDocumentType GetBillOfMaterialDocumentType(int id) => GetBillOfMaterialDocumentType(selector: e => e, id: id);
    public TResult GetBillOfMaterialDocumentType<TResult>(
        Expression<Func<BillOfMaterialDocumentType, TResult>> selector,
        int id)
    {

      var billOfMaterialDocumentType = GetBillOfMaterialDocumentTypes(
                    selector: selector,
                    billOfMaterialDocumentTypeId: id)


                .FirstOrDefault();
      if (billOfMaterialDocumentType == null)
        throw new BillOfMaterialDocumentTypeNotFoundException(id);
      return billOfMaterialDocumentType;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetBillOfMaterialDocumentTypes<TResult>(
            Expression<Func<BillOfMaterialDocumentType, TResult>> selector,
            TValue<int> billOfMaterialDocumentTypeId = null,
            TValue<string> title = null,
            TValue<string> description = null
            )
    {

      var query = repository.GetQuery<BillOfMaterialDocumentType>();
      if (billOfMaterialDocumentTypeId != null) query = query.Where(x => x.Id == billOfMaterialDocumentTypeId);
      if (title != null) query = query.Where(x => x.Title == title);
      if (description != null) query = query.Where(x => x.Description == description);
      return query.Select(selector);
    }
    #endregion

    #region Add
    public BillOfMaterialDocumentType AddBillOfMaterialDocumentType(string title, string description)
    {

      var billOfMaterialDocumentType = repository.Create<BillOfMaterialDocumentType>();
      billOfMaterialDocumentType.Title = title;
      billOfMaterialDocumentType.Description = description;

      repository.Add(billOfMaterialDocumentType);
      return billOfMaterialDocumentType;
    }
    #endregion
    #region AddProcess
    public BillOfMaterialDocumentType AddBillOfMaterialDocumentTypeProcess(
        string title,
        string description,
        int[] userGroupIds,
        int[] userIds)
    {

      #region AddBillOfMaterialDocumentType
      var billOfMaterialDocumentType = AddBillOfMaterialDocumentType(
              title: title,
              description: description);
      #endregion
      #region Get or add download bill-of-material-document SecurityActionGroup
      var securityActionGroupName = "Documents";
      var securityActionGroup = App.Internals.UserManagement.GetSecurityActionGroups(
                    selector: e => e,
                    name: securityActionGroupName)


                .FirstOrDefault();
      if (securityActionGroup == null)
      {
        securityActionGroup = App.Internals.UserManagement.AddSecurityActionGroup(
                      name: securityActionGroupName,
                      displayName: "دانلود مستندات تولید");
      }
      #endregion
      #region AddSecurityAction
      var parameter = new AddActionParameterInput();
      parameter.Key = "DocumentTypeId";
      parameter.Value = billOfMaterialDocumentType.Id.ToString();
      parameter.CheckParameterValue = true;
      var securityAction = App.Internals.UserManagement.AddSecurityActionProcess(
                    addActionParameterInputs: new[] { parameter },
                    name: "دانلود مستندات (" + title + ")فرمول تولید ",
                    actionName: Models.StaticData.StaticActionName.DownloadBillOfMaterialDocumentAction,
                    securityActionGroupId: securityActionGroup.Id);
      #endregion
      #region AddUserSecurityActionPermissions

      foreach (var userId in userIds)
      {
        App.Internals.UserManagement.AddPermission(
                      securityActionId: securityAction.Id,
                      userId: userId,
                      userGroupId: null,
                      accessType: AccessType.Allowed);

      }
      #endregion
      #region AddUserGroupSecurityActionPermissions
      foreach (var userGroupId in userGroupIds)
      {
        App.Internals.UserManagement.AddPermission(
                      securityActionId: securityAction.Id,
                      userId: null,
                      userGroupId: userGroupId,
                      accessType: AccessType.Allowed);
      }
      #endregion
      return billOfMaterialDocumentType;
    }
    #endregion
    #region Edit
    public BillOfMaterialDocumentType EditBillOfMaterialDocumentType(
        int id,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<string> description = null)
    {

      var billOfMaterialDocumentType = GetBillOfMaterialDocumentType(id: id);
      return EditBillOfMaterialDocumentType(
                billOfMaterialDocumentType: billOfMaterialDocumentType,
                rowVersion: rowVersion,
                 description: description,
                 title: title
                );

    }

    public BillOfMaterialDocumentType EditBillOfMaterialDocumentType(
                BillOfMaterialDocumentType billOfMaterialDocumentType,
                byte[] rowVersion,
                TValue<string> title = null,
                TValue<string> description = null

                )
    {


      if (title != null) billOfMaterialDocumentType.Title = title;
      if (description != null) billOfMaterialDocumentType.Description = description;

      repository.Update(rowVersion: rowVersion, entity: billOfMaterialDocumentType);
      return billOfMaterialDocumentType;
    }

    #endregion
    #region AddProcess
    public BillOfMaterialDocumentType EditBillOfMaterialDocumentTypeProcess(
        int id,
        byte[] rowVersion,
        string title,
        string description,
        int[] addUserGroupIds,
        int[] deleteUserGroupIds,
        int[] addUserIds,
        int[] deleteUserIds)
    {

      #region EditBillOfMaterialDocumentType
      var billOfMaterialDocumentType = EditBillOfMaterialDocumentType(
              id: id,
              rowVersion: rowVersion,
              title: title,
              description: description);
      #endregion
      #region Get or add download bill-of-material-document SecurityActionGroup
      var securityActionGroupName = "Documents";
      var securityActionGroup = App.Internals.UserManagement.GetSecurityActionGroups(
                    selector: e => e,
                    name: securityActionGroupName)


                .FirstOrDefault();
      if (securityActionGroup == null)
      {
        securityActionGroup = App.Internals.UserManagement.AddSecurityActionGroup(
                      name: securityActionGroupName,
                      displayName: "دانلود مستندات تولید");
      }
      #endregion
      #region GetSecurityAction
      var parameter = new ActionParameterInput();
      parameter.Key = "DocumentTypeId";
      parameter.Value = billOfMaterialDocumentType.Id.ToString();
      var actionName = Models.StaticData.StaticActionName.DownloadBillOfMaterialDocumentAction;
      var securityAction = App.Internals.UserManagement.FindMathSecurityAction(
                    actionName: actionName,
                    actionParameters: new[] { parameter });
      #endregion
      #region AddUserSecurityActionPermissions

      foreach (var userId in addUserIds)
      {
        App.Internals.UserManagement.AddPermission(
                      securityActionId: securityAction.Id,
                      userId: userId,
                      userGroupId: null,
                      accessType: AccessType.Allowed);

      }
      #endregion
      #region AddUserGroupSecurityActionPermissions
      foreach (var userGroupId in addUserGroupIds)
      {
        App.Internals.UserManagement.AddPermission(
                      securityActionId: securityAction.Id,
                      userId: null,
                      userGroupId: userGroupId,
                      accessType: AccessType.Allowed);
      }
      #endregion
      #region DeleteUserSecurityActionPermissions
      #region GetUsersPermissions
      var usersPermission = App.Internals.UserManagement.GetPermissions(
              securityActionId: securityAction.Id,
              userIds: deleteUserIds)


          .ToList();
      #endregion
      #region Delete User Permissions
      foreach (var userPermission in usersPermission)
      {
        App.Internals.UserManagement.DeletePermission(userPermission);

      }
      #endregion
      #endregion
      #region DeleteUserGroupSecurityActionPermissions
      #region GetUserGroupsPermissions
      var userGroupsPermission = App.Internals.UserManagement.GetPermissions(
              securityActionId: securityAction.Id,
              userGroupIds: deleteUserGroupIds)


          .ToList();
      #endregion
      #region Delete UserGroup Permissions
      foreach (var userGroupPermission in userGroupsPermission)
      {
        App.Internals.UserManagement.DeletePermission(userGroupPermission);

      }
      #endregion
      #endregion
      return billOfMaterialDocumentType;
    }
    #endregion
    #region Delete
    public void DeleteBillOfMaterialDocumentType(int id)
    {

      var billOfMaterialDocumentType = GetBillOfMaterialDocumentType(id: id);
      repository.Delete(billOfMaterialDocumentType);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<BillOfMaterialDocumentTypeResult> SortBillOfMaterialDocumentTypeResult(
        IQueryable<BillOfMaterialDocumentTypeResult> query,
        SortInput<BillOfMaterialDocumentTypeSortType> sort)
    {
      switch (sort.SortType)
      {
        case BillOfMaterialDocumentTypeSortType.Id: return query.OrderBy(a => a.Id, sort.SortOrder);
        case BillOfMaterialDocumentTypeSortType.Title: return query.OrderBy(a => a.Title, sort.SortOrder);
        case BillOfMaterialDocumentTypeSortType.Description: return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region FullSort
    public IOrderedQueryable<FullBillOfMaterialDocumentTypeResult> SortFullBillOfMaterialDocumentTypeResult(
        IQueryable<FullBillOfMaterialDocumentTypeResult> query,
        SortInput<BillOfMaterialDocumentTypeSortType> sort)
    {
      switch (sort.SortType)
      {
        case BillOfMaterialDocumentTypeSortType.Id: return query.OrderBy(a => a.Id, sort.SortOrder);
        case BillOfMaterialDocumentTypeSortType.Title: return query.OrderBy(a => a.Title, sort.SortOrder);
        case BillOfMaterialDocumentTypeSortType.Description: return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<BillOfMaterialDocumentTypeResult> SearchBillOfMaterialDocumentTypeResult(
        IQueryable<BillOfMaterialDocumentTypeResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.Title.Contains(searchText) ||
                item.Description.Contains(searchText)
                select item;

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region FullSearch
    public IQueryable<FullBillOfMaterialDocumentTypeResult> SearchFullBillOfMaterialDocumentTypeResult(
        IQueryable<FullBillOfMaterialDocumentTypeResult> query,
        string searchText)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.Title.Contains(searchText)
                select item;
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<BillOfMaterialDocumentType, BillOfMaterialDocumentTypeResult>> ToBillOfMaterialDocumentTypeResult =
                billOfMaterialDocumentType => new BillOfMaterialDocumentTypeResult
                {
                  Id = billOfMaterialDocumentType.Id,
                  Title = billOfMaterialDocumentType.Title,
                  Description = billOfMaterialDocumentType.Description,
                  RowVersion = billOfMaterialDocumentType.RowVersion
                };


    public FullBillOfMaterialDocumentTypeResult GetFullBillOfMaterialDocumentTypeResult(int id)
    {

      #region GetBillOfMaterialDocumentType

      var billOfMaterialDocumentType = GetBillOfMaterialDocumentType(id: id);
      #endregion
      #region ToResult
      var result = new FullBillOfMaterialDocumentTypeResult()
      {
        Id = billOfMaterialDocumentType.Id,
        Title = billOfMaterialDocumentType.Title,
        Description = billOfMaterialDocumentType.Description,
        RowVersion = billOfMaterialDocumentType.RowVersion
      };
      #endregion
      #region FindMathSecurityAction
      var parameter = new ActionParameterInput();
      parameter.Key = "DocumentTypeId";
      parameter.Value = billOfMaterialDocumentType.Id.ToString();
      var actionName = Models.StaticData.StaticActionName.DownloadBillOfMaterialDocumentAction;
      var securityAction = App.Internals.UserManagement.FindMathSecurityAction(
                    actionName: actionName,
                    actionParameters: new[] { parameter });
      #endregion
      #region GetPermissions user and userGroups
      var permissions = App.Internals.UserManagement.GetPermissions(
              securityActionId: securityAction.Id,
              accessType: AccessType.Allowed);
      var users = permissions.Where(i => i.UserId != null).Select(i => i.User);
      var userGroups = permissions.Where(i => i.UserGroupId != null).Select(i => i.UserGroup);
      result.Users = App.Internals.UserManagement.ToUserResultQuery(users).ToArray();
      result.UserGroups = App.Internals.UserManagement.ToUserGroupResultQuery(userGroups).ToArray();
      #endregion
      return result;
    }

    #endregion
    #region ToFullResult
    public Expression<Func<BillOfMaterialDocumentType, FullBillOfMaterialDocumentTypeResult>> ToFullBillOfMaterialDocumentTypeResult =
                billOfMaterialDocumentType => new FullBillOfMaterialDocumentTypeResult
                {
                  Id = billOfMaterialDocumentType.Id,
                  Title = billOfMaterialDocumentType.Title,
                  Description = billOfMaterialDocumentType.Description,
                  RowVersion = billOfMaterialDocumentType.RowVersion
                };


    public IQueryable<FullBillOfMaterialDocumentTypeResult> GetFullBillOfMaterialDocumentTypes()
    {

      #region GetBillOfMaterialDocumentType

      var billOfMaterialDocumentTypes = GetBillOfMaterialDocumentTypes(ToFullBillOfMaterialDocumentTypeResult)

              .ToList();
      #endregion

      #region FindMathSecurityAction
      foreach (var item in billOfMaterialDocumentTypes)
      {
        var parameter = new ActionParameterInput();

        parameter.Key = "DocumentTypeId";

        parameter.Value = item.Id.ToString();
        var actionName = Models.StaticData.StaticActionName.DownloadBillOfMaterialDocumentAction;
        var securityAction = App.Internals.UserManagement.FindMathSecurityAction(
                      actionName: actionName,
                      actionParameters: new[] { parameter });
        #endregion
        #region GetPermissions user and userGroups
        if (securityAction != null)
        {

          var permissions = App.Internals.UserManagement.GetPermissions(
                        securityActionId: securityAction.Id,
                        accessType: AccessType.Allowed);
          var users = permissions.Where(i => i.UserId != null).Select(i => i.User);
          var userGroups = permissions.Where(i => i.UserGroupId != null).Select(i => i.UserGroup);
          item.Users = App.Internals.UserManagement.ToUserResultQuery(users).ToArray();
          item.UserGroups = App.Internals.UserManagement.ToUserGroupResultQuery(userGroups).ToArray();
        }
        #endregion

      }
      return billOfMaterialDocumentTypes.AsQueryable();
    }

    #endregion

  }
}
