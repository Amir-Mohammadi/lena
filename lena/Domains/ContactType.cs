using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ContactType : IEntity
  {
    protected internal ContactType()
    {
      this.Contacts = new HashSet<Contact>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public CooperatorContactType Type { get; set; }
    public byte[] RowVersion { get; set; }
    public EssentialContactType EssentialContactType { get; set; }
    public virtual ICollection<Contact> Contacts { get; set; }
  }
}