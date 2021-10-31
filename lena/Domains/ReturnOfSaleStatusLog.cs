using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ReturnOfSaleStatusLog : BaseEntityLog, IEntity
  {
    protected internal ReturnOfSaleStatusLog()
    {
    }
    public ReturnOfSaleStatus Status { get; set; }
    public int ReturnOfSaleId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ReturnOfSale ReturnOfSale { get; set; }
  }
}