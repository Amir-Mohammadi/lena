using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ScrumEntityDependency : IEntity
  {
    protected internal ScrumEntityDependency()
    {
    }
    public int RequisiteScrumEntityId { get; set; }
    public int NextScrumEntityId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ScrumEntity RequisiteScrumEntity { get; set; }
    public virtual ScrumEntity NextScrumEntity { get; set; }
  }
}