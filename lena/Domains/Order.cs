using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Order : IEntity
  {
    protected internal Order()
    {
      this.OrderItems = new HashSet<OrderItem>();
      this.PaymentDueDates = new HashSet<PaymentDueDate>();
      this.OrderDocuments = new HashSet<OrderDocument>();
    }
    public int Id { get; set; }
    public string Description { get; set; }
    public int OrderTypeId { get; set; }
    public byte[] RowVersion { get; set; }
    public int CustomerId { get; set; }
    public string Orderer { get; set; }
    public string DocumentNumber { get; set; }
    public Nullable<OrderDocumentType> DocumentType { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Nullable<double> TotalAmount { get; set; }
    public virtual OrderType OrderType { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }
    public virtual Cooperator Customer { get; set; }
    public virtual ICollection<PaymentDueDate> PaymentDueDates { get; set; }
    public virtual ICollection<OrderDocument> OrderDocuments { get; set; }
  }
}