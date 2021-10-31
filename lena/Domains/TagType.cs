using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class TagType : IEntity
  {
    protected internal TagType()
    {
      this.StockCheckingTags = new HashSet<StockCheckingTag>();
      this.StockCheckings = new HashSet<StockChecking>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<StockCheckingTag> StockCheckingTags { get; set; }
    public virtual ICollection<StockChecking> StockCheckings { get; set; }
  }
}