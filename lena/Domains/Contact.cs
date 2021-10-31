using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Contact : IEntity
  {
    protected internal Contact()
    {
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public string ContactText { get; set; }
    public int ContactTypeId { get; set; }
    public bool IsMain { get; set; }
    public Nullable<int> CooperatorId { get; set; }
    public Nullable<int> EmployeeId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ContactType ContactType { get; set; }
    public virtual Cooperator Cooperator { get; set; }
    public virtual Employee Employee { get; set; }
  }
}