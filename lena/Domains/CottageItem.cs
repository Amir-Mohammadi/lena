using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CottageItem : IEntity
  {
    protected internal CottageItem()
    {
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<int> BankOrderDetailId { get; set; }
    public int StuffHSGroupId { get; set; }
    public virtual Cottage Cottage { get; set; }
    public virtual BankOrderDetail BankOrderDetail { get; set; }
    public virtual StuffHSGroup StuffHSGroup { get; set; }
  }
}