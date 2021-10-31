using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Cottage : IEntity
  {
    protected internal Cottage()
    {
      this.CottageItems = new HashSet<CottageItem>();
    }
    public int Id { get; set; }
    public string Index { get; set; }
    public int CustomsDeclarationId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual CustomsDeclaration CustomsDeclaration { get; set; }
    public virtual ICollection<CottageItem> CottageItems { get; set; }
  }
}