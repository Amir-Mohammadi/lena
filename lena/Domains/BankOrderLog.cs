using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BankOrderLog : IEntity
  {
    protected internal BankOrderLog()
    {
    }
    public int Id { get; set; }
    public int BankOrderStatusTypeId { get; set; }
    public int BankOrderId { get; set; }
    public int UserId { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public string Description { get; set; }
    public virtual User User { get; set; }
    public virtual BankOrderStatusType BankOrderStatusType { get; set; }
    public virtual BankOrder BankOrder { get; set; }
  }
}
