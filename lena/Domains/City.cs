using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class City : IEntity
  {
    public City()
    {
      this.Cooperators = new HashSet<Cooperator>();
    }
    public short Id { get; set; }
    public string Title { get; set; }
    public byte CountryId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Country Country { get; set; }
    public virtual ICollection<Cooperator> Cooperators { get; set; }
    public virtual ICollection<Lading> Ladings { get; set; }
  }
}
