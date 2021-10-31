using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PaymentSuggestStatusLog : IEntity
  {
    protected internal PaymentSuggestStatusLog()
    {
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int QualityControlId { get; set; }
    public DateTime RegisterDateTime { get; set; }
    public int RegisterarUserId { get; set; }
    public string Description { get; set; }
    public QualityControlPaymentSuggestStatus? QualityControlPaymentSuggestStatus { get; set; }
    public virtual User RegisterarUser { get; set; }
    public virtual QualityControl QualityControl { get; set; }
  }
}