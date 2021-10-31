using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CustomsDeclaration : BaseEntity, IEntity
  {
    protected internal CustomsDeclaration()
    {
      this.Cottages = new HashSet<Cottage>();
    }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<Cottage> Cottages { get; set; }
  }
}