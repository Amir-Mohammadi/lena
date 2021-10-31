using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PaymentType : IEntity
  {
    protected internal PaymentType()
    {
      this.PaymentDueDates = new HashSet<PaymentDueDate>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
    public bool IsActive { get; set; }
    public virtual ICollection<PaymentDueDate> PaymentDueDates { get; set; }
  }
}
