using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class LadingBankOrderLog : IEntity
  {
    protected internal LadingBankOrderLog()
    {
    }
    public int Id { get; set; }
    public int LadingId { get; set; }
    public byte[] RowVersion { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public string Description { get; set; }
    public int LadingBankOrderStatusId { get; set; }
    public virtual User User { get; set; }
    public virtual Lading Lading { get; set; }
    public virtual LadingBankOrderStatus LadingBankOrderStatus { get; set; }
  }
}
