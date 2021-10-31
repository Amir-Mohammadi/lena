using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinanceAllocation : IEntity
  {
    protected internal FinanceAllocation()
    { }
    public int Id { get; set; }
    public int FinanceId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public double Amount { get; set; }
    public string ChequeNumber { get; set; }
    public DateTime AllocationDateTime { get; set; }
    public int UserId { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public Finance Finance { get; set; }
    public User User { get; set; }
  }
}
