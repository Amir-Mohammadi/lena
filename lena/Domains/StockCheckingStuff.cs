using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StockCheckingStuff : IEntity
  {
    protected internal StockCheckingStuff()
    {
    }
    public int StockCheckingId { get; set; }
    public int StuffId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StockChecking StockChecking { get; set; }
    public virtual Stuff Stuff { get; set; }
  }
}