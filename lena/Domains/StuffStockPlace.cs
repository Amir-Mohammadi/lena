using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffStockPlace : IEntity
  {
    protected internal StuffStockPlace()
    {
    }
    public int StuffId { get; set; }
    public int StockPlaceId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual StockPlace StockPlace { get; set; }
  }
}