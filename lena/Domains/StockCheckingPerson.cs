using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StockCheckingPerson : IEntity
  {
    protected internal StockCheckingPerson()
    {
    }
    public int StockCheckingId { get; set; }
    public int UserId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StockChecking StockChecking { get; set; }
    public virtual User User { get; set; }
  }
}