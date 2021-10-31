using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PaymentDueDate : BaseEntity, IEntity
  {
    protected internal PaymentDueDate()
    {
    }
    public int OrderId { get; set; }
    public int PaymentTypeId { get; set; }
    public Nullable<double> Amount { get; set; }
    public Nullable<DateTime> PaymentDate { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual PaymentType PaymentType { get; set; }
    public virtual Order Order { get; set; }
  }
}
