using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class SecurityActionGroup : IEntity
  {
    protected internal SecurityActionGroup()
    {
      this.SecurityActions = new HashSet<SecurityAction>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<SecurityAction> SecurityActions { get; set; }
  }
}