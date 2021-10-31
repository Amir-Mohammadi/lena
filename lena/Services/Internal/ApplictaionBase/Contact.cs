using System;
using System.Linq;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase
{
  public partial class ApplicationBase
  {
    public IQueryable<Contact> GetContacts(
        TValue<int> id = null,
        TValue<string> title = null,
        TValue<string> contactText = null,
        TValue<int> contactTypeId = null,
        TValue<int?> employeeId = null,
        TValue<int?> cooperatorId = null)
    {

      var isIdNull = id == null;
      var isTitleNull = title == null;
      var isContactTextNull = contactText == null;
      var isContactTypeIdNull = contactTypeId == null;
      var isEmployeeIdNull = employeeId == null;
      var isCooperatorIdNull = cooperatorId == null;
      var contacts = from contact in repository.GetQuery<Contact>()
                     where (isIdNull || contact.Id == id) &&
                                 (isTitleNull || contact.Title == title) &&
                                 (isContactTextNull || contact.ContactText == contactText) &&
                                 (isContactTypeIdNull || contact.ContactTypeId == contactTypeId) &&
                                 (isEmployeeIdNull || contact.EmployeeId == employeeId) &&
                                 (isCooperatorIdNull || contact.CooperatorId == cooperatorId)
                     select contact;
      return contacts;
    }
    public Contact GetContact(int id)
    {

      var contact = this.GetContacts(id: id).FirstOrDefault();
      if (contact == null)
        throw new ContactNotFoundException(id);
      return contact;
    }
    public Contact AddContact(
        Contact contact,
        string title,
        string contactText,
        int contactTypeId,
        int? employeeId,
        int? cooperatorId,
        bool isMain)
    {

      contact.Title = title;
      contact.ContactText = contactText;
      contact.ContactTypeId = contactTypeId;
      contact.IsMain = isMain;
      contact.EmployeeId = employeeId;
      contact.CooperatorId = cooperatorId;
      repository.Add(contact);
      return contact;
    }
    public Contact EditContact(
        Contact contact,
        byte[] rowVersion,
        int id,
        TValue<string> title = null,
        TValue<bool> isMain = null,
        TValue<string> contactText = null,
        TValue<int> contactTypeId = null,
        TValue<int?> employeeId = null,
        TValue<int?> cooperatorId = null)
    {

      contact = contact ?? GetContact(id);
      if (title != null)
        contact.Title = title;
      if (contactText != null)
        contact.ContactText = contactText;
      if (contactTypeId != null)
        contact.ContactTypeId = contactTypeId;
      if (isMain != null)
        contact.IsMain = isMain;
      if (employeeId != null)
        contact.EmployeeId = employeeId;
      if (cooperatorId != null)
        contact.CooperatorId = cooperatorId;
      repository.Update(entity: contact, rowVersion: rowVersion);
      return contact;
    }
    public void DeleteContact(int id)
    {

      var contact = GetContact(id);
      repository.Delete(contact);
    }
    public IQueryable<ContactResult> ToContactResultQuery(IQueryable<Contact> query)
    {
      var result = from contact in query.OfType<Contact>()
                   let contactType = contact.ContactType
                   let cooperator = contact.Cooperator
                   select new ContactResult
                   {
                     Id = contact.Id,
                     Name = contact.Employee != null ? (contact.Employee.FirstName + " " + contact.Employee.LastName) : (contact.Cooperator != null ? contact.Cooperator.Name : ""),
                     Title = contact.Title,
                     IsMain = contact.IsMain,
                     ContactText = contact.ContactText,
                     ContactTypeId = contactType.Id,
                     ContactTypeName = contactType.Name,
                     CooperatorContactType = contactType.Type,
                     IsEmployeeContact = contact.EmployeeId != null,
                     IsCustomerContact = contact.CooperatorId != null ? cooperator.CooperatorType == CooperatorType.Customer : false,
                     IsProviderContact = contact.CooperatorId != null ? cooperator.CooperatorType == CooperatorType.Provider : false
                   };
      return result;
    }

    public IOrderedQueryable<ContactResult> SortContactResult(IQueryable<ContactResult> query, SortInput<ContactSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case ContactSortType.Name:
          return query.OrderBy(r => r.Name, sortInput.SortOrder);
        case ContactSortType.Title:
          return query.OrderBy(r => r.Title, sortInput.SortOrder);
        case ContactSortType.ContactText:
          return query.OrderBy(r => r.ContactText, sortInput.SortOrder);
        case ContactSortType.ContactTypeName:
          return query.OrderBy(r => r.ContactTypeName, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException(nameof(sortInput.SortOrder), sortInput.SortType, null);
      }
    }

    public IQueryable<ContactResult> SearchContactResultQuery(
        IQueryable<ContactResult> query, string searchText, AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(x => x.ContactText.Contains(searchText) ||
                                x.Title.Contains(searchText) ||
                                x.Name.Contains(searchText) ||
                                x.ContactTypeName.Contains(searchText));
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
  }
}
