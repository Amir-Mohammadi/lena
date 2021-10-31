using System;
using System.Linq;
using lena.Services.Common;
using lena.Services.Core;

using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    public Contact AddCooperatorContact(string title, string contactText, int contactTypeId,
        bool isMain, int cooperatorId)
    {

      var contact = repository.Create<Contact>();
      contact.CooperatorId = cooperatorId;
      var result = App.Internals.ApplicationBase
                .AddContact(contact,
                            title: title,
                            contactText: contactText,
                            contactTypeId: contactTypeId,
                            isMain: isMain,
                            cooperatorId: cooperatorId,
                            employeeId: null);
      return result as Contact;
    }
    public IQueryable<Contact> GetCooperatorContacts(TValue<int> id = null,
        TValue<string> title = null, TValue<string> contactText = null, TValue<int> contactTypeId = null,
        TValue<bool> isMain = null, TValue<int> cooperatorId = null)
    {

      var contactQuery = App.Internals.ApplicationBase
                .GetContacts(id: id, title: title, contactText: contactText, contactTypeId: contactTypeId);
      var cooperatorContacts = contactQuery.OfType<Contact>();
      if (cooperatorId != null)
        cooperatorContacts = cooperatorContacts.Where(i => i.CooperatorId == cooperatorId);
      return cooperatorContacts;
    }
    public Contact GetCooperatorContact(int id)
    {

      var contact = this.GetCooperatorContacts(id: id).FirstOrDefault();
      if (contact == null)
        throw new ContactNotFoundException(id);
      return contact;
    }
    public Contact EditCooperatorContact(byte[] rowVersion, int id, TValue<string> title = null,
        TValue<string> contactText = null, TValue<int> contactTypeId = null, TValue<bool> isMain = null,
        TValue<int?> cooperatorId = null)
    {

      var cooperatorContact = this.GetCooperatorContact(id: id);
      if (cooperatorId != null)
        cooperatorContact.CooperatorId = cooperatorId;
      var result = App.Internals.ApplicationBase.EditContact(
                contact: cooperatorContact,
                rowVersion: rowVersion,
                id: id,
                title: title,
                contactText: contactText,
                contactTypeId: contactTypeId,
                isMain: isMain,
                cooperatorId: cooperatorId);
      return result as Contact;
    }
    public void DeleteCooperatorContact(int id)
    {

      var cooperatorContact = GetCooperatorContact(id);
      repository.Delete(cooperatorContact);
    }
    public IQueryable<CooperatorContactResult> ToCooperatorContactResultQuery(IQueryable<Contact> cooperatorContacts)
    {
      var results =
          from cooperatorContact in cooperatorContacts
          let contactType = cooperatorContact.ContactType
          select new CooperatorContactResult
          {
            Id = cooperatorContact.Id,
            Title = cooperatorContact.Title,
            IsMain = cooperatorContact.IsMain,
            ContactText = cooperatorContact.ContactText,
            ContactTypeId = contactType.Id,
            ContactTypeName = contactType.Name,
            CooperatorId = cooperatorContact.CooperatorId,
            CooperatorContactType = cooperatorContact.ContactType.Type,
            RowVersion = cooperatorContact.RowVersion
          };
      return results;
    }
    public CooperatorContactResult ToCooperatorContactResult(Contact cooperatorContact)
    {
      return new CooperatorContactResult
      {
        Id = cooperatorContact.Id,
        ContactText = cooperatorContact.ContactText,
        IsMain = cooperatorContact.IsMain,
        ContactTypeId = cooperatorContact.ContactTypeId,
        ContactTypeName = cooperatorContact.ContactType.Name,
        CooperatorId = cooperatorContact.CooperatorId,
        Title = cooperatorContact.Title,
        RowVersion = cooperatorContact.RowVersion
      };
    }
    public IOrderedQueryable<CooperatorContactResult> SortCooperatorContactResult(IQueryable<CooperatorContactResult> input, SortInput<CooperatorContactSortType> options)
    {
      switch (options.SortType)
      {
        case CooperatorContactSortType.Title:
          return input.OrderBy(r => r.Title, options.SortOrder);
        case CooperatorContactSortType.ContactText:
          return input.OrderBy(r => r.ContactText, options.SortOrder);
        case CooperatorContactSortType.ContactTypeName:
          return input.OrderBy(r => r.ContactTypeName, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException(nameof(options.SortOrder), options.SortType, null);
      }
    }
  }
}
