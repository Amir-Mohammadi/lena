using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using System;
using System.Linq;
using System.Linq.Expressions;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Get
    public LinkSerial GetLinkSerial(string linkedSerial) => GetLinkSerial(selector: e => e, linkedSerial: linkedSerial);
    public TResult GetLinkSerial<TResult>(
        Expression<Func<LinkSerial, TResult>> selector,
        string linkedSerial)
    {

      var linkSerial = GetLinkSerials(selector: selector,
                linkedSerial: linkedSerial).FirstOrDefault();
      if (linkSerial == null)
        throw new LinkedSerialNotFoundException(linkedSerial);
      return linkSerial;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetLinkSerials<TResult>(
        Expression<Func<LinkSerial, TResult>> selector,
        TValue<int> stuffId = null,
        TValue<int> userId = null,
        TValue<int> customerId = null,
        TValue<string> toSerial = null,
        TValue<int> userLinkerId = null,
        TValue<string> fromSerial = null,
        TValue<LinkSerialType> linkSerialType = null,
        TValue<DateTime> dateTime = null,
        TValue<int> stuffSerialCode = null,
        TValue<string> linkedSerial = null,
        TValue<bool> printed = null,
        TValue<string[]> linkedSerials = null,
        TValue<int> billOfMaterialVersion = null,
        TValue<DateTime> linkDateTime = null)
    {

      var query = repository.GetQuery<LinkSerial>();
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (customerId != null)
        query = query.Where(i => i.CustomerId == customerId);
      if (userLinkerId != null)
        query = query.Where(i => i.UserLinkerId == userLinkerId);
      if (dateTime != null)
        query = query.Where(i => i.DateTime == dateTime);
      if (linkedSerial != null)
        query = query.Where(i => i.LinkedSerial == linkedSerial);
      if (linkDateTime != null)
        query = query.Where(i => i.LinkDateTime == linkDateTime);
      if (stuffId != null)
        query = query.Where(i => i.StuffSerial.StuffId == stuffId);
      if (stuffSerialCode != null)
        query = query.Where(i => i.StuffSerial.Code == stuffSerialCode);
      if (linkedSerials != null)
        query = query.Where(i => linkedSerials.Value.Contains(i.LinkedSerial));
      if (billOfMaterialVersion != null)
        query = query.Where(i => i.StuffSerial.BillOfMaterialVersion == billOfMaterialVersion);
      if (linkSerialType != null)
        query = query.Where(i => i.Type == linkSerialType);
      if (printed != null)
        query = query.Where(i => i.IranKhodroSerial.Print == printed);

      if (!string.IsNullOrWhiteSpace(fromSerial))
      {
        query = query.Where(i => i.LinkedSerial.CompareTo(fromSerial) >= 0);
      }
      if (!string.IsNullOrWhiteSpace(toSerial))
      {
        query = query.Where(i => i.LinkedSerial.CompareTo(toSerial) <= 0);
      }

      return query.Select(selector);
    }
    #endregion

    #region AddDetemineLinkForSerial
    public LinkSerial DetemineLinkForSerial(
      string linkSerial,
      int stuffId,
      int stuffSerialCode)
    {

      #region CheckLinkSerial          
      var existLinkSerial = GetLinkSerials(
          selector: e => e,
          stuffId: stuffId,
          linkedSerial: linkSerial,
          stuffSerialCode: stuffSerialCode)

      .FirstOrDefault();

      if (existLinkSerial?.StuffSerial != null)
        throw new LinkSerialHasBeenLinkedToSerialExistExceptions(
                  linkSerial: linkSerial,
                  serial: existLinkSerial.StuffSerial.Serial);

      #endregion

      #region CheckSerial
      var serialResult = GetStuffSerial(
          stuffId: stuffId,
          code: stuffSerialCode);

      if (serialResult.LinkSerial != null)
        throw new LinkSerialHasBeenLinkedToSerialExistExceptions(
                      linkSerial: serialResult.LinkSerial.LinkedSerial,
                      serial: serialResult.Serial);
      #endregion

      #region EditLinkSerial
      StuffSerial stuffSerial = null;
      if (stuffId != null && stuffSerialCode != null)
      {
        stuffSerial = GetStuffSerial(
                  stuffId: stuffId,
                  code: stuffSerialCode);
      }

      var linkedSerial = EditLinkSerial(
                linkedSerial: linkSerial,
                stuffSerial: stuffSerial,
                userLinkerId: App.Providers.Security.CurrentLoginData.UserId,
                linkDateTime: DateTime.UtcNow);
      #endregion

      return linkedSerial;
    }
    #endregion


    #region AddProcess
    public LinkSerial AddLinkSerialProcess(
      string[] linkedSerials,
      int customerId)
    {

      LinkSerial linkSerial = null;
      #region CheckExistLinkSerial
      foreach (var linkedSerial in linkedSerials)
      {
        var existLinkSerials = GetLinkSerials(
                  selector: e => e,
                  linkedSerial: linkedSerial);

        if (existLinkSerials.Any())
          throw new LinkSerialHasExistExceptions(linkSerial: linkedSerial);

        #region AddLinkSerial
        linkSerial = AddLinkSerial(
                 linkedSerial: linkedSerial,
                 customerId: customerId);
        #endregion
      }
      #endregion

      return linkSerial;
    }
    #endregion

    #region Add
    public LinkSerial AddLinkSerial(
    string linkedSerial,
    int customerId,
    TValue<LinkSerialType> type = null)
    {

      var linkSerial = repository.Create<LinkSerial>();
      linkSerial.LinkedSerial = linkedSerial;
      linkSerial.CustomerId = customerId;
      linkSerial.DateTime = DateTime.UtcNow;
      linkSerial.UserId = App.Providers.Security.CurrentLoginData.UserId;
      linkSerial.DateTime = DateTime.Now.ToUniversalTime();
      linkSerial.Type = type;
      repository.Add(linkSerial);
      return linkSerial;
    }
    #endregion

    #region RemoveRelationLinkSerial
    public LinkSerial RemoveRelationLinkSerial(
        string[] linkSerials = null)
    {


      LinkSerial linkSerial = null;
      #region EditLinkSerial

      foreach (var item in linkSerials)
      {
        #region CheckNotHasLink
        linkSerial = GetLinkSerial(linkedSerial: item);
        if (linkSerial.StuffSerial == null)
          throw new LinkedSerialNotHasRelationException(linkedSerial: item);
        #endregion
        linkSerial = EditLinkSerial(
            linkedSerial: linkSerial.LinkedSerial,
            userLinkerId: new TValue<int?>(null),
            linkDateTime: new TValue<DateTime?>(null),
            stuffSerial: new TValue<StuffSerial>(null));
      }

      #endregion

      return linkSerial;
    }

    #endregion

    #region EditProcess
    public LinkSerial EditLinkSerialProcess(
        TValue<int> customerId = null,
        string[] addLinkedSerials = null,
        string[] deleteLinkedSerials = null)
    {


      LinkSerial linkSerial = null;
      #region AddLinkSerial
      foreach (var addLinkedSerial in addLinkedSerials)
      {
        linkSerial = AddLinkSerial(
                    linkedSerial: addLinkedSerial,
                    customerId: customerId);
      }
      #endregion

      #region DeleteLinkSerial
      foreach (var deleteLinkedSerial in deleteLinkedSerials)
      {
        DeleteLinkSerial(
                   linkedSerial: deleteLinkedSerial);
      }
      #endregion

      return linkSerial;
    }

    #endregion

    #region Edit
    public LinkSerial EditLinkSerial(
        TValue<int> stuffId = null,
        TValue<int?> userLinkerId = null,
        TValue<string> linkedSerial = null,
        TValue<int> stuffSerialCode = null,
        TValue<DateTime?> linkDateTime = null,
        TValue<StuffSerial> stuffSerial = null,
        TValue<int> customerId = null)
    {

      var linkSerial = GetLinkSerial(linkedSerial: linkedSerial);
      if (linkedSerial != null)
        linkSerial.LinkedSerial = linkedSerial;
      if (customerId != null)
        linkSerial.CustomerId = customerId;
      if (linkDateTime != null)
        linkSerial.LinkDateTime = linkDateTime;
      if (userLinkerId != null)
        linkSerial.UserLinkerId = userLinkerId;
      if (stuffSerial != null)
        linkSerial.StuffSerial = stuffSerial;
      if (linkDateTime != null)
        linkSerial.LinkDateTime = linkDateTime;
      if (userLinkerId != null)
        linkSerial.UserLinkerId = userLinkerId;


      repository.Update(entity: linkSerial, rowVersion: linkSerial.RowVersion);
      return linkSerial;
    }
    #endregion

    #region Remove
    public void RemoveLinkSerial(string linkedSerial)
    {

      var linkSerial = GetLinkSerial(
                linkedSerial: linkedSerial);

      if (linkSerial.StuffSerial != null)
        throw new LinkedSerialHasBeenLinkedException(linkedSerial: linkSerial.LinkedSerial);

      DeleteLinkSerial(linkSerial.LinkedSerial);
    }
    #endregion
    #region Delete
    public void DeleteLinkSerial(string linkedSerial)
    {

      var linkSerial = GetLinkSerial(linkedSerial: linkedSerial);
      repository.Delete(linkSerial);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<LinkSerialResult> SortLinkSerialResult(
        IQueryable<LinkSerialResult> query, SortInput<LinkSerialSortType> type)
    {
      switch (type.SortType)
      {
        case LinkSerialSortType.Serial:
          return query.OrderBy(a => a.Serial, type.SortOrder);
        case LinkSerialSortType.LinkedSerial:
          return query.OrderBy(a => a.LinkedSerial, type.SortOrder);
        case LinkSerialSortType.CustomerCode:
          return query.OrderBy(a => a.CustomerCode, type.SortOrder);
        case LinkSerialSortType.CustomerName:
          return query.OrderBy(a => a.CustomerName, type.SortOrder);
        case LinkSerialSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, type.SortOrder);
        case LinkSerialSortType.DateTime:
          return query.OrderBy(a => a.DateTime, type.SortOrder);
        case LinkSerialSortType.LinkerEmployeeFullName:
          return query.OrderBy(a => a.LinkerEmployeeFullName, type.SortOrder);
        case LinkSerialSortType.LinkDateTime:
          return query.OrderBy(a => a.LinkDateTime, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<LinkSerialResult> SearchLinkSerialResult(
        IQueryable<LinkSerialResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.LinkedSerial.ToString().Contains(searchText) ||
                    item.CustomerName.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region ToResult
    public Expression<Func<LinkSerial, LinkSerialResult>> ToLinkSerialResult =

         linkSerial => new LinkSerialResult()
         {
           LinkedSerial = linkSerial.LinkedSerial,
           UserId = linkSerial.UserId,
           DateTime = linkSerial.DateTime,
           ProductionDateTime = linkSerial.IranKhodroSerial.ProductionDateTime,
           EmployeeFullName = linkSerial.User.Employee.FirstName + " " + linkSerial.User.Employee.LastName,
           UserLinkerId = linkSerial.UserLinkerId,
           LinkDateTime = linkSerial.LinkDateTime,
           LinkerEmployeeFullName = linkSerial.UserLinker.Employee.FirstName + " " + linkSerial.UserLinker.Employee.LastName,
           CustomerId = linkSerial.CustomerId == null ? linkSerial.IranKhodroSerial.CustomerStuff.CustomerId : linkSerial.CustomerId,
           CustomerCode = linkSerial.Customer.Code == null ? linkSerial.IranKhodroSerial.CustomerStuff.Customer.Code : linkSerial.Customer.Code,
           CustomerName = linkSerial.Customer.Name == null ? linkSerial.IranKhodroSerial.CustomerStuff.Customer.Name : linkSerial.Customer.Name,
           StuffId = linkSerial.StuffSerial.StuffId == null ? linkSerial.IranKhodroSerial.CustomerStuff.StuffId : linkSerial.StuffSerial.StuffId,
           StuffCode = linkSerial.StuffSerial.Stuff.Code == null ? linkSerial.IranKhodroSerial.CustomerStuff.Stuff.Code : linkSerial.StuffSerial.Stuff.Code,
           StuffName = linkSerial.StuffSerial.Stuff.Name == null ? linkSerial.IranKhodroSerial.CustomerStuff.Stuff.Name : linkSerial.StuffSerial.Stuff.Name,
           BillOfMaterialVersion = linkSerial.StuffSerial.BillOfMaterial.Version,
           StuffSerialCode = linkSerial.StuffSerial.Code,
           Serial = linkSerial.StuffSerial.Serial,
           SerialProfileCode = linkSerial.StuffSerial.SerialProfileCode,
           Status = linkSerial.StuffSerial.StuffId == null ? LinkSerialStatus.NotBeLinked : LinkSerialStatus.BeLinked,
           Type = linkSerial.Type,
           #region
           Print = linkSerial.IranKhodroSerial.Print,
           PrintQty = linkSerial.IranKhodroSerial.PrintQty,
           CustomerStuffCode = linkSerial.IranKhodroSerial.CustomerStuff.Code,
           CustomerStuffName = linkSerial.IranKhodroSerial.CustomerStuff.Name,
           TechnicalNumber = linkSerial.IranKhodroSerial.CustomerStuff.TechnicalNumber,
           #endregion
           RowVersion = linkSerial.RowVersion
         };
    #endregion

  }
}
